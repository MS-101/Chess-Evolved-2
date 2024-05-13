/*****************************************************************//**
 * \file   TitleCanvas.cs
 * \brief  Ovládač rozhrania titulnej obrazovky.
 * 
 * \author Martin Šváb
 * \date   Máj 2024
 *********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Táto trieda je zodpovedná za správu titulnej obrazovky.
 */
public class TitleCanvas : MonoBehaviour
{
    [SerializeField] private Button playBtn, quitBtn;
    [SerializeField] private GameCanvas gameCanvas;
    [SerializeField] private EngineController engineController;

    private GameObject createGameOverlay = null;

    private void Start()
    {
        playBtn.onClick.AddListener(OnPlayClick);
        quitBtn.onClick.AddListener(OnQuitClick);
    }

    /**
     * Po stlačení tlačidla spustenia hry sa zobrazí rozhranie vytvorenia novej hry.
     */
    private void OnPlayClick()
    {
        createGameOverlay = Instantiate((GameObject)Resources.Load("Prefabs/CreateGameOverlay"), gameObject.transform);

        CreateGameOverlay createGameOverlayScript = createGameOverlay.GetComponent<CreateGameOverlay>();
        createGameOverlayScript.SetEngineController(engineController);
        createGameOverlayScript.onGameCreated.AddListener(OnGameCreated);
    }

    /**
     * Pri prijatí hernej konfigurácie od rozhrania vytvorenia novej hry sa presunieme do rozhrania hernej obrazovky s danou konfiguráciou.
     * 
     * \param gameSettings Herná konfigurácia.
     */
    private void OnGameCreated(Chess.GameSettings gameSettings)
    {
        Destroy(createGameOverlay);

        gameCanvas.gameObject.SetActive(true);
        gameCanvas.SetGame(gameSettings);
        gameObject.SetActive(false);
    }

    /**
     * Po stlačení tlačidla vypnutia hry sa terminuje aplikácia.
     */
    private void OnQuitClick()
    {
        Application.Quit();
    }
}
