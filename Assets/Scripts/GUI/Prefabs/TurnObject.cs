using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnObject : MonoBehaviour
{
    [SerializeField] private TMP_Text display;

    private Turn myTurn = null;
    public Turn MyTurn
    {
        get { return myTurn; }
        set {
            myTurn = value;
        
            UpdateDisplayedText();
        }
    }

    public void UpdateWhitePly(Chess.PieceType pieceType, Chess.PieceType piecePromotion, Chess.MovementType movementType, Move move)
    {
        myTurn.SetWhitePly(pieceType, piecePromotion, movementType, move);
        UpdateDisplayedText();
    }

    public void UpdateBlackPly(Chess.PieceType pieceType, Chess.PieceType piecePromotion, Chess.MovementType movementType, Move move)
    {
        myTurn.SetBlackPly(pieceType, piecePromotion, movementType, move);
        UpdateDisplayedText();
    }

    private void UpdateDisplayedText()
    {
        display.text = myTurn.turnCounter.ToString() + ".  <color=\"white\">" + myTurn.GetWhitePly() + "</color> <color=\"black\">" + myTurn.GetBlackPly() + "</color>";
    }

}
