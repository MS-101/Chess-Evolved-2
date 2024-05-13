/*****************************************************************//**
 * \file   Movement.cs
 * \brief  Pohyb figúrky.
 * 
 * \author Martin Šváb
 * \date   Máj 2024
 *********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Táto trieda reprezentuje pohyb figúrky.
 */
public class Movement
{
    /**
     * Vlastník pohybu - šachová figúrka.
     */
    public Piece owner;

    /**
     * Typ pohybu.
     */
    public Chess.MovementType type;

    /**
     * Pohyb.
     */
    public Move move;

    /**
     * Konštruktor pohybu figúrky.
     * 
     * \param owner Vlastník pohybu.
     * \param type Typ pohybu.
     * \param move Pohyb.
     */
    public Movement(Piece owner, Chess.MovementType type, Move move)
    {
        this.owner = owner;
        this.type = type;
        this.move = move;
    }
}
