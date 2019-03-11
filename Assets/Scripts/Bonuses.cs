using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonuses : MonoBehaviour
{
    private bool isOnUpdate = false, isOnCollisionEnter = false;

    private int BonusType = 0, IsBonus = 0;

    private SpriteRenderer sp;
    private BoxCollider2D bc2D;

    private bool StartTimer;
    private float TimeBonus = 3, TimeSpent = 0;

    private int SpeedBoost = 2;
    private int GrowthBoost = 2;

    private string collideName;
    
    [SerializeField] private Sprite[] BonusSprites;

    // Start is called before the first frame update
    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
        bc2D = GetComponent<BoxCollider2D>();

        BonusType = Random.Range(1, BonusSprites.Length);
        IsBonus = Random.Range(0, 2);
        GetBonusType(BonusType, IsBonus);
    }

    private void GetBonusType(int bt, int IsBonus)
    {
        switch (bt)
        {
            case 1:
                sp.sprite = BonusSprites[0];
                break;
            case 2:
                sp.sprite = BonusSprites[1];
                break;
            case 3:
                sp.sprite = BonusSprites[2];
                break;
            case 4:
                sp.sprite = BonusSprites[3];
                break;
            default:
                sp.sprite = BonusSprites[4];
                BonusType = Random.Range(1, BonusSprites.Length - 1);
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
    private void Update()
    {
        isOnUpdate = true; isOnCollisionEnter = false;
        if (StartTimer)
        {
            string a = this.transform.name;
            TimeSpent += Time.deltaTime;
        }
        if (TimeSpent > TimeBonus)
        {
            SwitchBonusType();
            this.enabled = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOnUpdate = false; isOnCollisionEnter = true;
        StartTimer = true;
        sp.enabled = false;
        bc2D.enabled = false;
        List<Character> chars = SceneScript.characters;
        collideName = collision.transform.name;
        SwitchBonusType();
    }

    private void SwitchBonusType()
    {
        List<Character> chars = SceneScript.characters;
        switch (BonusType)
        {
            case 1:
                CaseSpeed(chars);
                break;
            case 2:
                CaseInvertControl(chars);
                break;
            case 3:
                CaseSwitchGravity(chars);
                break;
            case 4:
                CaseGrowth(chars);
                break;
            default:
                break;
        }
    }
    private void CaseSpeed(List<Character> chars)
    {
        int index = 0;
        if (IsBonus == 0)
        {
            foreach (var item in chars)
            {
                if (item.Player.name != collideName)
                {
                    if (isOnCollisionEnter)
                    {
                        item.Speed /= SpeedBoost;
                    }
                    if (isOnUpdate)
                    {
                        item.Speed *= SpeedBoost;
                    }
                }
            }
        }
        else
        {
            index = chars.IndexOf(chars.Find(x => x.Player.name == collideName));

            if (isOnCollisionEnter)
            {
                chars[index].Speed *= SpeedBoost;
            }
            if (isOnUpdate)
            {
                chars[index].Speed /= SpeedBoost;
            }
        }
    }
    private void CaseInvertControl(List<Character> chars)
    {
        int index = 0;
        if (IsBonus == 0)
        {
            foreach (var item in chars)
            {
                if (item.Player.name != collideName)
                {
                    item.invert = !item.invert;
                }
            }
        }
        else
        {
            index = chars.IndexOf(chars.Find(x => x.Player.name == collideName));
            chars[index].invert = !chars[index].invert;
        }
    }
    private void CaseSwitchGravity(List<Character> chars)
    {
        int index = 0;
        if (IsBonus == 0)
        {
            foreach (var item in chars)
            {
                if (item.Player.name != collideName)
                {
                    item.rb.gravityScale = -item.rb.gravityScale;
                    item.transform.localScale = new Vector3(item.transform.localScale.x, -item.transform.localScale.y, item.transform.localScale.z);
                }
            }
        }
        else
        {
            index = chars.IndexOf(chars.Find(x => x.Player.name == collideName));
            chars[index].rb.gravityScale = -chars[index].rb.gravityScale;
            chars[index].transform.localScale = new Vector3(chars[index].transform.localScale.x, -chars[index].transform.localScale.y, chars[index].transform.localScale.z);
        }
    }
    private void CaseGrowth(List<Character> chars)
    {
        int index = 0;
        Vector3 Scale;
        if (IsBonus == 0)
        {
            foreach (var item in chars)
            {
                if (item.Player.name != collideName)
                {
                    Scale = item.Player.transform.localScale;
                    if (isOnCollisionEnter)
                    {
                        item.Player.transform.localScale = new Vector3(Scale.x * GrowthBoost, Scale.y * GrowthBoost);
                    }
                    if (isOnUpdate)
                    {
                        item.Player.transform.localScale = new Vector3(Scale.x / GrowthBoost, Scale.y / GrowthBoost);
                    }
                }
            }
        }
        else
        {
            index = chars.IndexOf(chars.Find(x => x.Player.name == collideName));

            Scale = chars[index].Player.transform.localScale;
            if (isOnCollisionEnter)
            {
                chars[index].Player.transform.localScale = new Vector3(Scale.x * GrowthBoost, Scale.y * GrowthBoost);

            }
            if (isOnUpdate)
            {
                chars[index].Player.transform.localScale = new Vector3(Scale.x / GrowthBoost, Scale.y / GrowthBoost);
            }
        }
    }
}
