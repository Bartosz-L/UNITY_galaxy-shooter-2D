using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class GameManager : MonoBehaviour
{

    public bool gameOver = true;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject _pauseMenuPanel;


    private UIManager _uiManager;
    private SpawnManager _spawnManager;
    private Animator _pauseAnimator;

    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _pauseAnimator = GameObject.Find("Pause_Menu_Panel").GetComponent<Animator>();
        // to prevent animation of pause menu to stop when pausing the game
        _pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private void Update()
    {
        if (gameOver == true)
        {

            if (Input.GetKeyDown(KeyCode.Space) || CrossPlatformInputManager.GetButtonDown("Fire"))
            {
                Instantiate(player, Vector3.zero, Quaternion.identity);
                gameOver = false;
                _uiManager.HideTitleScreen();
                _spawnManager.StartSpawnCoroutines();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Menu");
            }
        }

        // pause menu implementation
        if (Input.GetKeyDown(KeyCode.P))
        {
            _pauseMenuPanel.SetActive(true);
            _pauseAnimator.SetBool("isPaused", true);
            // timescale 0 freezes the game
            Time.timeScale = 0;
        }
    }

    public void ResumeGame()
    {
        _pauseMenuPanel.SetActive(false);
        Time.timeScale = 1;
    }

}
