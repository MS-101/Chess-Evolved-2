using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost
{
    public int x, y;
    public Piece parent;

    public Ghost(int x, int y, Piece parent)
    {
        this.x = x;
        this.y = y;
        this.parent = parent;
    }
}
