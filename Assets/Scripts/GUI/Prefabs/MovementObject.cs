/*****************************************************************//**
 * \file   MovementObject.cs
 * \brief  Ovládač objektu pohybu.
 * 
 * \author Martin Šváb
 * \date   Máj 2024
 *********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

/**
 * Táto trieda spravuje objekt pohybu.
 */
public class MovementObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image image;

    private Movement movement = null;

    /**
     * Pohyb priradený tomuto objektu.
     * Ak ho zmeníme, tak sa aktualizuje jeho zobrazenie.
     */
    public Movement Movement
    {
        get { return movement; }
        set
        {
            movement = value;

            image.sprite = Chess.GetMovementImage(movement.type);
        }
    }

    public UnityEvent<Movement> onClick;

    /**
     * Pri kliknutí na tento objekt sa to oznámi poslucháčom jeho onClick eventu.
     * 
     * \param eventData Informácie o kliknutí.
     */
    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke(movement);
    }
}
