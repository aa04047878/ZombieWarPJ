using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllCardPanel : MonoBehaviour
{
    public GameObject cardPrefab;
    public GameObject Bg;
    private Button btnStart;
    // Start is called before the first frame update
    void Start()
    {
        btnStart = UITool.GetUIComponent<Button>(this.gameObject, "BtnStart");
        btnStart.onClick.AddListener(() => OnButtonStart());
        for (int i = 0; i < 40; i++)
        {
            GameObject beforeCard = Instantiate(cardPrefab);
            beforeCard.transform.SetParent(Bg.transform, false);
            beforeCard.name = "Card" + i.ToString();
            Debug.Log($"生成Card{i}");
        }

        Debug.Log("InitCard 初始化卡片");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitCard()
    {
        foreach (PlantInfoItem plantInfoItem in GameManager.instance.plantInfo.plantInfoList)
        {
            Transform cardParent = Bg.transform.Find("Card" + plantInfoItem.plantId.ToString());
            GameObject reallyCard = Instantiate(plantInfoItem.cardPrefab);
            reallyCard.transform.SetParent(cardParent, false);
            reallyCard.GetComponent<Card>().plantInfo = plantInfoItem;
            reallyCard.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            //reallyCard.transform.localScale = Vector2.zero;
            reallyCard.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 69);
            reallyCard.name = "BeforeCard";
        }
    }

    public void OnButtonStart()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        //把anchoredPosition 轉成世界座標
        Vector3 curWorldPosition = rectTransform.TransformPoint(rectTransform.anchoredPosition);
        Vector3 newWorldPosition = rectTransform.TransformPoint(new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y - 500));
        Debug.Log($"curWorldPosition : {curWorldPosition}");
        Debug.Log($"worldPosition : {newWorldPosition}");
        transform.DOMove(newWorldPosition, 2f);
        //發送遊戲開始通知
        EventCenter.Instance.EventTrigger(EventType.eventGameStart);
        UIManager.instance.OpenBeam();
    }
}
