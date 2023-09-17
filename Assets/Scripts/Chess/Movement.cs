using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement
{
    public Piece owner;
    public Chess.MovementType type;
    public int x, y;

    public Movement(Piece owner, Chess.MovementType type, int x, int y)
    {
        this.owner = owner;
        this.type = type;
        this.x = x;
        this.y = y;
    }
}
