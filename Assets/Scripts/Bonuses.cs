using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonuses : MonoBehaviour
{
    private int BonusType = 0;
    private int IsBonus = 0;
    private SpriteRenderer sp;
    private BoxCollider2D bc2D;
    private bool StartCount;
    private float TimeBonus = 3;
    private float TimeSpent = 0;
    private int SpeedBoost = 2;
    private string name;
    [SerializeField] private Sprite Sfast;
    [SerializeField] private Sprite Sinvert;
    [SerializeField] private Sprite Sswitchgravity;
    [SerializeField] private Sprite Srand;
    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        bc2D = GetComponent<BoxCollider2D>();

        BonusType = Random.Range(1, 4);
        IsBonus = Random.Range(0, 2);
        GetBonusType(BonusType, IsBonus);
    }
    private void GetBonusType(int bt, int IsBonus)
    {
        switch (bt)
        {
            case 1:
                sp.sprite = Sfast;
                break;
            case 2:
                sp.sprite = Sinvert;
                break;
            case 3:
                sp.sprite = Sswitchgravity;
                break;
            default:
                sp.sprite = Srand;
                BonusType = Random.Range(0, 3);
                break;
        }
        switch (IsBonus)
        {
            case 0:
                sp.color = new Color(1f, 0f, 1f);
                break;
            case 1:
                sp.color = Color.green;
                break;
        }

    }
    // Update is called once per frame
    void Update()
    {
        int index = 0;
        List<Character> chars = SceneScript.characters;
        if (StartCount)
        {
            string a = this.transform.name;
            TimeSpent += Time.deltaTime;
        }
        if (TimeSpent > TimeBonus)
        {
            switch (BonusType)
            {
                case 1:
                    if (IsBonus == 0)
                    {
                        foreach (var item in chars)
                        {
                            if (item.Player.name != name)
                            {
                                item.Speed *= SpeedBoost;
                            }
                        }
                    }
                    else
                    {
                        index = chars.IndexOf(chars.Find(x => x.Player.name == name));
                        chars[index].Speed /= SpeedBoost;
                    }
                    break;
                case 2:
                    if (IsBonus == 0)
                    {
                        foreach (var item in chars)
                        {
                            if (item.Player.name != name)
                            {
                                item.invert = !item.invert;
                            }
                        }
                    }
                    else
                    {
                        index = chars.IndexOf(chars.Find(x => x.Player.name == name));
                        chars[index].invert = !chars[index].invert;
                    }

                    break;
                case 3:
                    if (IsBonus == 0)
                    {
                        foreach (var item in chars)
                        {
                            if (item.Player.name != name)
                            {
                                item.rb.gravityScale = -item.rb.gravityScale;
                                item.transform.localScale = new Vector3(item.transform.localScale.x, -item.transform.localScale.y, item.transform.localScale.z);
                            }
                        }
                    }
                    else
                    {
                        index = chars.IndexOf(chars.Find(x => x.Player.name == name));
                        chars[index].rb.gravityScale = -chars[index].rb.gravityScale;
                        chars[index].transform.localScale = new Vector3(chars[index].transform.localScale.x, -chars[index].transform.localScale.y, chars[index].transform.localScale.z);
                    }
                    break;
                default:
                    break;
            }
            this.enabled = false;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCount = true;
        sp.enabled = false;
        bc2D.enabled = false;
        List<Character> chars = SceneScript.characters;
        name = collision.transform.name;
        int index = 0;
        switch (BonusType)
        {
            case 1:
                if (IsBonus == 0)
                {
                    foreach (var item in chars)
                    {
                        if (item.Player.name != name)
                        {
                            item.Speed /= SpeedBoost;
                        }
                    }
                }
                else
                {
                    index = chars.IndexOf(chars.Find(x => x.Player.name == name));
                    chars[index].Speed *= SpeedBoost;
                }
                break;
            case 2:
                if (IsBonus == 0)
                {
                    foreach (var item in chars)
                    {
                        if (item.Player.name != name)
                        {
                            item.invert = !item.invert;
                        }
                    }
                }
                else
                {
                    index = chars.IndexOf(chars.Find(x => x.Player.name == name));
                    chars[index].invert = !chars[index].invert;
                }

                break;
            case 3:
                if (IsBonus == 0)
                {
                    foreach (var item in chars)
                    {
                        if (item.Player.name != name)
                        {
                            item.rb.gravityScale = -item.rb.gravityScale;
                            item.transform.localScale = new Vector3(item.transform.localScale.x, -item.transform.localScale.y, item.transform.localScale.z);
                        }
                    }
                }
                else
                {
                    index = chars.IndexOf(chars.Find(x => x.Player.name == name));
                    chars[index].rb.gravityScale = -chars[index].rb.gravityScale;
                    chars[index].transform.localScale = new Vector3(chars[index].transform.localScale.x, -chars[index].transform.localScale.y, chars[index].transform.localScale.z);
                }
                break;
            default:
                break;
        }
    }

}
