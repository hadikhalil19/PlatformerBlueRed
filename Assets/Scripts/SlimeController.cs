using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{   
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D myRigidBody2D;
    BoxCollider2D myBoxCollider2d;
    [SerializeField] float slimeSize = 1F;


    
    void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myBoxCollider2d = GetComponent<BoxCollider2D>();
    }


    void Update()
    {
        myRigidBody2D.velocity = new Vector2 (moveSpeed, myRigidBody2D.velocity.y);
    }

    void FlipSlime()
    {   
        moveSpeed = -moveSpeed;
        transform.localScale = new Vector2 (Mathf.Sign(moveSpeed)*slimeSize, 1*slimeSize); 
    }
    void OnTriggerExit2D(Collider2D other) 
    {        
           FlipSlime();           
        
    }
}
