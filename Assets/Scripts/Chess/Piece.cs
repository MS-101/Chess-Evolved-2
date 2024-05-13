/*****************************************************************//**
 * \file   Piece.cs
 * \brief  Šachová figúrka.
 * 
 * \author Martin Šváb
 * \date   Máj 2024
 *********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Táto trieda reprezentuje šachovú figúrku.
 */
public class Piece
{
    /**
     * Typ šachovej figúrky.
     */
    public Chess.PieceType type;

    /**
     * Farba šachovej figúrky.
     */
    public Chess.Color color;

    /**
     * Esencia šachovej figúrky.
     */
    public Chess.Essence essence;

    /**
     * Koordinát X umiestnenia figúrky na šachovnici.
     */
    public int x = -1;

    /**
     * Koordinát Y umiestnenia figúrky na šachovnici.
     */
    public int y = -1;

    /**
     * Zoznam dostupných pohybov figúrky.
     */
    public List<Movement> availableMoves = new();

    /**
     * Konštruktor figúrky.
     * 
     * \param type Typ figúrky.
     * \param color Farba figúrky.
     * \param essence Esencia figúrky.
     */
    public Piece(Chess.PieceType type, Chess.Color color, Chess.Essence essence)
    {
        this.type = type;
        this.color = color;
        this.essence = essence;
    }
}
