using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleCanvas : MonoBehaviour
{
    [SerializeField] private Button playBtn, quitBtn;
    [SerializeField] private GameCanvas gameCanvas;

    private GameObject createGameOverlay = null;

    private void Start()
    {
        playBtn.onClick.AddListener(OnPlayClick);
        quitBtn.onClick.AddListener(OnQuitClick);
    }

    private void OnPlayClick()
    {
        createGameOverlay = Instantiate((GameObject)Resources.Load("Prefabs/CreateGameOverlay"), gameObject.transform);

        CreateGameOverlay createGameOverlayScript = createGameOverlay.GetComponent<CreateGameOverlay>();
        createGameOverlayScript.onGameCreated.AddListener(OnGameCreated);
    }

    private void OnGameCreated(Chess.GameSettings gameSettings)
    {
        Destroy(createGameOverlay);

        gameCanvas.gameObject.SetActive(true);
        gameCanvas.SetGame(gameSettings);
        gameObject.SetActive(false);
    }
    
    private void OnQuitClick()
    {
        Application.Quit();
    }
}
