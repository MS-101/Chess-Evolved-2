using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

public class CreateGameOverlay : MonoBehaviour
{
    [SerializeField] private Toggle playerWhiteColorToggle, playerBlackColorToggle, playerRandomColorToggle;
    [SerializeField] private Toggle malakhWhiteColorToggle, malakhBlackColorToggle, malakhRandomColorToggle;
    [SerializeField] private ClickableDropdown playerPawnDropdown, playerKnightDropdown, playerBishopDropdown, playerRookDropdown;
    [SerializeField] private ClickableDropdown malakhPawnDropdown, malakhKnightDropdown, malakhBishopDropdown, malakhRookDropdown;
    [SerializeField] private Toggle playerRandomPiecesToggle, malakhRandomPiecesToggle;
    [SerializeField] private TMP_Dropdown pieceDropdown, essenceDropdown;
    [SerializeField] private Button returnBtn, playBtn;
    [SerializeField] private GameObject board;

    public UnityEvent<Chess.GameSettings> onGameCreated;

    private GameObject piecePrefab; 
    private GameObject movementPrefab;
    private EngineController engineController;

    private void Awake()
    {
        piecePrefab = (GameObject)Resources.Load("Prefabs/Piece");
        movementPrefab = (GameObject)Resources.Load("Prefabs/Movement");

        // Colors
        playerWhiteColorToggle.onValueChanged.AddListener((bool value) => { if (value) OnPlayerColorSet(Chess.Color.White); });
        playerBlackColorToggle.onValueChanged.AddListener((bool value) => { if (value) OnPlayerColorSet(Chess.Color.Black); });
        playerRandomColorToggle.onValueChanged.AddListener((bool value) => { if (value) OnPlayerColorSet(Chess.Color.Random); });
        malakhWhiteColorToggle.onValueChanged.AddListener((bool value) => { if (value) OnMalakhColorSet(Chess.Color.White); });
        malakhBlackColorToggle.onValueChanged.AddListener((bool value) => { if (value) OnMalakhColorSet(Chess.Color.Black); });
        malakhRandomColorToggle.onValueChanged.AddListener((bool value) => { if (value) OnMalakhColorSet(Chess.Color.Random); });

        // Pieces
        playerPawnDropdown.onClicked.AddListener(() => SetPieceInfo(Chess.PieceType.Pawn, (Chess.Essence)playerPawnDropdown.value));
        playerPawnDropdown.onValueChanged.AddListener((int value) => SetPieceInfo(Chess.PieceType.Pawn, (Chess.Essence)value));
        playerKnightDropdown.onClicked.AddListener(() => SetPieceInfo(Chess.PieceType.Knight, (Chess.Essence)playerKnightDropdown.value));
        playerKnightDropdown.onValueChanged.AddListener((int value) => SetPieceInfo(Chess.PieceType.Knight, (Chess.Essence)value));
        playerBishopDropdown.onClicked.AddListener(() => SetPieceInfo(Chess.PieceType.Bishop, (Chess.Essence)playerBishopDropdown.value));
        playerBishopDropdown.onValueChanged.AddListener((int value) => SetPieceInfo(Chess.PieceType.Bishop, (Chess.Essence)value));
        playerRookDropdown.onClicked.AddListener(() => SetPieceInfo(Chess.PieceType.Rook, (Chess.Essence)playerRookDropdown.value));
        playerRookDropdown.onValueChanged.AddListener((int value) => SetPieceInfo(Chess.PieceType.Rook, (Chess.Essence)value));

        malakhPawnDropdown.onClicked.AddListener(() => SetPieceInfo(Chess.PieceType.Pawn, (Chess.Essence)malakhPawnDropdown.value));
        malakhPawnDropdown.onValueChanged.AddListener((int value) => SetPieceInfo(Chess.PieceType.Pawn, (Chess.Essence)value));
        malakhKnightDropdown.onClicked.AddListener(() => SetPieceInfo(Chess.PieceType.Knight, (Chess.Essence)malakhKnightDropdown.value));
        malakhKnightDropdown.onValueChanged.AddListener((int value) => SetPieceInfo(Chess.PieceType.Knight, (Chess.Essence)value));
        malakhBishopDropdown.onClicked.AddListener(() => SetPieceInfo(Chess.PieceType.Bishop, (Chess.Essence)malakhBishopDropdown.value));
        malakhBishopDropdown.onValueChanged.AddListener((int value) => SetPieceInfo(Chess.PieceType.Bishop, (Chess.Essence)value));
        malakhRookDropdown.onClicked.AddListener(() => SetPieceInfo(Chess.PieceType.Rook, (Chess.Essence)malakhRookDropdown.value));
        malakhRookDropdown.onValueChanged.AddListener((int value) => SetPieceInfo(Chess.PieceType.Rook, (Chess.Essence)value));

        playerRandomPiecesToggle.onValueChanged.AddListener(OnPlayerRandomPiecesToggled);
        malakhRandomPiecesToggle.onValueChanged.AddListener(OnMalakhRandomPiecesToggled);

        // Buttons
        returnBtn.onClick.AddListener(OnReturnClick);
        playBtn.onClick.AddListener(OnPlayClick);

        // Piece Info
        pieceDropdown.onValueChanged.AddListener((int value) => SetPieceInfo((Chess.PieceType)value, (Chess.Essence)essenceDropdown.value));
        essenceDropdown.onValueChanged.AddListener((int value) => SetPieceInfo((Chess.PieceType)pieceDropdown.value, (Chess.Essence)value));

        pieceObject = Instantiate(piecePrefab, board.transform).GetComponent<PieceObject>();
        pieceObject.Piece = new(Chess.PieceType.Pawn, Chess.Color.White, Chess.Essence.Classic);

        RectTransform parentRect = board.gameObject.GetComponent<RectTransform>();

        float baseX = parentRect.position.x - parentRect.rect.width / 2;
        float baseY = parentRect.position.y;
        float width = parentRect.rect.width / 7;
        float height = width;

        RectTransform rect = pieceObject.gameObject.GetComponent<RectTransform>();
        rect.position = new(baseX + 3 * width, baseY + 3 * height);
        rect.sizeDelta = new(width, height);
    }

