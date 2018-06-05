using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Sprite[] livesSprites;
    public Image livesImageDisplay;
    public GameObject titleScreen;

    public Text scoreText, bestText;
    public int score, bestScore;

    private void Start()
    {
        bestScore = PlayerPrefs.GetInt("HighScore", 0);
        bestText.text = "Best: " + bestScore;
    }

    public void UpdateLives(int currentLives)
    {
        livesImageDisplay.sprite = livesSprites[currentLives];
    }

    public void UpdateScore()
    {
        score += 10;
        scoreText.text = "Score: " + score;
    }

    public void CheckForBestScore()
    {
        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("HighScore", bestScore);
            bestText.text = "Best: " + bestScore;
        }
    }

    public void ShowTitleScreen()
    {
        titleScreen.SetActive(true);
        score = 0;
    }

    public void HideTitleScreen()
    {
        titleScreen.SetActive(false);
        scoreText.text = "Score: 0";
    }

    public void ResumePlay()
    {
        GameManager gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        gameManager.ResumeGame();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
