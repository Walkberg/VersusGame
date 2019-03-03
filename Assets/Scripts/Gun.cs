using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    [SerializeField] private GameObject bullet;
    private string id;

    private List<GameObject> Lbullets = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        id = transform.parent.name.Split(' ')[1];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Player" + id + "Fire")/* && id == "1"*/)
        {
            ShootBullet();
        }
        //if (Input.GetButtonDown("Player" + id + "Fire") && id == "2")
        //{
        //    ShootBullet();
        //}
        if (SceneScript.restart == true)
        {
            foreach (var bul in Lbullets)
                Destroy(bul);
            Lbullets.Clear();
            if (id == "1")
                SceneScript.restart = false;
        }

    }
    private void ShootBullet()
    {
        if (Lbullets.Count >= 3)
        {
            Destroy(Lbullets[0]);
            Lbullets.RemoveAt(0);
        }
        Lbullets.Add(Instantiate(bullet, transform.position, Quaternion.identity));
        rigidbody2D = Lbullets[Lbullets.Count - 1].GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = BulletOrientation();
    }

    private Vector2 BulletOrientation()
    {
        int ox = 0, oy = 0;
        
        if (Input.GetAxis("Player" + id + "xaxis") > 0.25)
            ox = 1;
        else if (Input.GetAxis("Player" + id + "xaxis") < -0.25)
            ox = -1;
        if (Input.GetAxis("Player" + id + "yaxis") > 0.25)
            oy = -1;
        else if (Input.GetAxis("Player" + id + "yaxis") < -0.25)
            oy = 1;

        return new Vector2(ox, oy) * 10;
    }
}
