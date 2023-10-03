using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceInfoController : MonoBehaviour
{
    [SerializeField] private Button closeBtn;

    private void Start()
    {
        closeBtn.onClick.AddListener(OnCloseButtonClicked);
    }

    private Piece myPiece;
    public Piece MyPiece
    {
        get { return myPiece; }
        set
        {
            myPiece = value;

            UpdateDisplayedPiece();
        }
    }

    private void UpdateDisplayedPiece()
    {

    }

    private void OnCloseButtonClicked()
    {
        gameObject.SetActive(false);
    }
}
