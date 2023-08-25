using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece
{
    public Chess.Type type;
    public Chess.Color color;
    public Chess.Essence essence;

    public Piece(Chess.Type type, Chess.Color color, Chess.Essence essence)
    {
        this.type = type;
        this.color = color;
        this.essence = essence;
    }
}
