using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
    [SerializeField] private Button returnBtn, playBtn;
    [SerializeField] private TitleCanvas titleCanvas;

    private void Start()
    {
        returnBtn.onClick.AddListener(OnReturnClick);
        playBtn.onClick.AddListener(OnPlayClick);
    }

    private void OnReturnClick()
    {
        gameObject.SetActive(false);
        titleCanvas.gameObject.SetActive(true);
    }

    private void OnPlayClick()
    {
        // TO DO
    }
}
