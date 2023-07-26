using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    BoxCollider2D myFeetCollider2D;
    CapsuleCollider2D myBodyCollider2D;
    [SerializeField] float walkSpeed = 5F;
    [SerializeField] float jumpSpeed = 5F;
    [SerializeField] float climbSpeed = 3F;
    float gravityScaleAtStart;
    [SerializeField] bool isAlive = true;
    [SerializeField] bool isShooting = false;
    [SerializeField] GameObject arrow;
    [SerializeField] Transform bow;

    

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider2D =  GetComponent<CapsuleCollider2D>();
        myFeetCollider2D =  GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidBody.gravityScale;
       
            
    }

    void Update()
    {
        if(!isAlive) {return;}
        Walk();
        FlipSprite();
        JumpAnimation();
        ClimbLadder();     
        Die();
        if (isShooting == true)
        {
            ShootingAnimEnd();
        }
          
    }

    void OnMove(InputValue value)
    {
        if(!isAlive) {return;}
        moveInput = value.Get<Vector2>();

    }

    void OnFire(InputValue value) {
        if(!isAlive) {return;}
        if (!isShooting && myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            myAnimator.SetBool("IsShooting", true);
            isShooting = true;
            //Instantiate(arrow, bow.position, transform.rotation);
        }   
    }

    void ShootingAnimEnd()
    {
        if (myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8 && myAnimator.GetCurrentAnimatorStateInfo(0).IsName("ShootArrow"))
        {
            Instantiate(arrow, bow.position, transform.rotation);
            isShooting = false;
            myAnimator.SetBool("IsShooting", false);
        } 
    }

    void OnJump(InputValue value)
    {
        if(!isAlive) {return;}

        bool isTouchingGround =  IsTouchingGround();
        
        if(value.isPressed && isTouchingGround)
        {
            myRigidBody.velocity += new Vector2(0f, jumpSpeed);
        }
        
    }

    void JumpAnimation()
    {   bool isTouchingGround =  IsTouchingGround();
        bool playerHasPostiveVerticalSpeed = myRigidBody.velocity.y > 0;
        bool playerHasNegativeVerticalSpeed = myRigidBody.velocity.y < 0;
         if(playerHasPostiveVerticalSpeed && !isTouchingGround)
        {
            myAnimator.SetBool("IsJumping", true);
            myAnimator.SetBool("IsFalling", false);
        } else if (playerHasNegativeVerticalSpeed && !isTouchingGround)
        {
            myAnimator.SetBool("IsFalling", true);
            myAnimator.SetBool("IsJumping", false);
        } else
        {
            myAnimator.SetBool("IsFalling", false);
            myAnimator.SetBool("IsJumping", false);
        }

    }

    void Walk()
    {   bool isTouchingGround =  IsTouchingGround();
        Vector2 playerVelocity = new Vector2(moveInput.x*walkSpeed,myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
        
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.speed = Mathf.Abs(moveInput.x*climbSpeed)/climbSpeed;
        if (isTouchingGround)
        {
            myAnimator.SetBool("IsWalking", playerHasHorizontalSpeed);
        } else {
            myAnimator.SetBool("IsWalking", false);
        }
    }

    void FlipSprite()
    {   
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2 (Mathf.Sign(myRigidBody.velocity.x), 1);
        }
        
    }

    public bool IsTouchingGround()
    {
        return myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    void ClimbLadder()
    {
        bool isTouchingLadder = myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Ladder"));
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, moveInput.y*climbSpeed);

        if (isTouchingLadder)
        {
            myRigidBody.gravityScale = 0f;
            myAnimator.SetBool("IsClimbing", true);
            myRigidBody.velocity = climbVelocity;
            myAnimator.speed = Mathf.Abs(moveInput.y*climbSpeed)/climbSpeed;
        } else {
            myAnimator.SetBool("IsClimbing", false);
            myRigidBody.gravityScale = gravityScaleAtStart;
            myAnimator.speed = 1;
        }
       
    }

    void Die()
    {   
        if (myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy","Hazards","Water")) | myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Hazards","Enemy")))
        {
            isAlive = false;
            myAnimator.SetBool("IsDead", true);
            myRigidBody.velocity = new Vector2((-myRigidBody.velocity.x*2)+1,5);
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
        
    }

    
}
