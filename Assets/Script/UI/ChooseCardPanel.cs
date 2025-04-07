using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCardPanel : MonoBehaviour
{
    public GameObject cards;
    public GameObject beforeCardPre;
    public List<GameObject> chooseCardList;
    // Start is called before the first frame update
    void Start()
    {
        chooseCardList = new List<GameObject>();
        for (int i = 0; i < 8; i++)
        {
            GameObject beforeCard = Instantiate(beforeCardPre);
            beforeCard.transform.SetParent(cards.transform, false);
            beforeCard.name = "BeforeCard" + i.ToString();
            beforeCard.transform .Find("Bg").gameObject.SetActive(false);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCardPosition()
    {
        for (int i = 0; i < chooseCardList.Count; i++)
        {
            GameObject useCard = chooseCardList[i];
            Transform targetObj = cards.transform.Find("BeforeCard" + i.ToString());
            //Vector2 targetPos = targetObj.gameObject.GetComponent<RectTransform>().anchoredPosition;
            //Debug.Log($"�ؼЦ�mtargetObj.position : {targetObj.position}");
            //Debug.Log($"�ؼЦ�mtargetPos : {targetPos}");
            Vector3 curPos = useCard.transform.position;
            //���]�w������
            useCard.transform.SetParent(targetObj, false);
            useCard.transform.position = curPos;
            useCard.GetComponent<Card>().isMoving = true;

            //�ϥ�DoMove����
            useCard.transform.DOMove(new Vector3(targetObj.position.x - 25, targetObj.position.y + 29, 0), 0.3f).OnComplete(() =>
            {
                ////���ʧ���]�w������
                //useCard.transform.SetParent(targetObj, false);
                //���s�]�w�d����m
                //useCard.transform.localPosition = Vector3.zero;
                useCard.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                //���ʧ���ѼƧ���
                useCard.GetComponent<Card>().isMoving = false;
            });
        }

    }
}
