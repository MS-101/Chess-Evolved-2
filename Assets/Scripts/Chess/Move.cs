/*****************************************************************//**
 * \file   Move.cs
 * \brief  Pohyb na šachovnici.
 * 
 * \author Martin Šváb
 * \date   Máj 2024
 *********************************************************************/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/**
 * Enumerátor reprezentujúci typ rošády.
 */
public enum Castling {
    /**
     * Null hodnota
     */
    None,
    /**
     * Rošáda z kráľovninej strany.
     */
    QueenSide,
    /**
     * Rošáda z kráľovej strany.
     */
    KingSide
};

/**
 * Trieda reprezentujúca pohyb na šachovnici.
 */
public class Move
{
    /**
     * Koordinát X zdrojovej pozície.
     */
    public readonly int x1;

    /**
     * Koordinát Y zdrojovej pozície.
     */
    public readonly int y1;

    /**
     * Koordinát X cieľovej pozície.
     */
    public readonly int x2;

    /**
     * Koordinát Y cieľovej pozície.
     */
    public readonly int y2;

    /**
     * Hasty flag pohybu (pohyb vytvorí ducha).
     */
    public bool hasty = false;

    /**
     * Vigilant flag pohybu (pohyb môže napadnúť ducha).
     */
    public bool vigilant = false;

    /**
     * Koordinát X vytvoreného ducha.
     */
    public int hastyX;

    /**
     * Koordinát Y vytvoreného ducha.
     */
    public int hastyY;

    /**
     * Typ rošády.
     */
    public readonly Castling castling = Castling.None;

    /**
     * Konštruktor normálneho pohybu na šachovnici.
     * 
     * \param x1 Koordinát X počiatočnej pozície.
     * \param y1 Koordinát Y počiatočnej pozície.
     * \param x2 Koordinát X cieľovej pozície.
     * \param y2 Koordinát Y cieľovej pozície.
     */
    public Move(int x1, int y1, int x2, int y2)
    {
        this.x1 = x1;
        this.y1 = y1;
        this.x2 = x2;
        this.y2 = y2;
    }

    /**
     * Konštruktor rošády.
     * 
     * \param castling Typ rošády.
     */
    public Move(Castling castling)
    {
        this.castling = castling;
    }

    /**
     * Nastavenie hasty flagu.
     * 
     * \param hastyX Koordinát X vytvoreného ducha.
     * \param hastyY Koordinát Y vytvoreného ducha.
     */
    public void SetHasty(int hastyX, int hastyY)
    {
        hasty = true;

        this.hastyX = hastyX;
        this.hastyY = hastyY;
    }

    /**
     * Zrušenie hasty flagu.
     */
    public void ClearHasty()
    {
        hasty = false;
    }

    /**
     * Nastavenie vigilant flagu.
     */
    public void SetVigilant()
    {
        vigilant = true;
    }

    /**
     * Zrušenie vigilant flagu.
     */
    public void ClearVigilant()
    {
        vigilant = false;
    }
}