    public void InitializeSettings(Chess.GameSettings gameSettins)
    {
        OnPlayerColorSet(gameSettins.playerColor);

        playerPawnDropdown.SetValueWithoutNotify((int)gameSettins.playerPawn);
        playerKnightDropdown.SetValueWithoutNotify((int)gameSettins.playerKnight);
        playerBishopDropdown.SetValueWithoutNotify((int)gameSettins.playerBishop);
        playerRookDropdown.SetValueWithoutNotify((int)gameSettins.playerRook);

        malakhPawnDropdown.SetValueWithoutNotify((int)gameSettins.malakhPawn);
        malakhKnightDropdown.SetValueWithoutNotify((int)gameSettins.malakhKnight);
        malakhBishopDropdown.SetValueWithoutNotify((int)gameSettins.malakhBishop);
        malakhRookDropdown.SetValueWithoutNotify((int)gameSettins.malakhRook);
    }

    private void OnPlayerColorSet(Chess.Color color)
    {
        playerPawnDropdown.options[0].image = Chess.GetPieceImage(Chess.PieceType.Pawn, color, Chess.Essence.Classic);
        playerPawnDropdown.options[1].image = Chess.GetPieceImage(Chess.PieceType.Pawn, color, Chess.Essence.Red);
        playerPawnDropdown.options[2].image = Chess.GetPieceImage(Chess.PieceType.Pawn, color, Chess.Essence.Blue);
        playerPawnDropdown.RefreshShownValue();

        playerKnightDropdown.options[0].image = Chess.GetPieceImage(Chess.PieceType.Knight, color, Chess.Essence.Classic);
        playerKnightDropdown.options[1].image = Chess.GetPieceImage(Chess.PieceType.Knight, color, Chess.Essence.Red);
        playerKnightDropdown.options[2].image = Chess.GetPieceImage(Chess.PieceType.Knight, color, Chess.Essence.Blue);
        playerKnightDropdown.RefreshShownValue();

        playerBishopDropdown.options[0].image = Chess.GetPieceImage(Chess.PieceType.Bishop, color, Chess.Essence.Classic);
        playerBishopDropdown.options[1].image = Chess.GetPieceImage(Chess.PieceType.Bishop, color, Chess.Essence.Red);
        playerBishopDropdown.options[2].image = Chess.GetPieceImage(Chess.PieceType.Bishop, color, Chess.Essence.Blue);
        playerBishopDropdown.RefreshShownValue();

        playerRookDropdown.options[0].image = Chess.GetPieceImage(Chess.PieceType.Rook, color, Chess.Essence.Classic);
        playerRookDropdown.options[1].image = Chess.GetPieceImage(Chess.PieceType.Rook, color, Chess.Essence.Red);
        playerRookDropdown.options[2].image = Chess.GetPieceImage(Chess.PieceType.Rook, color, Chess.Essence.Blue);
        playerRookDropdown.RefreshShownValue();

        switch (color)
        {
            case Chess.Color.White:
                malakhBlackColorToggle.isOn = true;
                break;
            case Chess.Color.Black:
                malakhWhiteColorToggle.isOn = true;
                break;
            case Chess.Color.Random:
                malakhRandomColorToggle.isOn = true;
                break;
        }
    }

