using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 moveVal;
    public float moveSpeed;
    public float jumpSpeed;
    private bool jumpable;
    private Rigidbody2D rigidbody2d;

    // Start is called before the first frame update
    void Start()
    {
        jumpable = false;
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void OnMove(InputValue value)
    {
        moveVal = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if(IsGrounded())
        {
            jumpable = true;
        }
    }

    bool IsGrounded()
    {
        return false;
    }

    void FlipPlayer()
    {
        //Change sprite orientation depending on movement direction
        transform.eulerAngles = new Vector3(0, 180, 0); // Flipped
    }

    void Update()
    {        

        transform.Translate(new Vector3(moveVal.x, 0, 0) * moveSpeed * Time.deltaTime);

        if(jumpable)
        {
            transform.Translate(new Vector3(0, 1, 0) * jumpSpeed * Time.deltaTime);
        }
    }

}