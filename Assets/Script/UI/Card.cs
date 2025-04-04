using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Card : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
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
    // Start is called before the first frame update
    void Start()
    {
        darkBg = transform.Find("DarkBg").gameObject;
        progressBar = transform.Find("ProgressBar").gameObject;
        card = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
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

    
}
