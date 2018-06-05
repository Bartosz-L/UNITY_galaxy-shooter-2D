using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadSinglePlayerGame()
    {
        SceneManager.LoadScene("Single_Player");
    }

    public void LoadHowToPlay()
    {
        SceneManager.LoadScene("How_To_Play");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitFromApp()
    {
        Application.Quit();
    }

}
