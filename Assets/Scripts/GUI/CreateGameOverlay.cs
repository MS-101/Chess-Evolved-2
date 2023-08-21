using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateGameOverlay : MonoBehaviour
{
    [SerializeField] private Toggle playerWhiteColorToggle, playerBlackColorToggle, playerRandomColorToggle;
    [SerializeField] private Toggle malakhWhiteColorToggle, malakhBlackColorToggle, malakhRandomColorToggle;
    [SerializeField] private PieceDropdown playerPawnDropdown;
    [SerializeField] private TMP_Dropdown playerKnightDropdown, playerBishopDropdown, playerRookDropdown;
    [SerializeField] private TMP_Dropdown malakhPawnDropdown, malakhKnightDropdown, malakhBishopDropdown, malakhRookDropdown;
    [SerializeField] private Toggle playerRandomPiecesToggle, malakhRandomPiecesToggle;
    [SerializeField] private TMP_Dropdown pieceDropdown, essenceDropdown;
    [SerializeField] private Button returnBtn, playBtn;

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
        playerPawnDropdown.onClicked.AddListener(OnPlayerPawnClicked);
        playerPawnDropdown.onValueChanged.AddListener(OnPlayerPawnChanged);
        playerKnightDropdown.onValueChanged.AddListener(OnPlayerKnightChanged);
        playerBishopDropdown.onValueChanged.AddListener(OnPlayerBishopChanged);
        playerRookDropdown.onValueChanged.AddListener(OnPlayerRookChanged);
        malakhPawnDropdown.onValueChanged.AddListener(OnMalakhPawnChanged);
        malakhKnightDropdown.onValueChanged.AddListener(OnMalakhKnightChanged);
        malakhBishopDropdown.onValueChanged.AddListener(OnMalakhBishopChanged);
        malakhRookDropdown.onValueChanged.AddListener(OnMalakhRookChanged);
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

    enum PieceType
    {
        Pawn = 0,
        Knight = 1,
        Bishop = 2,
        Rook = 3,
    }

    enum Essence
    {
        Classic = 0,
        Red = 1,
        Blue = 2
    }

    private void OnPlayerPawnClicked()
    {
        SetPieceInfo(PieceType.Pawn, (Essence)playerPawnDropdown.value);
    }

    private void OnPlayerPawnChanged(int value)
    {
        SetPieceInfo(PieceType.Pawn, (Essence)value);
    }

    private void OnPlayerKnightChanged(int value)
    {
        SetPieceInfo(PieceType.Knight, (Essence)value);
    }

    private void OnPlayerBishopChanged(int value)
    {
        SetPieceInfo(PieceType.Bishop, (Essence)value);
    }

    private void OnPlayerRookChanged(int value)
    {
        SetPieceInfo(PieceType.Rook, (Essence)value);
    }

    private void OnMalakhPawnChanged(int value)
    {
        SetPieceInfo(PieceType.Pawn, (Essence)value);
    }

    private void OnMalakhKnightChanged(int value)
    {
        SetPieceInfo(PieceType.Knight, (Essence)value);
    }

    private void OnMalakhBishopChanged(int value)
    {
        SetPieceInfo(PieceType.Bishop, (Essence)value);
    }

    private void OnMalakhRookChanged(int value)
    {
        SetPieceInfo(PieceType.Rook, (Essence)value);
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

    #endregion

    #region Buttons

    private void OnReturnClick()
    {
        gameObject.SetActive(false);
    }

    private void OnPlayClick()
    {
        gameObject.SetActive(false);
    }

    #endregion

    #region Piece Info

    private void SetPieceInfo(PieceType pieceType, Essence essence)
    {
        pieceDropdown.SetValueWithoutNotify((int)pieceType);
        essenceDropdown.SetValueWithoutNotify((int)essence);

        switch (essence)
        {
            case Essence.Classic:
                pieceDropdown.options[0].image = Resources.Load<Sprite>("Pieces/Random/Classic/Pawn_Random_Classic");
                pieceDropdown.options[1].image = Resources.Load<Sprite>("Pieces/Random/Classic/Knight_Random_Classic");
                pieceDropdown.options[2].image = Resources.Load<Sprite>("Pieces/Random/Classic/Bishop_Random_Classic");
                pieceDropdown.options[3].image = Resources.Load<Sprite>("Pieces/Random/Classic/Rook_Random_Classic");
                break;
            case Essence.Red:
                pieceDropdown.options[0].image = Resources.Load<Sprite>("Pieces/Random/Red/Pawn_Random_Red");
                pieceDropdown.options[1].image = Resources.Load<Sprite>("Pieces/Random/Red/Knight_Random_Red");
                pieceDropdown.options[2].image = Resources.Load<Sprite>("Pieces/Random/Red/Bishop_Random_Red");
                pieceDropdown.options[3].image = Resources.Load<Sprite>("Pieces/Random/Red/Rook_Random_Red");
                break;
            case Essence.Blue:
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
        SetPieceInfo((PieceType)value, (Essence)essenceDropdown.value);
    }

    private void OnEssenceChanged(int value)
    {
        SetPieceInfo((PieceType)pieceDropdown.value, (Essence)value);
    }

    #endregion
}
