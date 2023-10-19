using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Turn
{
    public int turnCounter;
    public Chess.PieceType whitePieceType = Chess.PieceType.Pawn, whitePromotion = Chess.PieceType.Pawn;
    public Chess.MovementType whiteMovementType = Chess.MovementType.Move;
    public Move whiteMove = null;
    public Chess.PieceType blackPieceType = Chess.PieceType.Pawn, blackPromotion = Chess.PieceType.Pawn;
    public Chess.MovementType blackMovementType = Chess.MovementType.Move;
    public Move blackMove = null;

    public Turn(int turnCounter)
    {
        this.turnCounter = turnCounter;
    }

    public string GetWhitePly()
    {
        return GetPly(whitePieceType, whiteMovementType, whiteMove, whitePromotion);
    }

    public string GetBlackPly()
    {
        return GetPly(blackPieceType, blackMovementType, blackMove, blackPromotion);
    }

    public void SetWhitePly(Chess.PieceType whitePieceType, Chess.PieceType whitePromotion, Chess.MovementType whiteMovementType, Move whiteMove)
    {
        this.whitePieceType = whitePieceType;
        this.whitePromotion = whitePromotion;
        this.whiteMovementType = whiteMovementType;
        this.whiteMove = whiteMove;
    }

    public void SetBlackPly(Chess.PieceType blackPieceType, Chess.PieceType blackPromotion, Chess.MovementType blackMovementType, Move blackMove)
    {
        this.blackPieceType = blackPieceType;
        this.blackPromotion = blackPromotion;
        this.blackMovementType = blackMovementType;
        this.blackMove = blackMove;
    }

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
