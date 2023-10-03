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
    [SerializeField] private GameObject board;

    private bool reversed = false;
    Chess.GameSettings gameSettings;

    public UnityEvent<Chess.PieceType, Chess.PieceType, Chess.MovementType, Move> onPlayerMove = new();

    public void InitBoard(bool reversed, Chess.GameSettings gameSettings)
    {
        this.gameSettings = gameSettings;

        SetBoardOrientation(reversed);
        CreateInitialPosition();
    }

    public void SetLegalMoves(List<Move> legalMoves)
    {
        foreach (PieceObject pieceObject in pieceObjects)
            pieceObject.Piece.availableMoves.Clear();

        foreach (Move legalMove in legalMoves)
        {
            PieceObject curPieceObject = GetPieceObject(legalMove.x1, legalMove.y1);
            PieceObject attackedPieceObject = GetPieceObject(legalMove.x2, legalMove.y2);

            Chess.MovementType movementType;
            if (attackedPieceObject == null)
                movementType = Chess.MovementType.Move;
            else
                movementType = Chess.MovementType.Attack;

            Piece curPiece = curPieceObject.Piece;
            curPiece.availableMoves.Add(new(curPiece, movementType, legalMove.x2, legalMove.y2));
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
        MovePiece(GetPieceObject(legalMove.x1, legalMove.y1), legalMove.x2, legalMove.y2);
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

        AddPiece(new(Chess.PieceType.Rook, Chess.Color.White, gameSettings.playerRook), 0, 0);
        AddPiece(new(Chess.PieceType.Knight, Chess.Color.White, gameSettings.playerKnight), 1, 0);
        AddPiece(new(Chess.PieceType.Bishop, Chess.Color.White, gameSettings.playerBishop), 2, 0);
        AddPiece(new(Chess.PieceType.Queen, Chess.Color.White, Chess.Essence.Classic), 3, 0);
        AddPiece(new(Chess.PieceType.King, Chess.Color.White, Chess.Essence.Classic), 4, 0);
        AddPiece(new(Chess.PieceType.Bishop, Chess.Color.White, gameSettings.playerBishop), 5, 0);
        AddPiece(new(Chess.PieceType.Knight, Chess.Color.White, gameSettings.playerKnight), 6, 0);
        AddPiece(new(Chess.PieceType.Rook, Chess.Color.White, gameSettings.playerRook), 7, 0);

        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.White, gameSettings.playerPawn), 0, 1);
        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.White, gameSettings.playerPawn), 1, 1);
        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.White, gameSettings.playerPawn), 2, 1);
        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.White, gameSettings.playerPawn), 3, 1);
        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.White, gameSettings.playerPawn), 4, 1);
        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.White, gameSettings.playerPawn), 5, 1);
        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.White, gameSettings.playerPawn), 6, 1);
        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.White, gameSettings.playerPawn), 7, 1);

        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.Black, gameSettings.malakhPawn), 0, 6);
        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.Black, gameSettings.malakhPawn), 1, 6);
        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.Black, gameSettings.malakhPawn), 2, 6);
        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.Black, gameSettings.malakhPawn), 3, 6);
        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.Black, gameSettings.malakhPawn), 4, 6);
        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.Black, gameSettings.malakhPawn), 5, 6);
        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.Black, gameSettings.malakhPawn), 6, 6);
        AddPiece(new(Chess.PieceType.Pawn, Chess.Color.Black, gameSettings.malakhPawn), 7, 6);

        AddPiece(new(Chess.PieceType.Rook, Chess.Color.Black, gameSettings.malakhRook), 0, 7);
        AddPiece(new(Chess.PieceType.Knight, Chess.Color.Black, gameSettings.malakhKnight), 1, 7);
        AddPiece(new(Chess.PieceType.Bishop, Chess.Color.Black, gameSettings.malakhBishop), 2, 7);
        AddPiece(new(Chess.PieceType.Queen, Chess.Color.Black, Chess.Essence.Classic), 3, 7);
        AddPiece(new(Chess.PieceType.King, Chess.Color.Black, Chess.Essence.Classic), 4, 7);
        AddPiece(new(Chess.PieceType.Bishop, Chess.Color.Black, gameSettings.malakhBishop), 5, 7);
        AddPiece(new(Chess.PieceType.Knight, Chess.Color.Black, gameSettings.malakhKnight), 6, 7);
        AddPiece(new(Chess.PieceType.Rook, Chess.Color.Black, gameSettings.malakhRook), 7, 7);
    }

    #region Pieces

    private List<PieceObject> pieceObjects = new();

    private void ClearPieces()
    {
        while (pieceObjects.Count > 0)
        {
            Destroy(pieceObjects.Last().gameObject);
            pieceObjects.RemoveAt(pieceObjects.Count - 1);
        }
    }

    private void AddPiece(Piece piece, int x, int y)
    {
        PieceObject pieceObject = Instantiate((GameObject)Resources.Load("Prefabs/Piece"), board.transform).GetComponent<PieceObject>();
        pieceObject.Piece = piece;
        pieceObjects.Add(pieceObject);

        MovePiece(pieceObject, x, y);

        pieceObject.onPieceClicked.AddListener(OnPieceClicked);
    }

    private void MovePiece(PieceObject pieceObject, int x, int y)
    {
        PieceObject capturedPiece = GetPieceObject(x, y);
        if (capturedPiece != null)
        {
            pieceObjects.Remove(pieceObject);
            Destroy(capturedPiece.gameObject);
        }

        MoveGameObject(pieceObject.gameObject, x, y);
        pieceObject.Piece.x = x;
        pieceObject.Piece.y = y;
    }

    private PieceObject GetPieceObject(int x, int y)
    {
        return pieceObjects.Find(target => (target.Piece.x == x && target.Piece.y == y));
    }

    private void OnPieceClicked(Piece piece)
    {
        ClearMovements();

        foreach (Movement movement in piece.availableMoves)
            AddMovement(movement);
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

        MoveGameObject(movementObject.gameObject, movement.x, movement.y);

        movementObject.onClick.AddListener(OnMovementClicked);
    }

    private void OnMovementClicked(Movement movement)
    {
        ClearMovements();
        foreach (PieceObject pieceObject in pieceObjects)
            pieceObject.Piece.availableMoves.Clear();

        Piece piece = movement.owner;
        
        int sourceX = piece.x;
        int sourceY = piece.y;
        int destinationX = movement.x;
        int destinationY = movement.y;

        PieceObject attackedPieceObject = GetPieceObject(destinationX, destinationY);
        Chess.PieceType attackedPieceType = Chess.PieceType.Pawn;
        if (attackedPieceObject != null)
            attackedPieceType = attackedPieceObject.Piece.type;

        MovePiece(GetPieceObject(sourceX, sourceY), destinationX, destinationY);

        onPlayerMove?.Invoke(piece.type, attackedPieceType, movement.type, new(sourceX, sourceY, destinationX, destinationY));
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
