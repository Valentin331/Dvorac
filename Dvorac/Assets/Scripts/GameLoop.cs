using System;
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
    public GameObject settingsScreen;
    public GameObject instructionsScreen;
    public Text endGameMsg;
    public int botDifficulty;

    private Dvorac dvoracScript;
    private AudioManager audioManagerScript;
        
    // Settings
    // Audio variables
    public float volumeMaster;
    public float volumeOST;
    public float volumeEffects;
    // Graphic variables
    public int UIRed;
    public int UIGreen;
    public int UIBlue;

    // Images
    public Image endGameImage;
    public Image pauseScreenImage;
    public Image settingsScreenImage;
    public Image difficutyScreenImage;


    // =========== Helper Functions ===========

    private float Round2Decimal(float value, int decPlaces)
    {
        return (float)Math.Round(value, decPlaces);
    }

    private float Round2DecimalInt(float value, int decPlaces)
    {
        return (int)Math.Round(value, decPlaces);
    }

    // Start is called before the first frame update
    void Start()
    {
        endGameScreen.SetActive(false);
        pauseScreen.SetActive(false);
        difficultySelectScreen.SetActive(false);
        settingsScreen.SetActive(false);
        instructionsScreen.SetActive(false);
        audioManagerScript.PlaySound("mainOST");
    }

    private void Awake()
    {
        dvoracScript = GetComponent<Dvorac>();
        audioManagerScript = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        UIRed = 190;
        UIGreen = 133;
        UIBlue = 46;
        volumeMaster = 1;
    }

    public void StartGame()
    {
        mainMenuScreen.SetActive(false);
        endGameScreen.SetActive(false);
        pauseScreen.SetActive(false);
        difficultySelectScreen.SetActive(false);
        instructionsScreen.SetActive(false);
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

    public void SettingsScreen()
    {
        settingsScreen.SetActive(true);
    }

    public void exitSettingsScreen()
    {
        settingsScreen.SetActive(false);
    }

    public void InstructionsScreen()
    {
        instructionsScreen.SetActive(true);
    }

    public void ExitInstructionsScreen()
    {
        instructionsScreen.SetActive(false);
    }

    public void EndGame(string state)
    {
        endGameScreen.SetActive(true);
        audioManagerScript.StopSound("gameAmbient");

        if (state == "victory")
        {
            endGameMsg.text = "Pobjedio si!";
            audioManagerScript.PlaySound("victoryScreen");
        }
        else if (state == "defeat")
        {
            endGameMsg.text = "Izgubio si.";
            audioManagerScript.PlaySound("defeatScreen");
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

    // Sliders volume input functions
    public void MasterVolumeInput(float value)
    {
        volumeMaster = Round2Decimal(value, 2);   
    }

    public void OSTVolumeInput(float value)
    {
        volumeOST = Round2Decimal(value, 2);
    }

    public void EffectsVolumeInput(float value)
    {
        volumeEffects = Round2Decimal(value, 2);
    }

    // Sliders colors input functions
    public void RedColorInput(float value)
    {
        UIRed = (int)value;
    }

    public void GreenColorInput(float value)
    {
        UIGreen = (int)value;
    }

    public void BlueColorInput(float value)
    {
        UIBlue = (int)value;
    }

    // Applying UI color changes function
    public void ApplyUIColorChanges()
    {
        endGameScreen.GetComponent<Image>().color = new Color32((byte)UIRed, (byte)UIGreen, (byte)UIBlue, 255);
        pauseScreen.GetComponent<Image>().color = new Color32((byte)UIRed, (byte)UIGreen, (byte)UIBlue, 255);
        difficultySelectScreen.GetComponent<Image>().color = new Color32((byte)UIRed, (byte)UIGreen, (byte)UIBlue, 255);
        settingsScreen.GetComponent<Image>().color = new Color32((byte)UIRed, (byte)UIGreen, (byte)UIBlue, 255);
    }
}
