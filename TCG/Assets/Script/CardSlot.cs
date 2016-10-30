using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardSlot : MonoBehaviour
{
    public CardData Data = new CardData(string.Empty, string.Empty, CardType.Minion, string.Empty);
    public GameObject CardView, ATK, HP;
    bool IsPointOn = false, IsDrag = false, CanMove = true;
    Vector3 StartPoint;

    public void Start()
    {
        StartPoint = transform.position;
    }

    public void ViewCard()
    {
        if (Data.Name.Equals(string.Empty))
        {
            return;
        }
        else
        {
            CardView.transform.GetChild(0).gameObject.GetComponent<Text>().text = Data.Name;
            CardView.transform.GetChild(1).gameObject.GetComponent<Text>().text = Data.Explane;
            ATK.transform.GetChild(0).gameObject.GetComponent<Text>().text = Data.Attack.ToString();
            HP.transform.GetChild(0).gameObject.GetComponent<Text>().text = Data.Health.ToString();
        }
        IsPointOn = false;
    }

    public void MouseOn()
    {
        IsPointOn = true;
        if (!Data.Name.Equals(string.Empty) && !IsDrag)
        {
            bool temp = (Data.Type == CardType.Minion) ? true : false;
            CardView.SetActive(true);
            ATK.SetActive(temp);
            HP.SetActive(temp);
        }
    }

    public void MouseOff()
    {
        IsPointOn = false;
        CardView.SetActive(false);
    }

    public void DragStart()
    {
        if (CanMove)
        {
            MouseOff();
            IsDrag = true;
            if (!Data.Name.Equals(string.Empty)) transform.localPosition = new Vector3(StartPoint.x, StartPoint.y, -5.0f);
        }
    }

    public void DragEnd()
    {
        if (CanMove)
        {
            IsDrag = false;
            if (!Data.Name.Equals(string.Empty))
            {
                Ray ray = new Ray(transform.position, Vector3.forward); //레이캐스트 준비
                RaycastHit hit;                                         //레이캐스트 준비
                Debug.Log("1");
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Debug.Log("2");
                    if (hit.transform.CompareTag("Field"))
                    {
                        Debug.Log("소환 : " + Data.Name);
                        CanMove = false;
                        GameObject temp = (GameObject)Instantiate(gameObject, StartPoint, transform.rotation, transform.parent);
                        temp.GetComponent<Image>().color = new Color(0, 0, 0, 255);
                        Camera.main.GetComponent<GameManager>().DeleteAndInsertToSlot(gameObject, temp);
                        Camera.main.GetComponent<GameManager>().AddToField(gameObject);
                        Camera.main.GetComponent<GameManager>().CardCnt--;
                    }
                    else transform.position = StartPoint;
                }
                else transform.position = StartPoint;

                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0.0f);
            }
        }
    }
	
    public void Drag()
    {
        if (CanMove)
        {
            IsPointOn = false;
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -5.0f);
        }
    }

    public void Update()
    {
        if (IsPointOn && !IsDrag)
            ViewCard();
        if (IsDrag && !Data.Name.Equals(string.Empty) && CanMove)
            Drag();
    }

    public void Attack()
    {
        Camera.main.GetComponent<CardEffect>().StartCoroutine(Data.EventName, gameObject);
    }

    public void OnDestroy()
    {
        Camera.main.GetComponent<GameManager>().DeleteFromField(gameObject);
    }
}
