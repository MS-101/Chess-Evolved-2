using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PieceObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image pieceImage, outlineImage, backgroundImage;

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

    private bool selected = false;
    public bool Selected
    {
        get { return selected; }
        set
        {
            selected = value;

            outlineImage.gameObject.SetActive(selected);
        }
    }

    private bool highlighted = false;
    public bool Highlighted
    {
        get { return highlighted; }
        set
        {
            highlighted = value;

            backgroundImage.gameObject.SetActive(highlighted);
        }
    }

    public Color HighlightColor
    {
        get { return backgroundImage.color; }
        set { backgroundImage.color = value; }
    }

    public UnityEvent<Piece> onPieceClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        onPieceClicked?.Invoke(piece);
    }
}
