using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    float _speed = 3f;
    // IDs : 0 - tripple shot, 1 - speed boost, 2 - shields
    [SerializeField]
    int _powerupID;

    [SerializeField]
    private AudioClip _pickupClip;

    void Start ()
    {
		
	}
	
	void Update ()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -7)
        {
            Destroy(this.gameObject);
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player") return;

        AudioSource.PlayClipAtPoint(_pickupClip, Camera.main.transform.position, 1f);

        // access player
        Player player = other.GetComponent<Player>();

        // only if player component was found
        if (player != null)
        {
            if (_powerupID == 0)
            {
                // enable triple shot
                player.TripleShotPowerupOn();
            } 
            else if (_powerupID == 1)
            {
                // enable speed boost
                player.SpeedBoostPowerupOn();
            }
            else if (_powerupID == 2)
            {
                // enable shields
                player.EnableShields();
            }

        }

        // destroy powerup object in the scene
        Destroy(this.gameObject);        
    }
}
