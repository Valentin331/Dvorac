using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLoop : MonoBehaviour
{
    public GameObject endGameScreen;
    public GameObject mainMenuScreen;
    public GameObject pauseScreen;
    public GameObject difficultySelectScreen;
    public Text endGameMsg;
    public int botDifficulty;

    private Dvorac dvoracScript;
    private AudioManager audioManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        endGameScreen.SetActive(false);
        pauseScreen.SetActive(false);
        difficultySelectScreen.SetActive(false);
        audioManagerScript.PlaySound("mainOST");
    }

    private void Awake()
    {
        dvoracScript = GetComponent<Dvorac>();
        audioManagerScript = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    public void StartGame()
    {
        mainMenuScreen.SetActive(false);
        endGameScreen.SetActive(false);
        pauseScreen.SetActive(false);
        difficultySelectScreen.SetActive(false);
        StartCoroutine(dvoracScript.StartNewGame());
    }

    public void EasyDifficulty()
    {
        botDifficulty = 1;
        audioManagerScript.StopSound("mainOST");
        audioManagerScript.PlaySound("gameAmbient");
    }

    public void MediumDifficulty()
    {
        botDifficulty = 2;
        audioManagerScript.StopSound("mainOST");
        audioManagerScript.PlaySound("gameAmbient");
    }

    public void HardDifficulty()
    {
        botDifficulty = 3;
        audioManagerScript.StopSound("mainOST");
        audioManagerScript.PlaySound("gameAmbient");
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
        audioManagerScript.StopSound("gameAmbient");

        if (state == "victory")
        {
            endGameMsg.text = "Dobio si,";
            audioManagerScript.PlaySound("victoryScreen");
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
        audioManagerScript.StopSound("gameAmbient");
        audioManagerScript.PlaySound("mainOST");
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
        audioManagerScript.StopSound("victoryScreen");
        audioManagerScript.PlaySound("mainOST");
        mainMenuScreen.SetActive(true);
        endGameScreen.SetActive(false);
    }

    public void DifficultySelect()
    {
        audioManagerScript.StopSound("victoryScreen");
        audioManagerScript.PlaySound("mainOST");
        difficultySelectScreen.SetActive(true);
        endGameScreen.SetActive(false);
        mainMenuScreen.SetActive(false);
    }
}
