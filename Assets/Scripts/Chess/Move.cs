using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Move
{
    public readonly int x1, y1, x2, y2;
    public bool hasty = false, vigilant = false, inspiring = false;
    public int hastyX, hastyY, inspiringX1, inspiringY1, inspiringX2, inspiringY2;

    public Move(int x1, int y1, int x2, int y2)
    {
        this.x1 = x1;
        this.y1 = y1;
        this.x2 = x2;
        this.y2 = y2;
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

    public void SetInspiring(int inspiringX1, int inspiringY1, int inspiringX2, int inspiringY2)
    {
        inspiring = true;

        this.inspiringX1 = inspiringX1;
        this.inspiringY1 = inspiringY1;
        this.inspiringX2 = inspiringX2;
        this.inspiringY2 = inspiringY2;
    }

    public void ClearInspiring()
    {
        inspiring = false;
    }
}
