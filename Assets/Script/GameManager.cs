using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }

        //�p�G�S���i���F�A�h����Ҧ���{
        if (canCreateNum == 0)
        {
            StopAllCoroutines();
            curProgressZombieList.Clear();
            gameStart = false;
            return;
        }

        //�гy�L��
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
        //Ū���L�͹w�m��
        GameObject zombiePre = Resources.Load<GameObject>($"Prefab/Zombie{levelItem.zombieType}");
        //�ͦ��L��
        GameObject zombie = Instantiate(zombiePre);
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

    public void ZombieDie(GameObject gameObject)
    {
        //�L�ͦ���A��ַ�e�L�ͼƶq
        if (curProgressZombieList.Contains(gameObject))
        {
            curProgressZombieList.Remove(gameObject);
        }

        // �L�ͦ���A��s�i�ױ�����
        UIManager.instance.UpdateProgressPanel();

        //��e�L�ͳ����F�A�h�i�J�U�@�i
        if (curProgressZombieList.Count == 0)
        {
            //�U�@�i
            curProgressId++;
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
                    Destroy(hit.gameObject);
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

        //�ͦ��Ӷ�
        GameObject sun = Instantiate(sunPre, bornPos, Quaternion.identity);
        Debug.Log("�ͦ��Ӷ�");

        //�]�w�Ӷ�����m
        float y = Random.Range(leftBottom.y + 80, leftBottom.y + 30);
        sun.GetComponent<Sun>().SetTargetPos(new Vector3(bornPos.x, y, 0));
        Debug.Log("�w�]�w�n�Ӷ���m");
    }

    /// <summary>
    /// Ū�����
    /// </summary>
    /// <returns></returns>
    IEnumerator CoLoadTable()
    {
        // �ϥ� Resources.LoadAsync ��k�D�P�B�[���W�� "Level" ���귽�A��^ ResourceRequest ��H�C
        ResourceRequest request1 = Resources.LoadAsync("Data/Level");
        ResourceRequest request2 = Resources.LoadAsync("Data/LevelInfo");
        ResourceRequest request3 = Resources.LoadAsync("Data/PlantInfo");
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
        TableCreateZombie();
        InvokeRepeating("CreateSunDown", 3, 3);
        SoundManager.instance.PlayBGM(Globals.BGM1);
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

   
}
