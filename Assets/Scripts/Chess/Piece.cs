using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece
{
    public Chess.PieceType type;
    public Chess.Color color;
    public Chess.Essence essence;
    public int x = -1, y = -1;

    public List<Movement> availableMoves = new();

    public Piece(Chess.PieceType type, Chess.Color color, Chess.Essence essence)
    {
        this.type = type;
        this.color = color;
        this.essence = essence;
    }
}
