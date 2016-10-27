using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour
{
    public int Index;
    public CardData Data;

    public void SetData(CardData Data)
    {
        this.Data = Data;
    }

    public void SetData(string name, string explane, CardType type, string eventName)
    {
        Data = new CardData(name, explane, type, eventName);
    }
}

public enum CardType
{
    Magic = 0,
    Minion,
}

public struct CardData
{
    public string Name;
    public string Explane;
    public CardType Type;
    public string EventName;

    public CardData(string name, string explane, CardType type, string eventName)
    {
        Name = name;
        Explane = explane;
        Type = type;
        EventName = eventName;
    }
}