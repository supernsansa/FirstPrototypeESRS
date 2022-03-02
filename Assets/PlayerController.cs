using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 moveVal;
    public float moveSpeed;
    public float jumpHeight;
    private bool isGrounded;
    private bool isJumping;
    private Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool facingRight;
    private bool isRunning;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask ground;

    // Start is called before the first frame update
    void Start()
    {
        isGrounded = true;
        isJumping = false;
        rigidbody2d = GetComponent<Rigidbody2D>();
        facingRight = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void OnMove(InputValue value)
    {
        moveVal = value.Get<Vector2>();
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
        CheckDirection();
        transform.Translate(new Vector2(moveVal.x, 0) * moveSpeed * Time.deltaTime);
        UpdateAnimations();

        if (isJumping == true)
        {
            rigidbody2d.velocity = Vector2.up * jumpHeight;
            isJumping = false;
        }
    }

    private void CheckDirection()
    {
        if(facingRight && moveVal.x < 0)
        {
            spriteRenderer.flipX = true;
            facingRight = !facingRight;
        }
        else if (!facingRight && moveVal.x > 0)
        {
            spriteRenderer.flipX = false;
            facingRight = !facingRight;
        }

        if(moveVal.x != 0)
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