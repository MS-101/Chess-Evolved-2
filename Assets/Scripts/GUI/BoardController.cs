using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BoardController : MonoBehaviour
{
    [SerializeField] private TMP_Text topColumn1, topColumn2, topColumn3, topColumn4, topColumn5, topColumn6, topColumn7, topColumn8;
    [SerializeField] private TMP_Text bottomColumn1, bottomColumn2, bottomColumn3, bottomColumn4, bottomColumn5, bottomColumn6, bottomColumn7, bottomColumn8;
    [SerializeField] private TMP_Text leftRow1, leftRow2, leftRow3, leftRow4, leftRow5, leftRow6, leftRow7, leftRow8;
    [SerializeField] private TMP_Text rightRow1, rightRow2, rightRow3, rightRow4, rightRow5, rightRow6, rightRow7, rightRow8;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject board;

    private bool reversed = false;
    private Chess.Color currentPly;
    Chess.GameSettings gameSettings;

    public UnityEvent<Movement, Chess.PieceType> onPlayerMove = new();
    public UnityEvent onPromotionRequested = new();

    public void InitBoard(bool reversed, Chess.GameSettings gameSettings)
    {
        this.gameSettings = gameSettings;

        SetBoardOrientation(reversed);
        ClearHighlightedSquare();
        ClearMovements();
        ClearGhost();
        CreateInitialPosition();
    }

    public void SetPly(Chess.Color currentPly)
    {
        this.currentPly = currentPly;
    }

    public void SetCheck()
    {
        foreach (PieceObject pieceObject in pieceObjects)
        {
            if (pieceObject.Piece.type == Chess.PieceType.King && pieceObject.Piece.color == currentPly)
            {
                pieceObject.HighlightColor = new(1, 0, 0, (float)0.4);
                pieceObject.Highlighted = true;
            }
        }
    }

    public void SetLegalMoves(List<Move> legalMoves)
    {
        foreach (PieceObject pieceObject in pieceObjects)
            pieceObject.Piece.availableMoves.Clear();

        foreach (Move legalMove in legalMoves)
        {
            PieceObject curPieceObject = GetPieceObject(legalMove.x1, legalMove.y1);
            PieceObject attackedPieceObject = GetPieceObject(legalMove.x2, legalMove.y2);

            Chess.MovementType movementType = Chess.MovementType.Move;
            if (attackedPieceObject != null)
                movementType = Chess.MovementType.Attack;
            else if (legalMove.vigilant && ghostObject != null)
            {
                Ghost ghost = ghostObject.Ghost;
                if (ghost.x == legalMove.x2 && ghost.y == legalMove.x2)
                    movementType = Chess.MovementType.Attack;
            }
            
            Piece curPiece = curPieceObject.Piece;
            curPiece.availableMoves.Add(new(curPiece, movementType, legalMove));
        }
    }

    public Piece GetPiece(int x, int y)
    {
        PieceObject pieceObject = GetPieceObject(x, y);
        if (pieceObject != null)
            return pieceObject.Piece;
        else
            return null;
    }

    public void PerformMove(Move legalMove)
    {
        MovePiece(legalMove);
    }

    public void PerformPromotion(Chess.PieceType promotedPieceType)
    {
        PieceObject promotedPieceObject = GetPieceObject(performedMovement.move.x2, performedMovement.move.y2);
        Piece promotedPiece = promotedPieceObject.Piece;

        if (promotedPiece.color == gameSettings.playerColor)
            onPlayerMove?.Invoke(performedMovement, promotedPieceType);

        promotedPiece.type = promotedPieceType;
        promotedPieceObject.Piece = promotedPiece;
    }

    private void SetBoardOrientation(bool reversed)
    {
        this.reversed = reversed;

        if (!reversed)
        {
            topColumn1.text = "A"; bottomColumn1.text = "A";
            topColumn2.text = "B"; bottomColumn2.text = "B";
            topColumn3.text = "C"; bottomColumn3.text = "C";
            topColumn4.text = "D"; bottomColumn4.text = "D";
            topColumn5.text = "E"; bottomColumn5.text = "E";
            topColumn6.text = "F"; bottomColumn6.text = "F";
            topColumn7.text = "G"; bottomColumn7.text = "G";
            topColumn8.text = "H"; bottomColumn8.text = "H";

            leftRow1.text = "1"; rightRow1.text = "1";
            leftRow2.text = "2"; rightRow2.text = "2";
            leftRow3.text = "3"; rightRow3.text = "3";
            leftRow4.text = "4"; rightRow4.text = "4";
            leftRow5.text = "5"; rightRow5.text = "5";
            leftRow6.text = "6"; rightRow6.text = "6";
            leftRow7.text = "7"; rightRow7.text = "7";
            leftRow8.text = "8"; rightRow8.text = "8";
        }
        else
        {
            topColumn1.text = "H"; bottomColumn1.text = "H";
            topColumn2.text = "G"; bottomColumn2.text = "G";
            topColumn3.text = "F"; bottomColumn3.text = "F";
            topColumn4.text = "E"; bottomColumn4.text = "E";
            topColumn5.text = "D"; bottomColumn5.text = "D";
            topColumn6.text = "C"; bottomColumn6.text = "C";
            topColumn7.text = "B"; bottomColumn7.text = "B";
            topColumn8.text = "A"; bottomColumn8.text = "A";

            leftRow1.text = "8"; rightRow1.text = "8";
            leftRow2.text = "7"; rightRow2.text = "7";
            leftRow3.text = "6"; rightRow3.text = "6";
            leftRow4.text = "5"; rightRow4.text = "5";
            leftRow5.text = "4"; rightRow5.text = "4";
            leftRow6.text = "3"; rightRow6.text = "3";
            leftRow7.text = "2"; rightRow7.text = "2";
            leftRow8.text = "1"; rightRow8.text = "1";
        }
    }

    private void CreateInitialPosition()
    {
        ClearPieces();

        Chess.Essence whitePawnEssence, whiteKnightEssence, whiteBishopEssence, whiteRookEssence;
        Chess.Essence blackPawnEssence, blackKnightEssence, blackBishopEssence, blackRookEssence;
        
        if (gameSettings.playerColor == Chess.Color.White)
        {
            whitePawnEssence = gameSettings.playerPawn;
            whiteKnightEssence = gameSettings.playerKnight;
            whiteBishopEssence = gameSettings.playerBishop;
            whiteRookEssence = gameSettings.playerRook;

            blackPawnEssence = gameSettings.malakhPawn;
            blackKnightEssence = gameSettings.malakhKnight;
            blackBishopEssence = gameSettings.malakhBishop;
            blackRookEssence = gameSettings.malakhRook;
        }
        else
        {
            whitePawnEssence = gameSettings.malakhPawn;
            whiteKnightEssence = gameSettings.malakhKnight;
            whiteBishopEssence = gameSettings.malakhBishop;
            whiteRookEssence = gameSettings.malakhRook;

            blackPawnEssence = gameSettings.playerPawn;
            blackKnightEssence = gameSettings.playerKnight;
            blackBishopEssence = gameSettings.playerBishop;
            blackRookEssence = gameSettings.playerRook;
        }

        AddPiece(new(Chess.PieceType.Rook, Chess.Color.White, whiteRookEssence), 0, 0);
        AddPiece(new(Chess.PieceType.Knight, Chess.Color.White, whiteKnightEssence), 1, 0);
        AddPiece(new(Chess.PieceType.Bishop, Chess.Color.White, whiteBishopEssence), 2, 0);
        AddPiece(new(Chess.PieceType.Queen, Chess.Color.White, Chess.Essence.Classic), 3, 0);
        AddPiece(new(Chess.PieceType.King, Chess.Color.White, Chess.Essence.Classic), 4, 0);
        AddPiece(new(Chess.PieceType.Bishop, Chess.Color.White, whiteBishopEssence), 5, 0);
        AddPiece(new(Chess.PieceType.Knight, Chess.Color.White, whiteKnightEssence), 6, 0);
        AddPiece(new(Chess.PieceType.Rook, Chess.Color.White, whiteRookEssence), 7, 0);

        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.White, whitePawnEssence), 0, 1);
        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.White, whitePawnEssence), 1, 1);
        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.White, whitePawnEssence), 2, 1);
        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.White, whitePawnEssence), 3, 1);
        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.White, whitePawnEssence), 4, 1);
        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.White, whitePawnEssence), 5, 1);
        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.White, whitePawnEssence), 6, 1);
        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.White, whitePawnEssence), 7, 1);

        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.Black, blackPawnEssence), 0, 6);
        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.Black, blackPawnEssence), 1, 6);
        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.Black, blackPawnEssence), 2, 6);
        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.Black, blackPawnEssence), 3, 6);
        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.Black, blackPawnEssence), 4, 6);
        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.Black, blackPawnEssence), 5, 6);
        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.Black, blackPawnEssence), 6, 6);
        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.Black, blackPawnEssence), 7, 6);

        AddPiece(new(Chess.PieceType.Rook, Chess.Color.Black, blackRookEssence), 0, 7);
        AddPiece(new(Chess.PieceType.Knight, Chess.Color.Black, blackKnightEssence), 1, 7);
        AddPiece(new(Chess.PieceType.Bishop, Chess.Color.Black, blackBishopEssence), 2, 7);
        AddPiece(new(Chess.PieceType.Queen, Chess.Color.Black, Chess.Essence.Classic), 3, 7);
        AddPiece(new(Chess.PieceType.King, Chess.Color.Black, Chess.Essence.Classic), 4, 7);
        AddPiece(new(Chess.PieceType.Bishop, Chess.Color.Black, blackBishopEssence), 5, 7);
        AddPiece(new(Chess.PieceType.Knight, Chess.Color.Black, blackKnightEssence), 6, 7);
        AddPiece(new(Chess.PieceType.Rook, Chess.Color.Black, blackRookEssence), 7, 7);
    }

    #region Pieces

    private List<PieceObject> pieceObjects = new();
    private GhostObject ghostObject = null;
    private GameObject highlightedSquare = null;

    private void ClearPieces()
    {
        while (pieceObjects.Count > 0)
        {
            Destroy(pieceObjects.Last().gameObject);
            pieceObjects.RemoveAt(pieceObjects.Count - 1);
        }
    }

    private void ClearGhost()
    {
        if (ghostObject != null)
        {
            Destroy(ghostObject.gameObject);
            ghostObject = null;
        }
    }

    private void SetGhost(Ghost ghost)
    {
        ClearGhost();

        ghostObject = Instantiate((GameObject)Resources.Load("Prefabs/Ghost"), board.transform).GetComponent<GhostObject>();
        ghostObject.Ghost = ghost;

        MoveGameObject(ghostObject.gameObject, ghost.x, ghost.y);
    }

    private void ClearHighlightedSquare()
    {
        if (highlightedSquare != null)
        {
            Destroy(highlightedSquare);
            highlightedSquare = null;
        }
    }

    private void SetHighlightedSquare(int x, int y)
    {
        ClearHighlightedSquare();

        highlightedSquare = Instantiate((GameObject)Resources.Load("Prefabs/HighlightedSquare"), board.transform);
        MoveGameObject(highlightedSquare, x, y);
    }

    private void AddPiece(Piece piece, int x, int y)
    {
        PieceObject pieceObject = Instantiate((GameObject)Resources.Load("Prefabs/Piece"), board.transform).GetComponent<PieceObject>();
        pieceObject.Piece = piece;
        pieceObjects.Add(pieceObject);

        MoveGameObject(pieceObject.gameObject, x, y);
        pieceObject.Piece.x = x;
        pieceObject.Piece.y = y;

        pieceObject.onPieceClicked.AddListener(OnPieceClicked);
    }

    private Movement MovePiece(Move move)
    {
        PieceObject movedPieceObject = GetPieceObject(move.x1, move.y1);
        Chess.MovementType movementType = Chess.MovementType.Move;

        PieceObject capturedPieceObject = GetPieceObject(move.x2, move.y2);
        if (capturedPieceObject != null)
        {
            movementType = Chess.MovementType.Attack;
            pieceObjects.Remove(capturedPieceObject);
            Destroy(capturedPieceObject.gameObject);
        }

        if (move.vigilant && ghostObject != null)
        {
            Ghost ghost = ghostObject.Ghost;
            if (ghost.x == move.x2 && ghost.y == move.y2)
            {
                movementType = Chess.MovementType.Attack;

                PieceObject ghostParentObject = GetPieceObject(ghost.parent.x, ghost.parent.y);
                pieceObjects.Remove(ghostParentObject);
                Destroy(ghostParentObject.gameObject);
            }
        }

        if (move.hasty)
            SetGhost(new(move.hastyX, move.hastyY, movedPieceObject.Piece));
        else
            ClearGhost();

        SetHighlightedSquare(movedPieceObject.Piece.x, movedPieceObject.Piece.y);

        MoveGameObject(movedPieceObject.gameObject, move.x2, move.y2);
        movedPieceObject.Piece.x = move.x2;
        movedPieceObject.Piece.y = move.y2;

        foreach (PieceObject pieceObject in pieceObjects)
            pieceObject.Highlighted = false;
        movedPieceObject.HighlightColor = new(1, 1, 0, (float)0.4);
        movedPieceObject.Highlighted = true;

        if (move.inspiring)
        {
            PieceObject inspiredPieceObject = GetPieceObject(move.inspiringX1, move.inspiringY1);
            if (inspiredPieceObject != null)
            {
                MoveGameObject(inspiredPieceObject.gameObject, move.inspiringX2, move.inspiringY2);
                inspiredPieceObject.Piece.x = move.inspiringX2;
                inspiredPieceObject.Piece.y = move.inspiringY2;
            }
        }

        audioSource.clip = (AudioClip)Resources.Load("Audio/PieceMove");
        audioSource.Play();

        return new(movedPieceObject.Piece, movementType, move);
    }

    private PieceObject GetPieceObject(int x, int y)
    {
        return pieceObjects.Find(target => (target.Piece.x == x && target.Piece.y == y));
    }

    private void OnPieceClicked(Piece piece)
    {
        if (currentPly == gameSettings.playerColor)
        {
            ClearMovements();

            foreach (PieceObject pieceObject in pieceObjects)
            {
                if (pieceObject.Piece != piece)
                    pieceObject.Selected = false;
                else
                {
                    if (piece.color == gameSettings.playerColor && !pieceObject.Selected)
                    {
                        pieceObject.Selected = true;
                        foreach (Movement movement in piece.availableMoves)
                            AddMovement(movement);
                    }
                    else
                        pieceObject.Selected = false;
                }
            }
        }
    }

    #endregion

    #region Movements

    private List<MovementObject> movementObjects = new();

    private void ClearMovements()
    {
        while (movementObjects.Count > 0)
        {
            Destroy(movementObjects.Last().gameObject);
            movementObjects.RemoveAt(movementObjects.Count - 1);
        }
    }

    private void AddMovement(Movement movement)
    {
        MovementObject movementObject = Instantiate((GameObject)Resources.Load("Prefabs/Movement"), board.transform).GetComponent<MovementObject>();
        movementObject.Movement = movement;
        movementObjects.Add(movementObject);

        MoveGameObject(movementObject.gameObject, movement.move.x2, movement.move.y2);

        movementObject.onClick.AddListener(OnMovementClicked);
    }

    Movement performedMovement = null;

    private void OnMovementClicked(Movement movement)
    {
        ClearMovements();
        foreach (PieceObject pieceObject in pieceObjects)
        {
            pieceObject.Selected = false;
            pieceObject.Piece.availableMoves.Clear();
        }

        Piece piece = movement.owner;

        performedMovement = MovePiece(movement.move);
        if (piece.type == Chess.PieceType.Pawn && ((piece.color == Chess.Color.White && piece.y == 7) || (piece.color == Chess.Color.Black && piece.y == 0)))
            onPromotionRequested?.Invoke();
        else
            onPlayerMove?.Invoke(performedMovement, Chess.PieceType.Pawn);
    }

    #endregion

    private void MoveGameObject(GameObject gameObject, int x, int y)
    {
        RectTransform parentRect = board.GetComponent<RectTransform>();

        float baseX = parentRect.position.x - parentRect.rect.width / 2;
        float baseY = parentRect.position.y - parentRect.rect.height / 2;
        float width = parentRect.rect.width / 8;
        float height = parentRect.rect.height / 8;

        RectTransform rect = gameObject.GetComponent<RectTransform>();

        if (!reversed)
            rect.position = new(baseX + x * width, baseY + y * height);
        else
            rect.position = new(baseX + (7 - x) * width, baseY + (7 - y) * height);
        rect.sizeDelta = new(width, height);
    }
}
