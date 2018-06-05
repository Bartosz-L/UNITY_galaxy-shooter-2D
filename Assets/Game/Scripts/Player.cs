using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField]
    float _speed = 5.0f;

    [SerializeField]
    GameObject _laserPrefab;

    [SerializeField]
    GameObject _tripleShotPrefab;

    [SerializeField]
    GameObject _explosionPrefab;

    [SerializeField]
    private GameObject _shieldGameObject;

    [SerializeField]
    float _powerUpsDuration = 5f;

    [SerializeField]
    float _speedUpIntensity = 1.5f;

    [SerializeField]
    float _fireRate = 0.25f;

    float _canFire = 0f;

    [SerializeField]
    private GameObject[] _engines;

    public int playerHealth = 3;
    public bool canTripleShot = false;
    public bool isSpeedBoostActive = false;
    public bool shieldsActive = false;

    private UIManager _uiManager;
    private GameManager _gameManager;
    private SpawnManager _spawnManager;
    private AudioSource _audioSource;

    private int hitCount = 0;

    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();

        transform.position = new Vector3(0, 0, 0);

        if (_uiManager != null)
        {
            _uiManager.UpdateLives(playerHealth);
        }

        hitCount = 0;

    }

    private void Update()
    {

        Movement();

        #if UNITY_ANDROID
                if (CrossPlatformInputManager.GetButtonDown("Fire"))
                {
                    Shoot();
                }
        #else
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    Shoot();
                }
        #endif


    }

    private void Shoot()
    {
        if (Time.time > _canFire)
        {
            _audioSource.Play();

            if (canTripleShot)
            {
                // triple laser
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                // single laser
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
            }

            _canFire = Time.time + _fireRate;
        }
    }

    private void Movement()
    {
        // player input
        #if UNITY_ANDROID
                var horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal");
                var verticalInput = CrossPlatformInputManager.GetAxis("Vertical");
        #else
                var horizontalInput = Input.GetAxis("Horizontal");
                var verticalInput = Input.GetAxis("Vertical");
        #endif

        if (isSpeedBoostActive)
        {
            // moving player with speedup
            transform.Translate(Vector3.right * Time.deltaTime * _speed * _speedUpIntensity * horizontalInput);
            transform.Translate(Vector3.up * Time.deltaTime * _speed * _speedUpIntensity * verticalInput);
        }
        else
        {
            // moving player
            transform.Translate(Vector3.right * Time.deltaTime * _speed * horizontalInput);
            transform.Translate(Vector3.up * Time.deltaTime * _speed * verticalInput);
        }

        // set boundaries of the scene
        if (transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y < -4.2f)
        {
            transform.position = new Vector3(transform.position.x, -4.2f, 0);
        }

        if (transform.position.x > 9.5f)
        {
            transform.position = new Vector3(-9.5f, transform.position.y, 0);
        }
        else if (transform.position.x < -9.5f)
        {
            transform.position = new Vector3(9.5f, transform.position.y, 0);
        }
    }

    public void TakeDamage()
    {
        // turn of shields when active and got hit
        if (shieldsActive)
        {
            shieldsActive = false;
            _shieldGameObject.SetActive(false);
            return;
        }

        // increment hit counter
        hitCount++;

        // show engines failure
        if (hitCount == 1)
        {
            _engines[0].SetActive(true);
        }
        else if (hitCount == 2)
        {
            _engines[1].SetActive(true);
        }

        // when hit decrease player's health
        playerHealth--;
        _uiManager.UpdateLives(playerHealth);

        // if health goes below 1 pt animate and destroy/ show game over
        if (playerHealth < 1)
        {
            _gameManager.gameOver = true;
            StopCoroutine(_spawnManager.EnemySpawnCoroutine());
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _uiManager.CheckForBestScore();
            _uiManager.ShowTitleScreen();
            Destroy(this.gameObject);

        }
    }

    public void TripleShotPowerupOn()
    {
        canTripleShot = true;
        StartCoroutine(TripleShotPowerDownCoroutine());
    }

    public IEnumerator TripleShotPowerDownCoroutine()
    {
        yield return new WaitForSeconds(_powerUpsDuration);
        canTripleShot = false;
    }


    public void SpeedBoostPowerupOn()
    {
        isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownCoroutine());
    }

    public IEnumerator SpeedBoostPowerDownCoroutine()
    {
        yield return new WaitForSeconds(_powerUpsDuration);
        isSpeedBoostActive = false;
    }

    public void EnableShields()
    {
        shieldsActive = true;
        // show shields
        _shieldGameObject.SetActive(true);

    }
}