    private void OnMalakhColorSet(Chess.Color color)
    {
        malakhPawnDropdown.options[0].image = Chess.GetPieceImage(Chess.PieceType.Pawn, color, Chess.Essence.Classic);
        malakhPawnDropdown.options[1].image = Chess.GetPieceImage(Chess.PieceType.Pawn, color, Chess.Essence.Red);
        malakhPawnDropdown.options[2].image = Chess.GetPieceImage(Chess.PieceType.Pawn, color, Chess.Essence.Blue);
        malakhPawnDropdown.RefreshShownValue();

        malakhKnightDropdown.options[0].image = Chess.GetPieceImage(Chess.PieceType.Knight, color, Chess.Essence.Classic);
        malakhKnightDropdown.options[1].image = Chess.GetPieceImage(Chess.PieceType.Knight, color, Chess.Essence.Red);
        malakhKnightDropdown.options[2].image = Chess.GetPieceImage(Chess.PieceType.Knight, color, Chess.Essence.Blue);
        malakhKnightDropdown.RefreshShownValue();

        malakhBishopDropdown.options[0].image = Chess.GetPieceImage(Chess.PieceType.Bishop, color, Chess.Essence.Classic);
        malakhBishopDropdown.options[1].image = Chess.GetPieceImage(Chess.PieceType.Bishop, color, Chess.Essence.Red);
        malakhBishopDropdown.options[2].image = Chess.GetPieceImage(Chess.PieceType.Bishop, color, Chess.Essence.Blue);
        malakhBishopDropdown.RefreshShownValue();

        malakhRookDropdown.options[0].image = Chess.GetPieceImage(Chess.PieceType.Rook, color, Chess.Essence.Classic);
        malakhRookDropdown.options[1].image = Chess.GetPieceImage(Chess.PieceType.Rook, color, Chess.Essence.Red);
        malakhRookDropdown.options[2].image = Chess.GetPieceImage(Chess.PieceType.Rook, color, Chess.Essence.Blue);
        malakhRookDropdown.RefreshShownValue();

        switch (color)
        {
            case Chess.Color.White:
                playerBlackColorToggle.isOn = true;
                break;
            case Chess.Color.Black:
                playerWhiteColorToggle.isOn = true;
                break;
            case Chess.Color.Random:
                playerRandomColorToggle.isOn = true;
                break;
        }
    }

    private void OnPlayerRandomPiecesToggled(bool value)
    {
        playerPawnDropdown.interactable = !value;
        playerKnightDropdown.interactable = !value;
        playerBishopDropdown.interactable = !value;
        playerRookDropdown.interactable = !value;
    }

    private void OnMalakhRandomPiecesToggled(bool value)
    {
        malakhPawnDropdown.interactable = !value;
        malakhKnightDropdown.interactable = !value;
        malakhBishopDropdown.interactable = !value;
        malakhRookDropdown.interactable = !value;
    }

    private void OnReturnClick()
    {
        Destroy(gameObject);
    }

