using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Generic;

public struct ParseData
{
    public string FileName;
    public List<CardData> list;
}

public class XML_Parser : MonoBehaviour
{

    public IEnumerator Parse(ParseData[] datas)
    {
        for (int i = 0; i < datas.Length; i++)
        {
            ParseData data = datas[i];
            string path = string.Empty;
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN)
            path += ("file:///" + Application.streamingAssetsPath + "/" + data.FileName);
#elif UNITY_ANDROID
        path += "jar:file://" + Application.dataPath + "!/assets/" + FileName);
#endif
            WWW www = new WWW(path);
            yield return www;
#if UNITY_EDITOR
            Debug.Log("Read Content : " + www.text);
#endif
            Interpret(www.text, data.list, data.FileName);
        }
        Camera.main.GetComponent<GameManager>().InitializeGame();
    }

    public void ParseCard()
    {
        ParseData[] dats = new ParseData[2];
        dats[0] = new ParseData();
        dats[1] = new ParseData();
        dats[0].FileName = "MinionDB.xml";
        dats[0].list = GetComponent<GameManager>().MinionList;
        dats[1].FileName = "MagicDB.xml";
        dats[1].list = GetComponent<GameManager>().MagicList;
        StartCoroutine(Parse(dats));
    }

    private void Interpret(string Source, List<CardData> list, string FileName)
    {
        string path = string.Empty;

#if (UNITY_EDITOR || UNITY_STANDALONE_WIN)
        path += (Application.streamingAssetsPath + "/" + FileName);
#elif UNITY_ANDROID
        path += "jar:file://" + Application.dataPath + "!/assets/" + FileName);
#endif

        StreamReader reader = new StreamReader(path, new UTF8Encoding(false));
        XmlNodeList nodeList = null;
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(reader.ReadToEnd());
        string temp = (FileName == "MinionDB.xml") ? "MinionList" : "MagicList";
        nodeList = xmlDoc.SelectNodes(temp);

        for (int i = 0; i<nodeList.Count; i++)
        {
            XmlNode node = nodeList[i];
            if (FileName.Equals("MinionDB.xml") && node.HasChildNodes)
            {
                for (int j = 0; j<node.ChildNodes.Count; j++)
                {
                    XmlAttributeCollection child = node.ChildNodes[j].Attributes;
                    CardData data = new CardData();
                    data.Name = child.GetNamedItem("Name").Value;
                    data.Explane = child.GetNamedItem("Explane").Value;
                    data.Type = CardType.Minion;
                    data.EventName = child.GetNamedItem("EventName").Value;
                    data.Attack = int.Parse(child.GetNamedItem("Attack").Value);
                    data.Health = int.Parse(child.GetNamedItem("Health").Value);
                    list.Add(data);
#if UNITY_EDITOR
                    Debug.Log("Card Loaded - 하수인 : " + data.Name + " | " + data.Attack.ToString() + " / " + data.Health.ToString());
#endif
                }
            }
            else if (FileName.Equals("MagicDB.xml") && node.HasChildNodes)
            {
                for (int j = 0; j < node.ChildNodes.Count; j++)
                {
                    XmlAttributeCollection child = node.ChildNodes[j].Attributes;
                    CardData data = new CardData();
                    data.Name = child.GetNamedItem("Name").Value;
                    data.Explane = child.GetNamedItem("Explane").Value;
                    data.Type = CardType.Magic;
                    data.EventName = child.GetNamedItem("EventName").Value;
                    data.Attack = 0;
                    data.Health = 0;
                    list.Add(data);
#if UNITY_EDITOR
                    Debug.Log("Card Loaded - 주문 : " + data.Name);
#endif
                }
            }
        }
    }
	
}
