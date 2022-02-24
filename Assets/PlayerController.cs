using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 moveVal;
    public float moveSpeed;
    public float jumpHeight;
    private bool jumpable;
    private bool isJumping;
    private Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool facingRight;
    private bool isRunning;

    // Start is called before the first frame update
    void Start()
    {
        jumpable = true;
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
        if(jumpable == true)
        {
            isJumping = true;
            jumpable = false;
        }
    }

    void Update()
    {
        CheckDirection();
        transform.Translate(new Vector2(moveVal.x, 0) * moveSpeed * Time.deltaTime);
        updateAnimations();

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Ground")
        {
            jumpable = true;
        }
    }

    private void updateAnimations()
    {
        animator.SetBool("isRunning", isRunning);
    }
}