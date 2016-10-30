using UnityEngine;
using System.Collections;

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
    public int Attack, Health;

    public CardData(string name, string explane, CardType type,string eventName, int atk = 0, int hp = 0)
    {
        Name = name;
        Explane = explane;
        Type = type;
        EventName = eventName;
        Attack = atk;
        Health = hp;
    }
}