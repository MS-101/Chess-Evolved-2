/*****************************************************************//**
 * \file   PieceObject.cs
 * \brief  Ovládač objektu figúrky.
 * 
 * \author Martin Šváb
 * \date   Máj 2024
 *********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * Táto trieda spravuje objekt figúrky.
 */
public class PieceObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image pieceImage, outlineImage, backgroundImage;

    private Piece piece = null;

    /**
     * Figúrka priradená tomuto objektu.
     * Ak ju zmeníme, tak sa aktualizuje jej zobrazenie.
     */
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

    /**
     * Informácia o výbere figúky.
     * Ak sa zmení výber figúrky, tak sa aktivuje alebo deaktivuje outline figúrky.
     */
    public bool Selected
    {
        get { return selected; }
        set
        {
            selected = value;

            outlineImage.gameObject.SetActive(selected);
        }
    }

    /**
     * Informácia o znázornení figúky.
     * Ak sa zmení znázornenie figúrky, tak sa aktivuje alebo deaktivuje pozadie figúrky.
     */
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

    /**
     * Aktuálna farba znázornenia figúrky.
     * Ak sa táto vlastnosť zmení, tak sa aktualizuje farba pozadia.
     */
    public Color HighlightColor
    {
        get { return backgroundImage.color; }
        set { backgroundImage.color = value; }
    }

    public UnityEvent<Piece> onPieceClicked;

    /**
     * Pri kliknutí na tento objekt sa to oznámi poslucháčom jeho onClick eventu.
     * 
     * \param eventData Informácie o kliknutí.
     */
    public void OnPointerClick(PointerEventData eventData)
    {
        onPieceClicked?.Invoke(piece);
    }
}
