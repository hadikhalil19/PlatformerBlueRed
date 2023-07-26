using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody2D myRigidbody;
    [SerializeField] float arrowSpeed = 20f;
    PlayerMovement player;
    float xSpeed;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = FindAnyObjectByType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * arrowSpeed;
    }

    
    void Update()
    {
        myRigidbody.velocity = new Vector2 (xSpeed,0f);
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Enemy")
        {
            Destroy(other.gameObject);
            
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        Destroy(gameObject);        
    }
}
