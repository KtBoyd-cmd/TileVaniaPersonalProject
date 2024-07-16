using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{    
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float gravityScaleAtStart;
    bool isAlive = true;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 2f;
    




    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2 (10f, 10f);
    [SerializeField] GameObject arrow;
    [SerializeField] GameObject bow;
    [SerializeField] TrailRenderer tr;
    [SerializeField] AudioClip DeathSFX;
    [SerializeField] AudioClip dashSFX;


    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
        tr.emitting = false;

    }

    void Update()
    {
       

        if (!isAlive) 
        { 
            return; 
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(OnDash());
        }
        if (isDashing)
        {
            return;
        }
        
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    void OnFire(InputValue value)
    {
        if (!isAlive) { return; }
        myAnimator.SetBool("isAttacking", true);
        Instantiate (arrow, myRigidbody.position, transform.rotation);
        myAnimator.SetBool("isAttacking", false);

    }

    
    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }

        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }

        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return;}

        if(value.isPressed)
        {
            if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Ladder", "Enemy", "Bouncing")))
            {
                myRigidbody.velocity += new Vector2(0f, jumpSpeed);
            }
        }
    
    }
    

    
    void Run()
    {
        Vector2 playerVelocity = new Vector2 (moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
        
            bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
            myAnimator.SetBool("isRunning", playerHasHorizontalSpeed); 
        
    }

   



    void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"))) 
        {
            myRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);   

            return;
        }

        Vector2 climbVelocity = new Vector2 (myRigidbody.velocity.x, moveInput.y * climbSpeed);
        myRigidbody.velocity = climbVelocity;
        myRigidbody.gravityScale = 0f;

            bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);   
        
        
    
    
    }
    
    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs (myRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2 (Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }    

    void Stomp()
    {
        if(myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            Destroy(gameObject);
        }
    }

    public void Die()  
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazard")))
        {
            AudioSource.PlayClipAtPoint(DeathSFX, Camera.main.transform.position);
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();

        }
    }

    IEnumerator OnDash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = myRigidbody.gravityScale;
        myRigidbody.gravityScale = 0f;
        myRigidbody.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        AudioSource.PlayClipAtPoint(dashSFX, Camera.main.transform.position);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        myRigidbody.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
