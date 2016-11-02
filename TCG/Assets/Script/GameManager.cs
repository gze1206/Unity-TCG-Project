using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public enum DrawList
{
    FromMinion = 0,
    FromMagic,
    FromAllList
}

public enum Result
{
    /// <summary>
    /// 정상 종료
    /// </summary>
    S_OK = 0,

    FullHand,

    /// <summary>
    /// Unknown Error
    /// </summary>
    E_FAIL
}

public class GameManager : MonoBehaviour
{
    public List<CardData> MinionList, MagicList, TempList;
    public Character Player;
    public Character Boss;
    public GameObject EnemyInfo, EnemyState, PlayerState, PlayerName;
    public GameObject AttackBtn, DrawBtn;
    public GameObject cardSlot;
    public GameObject gameOverWin;
    public List<GameObject> Slot = new List<GameObject>(), Field = new List<GameObject>();
    public DrawList drawList = DrawList.FromAllList;
    public int CardCnt = 0;
    bool IsGameOver = false;
	
    public void Start()
    {
        StageManager.Get().AddStage(new Stage(10, 10, "STAGE1", 5, 1, new List<MobType>() { MobType.None }));
        StageManager.Get().AddStage(new Stage(30, 30, "STAGE2", 5, 3, new List<MobType>() { MobType.DMGtoMinion }));
        StageManager.Get().AddStage(new Stage(100, 100, "STAGE3", 10, 5, new List<MobType>() { MobType.KillMinion }));
        Boss = StageManager.Get().GetNow().Boss;
        DrawBtn.SetActive(false);
        MinionList = new List<CardData>();
        MagicList = new List<CardData>();
        GetComponent<XML_Parser>().ParseCard();
        Player = new Character(30, 30, "YOU");
        DrawUI();
        for (int i = 0; i<cardSlot.transform.childCount; i++)
        {
            Slot.Add(cardSlot.transform.GetChild(i).gameObject);
        }
    }

    public void InitializeGame()
    {
        DrawCard();
        DrawCard();
        DrawCard();
        DrawBtn.SetActive(true);
    }

    public Result DrawCard()
    {
        if (CardCnt < Slot.Count)
        {
            switch (drawList)
            {
                case DrawList.FromMinion:
                    TempList = MinionList;
                    break;
                case DrawList.FromMagic:
                    TempList = MagicList;
                    break;
                case DrawList.FromAllList:
                    TempList = (Random.Range(0, 2) == 0) ? MagicList : MinionList;
                    break;
            }
            int temp = GetEmptySlot();
            if (temp == -1) return Result.FullHand;
            Slot[temp].GetComponent<CardSlot>().Data = TempList[Random.Range(0, TempList.Count)];
            Debug.Log(Slot[temp].GetComponent<CardSlot>().Data.Name);
            Slot[temp].gameObject.GetComponent<Image>().color = new Color(255,255,255,255);
            CardCnt++;
        }
        else
        {
            Player.NowHP -= 2;
        }
        DrawUI();
        return Result.S_OK;
    }

    public int GetEmptySlot()
    {
        int dest = 0;
        for (int i = 0; i < Slot.Count; i++)
        {
            if (Slot[i].GetComponent<CardSlot>().Data.Name.Equals(string.Empty))
            {
                dest = i;
                return dest;
            }
        }
        return -1;
    }

    public void OnClickedDraw()
    {
        if (!IsGameOver)
        {
            Player.NowHP -= 2;
            DrawCard();
            DrawUI();
            DrawBtn.SetActive(false);
            AttackBtn.SetActive(true);
        }
    }

    public void OnClickedAttack()
    {
        if (!IsGameOver)
        {
            DrawUI();
            DrawBtn.SetActive(true);
            AttackBtn.SetActive(false);
            for (int i = 0; i < Field.Count; i++)
            {
                Field[i].GetComponent<CardSlot>().Attack();
            }
            if (Boss.NowHP <= 0)
            {
                NextStage();
            }
            else StageManager.Get().GetNow().TurnEnd();
            DestroyDeadCard();
        }
    }

    public void DestroyDeadCard()
    {
        for (int i = 0; i<Field.Count; i++)
        {
            if (Field[i].GetComponent<CardSlot>().Data.Type == CardType.Minion && Field[i].GetComponent<CardSlot>().Data.Health <= 0) Destroy(Field[i--]);
        }
    }

    public void DrawUI()
    {
        PlayerName.GetComponent<Text>().text = Player.Name;
        PlayerState.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = Player.NowHP.ToString();
        PlayerState.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "/ " + Player.MaxHP.ToString();
        EnemyInfo.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = Boss.Name;
        EnemyInfo.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = StageManager.Get().GetNow().type_str;
        EnemyState.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = Boss.NowHP.ToString();
        EnemyState.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "/ " + Boss.MaxHP.ToString();
        if (IsGameOver) gameOverWin.SetActive(true);
        else gameOverWin.SetActive(false);
    }

    public int Heal(Character ch, int _hp)
    {
        int ret;
        if (ch.NowHP + _hp > ch.MaxHP + ch.MaxHP * 0.25f)
            ret = Mathf.FloorToInt(ch.MaxHP + ch.MaxHP * 0.25f);
        else
            ret = ch.NowHP + _hp;

        return ret;
    }

    public void Update()
    {
        if (Player.NowHP <= 0) IsGameOver = true;

        if ( IsGameOver )
        {
            DrawUI();
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))) Application.Quit();
        }
    }

    public GameObject DeleteFromSlot(GameObject obj)
    {
        GameObject temp = obj;
        Slot.Remove(obj);
        return temp;
    }

    public void InsertToSlot(GameObject obj)
    {
        Slot.Add(obj);
    }

    public Result DeleteAndInsertToSlot(GameObject del, GameObject ins)
    {
        int a = Slot.IndexOf(del);
        Slot.Insert(a, ins);
        Slot.Remove(del);
        return Result.S_OK;
    }

    public void AddToField(GameObject obj)
    {
        Field.Add(obj);
    }

    public void DeleteFromField(GameObject obj)
    {
        Field.Remove(obj);
    }

    public void NextStage()
    {
        StageManager.Get().NextStage();
        Boss = StageManager.Get().GetNow().Boss;
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