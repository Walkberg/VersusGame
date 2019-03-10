using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;


    private bool facingRight = true;

    private float speed = 6.0f;
    private float moveInput;
    public float jumpForce;

    private bool isGrounded;
    public Transform feetPose;
    public float checkRadius;

    private bool isJumping = false;
    private float jumpTime = 0.25f;
    private float jumpTimeCounter;

    public LayerMask whatIsGround;

    // Start is calledd before the first frame update
    void Start()
    {
        Debug.Log("cocufiodshoi");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveInput = Input.GetAxis("Player1xaxis");
        if (isGrounded || moveInput != 0)
        {
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x * 0.98f, rb.velocity.y);
        }

    }
    private void Update()
    {

        if (moveInput < 0 && facingRight)
        {
            Flip();
        }
        else if (moveInput > 0 && !facingRight)
        {
            Flip();
        }

        isGrounded = Physics2D.OverlapCircle(feetPose.position, checkRadius, whatIsGround);

        if (isGrounded == true && Input.GetKeyDown(KeyCode.Space))
        {
            jumpTimeCounter = jumpTime;
            isJumping = true;
            rb.velocity = Vector2.up * jumpForce;
        }
        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
