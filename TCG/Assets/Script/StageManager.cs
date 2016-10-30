using UnityEngine;
using System.Collections.Generic;

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

    public Stage(int mHP, int nHP, string NAME, int dmg, int dmg_range)
    {
        Boss = new Character(mHP, nHP, NAME);
        DMG = dmg;
        DMG_Range = dmg_range;
    }

    public void TurnEnd()
    {
        Camera.main.GetComponent<GameManager>().Player.NowHP -= (DMG + Random.Range(-DMG_Range, DMG_Range));
        Camera.main.GetComponent<GameManager>().DrawUI();
    }
}