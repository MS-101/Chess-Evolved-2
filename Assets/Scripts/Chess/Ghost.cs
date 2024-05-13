/*****************************************************************//**
 * \file   Ghost.cs
 * \brief  Špeciálna figúrka vytvorená po vykonaní pohybu s hasty flagom.
 * 
 * \author Martin Šváb
 * \date   Máj 2024
 *********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Táto trieda reprezentuje špeciálnu figúrku vytvorenú po vykonaní pohybu s hasty flagom.
 */
public class Ghost
{
    /**
     * Koordinát X na šachovnici.
     */
    public int x;

    /**
    * Koordinát Y na šachovnici.
    */
    public int y;

    /**
     * Figúrka, ktorá vlastní túto špeciálnu figúrku.
     */
    public Piece parent;

    /**
     * Konštruktor špeciálnej figúrky.
     * 
     * \param x Koordinát X na šachovnici.
     * \param y Koordinát Y na šachovnici.
     * \param parent Figúrka, ktorá vlastní túto špeciálnu figúrku.
     */
    public Ghost(int x, int y, Piece parent)
    {
        this.x = x;
        this.y = y;
        this.parent = parent;
    }
}
