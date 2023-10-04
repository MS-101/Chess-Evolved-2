using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
    [SerializeField] private BoardController boardController;
    [SerializeField] private PlayerInfoController playerInfoController, malakhInfoController;
    [SerializeField] private PieceInfoController pieceInfoController;
    [SerializeField] private EngineController engineController;
    [SerializeField] private TMP_Text turnDisplay;
    [SerializeField] private Button returnBtn, playBtn;
    [SerializeField] private TitleCanvas titleCanvas;

    private void Awake()
    {
        engineController.onLegalMovesReceived.AddListener(OnLegalMovesReceived);
        engineController.onBestMoveReceived.AddListener(OnBestMoveReceived);
    }

    private void Start()
    {
        returnBtn.onClick.AddListener(OnReturnClick);
        playBtn.onClick.AddListener(OnPlayClick);
        boardController.onPlayerMove.AddListener(OnPlayerMove);
        playerInfoController.onHelpButtonClicked.AddListener(OnHelpButtonClicked);
        malakhInfoController.onHelpButtonClicked.AddListener(OnHelpButtonClicked);
    }

    private Chess.GameSettings gameSettings;

    public void SetGame(Chess.GameSettings gameSettings)
    {
        this.gameSettings = gameSettings;

        playerInfoController.SetPieces(gameSettings.playerColor, gameSettings.playerPawn, gameSettings.playerKnight, gameSettings.playerBishop, gameSettings.playerRook);
        malakhInfoController.SetPieces(gameSettings.malakhColor, gameSettings.malakhPawn, gameSettings.malakhKnight, gameSettings.malakhBishop, gameSettings.malakhRook);

        ClearTurns();

        if (gameSettings.playerColor == Chess.Color.White)
            boardController.InitBoard(false, gameSettings);
        else
            boardController.InitBoard(true, gameSettings);

        engineController.StartGame(gameSettings);
        SetTurn(Chess.Color.White);
    }

    private void OnLegalMovesReceived(List<Move> legalMoves)
    {
        boardController.SetLegalMoves(legalMoves);
    }

    private void OnBestMoveReceived(Move bestMove)
    {
        Piece movingPiece = boardController.GetPiece(bestMove.x1, bestMove.y1);
        Piece attackedPiece = boardController.GetPiece(bestMove.x2, bestMove.y2);

        Chess.MovementType movementType; 
        if (attackedPiece == null)
            movementType = Chess.MovementType.Move;
        else
            movementType = Chess.MovementType.Attack;

        boardController.PerformMove(bestMove);

        if (gameSettings.malakhColor == Chess.Color.White)
            AddTurn(movingPiece.type, movementType, bestMove);
        else
            SetBlackPly(movingPiece.type, movementType, bestMove);

        if (movementType == Chess.MovementType.Attack)
            playerInfoController.DecPieceCounter(attackedPiece.type);

        engineController.MovePiece(bestMove.x1, bestMove.y1, bestMove.x2, bestMove.y2);
        SetTurn(gameSettings.playerColor);
    }

    private void OnPlayerMove(Chess.PieceType pieceType, Chess.PieceType attackedPieceType, Chess.MovementType movementType, Move move)
    {
        if (gameSettings.playerColor == Chess.Color.White)
            AddTurn(pieceType, movementType, move);
        else
            SetBlackPly(pieceType, movementType, move);

        if (movementType == Chess.MovementType.Attack)
            malakhInfoController.DecPieceCounter(attackedPieceType);

        engineController.MovePiece(move.x1, move.y1, move.x2, move.y2);
        SetTurn(gameSettings.malakhColor);
    }

    private void SetTurn(Chess.Color currentPly)
    {
        if (currentPly == gameSettings.playerColor)
        {
            turnDisplay.text = "<b><color=\"red\">Player</color></b>'s turn";
            engineController.RequestAvailableMoves(gameSettings.playerColor);
        }
        else
        {
            turnDisplay.text = "<b><color=\"red\">Malakh</color></b>'s turn";
            engineController.RequestBestMove();
        }
    }

    private void OnHelpButtonClicked(Chess.PieceType pieceType, Chess.Color color, Chess.Essence essence)
    {
        pieceInfoController.gameObject.SetActive(true);
        pieceInfoController.MyPiece = new(pieceType, color, essence);
    }

    #region TurnLogs

    [SerializeField] private GameObject turnListContent, turnObjectPrefab;

    private List<TurnObject> turnObjectList = new();
    private int turnCounter = 0;

    public void AddTurn(Chess.PieceType whitePieceType, Chess.MovementType whiteMovementType, Move whiteMove)
    {
        turnCounter++;

        GameObject newGameObject = Instantiate(turnObjectPrefab, turnListContent.transform);

        TurnObject newTurnObject = newGameObject.GetComponent<TurnObject>();
        Turn newTurn = new(turnCounter);
        newTurn.SetWhitePly(whitePieceType, whiteMovementType, whiteMove);
        newTurnObject.MyTurn = newTurn;

        turnObjectList.Add(newTurnObject);
    }

    public void SetBlackPly(Chess.PieceType blackPieceType, Chess.MovementType blackMovementType, Move blackMove)
    {
        turnObjectList.Last()?.UpdateBlackPly(blackPieceType, blackMovementType, blackMove);
    }

    public void ClearTurns()
    {
        while (turnObjectList.Count > 0)
        {
            TurnObject turnObject = turnObjectList[0];
            Destroy(turnObject.gameObject);
            turnObjectList.Remove(turnObject);
        }
    }

    #endregion

    #region Buttons

    private void OnReturnClick()
    {
        gameObject.SetActive(false);
        titleCanvas.gameObject.SetActive(true);
    }

    private GameObject createGameOverlay = null;

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

    #endregion
}
