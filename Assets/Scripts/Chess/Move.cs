using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Castling { None, QueenSide, KingSide };

public class Move
{
    public readonly int x1, y1, x2, y2;
    public bool hasty = false, vigilant = false;
    public int hastyX, hastyY;
    public readonly Castling castling = Castling.None;

    public Move(int x1, int y1, int x2, int y2)
    {
        this.x1 = x1;
        this.y1 = y1;
        this.x2 = x2;
        this.y2 = y2;
    }

    public Move(Castling castling)
    {
        this.castling = castling;
    }

    public void SetHasty(int hastyX, int hastyY)
    {
        hasty = true;

        this.hastyX = hastyX;
        this.hastyY = hastyY;
    }

    public void ClearHasty()
    {
        hasty = false;
    }

    public void SetVigilant()
    {
        vigilant = true;
    }

    public void ClearVigilant()
    {
        vigilant = false;
    }
}
