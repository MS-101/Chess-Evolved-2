/*****************************************************************//**
 * \file   GameCanvas.cs
 * \brief  Ovládač rozhrania hernej obrazovky.
 * 
 * \author Martin Šváb
 * \date   Máj 2024
 *********************************************************************/

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/**
 * Táto trieda je zodpovedná za správu hernej obrazovky.
 */
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

    /**
     * Inicializujeme toto rozhranie aby sa spustila hra s danou hernou konfiguráciou.
     * 
     * \param gameSettings Herná konfigurácia novej hry
     */
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

    /**
     * Pri prijatí legálnych pohybov od enginu prepošleme túto informáciu ovládaču šachovnice.
     */
    private void OnLegalMovesReceived(List<Move> legalMoves)
    {
        boardController.SetLegalMoves(legalMoves);
    }

    /**
     * Pri prijatí najlepšieho pohybu od enginu tento pohyb vykonáme na šachovnici a pohyb vložíme do histórie hry.
     * Následne prikážeme enginu aby daný pohyb vykonal a vyžiadame si legálne pohyby.
     * 
     * \param bestMove Najlepší pohyb podľa enginu.
     * \param promotedPieceType ak bola vykonaná promócia tak sa pešiak premení na tento typ figúrky.
     */
    private void OnBestMoveReceived(Move bestMove, Chess.PieceType promotedPieceType)
    {
        Piece movingPiece = boardController.GetPiece(bestMove.x1, bestMove.y1);
        Piece attackedPiece = boardController.GetPiece(bestMove.x2, bestMove.y2);

        Chess.MovementType movementType; 
        if (attackedPiece == null)
            movementType = Chess.MovementType.Move;
        else
            movementType = Chess.MovementType.Attack;

        if (gameSettings.malakhColor == Chess.Color.White)
            AddTurn(movingPiece.type, promotedPieceType, movementType, bestMove);
        else
            SetBlackPly(movingPiece.type, promotedPieceType, movementType, bestMove);

        boardController.PerformMove(bestMove);
        if (promotedPieceType != Chess.PieceType.Pawn)
            boardController.PerformPromotion(promotedPieceType);

        engineController.MovePiece(bestMove.x1, bestMove.y1, bestMove.x2, bestMove.y2, promotedPieceType);
        SetTurn(gameSettings.playerColor);
    }

    /**
     * Po prijatí pohybu hráča od ovládača šachovnice sa vykonaný pohyb zapíše do histórie hry.
     * Následne prikážeme enginu aby daný pohyb vykonal a vyžiadame si najlepší pohyb.
     * 
     * \param movement Vykonaný pohyb hráča.
     * \param promotedPieceType Ak bola vykonaná promócia pešiaka, tak toto bol typ figúrky na ktorú sa premenil.
     */
    private void OnPlayerMove(Movement movement, Chess.PieceType promotedPieceType)
    {
        if (gameSettings.playerColor == Chess.Color.White)
            AddTurn(movement.owner.type, promotedPieceType, movement.type, movement.move);
        else
            SetBlackPly(movement.owner.type, promotedPieceType, movement.type, movement.move);

        engineController.MovePiece(movement.move.x1, movement.move.y1, movement.move.x2, movement.move.y2, promotedPieceType);
        SetTurn(gameSettings.malakhColor);
    }

    /**
     * Pri prijatí výsledku hry od enginu hra je terminovaná a zobrazí sa výsledok hry.
     * 
     * \param victor Výherný hráč.
     */
    private void OnResultReceived(Chess.Color victor)
    {
        if (gameSettings.playerColor == victor)
            turnDisplay.text = "<b><color=\"red\">Player</color></b> wins!";
        else if (gameSettings.malakhColor == victor)
            turnDisplay.text = "<b><color=\"red\">Malakh</color></b> wins!";
        else if (victor == Chess.Color.Random)
            turnDisplay.text = "<b><color=\"red\">Stalemate</color></b>!";
    }

    /**
     * Pri prijatí informácie o napadnutí kráľa od enginu to nastavíme na šachovnici.
     */
    private void OnCheckReceived()
    {
        boardController.SetCheck();
    }

    /**
     * Pri prijatí žiadosti o výber promócie od ovládača šachovnice zobrazíme rozhranie na výber promócie.
     */
    private void OnPromotionRequested()
    {
        promotionController.gameObject.SetActive(true);
    }

    /**
     * Pri prijatí promócia od rozhrania promócie vykonáme promóciu na šachovnici.
     * 
     * \param pieceType Vybraná promócia pešiaka.
     */
    private void OnPromotionChosen(Chess.PieceType pieceType)
    {
        promotionController.gameObject.SetActive(false);
        boardController.PerformPromotion(pieceType);
    }

    /**
     * Nastavenie hráča na rade. Informáciu zobrazíme v nadpise.
     * Ak je na rade hráč, tak si vyžiadame dostupné pohyby.
     * Ak je na rade Malakh, tak si vyžiadame najlepší pohyb.
     * 
     * \param currentPly Hráč na rade.
     */
    private void SetTurn(Chess.Color currentPly)
    {
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

    /**
     * Pri kliknutí na tlačidlo nápovedy pri figúrke v bočnom paneli zobrazíme nápovedu pre danú figúrku.
     * 
     * \param pieceType Typ figúrky.
     * \param color Vlastník figúrky.
     * \param essence Esencia figúrky.
     */
    private void OnHelpButtonClicked(Chess.PieceType pieceType, Chess.Color color, Chess.Essence essence)
    {
        pieceInfoController.gameObject.SetActive(true);
        pieceInfoController.MyPiece = new(pieceType, color, essence);
    }

    #region TurnLogs

    [SerializeField] private GameObject turnListContent, turnObjectPrefab;
    [SerializeField] private Scrollbar turnScrollbar;

    private List<TurnObject> turnObjectList = new();
    private int turnCounter = 0;

    /**
     * Do histórie hry pridáme nový pohyb bieleho hráča.
     * 
     * \param whitePieceType Typ bielej figúrky.
     * \param whitePromotion Promócia bieleho pešiaka.
     * \param whiteMovementType Typ pohybu bielej figúrky.
     * \param whiteMove Vykonaný pohyb bielej figúrky.
     */
    public void AddTurn(Chess.PieceType whitePieceType, Chess.PieceType whitePromotion, Chess.MovementType whiteMovementType, Move whiteMove)
    {
        turnCounter++;

        GameObject newGameObject = Instantiate(turnObjectPrefab, turnListContent.transform);

        TurnObject newTurnObject = newGameObject.GetComponent<TurnObject>();
        Turn newTurn = new(turnCounter);
        newTurn.SetWhitePly(whitePieceType, whitePromotion, whiteMovementType, whiteMove);
        newTurnObject.MyTurn = newTurn;
        
        turnObjectList.Add(newTurnObject);

        Canvas.ForceUpdateCanvases();
        turnScrollbar.value = 0;
    }

    /**
     * Do histórie hry pridáme nová pohyb čierneho hráča.
     * 
     * \param blackPieceType Typ čiernej figúrky.
     * \param blackPromotion Promócia čierneho pešiaka.
     * \param blackMovementType Typ pohybu čiernej figúrky.
     * \param blackMove Vykonaný pohyb čiernej figúrky.
     */
    public void SetBlackPly(Chess.PieceType blackPieceType, Chess.PieceType blackPromotion, Chess.MovementType blackMovementType, Move blackMove)
    {
        turnObjectList.Last()?.UpdateBlackPly(blackPieceType, blackPromotion, blackMovementType, blackMove);
    }

    /**
     * Vymažeme históriu hry.
     */
    public void ClearTurns()
    {
        turnCounter = 0;

        while (turnObjectList.Count > 0)
        {
            TurnObject turnObject = turnObjectList[0];
            Destroy(turnObject.gameObject);
            turnObjectList.Remove(turnObject);
        }
    }

    /**
     * Pri kliknutí na tlačidlo exportu výsledok hry exportujeme do výstupného súboru.
     * Do súboru ukladáme informácie ako herná konfigurácia, výsledok hry a história hry.
     */
    public void OnExportClick()
    {
        string outputDir = "output";
        switch (gameSettings.ai) {
            case Chess.AI.Basic:
                outputDir += "/basic";
                break;
            case Chess.AI.Ensemble:
                outputDir += "/ensemble";
                break;
        }

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

    /**
     * Po stlačení tlačidla návratu sa vrátime do rozhrania hlavnej obrazovky.
     */
    private void OnReturnClick()
    {
        gameObject.SetActive(false);
        titleCanvas.gameObject.SetActive(true);
    }

    private GameObject createGameOverlay = null;


    /**
     * Po stlačení tlačidla novej hry zobrazíme rozhranie vytvorenia novej hry.
     */
    private void OnPlayClick()
    {
        createGameOverlay = Instantiate((GameObject)Resources.Load("Prefabs/CreateGameOverlay"), gameObject.transform);

        CreateGameOverlay createGameOverlayScript = createGameOverlay.GetComponent<CreateGameOverlay>();
        createGameOverlayScript.SetEngineController(engineController);
        createGameOverlayScript.onGameCreated.AddListener(OnGameCreated);
        if (gameSettings != null)
            createGameOverlayScript.InitializeSettings(gameSettings);
    }

    /**
     * Pri prijatí hernej konfigurácie od rozhrania vytvorenia novej hry nastavíme túto konfiguráciu v tomto rozhraní.
     * 
     * \param gameSettings Herná konfigurácia.
     */
    private void OnGameCreated(Chess.GameSettings gameSettings)
    {
        Destroy(createGameOverlay);
        SetGame(gameSettings);
    }

    #endregion
}
