using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PieceObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image image;

    private Piece piece = null;
    public Piece Piece
    {
        get { return piece; }
        set
        {
            piece = value;

            image.sprite = Chess.GetPieceImage(piece.type, piece.color, piece.essence);
        }
    }

    public UnityEvent<Piece> onPieceClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        onPieceClicked?.Invoke(piece);
    }
}
