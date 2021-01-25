using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLoop : MonoBehaviour
{
    public GameObject endGameScreen;
    public GameObject mainMenuScreen;
    public GameObject pauseScreen;
    public Text endGameMsg;

    private Dvorac dvoracScript;

    // Start is called before the first frame update
    void Start()
    {
        endGameScreen.SetActive(false);
        pauseScreen.SetActive(false);
    }

    private void Awake()
    {
        dvoracScript = this.GetComponent<Dvorac>();
    }

    public void doStartGame()
    {
        mainMenuScreen.SetActive(false);
        endGameScreen.SetActive(false);
        pauseScreen.SetActive(false);
        dvoracScript.StartNewGame();
    }

    public void doPauseGame()
    {
        pauseScreen.SetActive(true);
    }

    public void doContinueGame()
    {
        pauseScreen.SetActive(false);
    }

    public void doEndGame(string state)
    {
        endGameScreen.SetActive(true);
        if (state == "victory")
        {
            // TODO: write victory code here
        }
        else if (state == "defeat")
        {
            endGameMsg.text = "Bravo debilu glup si u pičku materinu";
        }
        dvoracScript.ClearBoard();
        dvoracScript.dropZoneCastle.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        dvoracScript.dropZoneYard.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
    }

    public void doTerminateGame()
    {
        dvoracScript.ClearBoard();
        dvoracScript.dropZoneCastle.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        dvoracScript.dropZoneYard.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        pauseScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
    }

    public void doExitGame()
    {
        Application.Quit();
    }

    public void BackToMM()
    {
        mainMenuScreen.SetActive(true);
        endGameScreen.SetActive(false);
    }
}
