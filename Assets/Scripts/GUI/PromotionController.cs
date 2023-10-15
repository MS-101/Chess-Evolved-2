using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PromotionController : MonoBehaviour
{
    [SerializeField] private Button queenBtn, rookBtn, bishopBtn, knightBtn;
    [SerializeField] private Image queenImage, rookImage, bishopImage, knightImage;

    private Chess.Essence rookEssence, bishopEssence, knightEssence;

    public UnityEvent<Chess.PieceType> onPromotionChosen;

    private void Start()
    {
        queenBtn.onClick.AddListener(() => OnPromotionBtnClicked(Chess.PieceType.Queen));
        rookBtn.onClick.AddListener(() => OnPromotionBtnClicked(Chess.PieceType.Rook));
        bishopBtn.onClick.AddListener(() => OnPromotionBtnClicked(Chess.PieceType.Bishop));
        knightBtn.onClick.AddListener(() => OnPromotionBtnClicked(Chess.PieceType.Knight));
    }

    public void SetPieces(Chess.Essence rookEssence, Chess.Essence bishopEssence, Chess.Essence knightEssence)
    {
        this.rookEssence = rookEssence;
        this.bishopEssence = bishopEssence;
        this.knightEssence = knightEssence;
    }

    private void OnPromotionBtnClicked(Chess.PieceType pieceType)
    {
        onPromotionChosen?.Invoke(pieceType);
    }

    public void SetColor(Chess.Color color)
    {
        queenImage.sprite = Chess.GetPieceImage(Chess.PieceType.Queen, color, Chess.Essence.Classic);
        rookImage.sprite = Chess.GetPieceImage(Chess.PieceType.Rook, color, rookEssence);
        bishopImage.sprite = Chess.GetPieceImage(Chess.PieceType.Bishop, color, bishopEssence);
        knightImage.sprite = Chess.GetPieceImage(Chess.PieceType.Knight, color, knightEssence);
    }
}
