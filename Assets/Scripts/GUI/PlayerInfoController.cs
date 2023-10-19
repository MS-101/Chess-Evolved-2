using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerInfoController : MonoBehaviour
{
    [SerializeField] private Image pawnImage, knightImage, bishopImage, rookImage, queenImage, kingImage;
    [SerializeField] private Button pawnButton, knightButton, bishopButton, rookButton, queenButton, kingButton;

    private Chess.Color color;
    private Chess.Essence pawnEssence, knightEssence, bishopEssence, rookEssence;

    public UnityEvent<Chess.PieceType, Chess.Color, Chess.Essence> onHelpButtonClicked = new();

    private void Start()
    {
        pawnButton.onClick.AddListener(() => OnHelpButtonClicked(Chess.PieceType.Pawn, color, pawnEssence));
        knightButton.onClick.AddListener(() => OnHelpButtonClicked(Chess.PieceType.Knight, color, knightEssence));
        bishopButton.onClick.AddListener(() => OnHelpButtonClicked(Chess.PieceType.Bishop, color, bishopEssence));
        rookButton.onClick.AddListener(() => OnHelpButtonClicked(Chess.PieceType.Rook, color, rookEssence));
        queenButton.onClick.AddListener(() => OnHelpButtonClicked(Chess.PieceType.Queen, color, Chess.Essence.Classic));
        kingButton.onClick.AddListener(() => OnHelpButtonClicked(Chess.PieceType.King, color, Chess.Essence.Classic));
    }

    public void SetPieces(Chess.Color color, Chess.Essence pawnEssence, Chess.Essence knightEssence, Chess.Essence bishopEssence, Chess.Essence rookEssence)
    {
        this.color = color;

        this.pawnEssence = pawnEssence;
        this.knightEssence = knightEssence;
        this.bishopEssence = bishopEssence;
        this.rookEssence = rookEssence;

        pawnImage.sprite = Chess.GetPieceImage(Chess.PieceType.Pawn, color, pawnEssence);
        knightImage.sprite = Chess.GetPieceImage(Chess.PieceType.Knight, color, knightEssence);
        bishopImage.sprite = Chess.GetPieceImage(Chess.PieceType.Bishop, color, bishopEssence);
        rookImage.sprite = Chess.GetPieceImage(Chess.PieceType.Rook, color, rookEssence);
        queenImage.sprite = Chess.GetPieceImage(Chess.PieceType.Queen, color, Chess.Essence.Classic);
        kingImage.sprite = Chess.GetPieceImage(Chess.PieceType.King, color, Chess.Essence.Classic);
    }

    private void OnHelpButtonClicked(Chess.PieceType pieceType, Chess.Color color, Chess.Essence pieceEssence)
    {
        onHelpButtonClicked?.Invoke(pieceType, color, pieceEssence);
    }
}
