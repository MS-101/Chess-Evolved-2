using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
    [SerializeField] private Image playerPawn, playerKnight, playerBishop, playerRook, playerQueen, playerKing;
    [SerializeField] private Image malakhPawn, malakhKnight, malakhBishop, malakhRook, malakhQueen, malakhKing;
    [SerializeField] private GameObject board;
    [SerializeField] private Button returnBtn, playBtn;
    [SerializeField] private TitleCanvas titleCanvas;
    [SerializeField] private EngineController engineController;

    private GameObject createGameOverlay = null;

    private void Start()
    {
        returnBtn.onClick.AddListener(OnReturnClick);
        playBtn.onClick.AddListener(OnPlayClick);
    }

    private Chess.GameSettings gameSettings;

    public void SetGame(Chess.GameSettings gameSettings)
    {
        this.gameSettings = gameSettings;

        Chess.Color playerColor = gameSettings.playerColor;

        playerPawn.sprite = Chess.GetPieceImage(Chess.PieceType.Pawn, playerColor, gameSettings.playerPawn);
        playerKnight.sprite = Chess.GetPieceImage(Chess.PieceType.Knight, playerColor, gameSettings.playerKnight);
        playerBishop.sprite = Chess.GetPieceImage(Chess.PieceType.Bishop, playerColor, gameSettings.playerBishop);
        playerRook.sprite = Chess.GetPieceImage(Chess.PieceType.Rook, playerColor, gameSettings.playerRook);
        playerQueen.sprite = Chess.GetPieceImage(Chess.PieceType.Queen, playerColor, Chess.Essence.Classic);
        playerKing.sprite = Chess.GetPieceImage(Chess.PieceType.King, playerColor, Chess.Essence.Classic);

        Chess.Color malakhColor = gameSettings.malakhColor;

        malakhPawn.sprite = Chess.GetPieceImage(Chess.PieceType.Pawn, malakhColor, gameSettings.malakhPawn);
        malakhKnight.sprite = Chess.GetPieceImage(Chess.PieceType.Knight, malakhColor, gameSettings.malakhKnight);
        malakhBishop.sprite = Chess.GetPieceImage(Chess.PieceType.Bishop, malakhColor, gameSettings.malakhBishop);
        malakhRook.sprite = Chess.GetPieceImage(Chess.PieceType.Rook, malakhColor, gameSettings.malakhRook);
        malakhQueen.sprite = Chess.GetPieceImage(Chess.PieceType.Queen, malakhColor, Chess.Essence.Classic);
        malakhKing.sprite = Chess.GetPieceImage(Chess.PieceType.King, malakhColor, Chess.Essence.Classic);

        ClearPieces();

        if (playerColor == Chess.Color.White)
        {
            CreatePiece(Chess.PieceType.Rook, Chess.Color.White, gameSettings.playerRook, 0, 0);
            CreatePiece(Chess.PieceType.Knight, Chess.Color.White, gameSettings.playerKnight, 1, 0);
            CreatePiece(Chess.PieceType.Bishop, Chess.Color.White, gameSettings.playerBishop, 2, 0);
            CreatePiece(Chess.PieceType.Queen, Chess.Color.White, Chess.Essence.Classic, 3, 0);
            CreatePiece(Chess.PieceType.King, Chess.Color.White, Chess.Essence.Classic, 4, 0);
            CreatePiece(Chess.PieceType.Bishop, Chess.Color.White, gameSettings.playerBishop, 5, 0);
            CreatePiece(Chess.PieceType.Knight, Chess.Color.White, gameSettings.playerKnight, 6, 0);
            CreatePiece(Chess.PieceType.Rook, Chess.Color.White, gameSettings.playerRook, 7, 0);

            CreatePiece(Chess.PieceType.Pawn, Chess.Color.White, gameSettings.playerPawn, 0, 1);
            CreatePiece(Chess.PieceType.Pawn, Chess.Color.White, gameSettings.playerPawn, 1, 1);
            CreatePiece(Chess.PieceType.Pawn, Chess.Color.White, gameSettings.playerPawn, 2, 1);
            CreatePiece(Chess.PieceType.Pawn, Chess.Color.White, gameSettings.playerPawn, 3, 1);
            CreatePiece(Chess.PieceType.Pawn, Chess.Color.White, gameSettings.playerPawn, 4, 1);
            CreatePiece(Chess.PieceType.Pawn, Chess.Color.White, gameSettings.playerPawn, 5, 1);
            CreatePiece(Chess.PieceType.Pawn, Chess.Color.White, gameSettings.playerPawn, 6, 1);
            CreatePiece(Chess.PieceType.Pawn, Chess.Color.White, gameSettings.playerPawn, 7, 1);

            CreatePiece(Chess.PieceType.Pawn, Chess.Color.Black, gameSettings.malakhPawn, 0, 6);
            CreatePiece(Chess.PieceType.Pawn, Chess.Color.Black, gameSettings.malakhPawn, 1, 6);
            CreatePiece(Chess.PieceType.Pawn, Chess.Color.Black, gameSettings.malakhPawn, 2, 6);
            CreatePiece(Chess.PieceType.Pawn, Chess.Color.Black, gameSettings.malakhPawn, 3, 6);
            CreatePiece(Chess.PieceType.Pawn, Chess.Color.Black, gameSettings.malakhPawn, 4, 6);
            CreatePiece(Chess.PieceType.Pawn, Chess.Color.Black, gameSettings.malakhPawn, 5, 6);
            CreatePiece(Chess.PieceType.Pawn, Chess.Color.Black, gameSettings.malakhPawn, 6, 6);
            CreatePiece(Chess.PieceType.Pawn, Chess.Color.Black, gameSettings.malakhPawn, 7, 6);

            CreatePiece(Chess.PieceType.Rook, Chess.Color.Black, gameSettings.malakhRook, 0, 7);
            CreatePiece(Chess.PieceType.Knight, Chess.Color.Black, gameSettings.malakhKnight, 1, 7);
            CreatePiece(Chess.PieceType.Bishop, Chess.Color.Black, gameSettings.malakhBishop, 2, 7);
            CreatePiece(Chess.PieceType.Queen, Chess.Color.Black, Chess.Essence.Classic, 3, 7);
            CreatePiece(Chess.PieceType.King, Chess.Color.Black, Chess.Essence.Classic, 4, 7);
            CreatePiece(Chess.PieceType.Bishop, Chess.Color.Black, gameSettings.malakhBishop, 5, 7);
            CreatePiece(Chess.PieceType.Knight, Chess.Color.Black, gameSettings.malakhKnight, 6, 7);
            CreatePiece(Chess.PieceType.Rook, Chess.Color.Black, gameSettings.malakhRook, 7, 7);
        }
        else
        {
            CreatePiece(Chess.PieceType.Rook, Chess.Color.Black, gameSettings.playerRook, 0, 0);
            CreatePiece(Chess.PieceType.Knight, Chess.Color.Black, gameSettings.playerKnight, 1, 0);
            CreatePiece(Chess.PieceType.Bishop, Chess.Color.Black, gameSettings.playerBishop, 2, 0);
            CreatePiece(Chess.PieceType.King, Chess.Color.Black, Chess.Essence.Classic, 3, 0);
            CreatePiece(Chess.PieceType.Queen, Chess.Color.Black, Chess.Essence.Classic, 4, 0);
            CreatePiece(Chess.PieceType.Bishop, Chess.Color.Black, gameSettings.playerBishop, 5, 0);
            CreatePiece(Chess.PieceType.Knight, Chess.Color.Black, gameSettings.playerKnight, 6, 0);
            CreatePiece(Chess.PieceType.Rook, Chess.Color.Black, gameSettings.playerRook, 7, 0);

            CreatePiece(Chess.PieceType.Pawn, Chess.Color.Black, gameSettings.playerPawn, 0, 1);
            CreatePiece(Chess.PieceType.Pawn, Chess.Color.Black, gameSettings.playerPawn, 1, 1);
            CreatePiece(Chess.PieceType.Pawn, Chess.Color.Black, gameSettings.playerPawn, 2, 1);
            CreatePiece(Chess.PieceType.Pawn, Chess.Color.Black, gameSettings.playerPawn, 3, 1);
            CreatePiece(Chess.PieceType.Pawn, Chess.Color.Black, gameSettings.playerPawn, 4, 1);
            CreatePiece(Chess.PieceType.Pawn, Chess.Color.Black, gameSettings.playerPawn, 5, 1);
            CreatePiece(Chess.PieceType.Pawn, Chess.Color.Black, gameSettings.playerPawn, 6, 1);
            CreatePiece(Chess.PieceType.Pawn, Chess.Color.Black, gameSettings.playerPawn, 7, 1);

            CreatePiece(Chess.PieceType.Pawn, Chess.Color.White, gameSettings.malakhPawn, 0, 6);
            CreatePiece(Chess.PieceType.Pawn, Chess.Color.White, gameSettings.malakhPawn, 1, 6);
            CreatePiece(Chess.PieceType.Pawn, Chess.Color.White, gameSettings.malakhPawn, 2, 6);
            CreatePiece(Chess.PieceType.Pawn, Chess.Color.White, gameSettings.malakhPawn, 3, 6);
            CreatePiece(Chess.PieceType.Pawn, Chess.Color.White, gameSettings.malakhPawn, 4, 6);
            CreatePiece(Chess.PieceType.Pawn, Chess.Color.White, gameSettings.malakhPawn, 5, 6);
            CreatePiece(Chess.PieceType.Pawn, Chess.Color.White, gameSettings.malakhPawn, 6, 6);
            CreatePiece(Chess.PieceType.Pawn, Chess.Color.White, gameSettings.malakhPawn, 7, 6);

            CreatePiece(Chess.PieceType.Rook, Chess.Color.White, gameSettings.malakhRook, 0, 7);
            CreatePiece(Chess.PieceType.Knight, Chess.Color.White, gameSettings.malakhKnight, 1, 7);
            CreatePiece(Chess.PieceType.Bishop, Chess.Color.White, gameSettings.malakhBishop, 2, 7);
            CreatePiece(Chess.PieceType.King, Chess.Color.White, Chess.Essence.Classic, 3, 7);
            CreatePiece(Chess.PieceType.Queen, Chess.Color.White, Chess.Essence.Classic, 4, 7);
            CreatePiece(Chess.PieceType.Bishop, Chess.Color.White, gameSettings.malakhBishop, 5, 7);
            CreatePiece(Chess.PieceType.Knight, Chess.Color.White, gameSettings.malakhKnight, 6, 7);
            CreatePiece(Chess.PieceType.Rook, Chess.Color.White, gameSettings.malakhRook, 7, 7);
        }
    }

    private List<PieceObject> pieceObjects = new();

    private void CreatePiece(Chess.PieceType type, Chess.Color color, Chess.Essence essence, int x, int y)
    {
        PieceObject newPieceObject = Instantiate((GameObject)Resources.Load("Prefabs/Piece"), board.transform).GetComponent<PieceObject>();
        newPieceObject.Piece = new(type, color, essence);

        RectTransform parentRect = board.gameObject.GetComponent<RectTransform>();

        float baseX = parentRect.position.x - parentRect.rect.width / 2;
        float baseY = parentRect.position.y - parentRect.rect.height / 2;
        float width = parentRect.rect.width / 8;
        float height = parentRect.rect.height / 8;

        RectTransform rect = newPieceObject.gameObject.GetComponent<RectTransform>();
        rect.position = new(baseX + x * width, baseY + y * height);
        rect.sizeDelta = new(width, height);

        pieceObjects.Add(newPieceObject);
    }

    private void ClearPieces()
    {
        while (pieceObjects.Count > 0)
        {
            Destroy(pieceObjects.Last());
            pieceObjects.RemoveAt(pieceObjects.Count - 1);
        }
    }

    private void OnReturnClick()
    {
        gameObject.SetActive(false);
        titleCanvas.gameObject.SetActive(true);
    }

    private void OnPlayClick()
    {
        createGameOverlay = Instantiate((GameObject)Resources.Load("Prefabs/CreateGameOverlay"), gameObject.transform);

        CreateGameOverlay createGameOverlayScript = createGameOverlay.GetComponent<CreateGameOverlay>();
        createGameOverlayScript.SetEngineController(engineController);
        createGameOverlayScript.onGameCreated.AddListener(OnGameCreated);
        if (gameSettings != null)
            createGameOverlayScript.InitializeSettings(gameSettings);
    }

    private void OnGameCreated(Chess.GameSettings gameSettings)
    {
        Destroy(createGameOverlay);

        SetGame(gameSettings);
    }
}
