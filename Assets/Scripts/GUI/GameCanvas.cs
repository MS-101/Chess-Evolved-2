using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
    [SerializeField] private Image playerPawn, playerKnight, playerBishop, playerRook, playerQueen, playerKing;
    [SerializeField] private Image malakhPawn, malakhKnight, malakhBishop, malakhRook, malakhQueen, malakhKing;
    [SerializeField] private Button returnBtn, playBtn;
    
    [SerializeField] private TitleCanvas titleCanvas;

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
    }

    private void OnReturnClick()
    {
        gameObject.SetActive(false);
        titleCanvas.gameObject.SetActive(true);
    }

    private void OnPlayClick()
    {
        // TO DO
    }
}
