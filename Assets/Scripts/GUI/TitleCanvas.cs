using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleCanvas : MonoBehaviour
{
    [SerializeField] private Button playBtn, quitBtn;
    [SerializeField] private CreateGameOverlay createGameOverlay;
    [SerializeField] private GameCanvas gameCanvas;

    private void Start()
    {
        playBtn.onClick.AddListener(OnPlayClick);
        quitBtn.onClick.AddListener(OnQuitClick);
        createGameOverlay.onGameCreated.AddListener(OnGameCreated);
    }

    private void OnPlayClick()
    {
        createGameOverlay.gameObject.SetActive(true);
    }

    private void OnGameCreated(Chess.GameSettings gameSettings)
    {
        createGameOverlay.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(true);
        gameCanvas.SetGame(gameSettings);
        gameObject.SetActive(false);
    }
    
    private void OnQuitClick()
    {
        Application.Quit();
    }
}
