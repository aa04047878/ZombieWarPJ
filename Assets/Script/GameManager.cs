using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*
    GameManager : 
        全局屬性 : 影響大部分的物件，大部分的物件都需要認識的公共屬性
        1. 寫全局的變數
        2. 寫全局的方法
    */

    public static GameManager instance;
    /// <summary>
    /// 陽光數量
    /// </summary>
    public int sunNum;

    #region 殭屍
    /// <summary>
    /// 殭屍生成父物件
    /// </summary>
    public GameObject bornParent;
    /// <summary>
    /// 殭屍預置物
    /// </summary>
    public GameObject zombiePre;
    public float createZombieTime;
    private int zOrderIndex;
    /// <summary>
    /// 當前波次殭屍列表
    /// </summary>
    private List<GameObject> curProgressZombieList;
    #endregion

    #region 關卡相關
    public LevelData levelData;
    private bool gameStart;
    /// <summary>
    /// 關卡
    /// </summary>
    private int curLevelId;
    /// <summary>
    /// 波次
    /// </summary>
    private int curProgressId;
    #endregion


    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        curProgressZombieList = new List<GameObject>();
        curLevelId = 1;
        curProgressId = 1;
        StartCoroutine(CoLoadTable());
    }

    // Update is called once per frame
    void Update()
    {
        ClickSun();
    }

    /// <summary>
    /// 改變太陽數量
    /// </summary>
    public void ChangeSunNum(int changeNum)
    {
        sunNum += changeNum;

        if (sunNum <= 0)
        {
            sunNum = 0;
        }
        //更新UI
        UIManager.instance.UpdateUI();
    }

    /// <summary>
    /// 從表格創造殭屍
    /// </summary>
    private void TableCreateZombie()
    {
        int canCreateNum = 0;

        //檢查波次是否超過上限
        for (int i = 0; i < levelData.levelDataList.Count; i++)
        {
            LevelItem levelItem = levelData.levelDataList[i];
            if (levelItem.progressId >= curProgressId)
            {
                canCreateNum++;       
            }
        }

        //如果沒有波次了，則停止所有協程
        if (canCreateNum == 0)
        {
            StopAllCoroutines();
            curProgressZombieList.Clear();
            gameStart = false;
            return;
        }

        for (int i = 0; i < levelData.levelDataList.Count; i++)
        {
            LevelItem levelItem = levelData.levelDataList[i];
            if (levelItem.levelId == curLevelId && levelItem.progressId == curProgressId)
            {
                StartCoroutine(CoTableCreateZombie(levelItem)); 
            }
        }
    }

    IEnumerator CoTableCreateZombie(LevelItem levelItem)
    {
        yield return new WaitForSeconds(levelItem.createTime);
        //讀取殭屍預置物
        GameObject zombiePre = Resources.Load<GameObject>($"Prefab/Zombie{levelItem.zombieType}");
        //生成殭屍
        GameObject zombie = Instantiate(zombiePre);
        //設定父物件
        Transform bornLine = bornParent.transform.Find($"Born{levelItem.bornPos}");
        zombie.transform.parent = bornLine;
        //設定殭屍位置(沒設定位置的話預設值為世界座標(0, 0, 0))
        zombie.transform.localPosition = Vector3.zero;
        //設定顯示層級
        zOrderIndex++;
        zombie.GetComponent<SpriteRenderer>().sortingOrder = zOrderIndex;
        //加入當前波次殭屍列表
        curProgressZombieList.Add(zombie);  
    }

    /// <summary>
    /// 創造殭屍
    /// </summary>
    /// <returns></returns>
    IEnumerator DelayCreateZombie()
    {
        while (true) //持續生成殭屍
        {
            yield return new WaitForSeconds(createZombieTime);
            //生成殭屍
            GameObject zombie = Instantiate(zombiePre);
            int index = Random.Range(0, 5);
            //設定父物件
            Transform zombieLine = bornParent.transform.Find($"Born{index}");
            zombie.transform.parent = zombieLine;

            //設定殭屍位置(沒設定位置的話預設值為世界座標(0, 0, 0))
            zombie.transform.localPosition = Vector3.zero;

            //設定顯示層級
            zOrderIndex++;
            zombie.GetComponent<SpriteRenderer>().sortingOrder = zOrderIndex;
        }
    }

    public void ZombieDie(GameObject gameObject)
    {
        if (curProgressZombieList.Contains(gameObject))
        {
            curProgressZombieList.Remove(gameObject);
        }

        if (curProgressZombieList.Count == 0)
        {
            //下一波
            curProgressId++;
            TableCreateZombie();
        }
    }

    /// <summary>
    /// 點擊太陽
    /// </summary>
    private void ClickSun()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D[] colliders = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            foreach (var hit in colliders)
            {
                if (hit.tag == "Sun")
                {
                    //改變太陽數量
                    ChangeSunNum(25);
                    //點擊完後，太陽消失
                    Destroy(hit.gameObject);
                }
            }
        }
       
    }

    /// <summary>
    /// 讀取表格
    /// </summary>
    /// <returns></returns>
    IEnumerator CoLoadTable()
    {
        // 使用 Resources.LoadAsync 方法非同步加載名為 "Level" 的資源，返回 ResourceRequest 對象。
        ResourceRequest request = Resources.LoadAsync("Level");
        yield return request;

        // 將加載的資源轉換為 LevelData 類型並保存到 levelData 變數中。
        levelData = request.asset as LevelData;
        for (int i = 0; i < levelData.levelDataList.Count; i++)
        {
            Debug.Log(levelData.levelDataList[i].id);
        }

        //讀取完表格馬上遊戲開始
        GameStart();
    }

    private void GameStart()
    {
        UIManager.instance.InitUI();
        //StartCoroutine(DelayCreateZombie());
        TableCreateZombie();
        //InvokeRepeating("CreateSunDown", 10, 10);
        gameStart = true;

    }
}
