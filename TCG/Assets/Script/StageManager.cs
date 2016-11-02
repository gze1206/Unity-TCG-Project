using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum MobType
{
    None = 0,
    DMGtoMinion,
    KillMinion,
}

public class StageManager
{
    private static List<Stage> StageList = new List<Stage>();
    private static StageManager instance;
    private static int Index = 0;

    public static StageManager Get()
    {
        if (instance == null) instance = new StageManager();
        return instance;
    }

    public void AddStage(Stage stg)
    {
        StageList.Add(stg);
    }

    public void DeleteStage(Stage stg)
    {
        StageList.Remove(stg);
    }

    public List<Stage> GetList()
    {
        return StageList;
    }

    public Stage this[int index]
    {
        get
        {
            return StageList[index];
        }
        set
        {
            StageList[index] = value;
        }
    }

    public Stage GetNow()
    {
        return StageList[Index];
    }

    public Stage NextStage()
    {
        return StageList[++Index];
    }

}

public class Stage
{
    public Character Boss;
    public int DMG, DMG_Range;
    public List<MobType> Type;
    public string type_str;

    public Stage(int mHP, int nHP, string NAME, int dmg, int dmg_range, List<MobType> list)
    {
        Boss = new Character(mHP, nHP, NAME);
        DMG = dmg;
        DMG_Range = dmg_range;
        Type = (list.Count == 0) ? new List<MobType>() : list;
        type_str = string.Empty;
        foreach (MobType tp in Type)
        {
            type_str += tp.ToString() + "\r\n";
        }
    }

    public void TurnEnd()
    {
        Camera.main.GetComponent<GameManager>().Player.NowHP -= (DMG + Random.Range(-DMG_Range, DMG_Range));
        Camera.main.GetComponent<GameManager>().DrawUI();
        if (Type.Contains(MobType.None)) return;
        if (Type.Contains(MobType.DMGtoMinion))
        {
            int temp = Camera.main.GetComponent<GameManager>().Field.Count;
            for (int i = 0; i<temp; i++)
            {
                if (Camera.main.GetComponent<GameManager>().Field[i].GetComponent<CardSlot>().Data.Type == CardType.Magic) continue;
                Camera.main.GetComponent<GameManager>().Field[i].GetComponent<CardSlot>().Data.Health -= (DMG + Random.Range(-DMG_Range, DMG_Range));
                //if (Camera.main.GetComponent<GameManager>().Field[i].GetComponent<CardSlot>().Data.Health <= 0)
                //    GameObject.Destroy(Camera.main.GetComponent<GameManager>().Field[i]);
            }
        }
    }
}