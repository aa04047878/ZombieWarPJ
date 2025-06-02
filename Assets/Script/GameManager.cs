using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /*
    GameManager : 
        �����ݩ� : �v�T�j����������A�j���������󳣻ݭn�{�Ѫ����@�ݩ�
        1. �g�������ܼ�
        2. �g��������k
    */

    public static GameManager instance;
    /// <summary>
    /// �����ƶq
    /// </summary>
    public int sunNum;
    public ObjectPool<Sun> sunPool;

    #region �L��
    /// <summary>
    /// �L�ͥͦ�������
    /// </summary>
    public GameObject bornParent;
    /// <summary>
    /// �L�͹w�m��
    /// </summary>
    public GameObject zombiePre;
    public float createZombieTime;
    private int zOrderIndex;
    /// <summary>
    /// ��e�i���Ѿl�L�ͼƶq
    /// </summary>
    public List<GameObject> curProgressZombieList;
    public int curKillZombieCount;
    public List<int> dieZombieIdList;
    public Queue<int> killCountResult;
    /// <summary>
    /// ��e�i�����L���`��
    /// </summary>
    public int curZombieTotalNum;
    #endregion

    #region ���d����
    public LevelData levelData;
    public LevelInfo levelInfo;
    public PlantInfo plantInfo;
    public bool gameStart;
    /// <summary>
    /// ��e���d
    /// </summary>
    public int curLevelId;
    /// <summary>
    /// ��e�i��
    /// </summary>
    public int curProgressId;
    #endregion

    #region ��������
    public GameObject victoryPanelPre;
    public GameObject victoryPanelObj;
    public GameObject failPanelPre;
    public GameObject failPanelObj;
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
        //�q�\�ƥ�
        EventCenter.Instance.AddEventListener(EventType.eventGameFail, GameOver);
        EventCenter.Instance.AddEventListener(EventType.eventGameStart, GameReallyStart);
    }

    // Update is called once per frame
    void Update()
    {
        ClickSun();
    }

    public void AddIdList(int id)
    {
        dieZombieIdList.Add(id);
    }

    /// <summary>
    /// ���ܤӶ��ƶq
    /// </summary>
    public void ChangeSunNum(int changeNum)
    {
        sunNum += changeNum;

        if (sunNum <= 0)
        {
            sunNum = 0;
        }
        //��sUI
        UIManager.instance.UpdateUI();
    }

   

    /// <summary>
    /// �q���гy�L��
    /// </summary>
    private void TableCreateZombie()
    {
        int canCreateNum = 0;

        //�ˬd�i���O�_�W�L�W��
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

        //�p�G�S���i���F�A�h����Ҧ���{
        if (canCreateNum == 0)
        {
            //�C�������޿�
            Debug.Log("��e���d�w�g�����A�S���i���F");
            EventCenter.Instance.EventTrigger(EventType.eventGameVictory);
            StopAllCoroutines();
            CancelInvoke("CreateSunDown");
            curProgressZombieList.Clear();
            gameStart = false;
            victoryPanelObj.SetActive(true);

            //����x�s
            UserData userData = LocalConfig.LoadUserData(BaseManager.Instance.curUserName);
            userData.level++;
            LocalConfig.SaveUserData(userData);

            //�Ȱ��C��
            TimeManager.PauseGame();
            return;
        }

        Debug.Log($"�гy��{curProgressId}�i���L��");
        //�гy�L��
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
        //Ū���L�͹w�m��
        GameObject zombiePre = Resources.Load<GameObject>($"Prefab/Zombie{levelItem.zombieType}");
        //�ͦ��L��
        GameObject zombie = Instantiate(zombiePre);
        //�]�wId
        zombie.GetComponent<ZombieNormal>().SetId(levelItem.id);
        //�]�w������
        Transform bornLine = bornParent.transform.Find($"Born{levelItem.bornPos}");
        zombie.transform.parent = bornLine;
        //�]�w�L�ͦ�m(�S�]�w��m���ܹw�]�Ȭ��@�ɮy��(0, 0, 0))
        zombie.transform.localPosition = Vector3.zero;
        //�]�w��ܼh��
        zOrderIndex++;
        zombie.GetComponent<SpriteRenderer>().sortingOrder = zOrderIndex;
        //�[�J��e�i���L�ͦC��
        curProgressZombieList.Add(zombie);  
    }

    /// <summary>
    /// �гy�L��
    /// </summary>
    /// <returns></returns>
    IEnumerator DelayCreateZombie()
    {
        while (true) //����ͦ��L��
        {
            yield return new WaitForSeconds(createZombieTime);
            //�ͦ��L��
            GameObject zombie = Instantiate(zombiePre);
            int index = Random.Range(0, 5);
            //�]�w������
            Transform zombieLine = bornParent.transform.Find($"Born{index}");
            zombie.transform.parent = zombieLine;

            //�]�w�L�ͦ�m(�S�]�w��m���ܹw�]�Ȭ��@�ɮy��(0, 0, 0))
            zombie.transform.localPosition = Vector3.zero;

            //�]�w��ܼh��
            zOrderIndex++;
            zombie.GetComponent<SpriteRenderer>().sortingOrder = zOrderIndex;
        }
    }

    public void ZombieDie(GameObject gameObject, int dieId)
    {
        //�L�ͦ���A��ַ�e�L�ͼƶq
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

        

        // �L�ͦ���A��s�i�ױ�����
        UIManager.instance.UpdateProgressPanel();

        //��e�L�ͳ����F�A�h�i�J�U�@�i
        //if (curProgressZombieList.Count == 0)
        //{
        //    //curProgressZombieList.Clear();
        //    //�U�@�i
        //    curProgressId++;
        //    curKillZombieCount = 0;
        //    killCountResult.Clear();
        //    Debug.Log($"��e�i�� : {curProgressId}");
        //    TableCreateZombie();
        //}

        Debug.Log($"curZombieTotalNum : {curZombieTotalNum}");
        if (curZombieTotalNum == 0)
        {
            //curProgressZombieList.Clear();
            //�U�@�i
            curProgressId++;
            curKillZombieCount = 0;
            killCountResult.Clear();
            Debug.Log($"��e�i�� : {curProgressId}");
            TableCreateZombie();
        }
    }

    /// <summary>
    /// �I���Ӷ�
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
                    //���ܤӶ��ƶq
                    ChangeSunNum(25);
                    //�I������A�Ӷ�����
                    //Destroy(hit.gameObject);
                    sunPool.Recycle(hit.GetComponent<Sun>());
                }
            }
        }
       
    }

    /// <summary>
    /// �Ӷ����U(start���ե�)
    /// </summary>
    public void CreateSunDown()
    {
        //Ū���ù����U���M�k�W�����y��
        Vector3 leftBottom = Camera.main.ViewportToWorldPoint(Vector2.zero);
        Vector3 rightTop = Camera.main.ViewportToWorldPoint(Vector2.one);

        //Ū��sun�w�m��
        GameObject sunPre = Resources.Load<GameObject>("Prefab/Sun");

        //��l�ƤӶ�����m
        float x = Random.Range(leftBottom.x + 30, rightTop.x - 30);
        Vector3 bornPos = new Vector3(x, rightTop.y, 0);
        float y = Random.Range(leftBottom.y + 80, leftBottom.y + 30);

        //�ͦ��Ӷ�
        //GameObject sun = Instantiate(sunPre, bornPos, Quaternion.identity);
        Sun sun = sunPool.Spawn(bornPos, Quaternion.identity);
        sun.ResetTimer();
        //Debug.Log("�ͦ��Ӷ�");

        //�]�w�Ӷ�����m
        //sun.GetComponent<Sun>().SetTargetPos(new Vector3(bornPos.x, y, 0));
        sun.SetTargetPos(new Vector3(bornPos.x, y, 0));
        //Debug.Log("�w�]�w�n�Ӷ���m");
    }

    /// <summary>
    /// Ū�����
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoLoadTable()
    {
        UserData userData = LocalConfig.LoadUserData(BaseManager.Instance.curUserName);

        // �ϥ� Resources.LoadAsync ��k�D�P�B�[���W�� "Level" ���귽�A��^ ResourceRequest ��H�C
        ResourceRequest request1 = Resources.LoadAsync($"Data/Level{userData.level}/Level");
        ResourceRequest request2 = Resources.LoadAsync($"Data/Level{userData.level}/LevelInfo");
        ResourceRequest request3 = Resources.LoadAsync($"Data/Level{userData.level}/PlantInfo");
        yield return request1;
        yield return request2;
        yield return request3;

        // �N�[�����귽�ഫ�� LevelData �����ëO�s�� levelData �ܼƤ��C
        levelData = request1.asset as LevelData;
        levelInfo = request2.asset as LevelInfo;
        plantInfo = request3.asset as PlantInfo;
        //for (int i = 0; i < levelData.levelDataList.Count; i++)
        //{
        //    Debug.Log(levelData.levelDataList[i].id);
        //}

        //Ū������氨�W�C���}�l
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
        InvokeRepeating("CreateSunDown", 3, 3);
        SoundManager.instance.PlayBGM(Globals.BattleMusic);
    }

    /// <summary>
    /// ���o�Ӫ��b���@��
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
    /// ���o���@�榳�h���L��
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
}
