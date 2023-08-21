using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PieceDropdown : TMP_Dropdown, IPointerClickHandler
{
    public UnityEvent onClicked;

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        onClicked?.Invoke();
    }
}
