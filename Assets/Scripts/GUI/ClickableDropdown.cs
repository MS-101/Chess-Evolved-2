/*****************************************************************//**
 * \file   ClickableDropdown.cs
 * \brief  Rozšírenie dropdown komponentu.
 * 
 * \author Martin Šváb
 * \date   Máj 2024
 *********************************************************************/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/**
 * Táto trieda je identická s dropdownom, jediné čo sme mu pridali je onClick event.
 * Toto správanie potrebujeme v rozhraní vytvorenia hry.
 */
public class ClickableDropdown : TMP_Dropdown, IPointerClickHandler
{
    public UnityEvent onClicked;

    /**
     * Pri kliknutí na tento objekt sa to oznámi poslucháčom jeho onClick eventu.
     * 
     * \param eventData Informácie o kliknutí.
     */
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        onClicked?.Invoke();
    }
}
