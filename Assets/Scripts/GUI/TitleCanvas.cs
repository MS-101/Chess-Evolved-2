using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleCanvas : MonoBehaviour
{
    [SerializeField] private Button playBtn, quitBtn;
    [SerializeField] private GameCanvas gameCanvas;

    private void Start()
    {
        playBtn.onClick.AddListener(OnPlayClick);
        quitBtn.onClick.AddListener(OnQuitClick);
    }

    private void OnPlayClick()
    {
        gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(true);
    }
    
    private void OnQuitClick()
    {
        Application.Quit();
    }
}
