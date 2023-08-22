using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CreateGameOverlay : MonoBehaviour
{
    [SerializeField] private Toggle playerWhiteColorToggle, playerBlackColorToggle, playerRandomColorToggle;
    [SerializeField] private Toggle malakhWhiteColorToggle, malakhBlackColorToggle, malakhRandomColorToggle;
    [SerializeField] private ClickableDropdown playerPawnDropdown, playerKnightDropdown, playerBishopDropdown, playerRookDropdown;
    [SerializeField] private ClickableDropdown malakhPawnDropdown, malakhKnightDropdown, malakhBishopDropdown, malakhRookDropdown;
    [SerializeField] private Toggle playerRandomPiecesToggle, malakhRandomPiecesToggle;
    [SerializeField] private TMP_Dropdown pieceDropdown, essenceDropdown;
    [SerializeField] private Button returnBtn, playBtn;

    public UnityEvent<Chess.GameSettings> onGameCreated;

    private void Start()
    {
        // Colors
        playerWhiteColorToggle.onValueChanged.AddListener(OnPlayerWhiteColorToggled);
        playerBlackColorToggle.onValueChanged.AddListener(OnPlayerBlackColorToggled);
        playerRandomColorToggle.onValueChanged.AddListener(OnPlayerRandomColorToggled);
        malakhWhiteColorToggle.onValueChanged.AddListener(OnMalakhWhiteColorToggled);
        malakhBlackColorToggle.onValueChanged.AddListener(OnMalakhBlackColorToggled);
        malakhRandomColorToggle.onValueChanged.AddListener(OnMalakhRandomColorToggled);

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
        pieceDropdown.onValueChanged.AddListener(OnPieceChanged);
        essenceDropdown.onValueChanged.AddListener(OnEssenceChanged);
    }

    #region Colors

    private void OnPlayerWhiteColorToggled(bool value)
    {
        if (value)
        {
            playerPawnDropdown.options[0].image = Resources.Load<Sprite>("Pieces/White/Classic/Pawn_White_Classic");
            playerPawnDropdown.options[1].image = Resources.Load<Sprite>("Pieces/White/Red/Pawn_White_Red");
            playerPawnDropdown.options[2].image = Resources.Load<Sprite>("Pieces/White/Blue/Pawn_White_Blue");
            playerPawnDropdown.RefreshShownValue();

            playerKnightDropdown.options[0].image = Resources.Load<Sprite>("Pieces/White/Classic/Knight_White_Classic");
            playerKnightDropdown.options[1].image = Resources.Load<Sprite>("Pieces/White/Red/Knight_White_Red");
            playerKnightDropdown.options[2].image = Resources.Load<Sprite>("Pieces/White/Blue/Knight_White_Blue");
            playerKnightDropdown.RefreshShownValue();

            playerBishopDropdown.options[0].image = Resources.Load<Sprite>("Pieces/White/Classic/Bishop_White_Classic");
            playerBishopDropdown.options[1].image = Resources.Load<Sprite>("Pieces/White/Red/Bishop_White_Red");
            playerBishopDropdown.options[2].image = Resources.Load<Sprite>("Pieces/White/Blue/Bishop_White_Blue");
            playerBishopDropdown.RefreshShownValue();

            playerRookDropdown.options[0].image = Resources.Load<Sprite>("Pieces/White/Classic/Rook_White_Classic");
            playerRookDropdown.options[1].image = Resources.Load<Sprite>("Pieces/White/Red/Rook_White_Red");
            playerRookDropdown.options[2].image = Resources.Load<Sprite>("Pieces/White/Blue/Rook_White_Blue");
            playerRookDropdown.RefreshShownValue();

            malakhBlackColorToggle.isOn = true;
        }
    }

    private void OnPlayerBlackColorToggled(bool value)
    {
        if (value)
        {
            playerPawnDropdown.options[0].image = Resources.Load<Sprite>("Pieces/Black/Classic/Pawn_Black_Classic");
            playerPawnDropdown.options[1].image = Resources.Load<Sprite>("Pieces/Black/Red/Pawn_Black_Red");
            playerPawnDropdown.options[2].image = Resources.Load<Sprite>("Pieces/Black/Blue/Pawn_Black_Blue");
            playerPawnDropdown.RefreshShownValue();

            playerKnightDropdown.options[0].image = Resources.Load<Sprite>("Pieces/Black/Classic/Knight_Black_Classic");
            playerKnightDropdown.options[1].image = Resources.Load<Sprite>("Pieces/Black/Red/Knight_Black_Red");
            playerKnightDropdown.options[2].image = Resources.Load<Sprite>("Pieces/Black/Blue/Knight_Black_Blue");
            playerKnightDropdown.RefreshShownValue();

            playerBishopDropdown.options[0].image = Resources.Load<Sprite>("Pieces/Black/Classic/Bishop_Black_Classic");
            playerBishopDropdown.options[1].image = Resources.Load<Sprite>("Pieces/Black/Red/Bishop_Black_Red");
            playerBishopDropdown.options[2].image = Resources.Load<Sprite>("Pieces/Black/Blue/Bishop_Black_Blue");
            playerBishopDropdown.RefreshShownValue();

            playerRookDropdown.options[0].image = Resources.Load<Sprite>("Pieces/Black/Classic/Rook_Black_Classic");
            playerRookDropdown.options[1].image = Resources.Load<Sprite>("Pieces/Black/Red/Rook_Black_Red");
            playerRookDropdown.options[2].image = Resources.Load<Sprite>("Pieces/Black/Blue/Rook_Black_Blue");
            playerRookDropdown.RefreshShownValue();

            malakhWhiteColorToggle.isOn = true;
        }
    }

    private void OnPlayerRandomColorToggled(bool value)
    {
        if (value)
        {
            playerPawnDropdown.options[0].image = Resources.Load<Sprite>("Pieces/Random/Classic/Pawn_Random_Classic");
            playerPawnDropdown.options[1].image = Resources.Load<Sprite>("Pieces/Random/Red/Pawn_Random_Red");
            playerPawnDropdown.options[2].image = Resources.Load<Sprite>("Pieces/Random/Blue/Pawn_Random_Blue");
            playerPawnDropdown.RefreshShownValue();

            playerKnightDropdown.options[0].image = Resources.Load<Sprite>("Pieces/Random/Classic/Knight_Random_Classic");
            playerKnightDropdown.options[1].image = Resources.Load<Sprite>("Pieces/Random/Red/Knight_Random_Red");
            playerKnightDropdown.options[2].image = Resources.Load<Sprite>("Pieces/Random/Blue/Knight_Random_Blue");
            playerKnightDropdown.RefreshShownValue();

            playerBishopDropdown.options[0].image = Resources.Load<Sprite>("Pieces/Random/Classic/Bishop_Random_Classic");
            playerBishopDropdown.options[1].image = Resources.Load<Sprite>("Pieces/Random/Red/Bishop_Random_Red");
            playerBishopDropdown.options[2].image = Resources.Load<Sprite>("Pieces/Random/Blue/Bishop_Random_Blue");
            playerBishopDropdown.RefreshShownValue();

            playerRookDropdown.options[0].image = Resources.Load<Sprite>("Pieces/Random/Classic/Rook_Random_Classic");
            playerRookDropdown.options[1].image = Resources.Load<Sprite>("Pieces/Random/Red/Rook_Random_Red");
            playerRookDropdown.options[2].image = Resources.Load<Sprite>("Pieces/Random/Blue/Rook_Random_Blue");
            playerRookDropdown.RefreshShownValue();

            malakhRandomColorToggle.isOn = true;
        }
    }

    private void OnMalakhWhiteColorToggled(bool value)
    {
        if (value)
        {
            malakhPawnDropdown.options[0].image = Resources.Load<Sprite>("Pieces/White/Classic/Pawn_White_Classic");
            malakhPawnDropdown.options[1].image = Resources.Load<Sprite>("Pieces/White/Red/Pawn_White_Red");
            malakhPawnDropdown.options[2].image = Resources.Load<Sprite>("Pieces/White/Blue/Pawn_White_Blue");
            malakhPawnDropdown.RefreshShownValue();

            malakhKnightDropdown.options[0].image = Resources.Load<Sprite>("Pieces/White/Classic/Knight_White_Classic");
            malakhKnightDropdown.options[1].image = Resources.Load<Sprite>("Pieces/White/Red/Knight_White_Red");
            malakhKnightDropdown.options[2].image = Resources.Load<Sprite>("Pieces/White/Blue/Knight_White_Blue");
            malakhKnightDropdown.RefreshShownValue();

            malakhBishopDropdown.options[0].image = Resources.Load<Sprite>("Pieces/White/Classic/Bishop_White_Classic");
            malakhBishopDropdown.options[1].image = Resources.Load<Sprite>("Pieces/White/Red/Bishop_White_Red");
            malakhBishopDropdown.options[2].image = Resources.Load<Sprite>("Pieces/White/Blue/Bishop_White_Blue");
            malakhBishopDropdown.RefreshShownValue();

            malakhRookDropdown.options[0].image = Resources.Load<Sprite>("Pieces/White/Classic/Rook_White_Classic");
            malakhRookDropdown.options[1].image = Resources.Load<Sprite>("Pieces/White/Red/Rook_White_Red");
            malakhRookDropdown.options[2].image = Resources.Load<Sprite>("Pieces/White/Blue/Rook_White_Blue");
            malakhRookDropdown.RefreshShownValue();

            playerBlackColorToggle.isOn = true;
        }
            
    }

    private void OnMalakhBlackColorToggled(bool value)
    {
        if (value)
        {
            malakhPawnDropdown.options[0].image = Resources.Load<Sprite>("Pieces/Black/Classic/Pawn_Black_Classic");
            malakhPawnDropdown.options[1].image = Resources.Load<Sprite>("Pieces/Black/Red/Pawn_Black_Red");
            malakhPawnDropdown.options[2].image = Resources.Load<Sprite>("Pieces/Black/Blue/Pawn_Black_Blue");
            malakhPawnDropdown.RefreshShownValue();

            malakhKnightDropdown.options[0].image = Resources.Load<Sprite>("Pieces/Black/Classic/Knight_Black_Classic");
            malakhKnightDropdown.options[1].image = Resources.Load<Sprite>("Pieces/Black/Red/Knight_Black_Red");
            malakhKnightDropdown.options[2].image = Resources.Load<Sprite>("Pieces/Black/Blue/Knight_Black_Blue");
            malakhKnightDropdown.RefreshShownValue();

            malakhBishopDropdown.options[0].image = Resources.Load<Sprite>("Pieces/Black/Classic/Bishop_Black_Classic");
            malakhBishopDropdown.options[1].image = Resources.Load<Sprite>("Pieces/Black/Red/Bishop_Black_Red");
            malakhBishopDropdown.options[2].image = Resources.Load<Sprite>("Pieces/Black/Blue/Bishop_Black_Blue");
            malakhBishopDropdown.RefreshShownValue();

            malakhRookDropdown.options[0].image = Resources.Load<Sprite>("Pieces/Black/Classic/Rook_Black_Classic");
            malakhRookDropdown.options[1].image = Resources.Load<Sprite>("Pieces/Black/Red/Rook_Black_Red");
            malakhRookDropdown.options[2].image = Resources.Load<Sprite>("Pieces/Black/Blue/Rook_Black_Blue");
            malakhRookDropdown.RefreshShownValue();

            playerWhiteColorToggle.isOn = true;
        }
    }

    private void OnMalakhRandomColorToggled(bool value)
    {
        if (value)
        {
            malakhPawnDropdown.options[0].image = Resources.Load<Sprite>("Pieces/Random/Classic/Pawn_Random_Classic");
            malakhPawnDropdown.options[1].image = Resources.Load<Sprite>("Pieces/Random/Red/Pawn_Random_Red");
            malakhPawnDropdown.options[2].image = Resources.Load<Sprite>("Pieces/Random/Blue/Pawn_Random_Blue");
            malakhPawnDropdown.RefreshShownValue();

            malakhKnightDropdown.options[0].image = Resources.Load<Sprite>("Pieces/Random/Classic/Knight_Random_Classic");
            malakhKnightDropdown.options[1].image = Resources.Load<Sprite>("Pieces/Random/Red/Knight_Random_Red");
            malakhKnightDropdown.options[2].image = Resources.Load<Sprite>("Pieces/Random/Blue/Knight_Random_Blue");
            malakhKnightDropdown.RefreshShownValue();

            malakhBishopDropdown.options[0].image = Resources.Load<Sprite>("Pieces/Random/Classic/Bishop_Random_Classic");
            malakhBishopDropdown.options[1].image = Resources.Load<Sprite>("Pieces/Random/Red/Bishop_Random_Red");
            malakhBishopDropdown.options[2].image = Resources.Load<Sprite>("Pieces/Random/Blue/Bishop_Random_Blue");
            malakhBishopDropdown.RefreshShownValue();

            malakhRookDropdown.options[0].image = Resources.Load<Sprite>("Pieces/Random/Classic/Rook_Random_Classic");
            malakhRookDropdown.options[1].image = Resources.Load<Sprite>("Pieces/Random/Red/Rook_Random_Red");
            malakhRookDropdown.options[2].image = Resources.Load<Sprite>("Pieces/Random/Blue/Rook_Random_Blue");
            malakhRookDropdown.RefreshShownValue();

            playerRandomColorToggle.isOn = true;
        }
    }

    #endregion

    #region Pieces

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

    #endregion

    #region Buttons

    private void OnReturnClick()
    {
        gameObject.SetActive(false);
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
            malakhPawn, malakhKnight, malakhBishop, malakhRook
        ));
    }

    #endregion

    #region Piece Info

    private void SetPieceInfo(Chess.PieceType pieceType, Chess.Essence essence)
    {
        pieceDropdown.SetValueWithoutNotify((int)pieceType);
        essenceDropdown.SetValueWithoutNotify((int)essence);

        switch (essence)
        {
            case Chess.Essence.Classic:
                pieceDropdown.options[0].image = Resources.Load<Sprite>("Pieces/Random/Classic/Pawn_Random_Classic");
                pieceDropdown.options[1].image = Resources.Load<Sprite>("Pieces/Random/Classic/Knight_Random_Classic");
                pieceDropdown.options[2].image = Resources.Load<Sprite>("Pieces/Random/Classic/Bishop_Random_Classic");
                pieceDropdown.options[3].image = Resources.Load<Sprite>("Pieces/Random/Classic/Rook_Random_Classic");
                break;
            case Chess.Essence.Red:
                pieceDropdown.options[0].image = Resources.Load<Sprite>("Pieces/Random/Red/Pawn_Random_Red");
                pieceDropdown.options[1].image = Resources.Load<Sprite>("Pieces/Random/Red/Knight_Random_Red");
                pieceDropdown.options[2].image = Resources.Load<Sprite>("Pieces/Random/Red/Bishop_Random_Red");
                pieceDropdown.options[3].image = Resources.Load<Sprite>("Pieces/Random/Red/Rook_Random_Red");
                break;
            case Chess.Essence.Blue:
                pieceDropdown.options[0].image = Resources.Load<Sprite>("Pieces/Random/Blue/Pawn_Random_Blue");
                pieceDropdown.options[1].image = Resources.Load<Sprite>("Pieces/Random/Blue/Knight_Random_Blue");
                pieceDropdown.options[2].image = Resources.Load<Sprite>("Pieces/Random/Blue/Bishop_Random_Blue");
                pieceDropdown.options[3].image = Resources.Load<Sprite>("Pieces/Random/Blue/Rook_Random_Blue");
                break;
        };

        pieceDropdown.RefreshShownValue();
    }

    private void OnPieceChanged(int value)
    {
        SetPieceInfo((Chess.PieceType)value, (Chess.Essence)essenceDropdown.value);
    }

    private void OnEssenceChanged(int value)
    {
        SetPieceInfo((Chess.PieceType)pieceDropdown.value, (Chess.Essence)value);
    }

    #endregion
}
