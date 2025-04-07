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
            //Debug.Log($"目標位置targetObj.position : {targetObj.position}");
            //Debug.Log($"目標位置targetPos : {targetPos}");
            Vector3 curPos = useCard.transform.position;
            //先設定父物件
            useCard.transform.SetParent(targetObj, false);
            useCard.transform.position = curPos;
            useCard.GetComponent<Card>().isMoving = true;

            //使用DoMove移動
            useCard.transform.DOMove(new Vector3(targetObj.position.x - 25, targetObj.position.y + 29, 0), 0.3f).OnComplete(() =>
            {
                ////移動完後設定父物件
                //useCard.transform.SetParent(targetObj, false);
                //重新設定卡片位置
                //useCard.transform.localPosition = Vector3.zero;
                useCard.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                //移動完後參數改變
                useCard.GetComponent<Card>().isMoving = false;
            });
        }

    }
}
