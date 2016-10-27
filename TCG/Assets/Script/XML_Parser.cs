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
    public IEnumerator Parse(ParseData data)
    {
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
        Interpret(www.text, data.list);
    }

    public void ParseCard()
    {
        ParseData dat = new ParseData();
        dat.FileName = "CardDB.xml";
        dat.list = GetComponent<GameManager>().CardList;
        StartCoroutine(Parse(dat));
    }

    private void Interpret(string Source, List<CardData> list)
    {
        StringReader strReader = new StringReader(Source);
        string path = string.Empty;
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN)
        path += (Application.streamingAssetsPath + "/CardDB.xml");
#elif UNITY_ANDROID
        path += "jar:file://" + Application.dataPath + "!/assets/CardDB.xml");
#endif
        StreamReader reader = new StreamReader(path, new UTF8Encoding(false));
        XmlNodeList nodeList = null;
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(reader.ReadToEnd());
        nodeList = xmlDoc.SelectNodes("CardList");

        for (int i = 0; i<nodeList.Count; i++)
        {
            XmlNode node = nodeList[i];
            if (node.Name.Equals("CardList") && node.HasChildNodes)
            {
                for (int j = 0; j<node.ChildNodes.Count; j++)
                {
                    XmlAttributeCollection child = node.ChildNodes[j].Attributes;
                    CardData data = new CardData();
                    data.Name = child.GetNamedItem("Name").Value;
                    data.Explane = child.GetNamedItem("Explane").Value;
                    data.Type = (CardType)int.Parse(child.GetNamedItem("Type").Value);
                    data.EventName = child.GetNamedItem("EventName").Value;
                    list.Add(data);
#if UNITY_EDITOR
                    Debug.Log(data.Name + "/" + data.Explane + "/" + data.Type.ToString() + "/" + data.EventName);
#endif
                }
            }
        }
    }
	
}
