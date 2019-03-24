using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private float dx = 0.5f;
    [SerializeField] private float dy = 0.5f;
    
    private float MinLifeTime = 0.2f;
    private float LifeTime;
    
    private Vector3 v3rb;

    private Rigidbody2D Rigidbody2D;
    private GameObject contactgo;

    private float raycastDistance = 0.5f;

    private RaycastHit2D raycastHit2DLT = new RaycastHit2D();
    private RaycastHit2D raycastHit2DLB = new RaycastHit2D();
    private RaycastHit2D raycastHit2DRT = new RaycastHit2D();
    private RaycastHit2D raycastHit2DRB = new RaycastHit2D();

    private RaycastHit2D raycastHit2DTL = new RaycastHit2D();
    private RaycastHit2D raycastHit2DTR = new RaycastHit2D();
    private RaycastHit2D raycastHit2DBL = new RaycastHit2D();
    private RaycastHit2D raycastHit2DBR = new RaycastHit2D();
    
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        LifeTime += Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        contactgo = collision.gameObject;
        if (contactgo.name == "Platforms")
        {
            iniRayCast();
            DirectionOfContact();
        }
        if (contactgo.tag == "Player" && LifeTime > MinLifeTime)
        {
            PlayerController script = contactgo.GetComponent<PlayerController>();
            Debug.Log(script);
            script.setLife(script.getLife() -1);
            Destroy(this.gameObject);
        }
    }
    private void iniRayCast()
    {
        raycastHit2DLT = Physics2D.Raycast(transform.position + new Vector3(-dx, dy), Vector2.left, raycastDistance);
        raycastHit2DLB = Physics2D.Raycast(transform.position + new Vector3(-dx, -dy), Vector2.left, raycastDistance);
        raycastHit2DRT = Physics2D.Raycast(transform.position + new Vector3(dx, dy), Vector2.right, raycastDistance);
        raycastHit2DRB = Physics2D.Raycast(transform.position + new Vector3(dx, -dy), Vector2.right, raycastDistance);

        raycastHit2DTL = Physics2D.Raycast(transform.position + new Vector3(dx, -dy), Vector2.up, raycastDistance);
        raycastHit2DTR = Physics2D.Raycast(transform.position + new Vector3(dx, -dy), Vector2.up, raycastDistance);
        raycastHit2DBL = Physics2D.Raycast(transform.position + new Vector3(dx, -dy), Vector2.down, raycastDistance);
        raycastHit2DBR = Physics2D.Raycast(transform.position + new Vector3(dx, -dy), Vector2.down, raycastDistance);
    }
    private void DirectionOfContact()
    {
        bool hitSide = false;
        bool hitUpDown = false;
        bool whereHit = true;
        if (raycastHit2DLT.collider != null)
            if (raycastHit2DLT.collider.name == "Wall L")
            {
                hitSide = true;
                InvertVelocity(!whereHit);
            }
        if (raycastHit2DLB.collider != null && !hitSide)
            if (raycastHit2DLB.collider.name == "Wall L")
            {
                hitSide = true;
                InvertVelocity(!whereHit);
            }
        if (raycastHit2DRB.collider != null && !hitSide)
            if (raycastHit2DRB.collider.name == "Wall R")
            {
                hitSide = true;
                InvertVelocity(!whereHit);
            }
        if (raycastHit2DRT.collider != null && !hitSide)
            if (raycastHit2DRT.collider.name == "Wall R")
            {
                hitSide = true;
                InvertVelocity(!whereHit);
            }

        if (raycastHit2DTL.collider != null)
            if (raycastHit2DTL.collider.name == "Wall T")
            {
                hitUpDown = true;
                InvertVelocity(whereHit);
            }
        if (raycastHit2DTR.collider != null && !hitUpDown)
            if (raycastHit2DTR.collider.name == "Wall T")
            {
                hitUpDown = true;
                InvertVelocity(whereHit);
            }
        if (raycastHit2DBL.collider != null && !hitUpDown)
            if (raycastHit2DBL.collider.name == "Wall B")
            {
                hitUpDown = true;
                InvertVelocity(whereHit);
            }
        if (raycastHit2DBR.collider != null && !hitUpDown)
            if (raycastHit2DBR.collider.name == "Wall B")
            {
                hitUpDown = true;
                InvertVelocity(whereHit);
            }
    }

    private void InvertVelocity(bool updown) // updown = true => up
    {
        if (updown)
        {
            Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, -Rigidbody2D.velocity.y);
        }
        else
        {
            Rigidbody2D.velocity = new Vector2(-Rigidbody2D.velocity.x, Rigidbody2D.velocity.y);
        }
    }
}
