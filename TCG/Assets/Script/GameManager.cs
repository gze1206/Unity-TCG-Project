using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<CardData> CardList;
    public Character Player, Boss;
    public GameObject EnemyInfo, EnemyState, PlayerState, PlayerName;
    public GameObject AttackBtn, DrawBtn;
    public CardSlot[] Slot = new CardSlot[5];
    int CardCnt = 0;
	
    public void Start()
    {
        CardList = new List<CardData>();
        GetComponent<XML_Parser>().ParseCard();
        Player = new Character(30, 30, "YOU");
        DrawUI();
    }

    public void DrawCard()
    {
        if (CardCnt < 5)
        {
            Slot[CardCnt].Data = CardList[Random.Range(0, CardList.Count)];
            Debug.Log(Slot[CardCnt].Data.Name);
            Slot[CardCnt].gameObject.GetComponent<Image>().color = new Color(255,255,255,255);
            CardCnt++;
        }
        else
        {
            Damege(Player, 2);
        }
        DrawUI();
    }

    public void OnClickedDraw()
    {
        Damege(Player, 2);
        DrawCard();
        DrawUI();
        //DrawBtn.SetActive(false);
        //AttackBtn.SetActive(true);
    }

    public void OnClickedAttack()
    {

    }

    public void DrawUI()
    {
        PlayerName.GetComponent<Text>().text = Player.Name;
        PlayerState.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = Player.NowHP.ToString();
        PlayerState.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "/ " + Player.MaxHP.ToString();
        EnemyInfo.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = Boss.Name;
        EnemyInfo.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "TYPE";
        EnemyState.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = Boss.NowHP.ToString();
        EnemyState.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "/ " + Boss.MaxHP.ToString();
    }

    public void Heal(Character ch, int _hp)
    {
        if (ch.NowHP + _hp > ch.MaxHP + ch.MaxHP * 0.25f)
            ch.NowHP = Mathf.FloorToInt(ch.MaxHP + ch.MaxHP * 0.25f);
        else
            ch.NowHP = ch.NowHP + _hp;

        EnemyState.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = Boss.NowHP.ToString();
        EnemyState.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "/ " + Boss.MaxHP.ToString();
        PlayerState.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = Player.NowHP.ToString();
        PlayerState.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "/ " + Player.MaxHP.ToString();
    }

    public void Damege(Character ch, int _hp)
    {
        if (ch.NowHP - _hp <= 0)
        {
            ch.NowHP -= _hp;
            GameOver();
        }
        else
            ch.NowHP -= _hp;

        EnemyState.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = Boss.NowHP.ToString();
        EnemyState.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "/ " + Boss.MaxHP.ToString();
        PlayerState.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = Player.NowHP.ToString();
        PlayerState.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "/ " + Player.MaxHP.ToString();
    }

    public void GameOver()
    {
        while (!Input.anyKeyDown) ;
        Application.Quit();
    }
}

public struct Character
{
    public int MaxHP, NowHP;
    public string Name;

    public Character(int mHP, int nHP, string sNM)
    {
        MaxHP = mHP;
        NowHP = nHP;
        Name = sNM;
    }
}