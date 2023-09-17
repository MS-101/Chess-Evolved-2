using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mobility
{
    public Chess.MovementType type;
    public int start_x, start_y, direction_x, direction_y, limit;

    public Mobility(Chess.MovementType type, int start_x, int start_y, int direction_x, int direction_y, int limit)
    {
        this.type = type;
        this.start_x = start_x;
        this.start_y = start_y;
        this.direction_x = direction_x;
        this.direction_y = direction_y;
        this.limit = limit;
    }
}
