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
    public float moveSpeed;
    private bool isRunning;

    //Jumping vars
    public float jumpHeight;
    private bool isGrounded;
    private bool isJumping;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask ground;

    //For double/triple jumping etc
    public int jumpAmount;
    private int jumpsLeft;

    //Shooting
    public Transform firePoint;
    private float firePointPosition;

    // Start is called before the first frame update
    void Start()
    {
        firePointPosition = firePoint.position.x;
        isGrounded = false;
        isJumping = false;
        rigidbody2d = GetComponent<Rigidbody2D>();
        facingRight = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void OnMove(InputValue value)
    {
        //Get movement vector
        moveVector = value.Get<Vector2>();
        moveValX = System.Math.Abs(moveVector.x);
    }

    void OnJump(InputValue value)
    {
        CheckSurroundings();
        if(isGrounded == true)
        {
            isJumping = true;
            isGrounded = false;
        }
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        CheckDirection();
        transform.Translate(moveSpeed * Time.deltaTime * new Vector2(moveValX, 0));
        UpdateAnimations();

        if (isJumping == true)
        {
            rigidbody2d.velocity = Vector2.up * jumpHeight;
            isJumping = false;
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
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}