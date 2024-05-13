/*****************************************************************//**
 * \file   PromotionController.cs
 * \brief  Ovládač rozhrania výberu promócie pešiaka.
 * 
 * \author Martin Šváb
 * \date   Máj 2024
 *********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/**
 * Táto trieda je zodpovedná za správu rozhrania výberu promócie pešiaka.
 */
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

    /**
     * Pri výbere promócie sa pešiak zmení na esenciu vybraného typu figúrky.
     * Táto metóda nastaví prislúchajúce esencie dostupným typom figúrok.
     * 
     * \param rookEssence Esencia veže.
     * \param bishopEssence Esencia strelca.
     * \param knightEssence Esencia rytiera.
     */
    public void SetPieces(Chess.Essence rookEssence, Chess.Essence bishopEssence, Chess.Essence knightEssence)
    {
        this.rookEssence = rookEssence;
        this.bishopEssence = bishopEssence;
        this.knightEssence = knightEssence;
    }

    /**
     * Táto metóda nastaví farbu hráča pre dostupné promócie pešiaka.
     * 
     * \param color Farba hráča, ktorý si vyberá promóciu.
     */
    public void SetColor(Chess.Color color)
    {
        queenImage.sprite = Chess.GetPieceImage(Chess.PieceType.Queen, color, Chess.Essence.Classic);
        rookImage.sprite = Chess.GetPieceImage(Chess.PieceType.Rook, color, rookEssence);
        bishopImage.sprite = Chess.GetPieceImage(Chess.PieceType.Bishop, color, bishopEssence);
        knightImage.sprite = Chess.GetPieceImage(Chess.PieceType.Knight, color, knightEssence);
    }

    /**
     * Po vybratí promócie sa táto informácia odošle poslucháčom (tí vykonajú promóciu pešiaka).
     * 
     * \param pieceType Typ vybranej figúrky.
     */
    private void OnPromotionBtnClicked(Chess.PieceType pieceType)
    {
        onPromotionChosen?.Invoke(pieceType);
    }
}
