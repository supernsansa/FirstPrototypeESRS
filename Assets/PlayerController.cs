using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;

    //Animation vars
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool facingRight;

    //Lateral movement vars
    private float moveValX;
    private Vector2 moveVector;
    private float moveSpeed;
    private bool isRunning;

    //Jumping vars
    public float jumpSpeed;
    private bool isGrounded;
    private bool isJumping;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask ground;
    private bool midair;

    //For double/triple jumping etc
    public int jumpAmount;
    private int jumpsLeft;

    //Shooting
    public Transform firePoint;
    private float firePointPosition;
    private bool beenHit;
    public float knockback;

    //Dash move
    public float dashSpeed;
    public float jogSpeed;

    //Misc
    PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = jogSpeed;
        firePointPosition = firePoint.position.x;
        isGrounded = false;
        isJumping = false;
        midair = false;
        jumpsLeft = jumpAmount;
        rigidbody2d = GetComponent<Rigidbody2D>();
        facingRight = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        beenHit = false;
    }

    //Executes when left stick is moved
    void OnMove(InputValue value)
    {
        //Get movement vector
        moveVector = value.Get<Vector2>();
        moveValX = System.Math.Abs(moveVector.x);
    }

    //Executes when A/South button is pressed
    void OnJump()
    {
        CheckSurroundings();
        if(isGrounded == true)
        {
            isJumping = true;
            isGrounded = false;
            jumpsLeft = jumpAmount;
        }
        else if(jumpsLeft > 0) {
            isJumping = true;
            isGrounded = false;
        }
    }

    //Checks if player is holding down the left bumper
    private void DashCheck()
    {
        CheckSurroundings();
        if (playerInput.actions["Dash"].IsPressed() && midair == false)
        {
            moveSpeed = dashSpeed;
        }
        else
        {
            moveSpeed = jogSpeed;
        }
    }

    //Use for everything else, invoked once per frame
    void Update()
    {
        UpdateAnimations();
    }

    //Use for physics/rigidbodies (runs in lock-step with the physics engine)
    private void FixedUpdate()
    {
        DashCheck();
        CheckDirection();
        if(isRunning)
        {
            transform.Translate(moveSpeed * Time.deltaTime * new Vector2(moveValX, 0));
        }

        if (isJumping == true && jumpsLeft > 0)
        {
            rigidbody2d.velocity = Vector2.zero;
            rigidbody2d.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            jumpsLeft--;
            isJumping = false;
            midair = true;
        }

        //Knockback player if they have been hit by a projectile
        if(beenHit)
        {
            //rigidbody2d.AddRelativeForce((-transform.right + Vector3.up) * knockback, ForceMode2D.Impulse);
            beenHit = false;
        }
    }

    private void CheckDirection()
    {
        if (facingRight && moveVector.x < 0)
        {
            facingRight = !facingRight;
            rigidbody2d.transform.Rotate(0f, 180f, 0f);
        }
        else if (!facingRight && moveVector.x > 0)
        {
            facingRight = !facingRight;
            rigidbody2d.transform.Rotate(0f, 180f, 0f);
        }

        if(moveValX != 0)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    private void UpdateAnimations()
    {
        animator.SetBool("isRunning", isRunning);
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, ground);
        midair = !isGrounded;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    //If player collider collides with a trigger collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Projectile"))
        {
            beenHit = true;
            Debug.Log("Been hit by projectile!");
        }
    }
}