using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyShipPrefab;
    [SerializeField]
    private GameObject[] powerups;

    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
    }

    void Start ()
    {
        StartCoroutine(EnemySpawnCoroutine());
        StartCoroutine(PowerupsSpawnCoroutine());
	}

    public void StartSpawnCoroutines()
    {
        StartCoroutine(EnemySpawnCoroutine());
        StartCoroutine(PowerupsSpawnCoroutine());
    }
    
    public IEnumerator EnemySpawnCoroutine()
    {
        while(_gameManager.gameOver == false)
        {
            Instantiate(enemyShipPrefab, new Vector3(Random.Range(-7f, 7f), 7, 0), Quaternion.identity);
            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator PowerupsSpawnCoroutine()
    {
        while(_gameManager.gameOver == false)
        {
            int randomPowerup = Random.Range(0, 3);
            Instantiate(powerups[randomPowerup], new Vector3(Random.Range(-7f, 7f), 7, 0), Quaternion.identity);
            yield return new WaitForSeconds(5f);
        }
    }
}
