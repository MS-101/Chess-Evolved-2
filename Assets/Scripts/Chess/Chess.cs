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
        Queen = 4,
        King = 5
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

    public enum MovementType
    {
        Move = 0,
        Attack = 1,
        AttackMove = 2
    }

    public class GameSettings
    {
        public Color playerColor, malakhColor;
        public Essence playerPawn, playerKnight, playerBishop, playerRook;
        public Essence malakhPawn, malakhKnight, malakhBishop, malakhRook;

        public GameSettings(Color playerColor, Essence playerPawn, Essence playerKnight, Essence playerBishop, Essence playerRook,
            Color malakhColor, Essence malakhPawn, Essence malakhKnight, Essence malakhBishop, Essence malakhRook)
        {
            this.playerColor = playerColor;

            this.playerPawn = playerPawn;
            this.playerKnight = playerKnight;
            this.playerBishop = playerBishop;
            this.playerRook = playerRook;

            this.malakhColor = malakhColor;

            this.malakhPawn = malakhPawn;
            this.malakhKnight = malakhKnight;
            this.malakhBishop = malakhBishop;
            this.malakhRook = malakhRook;
        }
    }

    public static Sprite GetPieceImage(PieceType type, Color color, Essence essence)
    {
        return Resources.Load<Sprite>("Pieces/" + type.ToString() + "_" + color.ToString() + "_" + essence.ToString());
    }

    public static Sprite GetMovementImage(MovementType type)
    {
        return Resources.Load<Sprite>("Movements/" + type.ToString());
    }
}
