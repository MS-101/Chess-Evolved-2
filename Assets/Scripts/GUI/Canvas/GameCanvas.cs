using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
    [SerializeField] private BoardController boardController;
    [SerializeField] private PlayerInfoController playerInfoController, malakhInfoController;
    [SerializeField] private PieceInfoController pieceInfoController;
    [SerializeField] private PromotionController promotionController;
    [SerializeField] private EngineController engineController;
    [SerializeField] private TMP_Text turnDisplay;
    [SerializeField] private Button returnBtn, playBtn, exportBtn;
    [SerializeField] private TitleCanvas titleCanvas;

    private void Awake()
    {
        engineController.onLegalMovesReceived.AddListener(OnLegalMovesReceived);
        engineController.onBestMoveReceived.AddListener(OnBestMoveReceived);
        engineController.onResultReceived.AddListener(OnResultReceived);
        engineController.onCheckReceived.AddListener(OnCheckReceived);
    }

    private void Start()
    {
        returnBtn.onClick.AddListener(OnReturnClick);
        playBtn.onClick.AddListener(OnPlayClick);
        exportBtn.onClick.AddListener(OnExportClick);
        boardController.onPlayerMove.AddListener(OnPlayerMove);
        boardController.onPromotionRequested.AddListener(OnPromotionRequested);
        promotionController.onPromotionChosen.AddListener(OnPromotionChosen);
        playerInfoController.onHelpButtonClicked.AddListener(OnHelpButtonClicked);
        malakhInfoController.onHelpButtonClicked.AddListener(OnHelpButtonClicked);
    }

    private Chess.GameSettings gameSettings;

    public void SetGame(Chess.GameSettings gameSettings)
    {
        this.gameSettings = gameSettings;

        playerInfoController.SetPieces(gameSettings.playerColor, gameSettings.playerPawn, gameSettings.playerKnight, gameSettings.playerBishop, gameSettings.playerRook);
        malakhInfoController.SetPieces(gameSettings.malakhColor, gameSettings.malakhPawn, gameSettings.malakhKnight, gameSettings.malakhBishop, gameSettings.malakhRook);
        promotionController.SetPieces(gameSettings.playerRook, gameSettings.playerBishop, gameSettings.playerKnight);

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

    private void OnBestMoveReceived(Move bestMove, Chess.PieceType promotedPieceType)
    {
        Piece movingPiece = boardController.GetPiece(bestMove.x1, bestMove.y1);
        Piece attackedPiece = boardController.GetPiece(bestMove.x2, bestMove.y2);

        Chess.MovementType movementType; 
        if (attackedPiece == null)
            movementType = Chess.MovementType.Move;
        else
            movementType = Chess.MovementType.Attack;

        boardController.PerformMove(bestMove);
        if (promotedPieceType != Chess.PieceType.Pawn)
            boardController.PerformPromotion(promotedPieceType);

        if (gameSettings.malakhColor == Chess.Color.White)
            AddTurn(movingPiece.type, promotedPieceType, movementType, bestMove);
        else
            SetBlackPly(movingPiece.type, promotedPieceType, movementType, bestMove);

        engineController.MovePiece(bestMove.x1, bestMove.y1, bestMove.x2, bestMove.y2, promotedPieceType);
        SetTurn(gameSettings.playerColor);
    }

    private void OnPlayerMove(Movement movement, Chess.PieceType promotedPieceType)
    {
        if (gameSettings.playerColor == Chess.Color.White)
            AddTurn(movement.owner.type, promotedPieceType, movement.type, movement.move);
        else
            SetBlackPly(movement.owner.type, promotedPieceType, movement.type, movement.move);

        engineController.MovePiece(movement.move.x1, movement.move.y1, movement.move.x2, movement.move.y2, promotedPieceType);
        SetTurn(gameSettings.malakhColor);
    }

    private void OnResultReceived(Chess.Color victor)
    {
        if (gameSettings.playerColor == victor)
            turnDisplay.text = "<b><color=\"red\">Player</color></b> wins!";
        else if (gameSettings.malakhColor == victor)
            turnDisplay.text = "<b><color=\"red\">Malakh</color></b> wins!";
        else if (victor == Chess.Color.Random)
            turnDisplay.text = "<b><color=\"red\">Stalemate</color></b>!";
    }

    private void OnCheckReceived()
    {
        boardController.SetCheck();
    }

    private void OnPromotionRequested()
    {
        promotionController.gameObject.SetActive(true);
    }

    private void OnPromotionChosen(Chess.PieceType pieceType)
    {
        promotionController.gameObject.SetActive(false);
        boardController.PerformPromotion(pieceType);
    }

    Chess.Color currentPly;

    private void SetTurn(Chess.Color currentPly)
    {
        this.currentPly = currentPly;
        boardController.SetPly(currentPly);

        if (currentPly == gameSettings.playerColor)
        {
            turnDisplay.text = "<b><color=\"red\">Player</color></b>'s turn";
            engineController.RequestAvailableMoves();
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

    public void AddTurn(Chess.PieceType whitePieceType, Chess.PieceType whitePromotion, Chess.MovementType whiteMovementType, Move whiteMove)
    {
        turnCounter++;

        GameObject newGameObject = Instantiate(turnObjectPrefab, turnListContent.transform);

        TurnObject newTurnObject = newGameObject.GetComponent<TurnObject>();
        Turn newTurn = new(turnCounter);
        newTurn.SetWhitePly(whitePieceType, whitePromotion, whiteMovementType, whiteMove);
        newTurnObject.MyTurn = newTurn;

        turnObjectList.Add(newTurnObject);
    }

    public void SetBlackPly(Chess.PieceType blackPieceType, Chess.PieceType blackPromotion, Chess.MovementType blackMovementType, Move blackMove)
    {
        turnObjectList.Last()?.UpdateBlackPly(blackPieceType, blackPromotion, blackMovementType, blackMove);
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

    public void OnExportClick()
    {
        string outputDir = "output";
        string outputName = "game";
        string outputExtension = ".txt";
        
        string fullFilename;
        int fileCounter = 1;

        while (true)
        {
            fullFilename = outputDir + "/" + outputName + fileCounter.ToString() + outputExtension;

            if (File.Exists(fullFilename))
                fileCounter++;
            else
                break;
        }

        using (StreamWriter sw = new(fullFilename))
        {
            sw.WriteLine("Player color = " + gameSettings.playerColor);
            sw.WriteLine("Game result = " + turnDisplay.GetParsedText());
            sw.WriteLine();

            sw.WriteLine("Essence config:");
            sw.WriteLine("Player pawn = " + gameSettings.playerPawn.ToString());
            sw.WriteLine("Player rook = " + gameSettings.playerRook.ToString());
            sw.WriteLine("Player knight = " + gameSettings.playerKnight.ToString());
            sw.WriteLine("Player bishop = " + gameSettings.playerBishop.ToString());
            sw.WriteLine("Malakh pawn = " + gameSettings.malakhPawn.ToString());
            sw.WriteLine("Malakh rook = " + gameSettings.malakhRook.ToString());
            sw.WriteLine("Malakh knight = " + gameSettings.malakhKnight.ToString());
            sw.WriteLine("Malakh bishop = " + gameSettings.malakhBishop.ToString());
            sw.WriteLine();

            sw.WriteLine("Turns history:");
            foreach (TurnObject turnObject in turnObjectList)
            {
                Turn myTurn = turnObject.MyTurn;
                string turnString = myTurn.turnCounter.ToString() + ". " + myTurn.GetWhitePly() + " " + myTurn.GetBlackPly();
                sw.WriteLine(turnString);
            }
            sw.WriteLine();
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
