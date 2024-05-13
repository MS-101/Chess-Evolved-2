/*****************************************************************//**
 * \file   Mobility.cs
 * \brief  Konfigurácia mobility figúrky.
 * 
 * \author Martin Šváb
 * \date   Máj 2024
 *********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Táto trieda reprezentuje konfiguráciu mobility figúrky.
 */
public class Mobility
{
    /**
     * Typ pohybu.
     */
    public Chess.MovementType type;

    /**
     * Koordinát X počiatočnej pozície.
     */
    public int start_x;

    /**
    * Koordinát Y počiatočnej pozície.
    */
    public int start_y;

    /**
     * Koordinát X cieľovej pozície.
     */
    public int direction_x;

    /**
     * Koordinát Y cieľovej pozície.
     */
    public int direction_y;

    /**
     * Limit pohybu. Ak je hodnota 0, tak pohyb nemá limit.
     */
    public int limit;

    /**
     * Konštruktor mobility.
     * 
     * \param type Typ pohybu.
     * \param start_x Koordinát X počiatočnej pozície.
     * \param start_y Koordinát Y počiatočnej pozície.
     * \param direction_x Koordinát X cieľovej pozície.
     * \param direction_y Koordinát Y cieľovej pozície.
     * \param limit Limit pohybu.
     */
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
