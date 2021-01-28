﻿using System.Collections;
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

    public void StartGame()
    {
        mainMenuScreen.SetActive(false);
        endGameScreen.SetActive(false);
        pauseScreen.SetActive(false);
        dvoracScript.StartNewGame();
    }

    public void PauseGame()
    {
        pauseScreen.SetActive(true);
    }

    public void ContinueGame()
    {
        pauseScreen.SetActive(false);
    }

    public void EndGame(string state)
    {
        endGameScreen.SetActive(true);
        if (state == "victory")
        {
            endGameMsg.text = "Dobio si,";
        }
        else if (state == "defeat")
        {
            endGameMsg.text = "Bravo debilu glup si u pičku materinu";
        }
        dvoracScript.ClearBoard();
        dvoracScript.dropZoneCastle.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        dvoracScript.dropZoneYard.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
    }

    public void TerminateGame()
    {
        dvoracScript.ClearBoard();
        dvoracScript.dropZoneCastle.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        dvoracScript.dropZoneYard.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        pauseScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        mainMenuScreen.SetActive(true);
        endGameScreen.SetActive(false);
    }
}
