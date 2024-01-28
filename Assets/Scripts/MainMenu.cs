using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject mainMenuButtons;
    public GameObject howToPlay;
    public GameObject Options;
    public GameObject returnToMenuButton;

    public void OnClickPlayGame()
    {
        mainMenu.SetActive(false);
        mainMenuButtons.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void OnClickHowToPlay()
    {
        howToPlay.SetActive(true);
        mainMenuButtons.SetActive(false);
        returnToMenuButton.SetActive(true);
    }

    public void GameOptions()
    {
        Options.SetActive(true);
        mainMenuButtons.SetActive(false);
        returnToMenuButton.SetActive(true);
    }

    public void OnClickToMainMenu()
    {
        howToPlay.SetActive(false);
        Options.SetActive(false);
        returnToMenuButton.SetActive(false);
        mainMenuButtons.SetActive(true);
    }

    public void QuitGame()
    {
       Application.Quit();
    }
}