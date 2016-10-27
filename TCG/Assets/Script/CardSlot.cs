using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardSlot : MonoBehaviour
{
    public CardData Data = default(CardData);
    public GameObject CardView;

    public void ViewCard()
    {
        if (Data.Equals(default(CardData)))
        {
            return;
        }
        else
        {
            CardView.transform.GetChild(0).gameObject.GetComponent<Text>().text = Data.Name;
            CardView.transform.GetChild(1).gameObject.GetComponent<Text>().text = Data.Explane;
        }
    }
	
}
