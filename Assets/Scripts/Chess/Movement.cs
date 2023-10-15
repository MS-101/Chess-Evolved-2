using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement
{
    public Piece owner;
    public Chess.MovementType type;
    public Move move;

    public Movement(Piece owner, Chess.MovementType type, Move move)
    {
        this.owner = owner;
        this.type = type;
        this.move = move;
    }
}