    private void OnPlayClick()
    {
        Random.InitState(System.DateTime.Now.Millisecond);

        Chess.Color playerColor;
        if (playerWhiteColorToggle.isOn)
            playerColor = Chess.Color.White;
        else if (playerBlackColorToggle.isOn)
            playerColor = Chess.Color.Black;
        else
            playerColor = (Chess.Color)Random.Range(0, 2);

        Chess.Color malakhColor = (playerColor == Chess.Color.White) ? Chess.Color.Black : Chess.Color.White;

        Chess.Essence playerPawn, playerKnight, playerBishop, playerRook;
        if (!playerRandomPiecesToggle.isOn)
        {
            playerPawn = (Chess.Essence)playerPawnDropdown.value;
            playerKnight = (Chess.Essence)playerKnightDropdown.value;
            playerBishop = (Chess.Essence)playerBishopDropdown.value;
            playerRook = (Chess.Essence)playerRookDropdown.value;
        }
        else
        {
            playerPawn = (Chess.Essence)Random.Range(1, 3);
            playerKnight = (Chess.Essence)Random.Range(1, 3);
            playerBishop = (Chess.Essence)Random.Range(1, 3);
            playerRook = (Chess.Essence)Random.Range(1, 3);
        }

        Chess.Essence malakhPawn, malakhKnight, malakhBishop, malakhRook;
        if (!malakhRandomPiecesToggle.isOn)
        {
            malakhPawn = (Chess.Essence)malakhPawnDropdown.value;
            malakhKnight = (Chess.Essence)malakhKnightDropdown.value;
            malakhBishop = (Chess.Essence)malakhBishopDropdown.value;
            malakhRook = (Chess.Essence)malakhRookDropdown.value;
        }
        else
        {
            malakhPawn = (Chess.Essence)Random.Range(1, 3);
            malakhKnight = (Chess.Essence)Random.Range(1, 3);
            malakhBishop = (Chess.Essence)Random.Range(1, 3);
            malakhRook = (Chess.Essence)Random.Range(1, 3);
        }

        onGameCreated?.Invoke(new(
            playerColor, playerPawn, playerKnight, playerBishop, playerRook,
            malakhColor, malakhPawn, malakhKnight, malakhBishop, malakhRook
        ));
    }

    private PieceObject pieceObject;
    private List<MovementObject> movements = new();

    private void SetPieceInfo(Chess.PieceType pieceType, Chess.Essence essence)
    {
        pieceDropdown.SetValueWithoutNotify((int)pieceType);
        essenceDropdown.SetValueWithoutNotify((int)essence);

        pieceDropdown.options[0].image = Chess.GetPieceImage(Chess.PieceType.Pawn, Chess.Color.Random, essence);
        pieceDropdown.options[1].image = Chess.GetPieceImage(Chess.PieceType.Knight, Chess.Color.Random, essence);
        pieceDropdown.options[2].image = Chess.GetPieceImage(Chess.PieceType.Bishop, Chess.Color.Random, essence);
        pieceDropdown.options[3].image = Chess.GetPieceImage(Chess.PieceType.Rook, Chess.Color.Random, essence);

        pieceDropdown.RefreshShownValue();

        pieceObject.Piece = new(pieceType, Chess.Color.White, essence);

        while(movements.Count > 0)
        {
            Destroy(movements.Last().gameObject);
            movements.RemoveAt(movements.Count - 1);
        }

        RectTransform parentRect = board.gameObject.GetComponent<RectTransform>();

        float baseX = parentRect.position.x - parentRect.rect.width / 2;
        float baseY = parentRect.position.y - parentRect.rect.height / 2;
        float width = parentRect.rect.width / 7;
        float height = parentRect.rect.height / 7;

        List<Mobility> mobilities = engineController.mobilities[pieceType][essence];
        /*
        if (pieceType == Chess.PieceType.Rook && essence == Chess.Essence.Classic)
        {
            mobilities.Add(new(Chess.MovementType.AttackMove, -1, +0, -1, +0, 0));
            mobilities.Add(new(Chess.MovementType.AttackMove, +0, +1, +0, +1, 0));
            mobilities.Add(new(Chess.MovementType.AttackMove, +1, +0, +1, +0, 0));
            mobilities.Add(new(Chess.MovementType.AttackMove, +0, -1, +0, -1, 0));
        }
        */

        foreach (Mobility mobility in mobilities)
        {
            int x = 3 + mobility.start_x;
            int y = 3 + mobility.start_y;
            int moveCounter = 0;

            while(x >= 0 && x <= 6 && y >= 0 && y <= 6 && (mobility.limit == 0 || moveCounter < mobility.limit))
            {
                MovementObject movementObject = Instantiate(movementPrefab, board.transform).GetComponent<MovementObject>();
                movementObject.Movement = new(pieceObject.Piece, mobility.type, x, y);

                RectTransform rect = movementObject.gameObject.GetComponent<RectTransform>();
                rect.position = new(baseX + x * width, baseY + y * height);
                rect.sizeDelta = new(width, height);

                movements.Add(movementObject);

                x += mobility.direction_x;
                y += mobility.direction_y;
                moveCounter++;
            }
        }
    }

    public void SetEngineController(EngineController engineController)
    {
        this.engineController = engineController;
        SetPieceInfo(Chess.PieceType.Pawn, Chess.Essence.Classic);
    }
}
