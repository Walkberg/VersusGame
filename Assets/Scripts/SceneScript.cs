using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SceneScript : MonoBehaviour
{
    //http://wiki.unity3d.com/index.php/Xbox360Controller
    
    private Vector3 MaxGlobalBonusPos = new Vector3(5f, 5f);    
    private Vector3 MinGlobalBonusPos = new Vector3(-5f, -5f); // TODO changer pour utiliser la pos de la camera

    [SerializeField] private Text HealthTextB;
    [SerializeField] private Text HealthTextR;
    [SerializeField] private LayerMask Ground;
    [SerializeField] private int NbPlayer = 2;
    public static bool GameOver = false;
    public static bool restart = false;

    private Vector3 IniPos = new Vector3(0, 1, 0);
    [SerializeField] private GameObject[] gos = new GameObject[2];
    public static List<Character> characters = new List<Character>();

    [SerializeField] private GameObject Bonus;
    private List<GameObject> LBonus = new List<GameObject>();

    [SerializeField] private GameObject Crown;
    [SerializeField] private GameObject Poop;


    // Start is called before the first frame update
    void Awake()
    {
        SetCharacters(NbPlayer);
        Setconfig();
        SetHealthText();
    }

    private void Setconfig()
    {
        //Char Controls
        characters[0].configH = "Player1xaxis";
        characters[0].configV = "Player1yaxis";
        characters[0].configJump = "Player1Jump";
        characters[0].configFire = "Player1Fire";

        characters[1].configH = "Player2xaxis";
        characters[1].configV = "Player2yaxis";
        characters[1].configJump = "Player2Jump";
        characters[1].configFire = "Player2Fire";

        //Bonus
        Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    private void SetCharacters(int nbChar)
    {
        for (int i = 0; i < nbChar; i++)
        {
            if (gos.Length > i)
            {
                characters.Add(new Character(gos[i], (i + 1).ToString()));
                characters[i].LayerGround = Ground;
                characters[i].Player.name = "Buddy " + (i + 1).ToString();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            restart = true;
            foreach (var item in characters) //position ini
            {
                item.Player.transform.position = IniPos + new Vector3(-5 + 5f * characters.IndexOf(item), 0, 0);
            }
            foreach (var item in LBonus) // On enlève les bonus apparus
            {
                Destroy(item);
            }
            foreach (var item in characters)
            {
                item.Lives = 5;
            }

            if (GameObject.Find("Crown(Clone)") != null)
            {
                Destroy(GameObject.Find("Crown(Clone)"));
                Destroy(GameObject.Find("poopHat(Clone)"));
            }

            Time.timeScale = 1;
            GameOver = false;
        }

        SetHealthText();
        
        RandomBonusSpawn();

        CheckLives();
        Behaviors();
    }

    private void CheckLives()
    {
        foreach (var item in characters)
        {
            if (item.Lives == 0 && !GameOver)
            {
                GameOver = true;
                Instantiate(Poop, item.CrownPos + item.Player.transform.position, Quaternion.identity).transform.parent = item.Player.transform;

                foreach (var chars in characters)
                {
                    if (characters.IndexOf(item) != characters.IndexOf(chars))
                    {
                        Instantiate(Crown, chars.CrownPos + chars.Player.transform.position, Quaternion.identity).transform.parent = chars.Player.transform;
                    }
                }
                Time.timeScale = 0.2f;
                return;
            }
        }
    }

    private void Behaviors()
    {
        foreach (var item in characters)
        {
            item.Behavior();
        }
    }
    private void RandomBonusSpawn()
    {
        if (Random.Range(0, 450f) < 1 /*&& LBonus.Count < 5*/)
        {
            LBonus.Add(Instantiate(Bonus, new Vector3(Random.Range(MinGlobalBonusPos.x, MaxGlobalBonusPos.x), Random.Range(MinGlobalBonusPos.y, MaxGlobalBonusPos.y), 0), Quaternion.identity));
        }
    }
    private void SetHealthText()
    {
        HealthTextB.text = "Blue Buddy : " + characters[0].Lives.ToString();
        HealthTextR.text = "Red Buddy : " + characters[1].Lives.ToString();
    }
}
