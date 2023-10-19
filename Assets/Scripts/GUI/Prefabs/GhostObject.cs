using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostObject : MonoBehaviour
{
    private Ghost ghost = null;
    public Ghost Ghost
    {
        get { return ghost; }
        set { ghost = value; }
    }
}
