using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class MovementObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image image;

    private Movement movement = null;
    public Movement Movement
    {
        get { return movement; }
        set
        {
            movement = value;

            image.sprite = Chess.GetMovementImage(movement.type);
        }
    }

    public UnityEvent onClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
    }
}
