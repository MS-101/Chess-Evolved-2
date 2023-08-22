using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chess : MonoBehaviour
{
    public enum PieceType
    {
        Pawn = 0,
        Knight = 1,
        Bishop = 2,
        Rook = 3,
    }

    public enum Essence
    {
        Classic = 0,
        Red = 1,
        Blue = 2
    }

    public enum Color
    {
        White = 0,
        Black = 1,
        Random = 2
    }

    public class GameSettings
    {
        public Color playerColor;
        public Essence playerPawn, playerKnight, playerBishop, playerRook;
        public Essence malakhPawn, malakhKnight, malakhBishop, malakhRook;

        public GameSettings(Color playerColor, Essence playerPawn, Essence playerKnight, Essence playerBishop, Essence playerRook,
            Essence malakhPawn, Essence malakhKnight, Essence malakhBishop, Essence malakhRook)
        {
            this.playerColor = playerColor;

            this.playerPawn = playerPawn;
            this.playerKnight = playerKnight;
            this.playerBishop = playerBishop;
            this.playerRook = playerRook;

            this.malakhPawn = malakhPawn;
            this.malakhKnight = malakhKnight;
            this.malakhBishop = malakhBishop;
            this.malakhRook = malakhRook;
        }
    }
}
