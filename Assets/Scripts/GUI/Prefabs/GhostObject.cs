/*****************************************************************//**
 * \file   GhostObject.cs
 * \brief  Ovládač objektu ducha.
 * 
 * \author Martin Šváb
 * \date   Máj 2024
 *********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Táto trieda spravuje objekt ducha.
 */
public class GhostObject : MonoBehaviour
{
    private Ghost ghost = null;

    /**
     * Duch priradený tomuto objektu.
     */
    public Ghost Ghost
    {
        get { return ghost; }
        set { ghost = value; }
    }
}
