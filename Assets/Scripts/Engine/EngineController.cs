﻿/*****************************************************************//**
 * \file   EngineController.cs
 * \brief  Ovládač šachového enginu Malakh.
 * 
 * \author Martin Šváb
 * \date   Máj 2024
 *********************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;


/**
 * Táto trieda je zodpovedná za komunikáciu s enginom Malakh pomocou UCI protokolu.
 */
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
    public UnityEvent<Move, Chess.PieceType> onBestMoveReceived = new();
    public UnityEvent<Chess.Color> onResultReceived = new();
    public UnityEvent onCheckReceived = new();

    private Move receivedBestMove = null;
    private Chess.PieceType promotedPieceType = Chess.PieceType.Pawn;

    private bool unhandledResult = false;
    private Chess.Color receivedResult = Chess.Color.Random;

    private bool receivedCheck = false;

    /**
     * Pri spustení tohto skriptu zapneme engine.
     */
    private void Start()
    {
        StartEngine();
    }

    /**
     * Pri každej aktualizácii kontrolujeme, či sme nedostali od enginu novú nespracovanú správu.
     * Niektoré operácie musia byť vykonané cez update procedúru a nemôžu byť vykonané priamo.
     */
    private void Update()
    {
        if (receivedCheck)
        {
            receivedCheck = false;
            onCheckReceived?.Invoke();
        }

        if (receivedBestMove != null)
        {
            onBestMoveReceived?.Invoke(receivedBestMove, promotedPieceType);
            receivedBestMove = null;
        }

        if (unhandledResult)
        {
            unhandledResult = false;
            onResultReceived?.Invoke(receivedResult);
        }
    }

    /**
     * Pri ukončení aplikácie terminuje šachový engine.
     */
    private void OnApplicationQuit()
    {
        StopEngine();
    }

    /**
     * Táto funkcia spustí engine Malakh v UCI režime.
     */
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
        SendCommand("uci");
    }

    /**
     * Táto funkcia terminuje engine pomocou UCI príkazu.
     */
    public void StopEngine()
    {
        SendCommand("quit");

        engineProcess.WaitForExit();
        engineProcess.Close();
    }

    /**
     * Hra je spustená s danou hernou konfiguráciou.
     * 
     * \ref gameSettings Nastavenia novej hry.
     */
    public void StartGame(Chess.GameSettings gameSettings)
    {
        SendCommand("setoption name AI value " + gameSettings.ai.ToString());
        SendCommand("setoption name WhitePawn value " + (gameSettings.playerColor == Chess.Color.White ? gameSettings.playerPawn.ToString() : gameSettings.malakhPawn.ToString()));
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

    /**
     * Od enginu si vyžiadame všetky dostupné legálne pohyby.
     */
    public void RequestAvailableMoves()
    {
        SendCommand("legalmoves");
    }

    /**
     * Od enginu si vyžiadame najlepší pohyb.
     */
    public void RequestBestMove()
    {
        SendCommand("go depth 3");
    }

    /**
     * Enginu prikážeme aby vykonal daný pohyb.
     * 
     * \param x1 Koordinát X zdrojovej pozície.
     * \param y1 Koordinát Y zdrojovej pozície.
     * \param x2 Koordinát X cieľovej pozície.
     * \param y2 Koordinát Y cieľovej pozície.
     * \param promotedPieceType Typ figúrky na ktorú sa zmení pešiak pri promócii.
     */
    public void MovePiece(int x1, int y1, int x2, int y2, Chess.PieceType promotedPieceType)
    {
        string move = "";
        move += (char)('a' + x1);
        move += (char)('1' + y1);
        move += (char)('a' + x2);
        move += (char)('1' + y2);
        switch (promotedPieceType)
        {
            case Chess.PieceType.Queen:
                move += "q";
                break;
            case Chess.PieceType.Rook:
                move += "r";
                break;
            case Chess.PieceType.Bishop:
                move += "b";
                break;
            case Chess.PieceType.Knight:
                move += "n";
                break;
        }

        SendCommand("position curpos moves " + move);
    }

    /**
     * Generická metóda na odoslanie ľubovoľnej správy enginu.
     */
    private void SendCommand(string command)
    {
        UnityEngine.Debug.Log("GUI->ENGINE: " + command);
        engineProcess.StandardInput.WriteLine(command);
    }

    /**
     * V tejto metóde periodicky prijímame a spracuvávame prijaté správy od enginu.
     */
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
                    legalMoves.Add(ParseMove(tokens[i]));

                onLegalMovesReceived?.Invoke(legalMoves);
            }
            else if (tokens[0] == "bestmove")
            {
                string[] bestMoveTokens = tokens[1].Split('_');
                if (bestMoveTokens[0].Length == 5)
                {
                    char promotedPieceTypeChar = bestMoveTokens[0][4];
                    switch(promotedPieceTypeChar)
                    {
                        case 'q':
                            promotedPieceType = Chess.PieceType.Queen;
                            break;
                        case 'r':
                            promotedPieceType = Chess.PieceType.Rook;
                            break;
                        case 'b':
                            promotedPieceType = Chess.PieceType.Bishop;
                            break;
                        case 'n':
                            promotedPieceType = Chess.PieceType.Knight;
                            break;
                    }
                }
                else
                    promotedPieceType = Chess.PieceType.Pawn;

                receivedBestMove = ParseMove(bestMoveTokens[0]);
            }
            else if (tokens[0] == "check")
            {
                receivedCheck = true;
            }
            else if (tokens[0] == "result")
            {
                if (tokens[1] == "White")
                    receivedResult = Chess.Color.White;
                else if (tokens[1] == "Black")
                    receivedResult = Chess.Color.Black;
                else if (tokens[1] == "Stalemate")
                    receivedResult = Chess.Color.Random;
                unhandledResult = true;
            }
        }
    }

    /**
     * String konvertujeme na pohyb figúrky.
     * 
     * \param move Slovo reprezentujúce pohyb figúrky v algebraickej notácii.
     * \return Pohyb figúrky.
     */
    private Move ParseMove(string move)
    {
        if (move.Equals("O-O"))
            return new(Castling.KingSide);

        if (move.Equals("O-O-O"))
            return new(Castling.QueenSide);

        string[] tokens = move.Split('_');

        string moveToken = tokens[0];
        int sourceColumn = moveToken[0] - 'a';
        int sourceRow = moveToken[1] - '1';
        int targetColumn = moveToken[2] - 'a';
        int targetRow = moveToken[3] - '1';

        Move retVal = new(sourceColumn, sourceRow, targetColumn, targetRow);

        for (int i = 1; i < tokens.Length; i++)
        {
            string flagToken = tokens[i];
            if (flagToken[0] == 'H')
            {
                int hastyColumn = flagToken[1] - 'a';
                int hastyRow = flagToken[2] - '1';

                retVal.SetHasty(hastyColumn, hastyRow);
            }
            else if (flagToken[0] == 'V')
            {
                retVal.SetVigilant();
            }
        }

        return retVal;
    }
}
