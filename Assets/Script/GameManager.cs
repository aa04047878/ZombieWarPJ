using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public ObjectPool<Sun> sunPool;

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
    /// 當前波次剩餘殭屍數量
    /// </summary>
    public List<GameObject> curProgressZombieList;
    public int curKillZombieCount;
    public List<int> dieZombieIdList;
    public Queue<int> killCountResult;
    /// <summary>
    /// 當前波次的殭屍總數
    /// </summary>
    public int curZombieTotalNum;
    #endregion

    #region 關卡相關
    public LevelData levelData;
    public LevelInfo levelInfo;
    public PlantInfo plantInfo;
    public bool gameStart;
    /// <summary>
    /// 當前關卡
    /// </summary>
    public int curLevelId;
    /// <summary>
    /// 當前波次
    /// </summary>
    public int curProgressId;
    #endregion

    #region 介面相關
    public GameObject victoryPanelPre;
    public GameObject victoryPanelObj;
    public GameObject failPanelPre;
    public GameObject failPanelObj;
    #endregion

    #region 設定相關
    public Button btnSetting;
    public Canvas canvas;
    #endregion

    
    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
           
        }
    }
    void Start()
    {

        //victoryPanelPre = GameObject.Find("VictoryPanel");
        //failPanelPre = GameObject.Find("FailPanel");
        //curKillZombieCount = 0;
        //killCountResult = new Queue<int>();
        //dieZombieIdList = new List<int>();
        sunPool = ObjectPool<Sun>.Instance;
        zombiePre = Resources.Load<GameObject>("Prefab/Zombie1");
        victoryPanelPre = Resources.Load<GameObject>("Prefab/Panel/Menu/VictoryPanel");
        failPanelPre = Resources.Load<GameObject>("Prefab/Panel/Menu/FailPanel");
        //訂閱事件
        EventCenter.Instance.AddEventListener(EventType.eventGameFail, GameOver);
        EventCenter.Instance.AddEventListener(EventType.eventGameStart, GameReallyStart);
    }

    // Update is called once per frame
    void Update()
    {
        ClickSun();
    }

    #region 遊戲主邏輯相關
    public void AddIdList(int id)
    {
        dieZombieIdList.Add(id);
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

            if (levelItem.progressId == curProgressId)
            {
                curZombieTotalNum++;
            }
        }

        //如果沒有波次了，則停止所有協程
        if (canCreateNum == 0)
        {
            //遊戲結束邏輯
            Debug.Log("當前關卡已經結束，沒有波次了");
            EventCenter.Instance.EventTrigger(EventType.eventGameVictory);
            StopAllCoroutines();
            CancelInvoke("CreateSunDown");
            curProgressZombieList.Clear();
            gameStart = false;
            victoryPanelObj.SetActive(true);

            //資料儲存
            UserData userData = LocalConfig.LoadUserData(BaseManager.Instance.curUserName);
            userData.level++;
            LocalConfig.SaveUserData(userData);

            //暫停遊戲
            TimeManager.PauseGame();
            return;
        }

        Debug.Log($"創造第{curProgressId}波的殭屍");
        //創造殭屍
        for (int i = 0; i < levelData.levelDataList.Count; i++)
        {
            LevelItem levelItem = levelData.levelDataList[i];
            if (levelItem.progressId == curProgressId)
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
        //設定Id
        zombie.GetComponent<ZombieNormal>().SetId(levelItem.id);
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

    public void ZombieDie(GameObject gameObject, int dieId)
    {
        //殭屍死後，減少當前殭屍數量
        //if (curProgressZombieList.Contains(gameObject))
        //{
        //    curProgressZombieList.Remove(gameObject);
        //}

        foreach (int id in dieZombieIdList)
        {
            if (dieId == id)
            {
                curProgressZombieList.Remove(gameObject);
            }
        }

        

        // 殭屍死後，更新進度條介面
        UIManager.instance.UpdateProgressPanel();

        //當前殭屍都死了，則進入下一波
        //if (curProgressZombieList.Count == 0)
        //{
        //    //curProgressZombieList.Clear();
        //    //下一波
        //    curProgressId++;
        //    curKillZombieCount = 0;
        //    killCountResult.Clear();
        //    Debug.Log($"當前波次 : {curProgressId}");
        //    TableCreateZombie();
        //}

        Debug.Log($"curZombieTotalNum : {curZombieTotalNum}");
        if (curZombieTotalNum == 0)
        {
            //curProgressZombieList.Clear();
            //下一波
            curProgressId++;
            curKillZombieCount = 0;
            killCountResult.Clear();
            Debug.Log($"當前波次 : {curProgressId}");
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
                    //Destroy(hit.gameObject);
                    sunPool.Recycle(hit.GetComponent<Sun>());
                }
            }
        }
       
    }

    /// <summary>
    /// 太陽落下(start有調用)
    /// </summary>
    public void CreateSunDown()
    {
        //讀取螢幕左下角和右上角的座標
        Vector3 leftBottom = Camera.main.ViewportToWorldPoint(Vector2.zero);
        Vector3 rightTop = Camera.main.ViewportToWorldPoint(Vector2.one);

        //讀取sun預置物
        GameObject sunPre = Resources.Load<GameObject>("Prefab/Sun");

        //初始化太陽的位置
        float x = Random.Range(leftBottom.x + 30, rightTop.x - 30);
        Vector3 bornPos = new Vector3(x, rightTop.y, 0);
        float y = Random.Range(leftBottom.y + 80, leftBottom.y + 30);

        //生成太陽
        //GameObject sun = Instantiate(sunPre, bornPos, Quaternion.identity);
        Sun sun = sunPool.Spawn(bornPos, Quaternion.identity);
        sun.ResetTimer();
        //Debug.Log("生成太陽");

        //設定太陽的位置
        //sun.GetComponent<Sun>().SetTargetPos(new Vector3(bornPos.x, y, 0));
        sun.SetTargetPos(new Vector3(bornPos.x, y, 0));
        //Debug.Log("已設定好太陽位置");
    }

    /// <summary>
    /// 太陽落下(協程)
    /// </summary>
    IEnumerator CoCreateSunDown()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            //讀取螢幕左下角和右上角的座標
            Vector3 leftBottom = Camera.main.ViewportToWorldPoint(Vector2.zero);
            Vector3 rightTop = Camera.main.ViewportToWorldPoint(Vector2.one);

            //讀取sun預置物
            GameObject sunPre = Resources.Load<GameObject>("Prefab/Sun");

            //初始化太陽的位置
            float x = Random.Range(leftBottom.x + 30, rightTop.x - 30);
            Vector3 bornPos = new Vector3(x, rightTop.y, 0);
            float y = Random.Range(leftBottom.y + 80, leftBottom.y + 30);

            //生成太陽
            //GameObject sun = Instantiate(sunPre, bornPos, Quaternion.identity);
            Sun sun = sunPool.Spawn(bornPos, Quaternion.identity);
            sun.ResetTimer();
            //Debug.Log("生成太陽");

            //設定太陽的位置
            //sun.GetComponent<Sun>().SetTargetPos(new Vector3(bornPos.x, y, 0));
            sun.SetTargetPos(new Vector3(bornPos.x, y, 0));
            //Debug.Log("已設定好太陽位置");
        }
    }

    /// <summary>
    /// 讀取表格
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoLoadTable()
    {
        UserData userData = LocalConfig.LoadUserData(BaseManager.Instance.curUserName);

        // 使用 Resources.LoadAsync 方法非同步加載名為 "Level" 的資源，返回 ResourceRequest 對象。
        ResourceRequest request1 = Resources.LoadAsync($"Data/Level{userData.level}/Level");
        ResourceRequest request2 = Resources.LoadAsync($"Data/Level{userData.level}/LevelInfo");
        ResourceRequest request3 = Resources.LoadAsync($"Data/Level{userData.level}/PlantInfo");
        yield return request1;
        yield return request2;
        yield return request3;

        // 將加載的資源轉換為 LevelData 類型並保存到 levelData 變數中。
        levelData = request1.asset as LevelData;
        levelInfo = request2.asset as LevelInfo;
        plantInfo = request3.asset as PlantInfo;
        //for (int i = 0; i < levelData.levelDataList.Count; i++)
        //{
        //    Debug.Log(levelData.levelDataList[i].id);
        //}

        //讀取完表格馬上遊戲開始
        GameStart();
    }

    private void GameStart()
    {
        UIManager.instance.InitUI();
        //StartCoroutine(DelayCreateZombie());
        
        UIManager.instance.InitProgressPanel();
        //InvokeRepeating("CreateSunDown", 10, 10);
       
    }

    public void GameReallyStart()
    {
        gameStart = true;
        //sunNum = 0;
        
        TableCreateZombie();
        //InvokeRepeating("CreateSunDown", 3, 3);
        StartCoroutine(CoCreateSunDown());
        SoundManager.instance.PlayBGM(Globals.BattleMusic);
    }

    /// <summary>
    /// 取得植物在哪一行
    /// </summary>
    /// <param name="plant"></param>
    /// <returns></returns>
    public int GetPlantLine(GameObject plant)
    {
        GameObject lineObject = plant.transform.parent.parent.gameObject;
        string lineStr = lineObject.name;
        // int line = int.Parse(lineStr.Split("line")[1]);
        //int line = int.Parse(Split(lineStr, "line")[1]);
        int line = int.Parse(lineStr.Replace("Line", ""));
        return line;
    }

    /// <summary>
    /// 取得哪一行有多少殭屍
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    public List<GameObject> GetLineZombies(int line)
    {
        string lineName = "Born" + line.ToString();
        Transform bornObject = bornParent.transform.Find(lineName);
        List<GameObject> zombies = new List<GameObject>();
        for (int i = 0; i < bornObject.childCount; i++)
        {
            zombies.Add(bornObject.GetChild(i).gameObject);
        }
        return zombies;
    }

    
    public void GameOver()
    {
        //failPanelPre.GetComponent<BasePanel>().OpenPanel(UIConst.victoryPanelPre);
        StopAllCoroutines();
        CancelInvoke("CreateSunDown");
        curProgressZombieList.Clear();
        gameStart = false;
        failPanelObj.SetActive(true);
        TimeManager.PauseGame();
    }
    #endregion

    #region 設定相關
    public void OpenCanvasSetting()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            TimeManager.PauseGame();
        }

        CanvasScaler canvasScaler = canvas.GetComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        //canvasScaler.referenceResolution = new Vector2(800, 600);
    }

    public void CloseCanvasSetting()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
            return;

        Debug.Log("關閉Canvas設定");
        if (SceneManager.GetActiveScene().name == "Game")
        {
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.transform.position = new Vector3(0, 0, 0); //調整位置
            RectTransform rectTransform = canvas.GetComponent<RectTransform>();            
            rectTransform.sizeDelta = new Vector2(1075.6f, 601.2f); //調整寬高
            rectTransform.localScale = new Vector3(1, 1, 1); //調整縮放
            TimeManager.ResumeGame();
            CanvasScaler canvasScaler = canvas.GetComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        }

              
    }
    #endregion
}
