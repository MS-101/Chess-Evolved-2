using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PieceObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image pieceImage, outlineImage;

    private Piece piece = null;
    public Piece Piece
    {
        get { return piece; }
        set
        {
            piece = value;

            pieceImage.sprite = Chess.GetPieceImage(piece.type, piece.color, piece.essence);
        }
    }

    public void SetOutlineVisibility(bool visible)
    {
        outlineImage.gameObject.SetActive(visible);
    }

    public UnityEvent<Piece> onPieceClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        onPieceClicked?.Invoke(piece);
    }
}
