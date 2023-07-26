using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int pickupValue = 100;
    [SerializeField] AudioClip coinPickupSFX;

    bool wasCollected = false;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"&& !wasCollected)
        {
            wasCollected = true;
            FindObjectOfType<GameSession>().IncreaseScore(pickupValue);
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);
            Destroy(gameObject);

            
        }
    }
}
