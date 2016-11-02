using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardEffect : MonoBehaviour
{
    public IEnumerator GOD_CARD(GameObject obj)
    {
        Camera.main.GetComponent<GameManager>().Boss.NowHP -= obj.GetComponent<CardSlot>().Data.Attack;
        Camera.main.GetComponent<GameManager>().DrawUI();
        if (obj.GetComponent<CardSlot>().Data.Health == 0)
        {
            Destroy(obj);
        }
        yield return null;
    }

    public IEnumerator MgmCard(GameObject obj)
    {
        Camera.main.GetComponent<GameManager>().Boss.NowHP -= obj.GetComponent<CardSlot>().Data.Attack;
        Camera.main.GetComponent<GameManager>().DrawUI();
        List<GameObject> list = Camera.main.GetComponent<GameManager>().Field;
        int temp = 0;
        for (int i = 0; i<list.Count; i++)
        {
            if (list[i].GetComponent<CardSlot>().Data.Attack <= 1) temp++;
        }
        if (temp == 0)
            Destroy(obj);
        yield return null;
    }

    public IEnumerator SprintCard(GameObject obj)
    {
        List<GameObject> list = Camera.main.GetComponent<GameManager>().Field;
        int temp = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].GetComponent<CardSlot>().Data.Type == CardType.Minion) temp++;
        }
        Camera.main.GetComponent<GameManager>().Boss.NowHP -= temp;
        Camera.main.GetComponent<GameManager>().DrawUI();
        Destroy(obj);
        yield return null;
    }

    public IEnumerator RenoJackson(GameObject obj)
    {
        List<GameObject> list = Camera.main.GetComponent<GameManager>().Slot;
        Debug.Log(list.Count);
        bool temp = true;
        for (int i = 0; i<list.Count; i++)
        {
            for (int j = 0; j<list.Count; j++)
            {
                if (list[i].GetComponent<CardSlot>().Data.Name.Equals(string.Empty)) continue;
                if (list[j].GetComponent<CardSlot>().Data.Name.Equals(string.Empty)) continue;
                temp = (list[i].GetComponent<CardSlot>().Data.Name.Equals(list[j].GetComponent<CardSlot>().Data.Name) && i != j);
                Debug.Log("I : " + list[i].GetComponent<CardSlot>().Data.Name + " " + i + " J : " + list[j].GetComponent<CardSlot>().Data.Name + " " + j + " | " + temp);
            }
        }
        //Debug.Log((temp) ? "부우자" : "실패");
        if (!temp)
        {
            Camera.main.GetComponent<GameManager>().Player.NowHP = Camera.main.GetComponent<GameManager>().Player.MaxHP;
            Camera.main.GetComponent<GameManager>().DrawUI();
        }
        Destroy(obj);
        yield return null;
    }

    public IEnumerator HealthTwice(GameObject obj)
    {
        int temp = Camera.main.GetComponent<GameManager>().Field.Count;

        for (int i = 0; i<temp; i++)
        {
            CardSlot dest = Camera.main.GetComponent<GameManager>().Field[i].GetComponent<CardSlot>();
            if (dest.Data.Name.Equals(string.Empty) || dest.Data.Type == CardType.Magic) continue;
            else dest.Data.Health *= 2;
        }
        Destroy(obj);
        yield return null;
    }
}