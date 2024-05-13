/*****************************************************************//**
 * \file   Turn.cs
 * \brief  Herný ťah.
 * 
 * \author Martin Šváb
 * \date   Máj 2024
 *********************************************************************/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * Trieda reprezentujúca herný ťah.
 */
public class Turn
{
    /**
     * Poradie ťahu.
     */
    public int turnCounter;

    /**
     * Typ bielej figúrky (biely polťah).
     */
    public Chess.PieceType whitePieceType = Chess.PieceType.Pawn;

    /**
     * Typ bielej promócie (biely polťah).
     */
    public Chess.PieceType whitePromotion = Chess.PieceType.Pawn;

    /**
     * Typ bieleho pohybu (biely polťah).
     */
    public Chess.MovementType whiteMovementType = Chess.MovementType.Move;

    /**
     * Vykonaný pohyb bieleho hráča (biely polťah).
     */
    public Move whiteMove = null;

    /**
     * Typ čiernej figúrky (čierny polťah).
     */
    public Chess.PieceType blackPieceType = Chess.PieceType.Pawn;

    /**
     * Typ čiernej promócie (čierny polťah).
     */
    public Chess.PieceType blackPromotion = Chess.PieceType.Pawn;

    /**
     * Typ čierneho pohybu (čierny polťah).
     */
    public Chess.MovementType blackMovementType = Chess.MovementType.Move;

    /**
     * Vykonaný pohyb čierneho hráča (čierny polťah).
     */
    public Move blackMove = null;

    /**
     * Konštruktor herného ťahu.
     * 
     * \param turnCounter Poradie herného ťahu.
     */
    public Turn(int turnCounter)
    {
        this.turnCounter = turnCounter;
    }

    /**
     * Táto metóda vráti string reprezentujúci biely polťah.
     * 
     * \return Biely polťah v algebraickej notácii.
     **/
    public string GetWhitePly()
    {
        return GetPly(whitePieceType, whiteMovementType, whiteMove, whitePromotion);
    }

    /**
     * Táto metóda vráti string reprezentujúci čierny polťah.
     * 
     * \return Čierny polťah v algebraickej notácii.
     **/
    public string GetBlackPly()
    {
        return GetPly(blackPieceType, blackMovementType, blackMove, blackPromotion);
    }

    /**
     * Táto metóda nastaví biely polťah.
     * 
     * \param whitePieceType Typ presunutej figúrky.
     * \param whitePromotion Promócia pešiaka.
     * \param whiteMovementType Typ pohybu.
     * \param whiteMove Vykonaný pohyb
     **/
    public void SetWhitePly(Chess.PieceType whitePieceType, Chess.PieceType whitePromotion, Chess.MovementType whiteMovementType, Move whiteMove)
    {
        this.whitePieceType = whitePieceType;
        this.whitePromotion = whitePromotion;
        this.whiteMovementType = whiteMovementType;
        this.whiteMove = whiteMove;
    }

    /**
     * Táto metóda nastaví čierny polťah.
     * 
     * \param blackPieceType Typ presunutej figúrky.
     * \param blackPromotion Promócia pešiaka.
     * \param blackMovementType Typ pohybu.
     * \param blackMove Vykonaný pohyb
     **/
    public void SetBlackPly(Chess.PieceType blackPieceType, Chess.PieceType blackPromotion, Chess.MovementType blackMovementType, Move blackMove)
    {
        this.blackPieceType = blackPieceType;
        this.blackPromotion = blackPromotion;
        this.blackMovementType = blackMovementType;
        this.blackMove = blackMove;
    }

    /**
     * Táto metóda vytvorí algebraickú notáciu daného pohybu.
     * 
     * \param pieceType Typ presunutej figúrky.
     * \param movementType Typ vykoneného pohybu.
     * \param promotionType Typ vykonanej promócie pešiaka.
     */
    private string GetPly(Chess.PieceType pieceType, Chess.MovementType movementType, Move move, Chess.PieceType promotionType)
    {
        string ply = string.Empty;
        if (move == null)
            return ply;

        switch(pieceType)
        {
            case Chess.PieceType.Pawn:
                ply += "";
                break;
            case Chess.PieceType.Knight:
                ply += "N";
                break;
            case Chess.PieceType.Bishop:
                ply += "B";
                break;
            case Chess.PieceType.Rook:
                ply += "R";
                break;
            case Chess.PieceType.Queen:
                ply += "Q";
                break;
            case Chess.PieceType.King:
                ply += "K";
                break;
        }

        ply += (char)(move.x1 + 'a');
        ply += (char)(move.y1 + '1');

        if (movementType == Chess.MovementType.Attack)
            ply += "x";
        else
            ply += "-";

        ply += (char)(move.x2 + 'a');
        ply += (char)(move.y2 + '1');

        switch (promotionType)
        {
            case Chess.PieceType.Knight:
                ply += 'n';
                break;
            case Chess.PieceType.Bishop:
                ply += 'b';
                break;
            case Chess.PieceType.Rook:
                ply += 'r';
                break;
            case Chess.PieceType.Queen:
                ply += 'q';
                break;
        }

        return ply;
    }
}
