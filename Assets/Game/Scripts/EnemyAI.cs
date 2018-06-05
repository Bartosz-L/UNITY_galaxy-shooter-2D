using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    float _speed = 3f;

    [SerializeField]
    GameObject _enemyExplosionPrefab;

    [SerializeField]
    private AudioClip _explosionClip;

    private UIManager _uiManager;

	void Start ()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
	}
	
	void Update ()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -7)
        {
            var randomX = Random.Range(-7f, 7f);
            transform.position = new Vector3(randomX, 7, 0);
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            if (other.transform.parent != null)
            {
                Destroy(other.transform.parent.gameObject);
            }

            Destroy(other.gameObject);
            Instantiate(_enemyExplosionPrefab, transform.position, Quaternion.identity);
            _uiManager.UpdateScore();
            AudioSource.PlayClipAtPoint(_explosionClip, Camera.main.transform.position, 1f);
            Destroy(this.gameObject);
        }
        else if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.TakeDamage();
            }
            Instantiate(_enemyExplosionPrefab, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_explosionClip, Camera.main.transform.position, 1f);
            Destroy(this.gameObject);
        }
    }
}
