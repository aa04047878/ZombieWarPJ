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
        StartCoroutine(CoCreateZombie());
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
    /// 創造殭屍
    /// </summary>
    /// <returns></returns>
    IEnumerator CoCreateZombie()
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

        //yield return new WaitForSeconds(createZombieTime);
        //GameObject zombie = Instantiate(zombiePre);
        //int index = Random.Range(0, 5);
        //Transform zombieLine = bornParent.transform.Find($"Born{index}");
        //zombie.transform.parent = zombieLine;

        ////設定殭屍位置(沒設定位置的話預設值為世界座標(0, 0, 0))
        //zombie.transform.localPosition = Vector3.zero;

        ////設定顯示層級
        //zOrderIndex++;
        //zombie.GetComponent<SpriteRenderer>().sortingOrder = zOrderIndex;
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
}
