using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    private bool FaceRight = true;
    private float Xaxis;

    public int Speed = 4;
    public bool invert = false;
    public Rigidbody2D rb;
    public Transform transform;
    public string Id;
    public string configH;
    public string configV;
    public string configJump;
    public string configFire;
    public int Lives = 5;

    [SerializeField] private float jumpLimit = 10.4f;
    [SerializeField] private float jumpStart = 10;

    public Vector3 CrownPos = new Vector3(0, 1f, 0);
    public Vector3 FeetPos = new Vector3(0, -0.6f, 0);
    private float jumpTime;

    public LayerMask LayerGround;

    [SerializeField] private float FeetRadius = 0.37f;

    private bool isgrounded = true;
    private bool keepjumping = false;

    public GameObject Player;

    public Character(GameObject go, string ident)
    {
        Player = go;
        rb = Player.GetComponent<Rigidbody2D>();
        transform = Player.GetComponent<Transform>();
        Id = ident;
        configH = "Player" + Id + "xaxis";
        configV = "Player" + Id + "yaxis";
    }
    //private void CheckLives()
    //{

    //}
    public void Behavior()
    {
        Xaxis = Input.GetAxis(configH); //Nom de la touche de déplacement 
        if (invert)
        {
            Xaxis = -Xaxis;
        }
        //move
        if (Xaxis != 0)
        {
            if (Xaxis > 0)
            {
                FaceRight = true;
                Xaxis = 1;
            }
            else
            {
                FaceRight = false;
                Xaxis = -1;
            }
            rb.velocity = new Vector2(Speed * Xaxis, rb.velocity.y);
        }
        InvertOrientation(FaceRight);

        //jump with aircontrol and push to jumphigher
        isgrounded = Physics2D.OverlapCircle(FeetPos + Player.transform.position, FeetRadius, LayerGround);
        Debug.DrawRay(FeetPos + Player.transform.position, Vector3.down * FeetRadius);
        if (isgrounded || keepjumping)
        {
            if (Input.GetButtonDown(configJump)) //Nom de la touche de tir à changer si on veux une config
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpStart);
                keepjumping = true;
                jumpTime = 0;
            }
            if (Input.GetButton(configJump) && jumpTime + jumpStart < jumpLimit)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpTime + jumpStart);
                jumpTime += Time.deltaTime;
            }
            if (Input.GetButtonUp(configJump))
            {
                keepjumping = false;
            }
        }
    }

    private void InvertOrientation(bool right)
    {
        if (right)
        {
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
            }
        }
        else
        {
            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
            }
        }
    }
}
