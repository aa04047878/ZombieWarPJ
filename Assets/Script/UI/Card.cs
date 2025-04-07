using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Card : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    private RectTransform card;
    /// <summary>
    /// 卡片的預置物
    /// </summary>
    public GameObject objectprefab;
    /// <summary>
    /// 生成出來的卡片物件
    /// </summary>
    public GameObject curGameObject;
    /// <summary>
    /// 變暗背景(冷卻中的圖示)
    /// </summary>
    private GameObject darkBg;
    /// <summary>
    /// 進度條
    /// </summary>
    private GameObject progressBar;
    /// <summary>
    /// 此卡片需要的陽光數量
    /// </summary>
    public int useSun;
    public float waitTime;
    private float timer;
    public PlantInfoItem plantInfo;
    /// <summary>
    /// 卡片已在上方選卡欄位裡面
    /// </summary>
    public bool hasUse;
    /// <summary>
    /// 卡片已使用就會上鎖
    /// </summary>
    public bool hasLook;
    public bool isMoving;
    /// <summary>
    /// 卡片是否初始化
    /// </summary>
    public bool hasStart;
    // Start is called before the first frame update
    void Start()
    {
        darkBg = transform.Find("DarkBg").gameObject;
        progressBar = transform.Find("ProgressBar").gameObject;
        card = GetComponent<RectTransform>();

        //遊戲還沒開始不應該執行
        darkBg.SetActive(false);
        progressBar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.gameStart)
            return;

        if (!hasStart)
        {
            //初始化卡片
            hasStart = true;
            //顯示進度條
            progressBar.SetActive(true);
            //顯示暗背景
            darkBg.SetActive(true);
        }

        timer += Time.deltaTime;
        UpdateProgress();
        UpdateDarkBg();
    }

    /// <summary>
    /// 更新進度條
    /// </summary>
    private void UpdateProgress()
    {
        float per = Mathf.Clamp(timer / waitTime, 0, 1);

        //進度條剩餘時間比例
        progressBar.GetComponent<Image>().fillAmount = 1 - per;
    }

    /// <summary>
    /// 更新暗背景
    /// </summary>
    private void UpdateDarkBg()
    {
        //檢查是否冷卻完成 && 陽光數量足夠
        if (progressBar.GetComponent<Image>().fillAmount == 0 && GameManager.instance.sunNum >= useSun)
        {
            //冷卻完成，隱藏暗背景
            darkBg.SetActive(false);
        }
        else
        {
            //冷卻中，顯示暗背景
            darkBg.SetActive(true);
        }
    }

    public void OnBeginDrag(PointerEventData Data)
    {
        if (!hasStart)
            return;

        //顯示暗背景，代表不能拖曳(種植條件不足)
        if (darkBg.activeSelf)
        {
            return;
        }
        //開始拖曳時執行(滑鼠點下的那一瞬間，點擊此物體)
        Debug.Log("開始拖曳時執行(滑鼠點下的那一瞬間)" + Data.ToString());

        curGameObject = Instantiate(objectprefab);
        //播放點擊卡片的聲音
        SoundManager.instance.PlaySound(Globals.S_Seedlift);
    }

    public void OnDrag(PointerEventData Data)
    {   
        //顯示暗背景，代表不能拖曳(種植條件不足)
        if (darkBg.activeSelf)
        {
            return;
        }

        //正在拖曳時執行(滑鼠按住不放)
        Debug.Log("正在拖曳時執行(滑鼠按住不放)");
        if (curGameObject == null)
            return;
        
        // 獲取滑鼠在螢幕上的位置
        Vector3 screenPosition = Input.mousePosition;

        // 如果是 3D 場景，必須提供正確的 Z 深度
        screenPosition.z = 10f; // 設置深度（距離攝影機的距離）

        // 將螢幕座標轉換為世界座標
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        //拖曳植物
        curGameObject.transform.position = worldPosition;
        
    }

    public void OnEndDrag(PointerEventData Data)
    {
        //顯示暗背景，代表不能拖曳(種植條件不足)
        if (darkBg.activeSelf)
        {
            return;
        }
        //結束拖曳時執行(滑鼠放開時執行)
        Debug.Log("結束拖曳時執行(滑鼠放開時執行)");
        //檢查滑鼠位置的世界座標(curGameObject的位置)是否有碰撞體
        Collider2D[] colliders = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        foreach (var collider in colliders)
        {
            //屬標碰到的碰撞體有地板
            if (collider.tag == "Land")
            {
                //且地板上沒有其他物件
                if (collider.transform.childCount == 0)
                {
                    Debug.Log("碰撞到的物件名稱：" + collider.name);
                    //執行放置卡片
                    curGameObject.transform.position = collider.transform.position;
                    //播放種植音效
                    SoundManager.instance.PlaySound(Globals.S_Plant);
                    //將卡片設定為地板的子物件
                    curGameObject.transform.parent = collider.transform;
                    //種植完成後開始啟動植物
                    curGameObject.GetComponent<Plant>().SetPlantStart();
                    //消耗陽光
                    GameManager.instance.ChangeSunNum(-useSun);
                    //重置計時器
                    timer = 0;
                    darkBg.SetActive(true);
                    //清空curGameObject
                    curGameObject = null;
                    break;
                }
                else
                {
                    //地板上已有其他物件，執行銷毀卡片
                    Debug.Log("地板上已有其他物件，刪除卡片");
                    Destroy(curGameObject);
                    //curGameObject = null;
                }
            }
            else
            {
                //沒有碰撞到地板，執行銷毀卡片
                Debug.Log("沒有碰撞Tag為Land，刪除卡片");
                Destroy(curGameObject);
                //curGameObject = null;
            }

        }

        //什麼都沒碰到，執行銷毀卡片
        if (colliders.Length == 0)
        {
            Debug.Log("什麼都沒碰到，刪除卡片");
            Destroy(curGameObject);
            //curGameObject = null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //點擊卡片時執行

        //正在移動時不做事情
        if (isMoving)
            return;

        if (hasLook)
            return;

        if (hasUse)
            RemoveCard(gameObject);
        else
            AddCard(gameObject);

    }

    public void RemoveCard(GameObject removeCard)
    {
        ChooseCardPanel chooseCardPanel = UIManager.instance.chooseCardPanel;
       
        if (chooseCardPanel.chooseCardList.Contains(removeCard))
        {
            //先移除資料
            chooseCardPanel.chooseCardList.Remove(removeCard);
            removeCard.GetComponent<Card>().isMoving = true;
            UIManager.instance.chooseCardPanel.UpdateCardPosition();

            //在移動卡片
            Transform cardParent = UIManager.instance.allCardPanel.Bg.transform.Find("Card" + removeCard.GetComponent<Card>().plantInfo.plantId.ToString());
            Vector3 curPos = removeCard.transform.position;
            removeCard.transform.SetParent(UIManager.instance.transform, false);
            removeCard.transform.position = curPos; //不把位置先記下來，設定父物件後位置會跑掉
            removeCard.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 69);

            //使用DoMove移動
            removeCard.transform.DOMove(new Vector3(cardParent.position.x - 25, cardParent.position.y + 26, cardParent.position.z), 0.3f).OnComplete(() =>
            {
                cardParent.Find("BeforeCard").GetComponent<Card>().darkBg.SetActive(false);
                cardParent.Find("BeforeCard").GetComponent<Card>().hasLook = false;
                //移動完後刪除物件
                removeCard.GetComponent<Card>().isMoving = false;
                Destroy(removeCard);
            });
        }

        
    }

    public void AddCard(GameObject gameObject)
    {
        ChooseCardPanel chooseCardPanel = UIManager.instance.chooseCardPanel;
        Debug.Log($"顯示位置 : {chooseCardPanel.transform.position}");
        int curIndex = chooseCardPanel.chooseCardList.Count;
        Debug.Log($"當前選卡欄位數量 : {curIndex}");
        //如果選卡欄位已滿，則不執行
        if (curIndex >= 8)
        {
            Debug.Log("選卡欄位已滿，無法添加卡片");
            return;
        }

        hasLook = true;
        darkBg.SetActive(true);
        //如果選卡欄位不滿，則添加卡片
        GameObject useCard = Instantiate(plantInfo.cardPrefab);
        useCard.transform.SetParent(UIManager.instance.transform);
        useCard.transform.position = transform.position;
        useCard.GetComponent<RectTransform>().sizeDelta = new Vector2(41, 56);
        useCard.name = "Card";
        // 資訊拷貝
        useCard.GetComponent<Card>().plantInfo = plantInfo;
        //移動到目標位置
        Transform targetObject = chooseCardPanel.cards.transform.Find("BeforeCard" + curIndex);
        Debug.Log("目標位置：" + targetObject.name);
        useCard.GetComponent<Card>().isMoving = true;
        useCard.GetComponent<Card>().hasUse = true;
        chooseCardPanel.chooseCardList.Add(useCard);

        //使用DoTwing的DoMove方法移動到目標位置
        useCard.transform.DOMove(new Vector3(targetObject.position.x - 25, targetObject.position.y + 26, targetObject.position.z), 0.3f).OnComplete(() =>
        {
            //移動完成後，將卡片的父物件設置為選卡欄位
            useCard.transform.SetParent(targetObject);
            //useCard.transform.localPosition = Vector2.zero;
            useCard.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            useCard.GetComponent<Card>().isMoving = false;
        });
    }
}
