using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class EngineController : MonoBehaviour
{
    private Process engineProcess = new();

    public string name = string.Empty;
    public string author = string.Empty;
    public Dictionary<Chess.PieceType, Dictionary<Chess.Essence, List<Mobility>>> mobilities = new();

    private void Start()
    {
        StartEngine();
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

        engineProcess.StartInfo.FileName = "Malakh.exe";
        engineProcess.StartInfo.UseShellExecute = false;
        engineProcess.StartInfo.RedirectStandardInput = true;
        engineProcess.StartInfo.RedirectStandardOutput = true;

        engineProcess.Start();
        Task.Run(() => ReceiveEngineResponses());
        SendCommand("uci");
    }

    public void StopEngine()
    {
        engineProcess.StandardInput.WriteLine("quit");
        engineProcess.StandardInput.Flush();

        engineProcess.WaitForExit();
        engineProcess.Close();
    }

    public void SendCommand(string command)
    {
        engineProcess.StandardInput.WriteLine(command);
        engineProcess.StandardInput.Flush();
    }

    private void ReceiveEngineResponses()
    {
        string response;
        while ((response = engineProcess.StandardOutput.ReadLine()) != null)
        {
            string[] tokens = response.Split(' ');
            if (tokens[0] == "id")
            {
                if (tokens[1] == "name")
                {
                    name = string.Empty;
                    for (int i = 2; i < tokens.Length; i++)
                        name += tokens[i];
                }
                else if (tokens[1] == "author")
                {
                    author = string.Empty;
                    for (int i = 2; i < tokens.Length; i++)
                        author += tokens[i];
                }
            }
            else if (tokens[0] == "uciok")
            {
                SendCommand("isready");
            }
            else if (tokens[0] == "info" && tokens[1] == "string")
            {
                if (tokens[2] == "#MOBILITY")
                {
                    Chess.PieceType pieceType = (Chess.PieceType)Enum.Parse(typeof(Chess.PieceType), tokens[3]);
                    Chess.Essence essence = (Chess.Essence)Enum.Parse(typeof(Chess.Essence), tokens[4]);
                    Chess.MovementType movementType = (Chess.MovementType)Enum.Parse(typeof(Chess.MovementType), tokens[5]);

                    int start_x = int.Parse(tokens[6]);
                    int start_y = int.Parse(tokens[7]);
                    int direction_x = int.Parse(tokens[8]);
                    int direction_y = int.Parse(tokens[9]);
                    int limit = int.Parse(tokens[10]);

                    mobilities[pieceType][essence].Add(new(movementType, start_x, start_y, direction_x, direction_y, limit));
                }
                else
                {
                    string message = string.Empty;
                    for (int i = 2; i < tokens.Length; i++)
                        message += tokens[i];

                    UnityEngine.Debug.Log(message);
                }
            }
        }
    }
}
