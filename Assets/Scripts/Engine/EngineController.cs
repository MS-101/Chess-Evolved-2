using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class EngineController : MonoBehaviour
{
    private Process engineProcess = new()
    {
        StartInfo = new()
        {
            FileName = "Malakh-chess-engine.exe",
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        }
    };

    public string engineName = string.Empty;
    public string author = string.Empty;
    public Dictionary<Chess.PieceType, Dictionary<Chess.Essence, List<Mobility>>> mobilities = new();

    public UnityEvent<List<Move>> onLegalMovesReceived = new();
    public UnityEvent<Move> onBestMoveReceived = new();

    private Move receivedBestMove = null;

    private void Start()
    {
        StartEngine();
    }

    private void Update()
    {
        if (receivedBestMove != null)
        {
            onBestMoveReceived?.Invoke(receivedBestMove);
            receivedBestMove = null;
        }   
    }

    private void OnApplicationQuit()
    {
        StopEngine();
    }

    public void StartEngine()
    {
        mobilities.Clear();
        foreach (Chess.PieceType pieceType in (Chess.PieceType[])Enum.GetValues(typeof(Chess.PieceType)))
        {
            mobilities.Add(pieceType, new());
            foreach (Chess.Essence essence in (Chess.Essence[])Enum.GetValues(typeof(Chess.Essence)))
                mobilities[pieceType].Add(essence, new());
        }

        engineProcess.Start();
        Task.Run(() => ReceiveEngineResponses());
        Task.Run(() => ReceiveEngineErrors());
        SendCommand("uci");
    }

    public void StopEngine()
    {
        SendCommand("quit");

        engineProcess.WaitForExit();
        engineProcess.Close();
    }

    public void StartGame(Chess.GameSettings gameSettings)
    {
        SendCommand("setoption name WhitePawn value " + (gameSettings.playerColor == Chess.Color.White ? gameSettings.playerPawn.ToString() : gameSettings.malakhPawn.ToString()));
        SendCommand("setoption name WhiteKnight value " + (gameSettings.playerColor == Chess.Color.White ? gameSettings.playerKnight.ToString() : gameSettings.malakhKnight.ToString()));
        SendCommand("setoption name WhiteBishop value " + (gameSettings.playerColor == Chess.Color.White ? gameSettings.playerBishop.ToString() : gameSettings.malakhBishop.ToString()));
        SendCommand("setoption name WhiteRook value " + (gameSettings.playerColor == Chess.Color.White ? gameSettings.playerRook.ToString() : gameSettings.malakhRook.ToString()));
        SendCommand("setoption name BlackPawn value " + (gameSettings.playerColor == Chess.Color.White ? gameSettings.malakhPawn.ToString() : gameSettings.playerPawn.ToString()));
        SendCommand("setoption name BlackKnight value " + (gameSettings.playerColor == Chess.Color.White ? gameSettings.malakhKnight.ToString() : gameSettings.playerKnight.ToString()));
        SendCommand("setoption name BlackBishop value " + (gameSettings.playerColor == Chess.Color.White ? gameSettings.malakhBishop.ToString() : gameSettings.playerBishop.ToString()));
        SendCommand("setoption name BlackRook value " + (gameSettings.playerColor == Chess.Color.White ? gameSettings.malakhRook.ToString() : gameSettings.playerRook.ToString()));
        SendCommand("ucinewgame");
    }

    public void RequestAvailableMoves(Chess.Color color)
    {
        SendCommand("legalmoves " + color.ToString());
    }

    public void RequestBestMove()
    {
        SendCommand("go depth 10");
    }

    public void MovePiece(int x1, int y1, int x2, int y2)
    {
        char sourceColumn = (char)('a' + x1);
        char sourceRow = (char)('1' + y1);
        char targetColumn = (char)('a' + x2);
        char targetRow = (char)('1' + y2);

        SendCommand("position curpos moves " + sourceColumn + sourceRow + targetColumn + targetRow);
    }

    private void SendCommand(string command)
    {
        UnityEngine.Debug.Log("GUI->ENGINE: " + command);
        engineProcess.StandardInput.WriteLine(command);
        engineProcess.StandardInput.Flush();
    }

    private void ReceiveEngineErrors()
    {
        string respone;
        while ((respone = engineProcess.StandardError.ReadLine()) != null)
        {
            UnityEngine.Debug.Log("ENGINE ERROR: " + respone);
        }
    }

    private void ReceiveEngineResponses()
    {
        string response;
        while ((response = engineProcess.StandardOutput.ReadLine()) != null)
        {
            UnityEngine.Debug.Log("ENGINE->GUI: " + response);

            string[] tokens = response.Split(' ');
            if (tokens[0] == "id")
            {
                if (tokens[1] == "name")
                {
                    engineName = string.Empty;
                    for (int i = 2; i < tokens.Length; i++)
                        engineName += tokens[i];
                }
                else if (tokens[1] == "author")
                {
                    author = string.Empty;
                    for (int i = 2; i < tokens.Length; i++)
                        author += tokens[i];
                }
            }
            else if (tokens[0] == "mobility")
            {
                Chess.PieceType pieceType = (Chess.PieceType)Enum.Parse(typeof(Chess.PieceType), tokens[1]);
                Chess.Essence essence = (Chess.Essence)Enum.Parse(typeof(Chess.Essence), tokens[2]);
                Chess.MovementType movementType = (Chess.MovementType)Enum.Parse(typeof(Chess.MovementType), tokens[3]);

                int start_x = int.Parse(tokens[4]);
                int start_y = int.Parse(tokens[5]);
                int direction_x = int.Parse(tokens[6]);
                int direction_y = int.Parse(tokens[7]);
                int limit = int.Parse(tokens[8]);

                mobilities[pieceType][essence].Add(new(movementType, start_x, start_y, direction_x, direction_y, limit));
            }
            else if (tokens[0] == "uciok")
            {
                SendCommand("isready");
            }
            else if (tokens[0] == "readyok")
            {
                // dont need to do anything
            }
            else if (tokens[0] == "legalmoves")
            {
                List<Move> legalMoves = new();
                for (int i = 1; i < tokens.Length; i++)
                {
                    string legalMove = tokens[i];

                    int sourceColumn = legalMove[0] - 'a';
                    int sourceRow = legalMove[1] - '1';
                    int targetColumn = legalMove[2] - 'a';
                    int targetRow = legalMove[3] - '1';

                    legalMoves.Add(new(sourceColumn, sourceRow, targetColumn, targetRow));
                }

                onLegalMovesReceived?.Invoke(legalMoves);
            }
            else if (tokens[0] == "bestmove")
            {
                string bestMove = tokens[1];

                int sourceColumn = bestMove[0] - 'a';
                int sourceRow = bestMove[1] - '1';
                int targetColumn = bestMove[2] - 'a';
                int targetRow = bestMove[3] - '1';

                receivedBestMove = new(sourceColumn, sourceRow, targetColumn, targetRow);
            }
        }
    }
}
