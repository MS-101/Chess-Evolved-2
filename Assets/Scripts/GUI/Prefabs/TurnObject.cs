/*****************************************************************//**
 * \file   TurnObject.cs
 * \brief  Ovládač objektu herného ťahu.
 * 
 * \author Martin Šváb
 * \date   Máj 2024
 *********************************************************************/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * Táto trieda spravuje objekt herného ťahu.
 */
public class TurnObject : MonoBehaviour
{
    [SerializeField] private TMP_Text display;

    private Turn myTurn = null;

    /**
     * Herný ťah priradený tomuto objektu.
     * Ak ho zmeníme, tak sa aktualizuje zobrazený text.
     */
    public Turn MyTurn
    {
        get { return myTurn; }
        set {
            myTurn = value;
        
            UpdateDisplayedText();
        }
    }

    /**
     * Upravíme biely polťah herného ťahu.
     * 
     * \param pieceType Typ pohnutej figúrky.
     * \param piecePromotion Promócia pešiaka.
     * \param movementType Typ pohybu.
     * \param move Vykonaný pohyb.
     */
    public void UpdateWhitePly(Chess.PieceType pieceType, Chess.PieceType piecePromotion, Chess.MovementType movementType, Move move)
    {
        myTurn.SetWhitePly(pieceType, piecePromotion, movementType, move);
        UpdateDisplayedText();
    }

    /**
     * Upravíme čierny polťah herného ťahu.
     * 
     * \param pieceType Typ pohnutej figúrky.
     * \param piecePromotion Promócia pešiaka.
     * \param movementType Typ pohybu.
     * \param move Vykonaný pohyb.
     */
    public void UpdateBlackPly(Chess.PieceType pieceType, Chess.PieceType piecePromotion, Chess.MovementType movementType, Move move)
    {
        myTurn.SetBlackPly(pieceType, piecePromotion, movementType, move);
        UpdateDisplayedText();
    }

    /**
     * Aktualizujeme zobrazený text herného ťahu.
     * Herný ťah ma formát: @turnCounter. @whitePly @blackPly
     */
    private void UpdateDisplayedText()
    {
        display.text = myTurn.turnCounter.ToString() + ".  <color=\"white\">" + myTurn.GetWhitePly() + "</color> <color=\"black\">" + myTurn.GetBlackPly() + "</color>";
    }

}
