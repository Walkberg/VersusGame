using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;
    public string HorizontalInput;
    public string JumpInput;

    private bool facingRight = true;


    private float airFriction = 0.95f;
    private float groundFriction = 0.85f;
    private float friction;

    private float life = 4f;
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
    private Animator anim;

    private RaycastHit2D frontWall;

    private bool headRaycastHit;
    private bool feetRaycastHit;

    [SerializeField]
    private Transform feetPosition;
    [SerializeField]
    private Transform headPosition;

    [SerializeField]
    private Color color;

    public ParticleSystem ps;
    public ParticleSystem psSlide;

    private bool isAlive = true;



    // Start is calledd before the first frame update
    void Start()
    {
       
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        var main = ps.main;
        main.startColor = color;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isAlive) { 
        moveInput = Input.GetAxis(HorizontalInput);
        if (isGrounded && moveInput != 0 && !feetRaycastHit && !headRaycastHit)
        {
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
            anim.SetBool("iswalking", true);
            friction = groundFriction;

        }
        else if (!isGrounded && moveInput != 0)
        {
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
            friction = airFriction;

        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x * friction, rb.velocity.y);
            
        }
        if(rb.velocity.x != 0)
        {
            
        }
        else
        {
            anim.SetBool("iswalking", false);
        }
        if (rb.velocity.y < 0)
        {
            anim.SetBool("IsFalling", true);
            anim.SetBool("IsJumping", false);
        }
        }
    }
    private void Update()
    {
        Debug.DrawRay(feetPosition.position, (facingRight? Vector2.right: Vector2.left) * 0.5f, Color.yellow);
        Debug.DrawRay(headPosition.position, (facingRight ? Vector2.right : Vector2.left) * 0.5f, Color.yellow);
        Debug.DrawRay(transform.position, Vector2.left * 0.5f, Color.red);
        Debug.DrawRay(transform.position, Vector2.right * 0.5f, Color.red);

        if (isAlive) { 
        var emission = psSlide.emission;
        

        feetRaycastHit = Physics2D.Raycast(feetPosition.position, (facingRight ? Vector2.right : Vector2.left), 0.5f, whatIsGround);
        headRaycastHit = Physics2D.Raycast(headPosition.position, (facingRight ? Vector2.right : Vector2.left), 0.5f, whatIsGround);


        if (Physics2D.Raycast(transform.position, Vector2.left, 0.5f, whatIsGround) && moveInput<0)
        {
            isJumping = true;
            rb.velocity = (new Vector2(rb.velocity.x,0));
            anim.SetBool("isClimbing", true);
            emission.enabled = true;
        }
        else if (Physics2D.Raycast(transform.position, Vector2.right, 0.5f, whatIsGround) && moveInput > 0)
        {
            isJumping = true;
            rb.velocity = (new Vector2(rb.velocity.x, 0));
            anim.SetBool("isClimbing", true);
            emission.enabled = true;
        }
        else
        {
            anim.SetBool("isClimbing", false);
            emission.enabled = false;
        }
        
        if (moveInput < 0 && facingRight)
        {
            Flip();
        }
        else if (moveInput > 0 && !facingRight)
        {
            Flip();
        }

        isGrounded = Physics2D.OverlapCircle(feetPose.position, checkRadius, whatIsGround);
        if (isGrounded)
        {
            anim.SetBool("IsFalling", false);
        }

        if (isGrounded == true && Input.GetKeyDown(JumpInput))
        {
            jumpTimeCounter = jumpTime;
            isJumping = true;
            rb.velocity = Vector2.up * jumpForce;
        }
        if (Input.GetKey(JumpInput) && isJumping)
        {
            anim.SetBool("IsJumping", true);
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
        if (Input.GetKeyUp(JumpInput))
        {
            isJumping = false;
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            life--;
            anim.SetFloat("Life", life);
        }
        if (Input.GetKeyUp(KeyCode.T))
        {
            life++;
            anim.SetFloat("Life", life);
        }
        

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
    public float getLife()
    {
        Debug.Log(this.life);
        return (this.life);
    }
    public void setLife(float l)
    {
        life = l;
        anim.SetFloat("Life", l);
    }
}
