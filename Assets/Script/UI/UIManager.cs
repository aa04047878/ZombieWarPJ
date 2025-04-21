using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public TMP_Text sunNumText;
    public ProgressPanel progressPanel;
    public AllCardPanel allCardPanel;
    public ChooseCardPanel chooseCardPanel;
    
    /// <summary>
    /// 雷射光線
    /// </summary>
    public GameObject LaserBeam;

   
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        LaserBeam = GameObject.Find("LaserBeam");
        LaserBeam.SetActive(false);
        //訂閱事件
        //EventCenter.Instance.AddEventListener(EventType.eventGameStart, OpenBeam);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitUI()
    {
        sunNumText.text = GameManager.instance.sunNum.ToString();
        allCardPanel.InitCard();       
    }

    public void UpdateUI()
    {
        sunNumText.text = GameManager.instance.sunNum.ToString();
    }

    public void InitProgressPanel()
    {
        //取得該關卡的LevelInfoItem
        LevelInfoItem levelInfo = GameManager.instance.levelInfo.levelInfoList[GameManager.instance.curLevelId - 1];

        for (int i = 0; i < levelInfo.progressPrecent.Length; i++)
        {
            float percent = levelInfo.progressPrecent[i];
            if (percent == 1)
                continue;
            //設置旗子的位置
            progressPanel.SetFlagPercent(percent);
        }

        //初始化進度條介面
        progressPanel.SetPrecent(0);
    }

    public void UpdateProgressPanel()
    {
        int progressNum = 0;
        //計算當前波次的殭屍數量
        for (int i = 0; i < GameManager.instance.levelData.levelDataList.Count; i++)
        {
            LevelItem levelItem = GameManager.instance.levelData.levelDataList[i];
            if (levelItem.progressId == GameManager.instance.curProgressId)
            {
                progressNum++;
            }
        }
        Debug.Log($"當前波次第 {GameManager.instance.curProgressId} 波的殭屍數量 : {progressNum}");
        //取得當前波次的殭屍剩餘數量(不能取這個值，因為第二波開始沒有完全產生出殭屍來，你就把一部份的殭屍打死了)
        //int remainNum = GameManager.instance.curProgressZombieList.Count;

        //取得當前波次擊殺的殭屍數量
        //int curKillNum = GameManager.instance.curKillZombieCount;

        int curKillNum = GameManager.instance.killCountResult.Dequeue();
        Debug.Log($"當前擊殺數量 : {curKillNum}");
        //當前波次進行多少百分比
        float percent = (float)curKillNum / progressNum;
        Debug.Log($"當前波次進行多少百分比 : {percent}");
        LevelInfoItem levelInfo = GameManager.instance.levelInfo.levelInfoList[GameManager.instance.curLevelId - 1];
        //取得當前波次的進度條百分比
        float progressPercent = levelInfo.progressPrecent[GameManager.instance.curProgressId - 1];
        //取得上一波的進度條百分比
        float lastProgressPercent = 0;
        if (GameManager.instance.curProgressId > 1)
        {
            lastProgressPercent = levelInfo.progressPrecent[GameManager.instance.curProgressId - 2];
        }
        //最終進度百分比 = 當前波次的進度條百分比(當前波次進行多少百分比 * 當前波次進度條長度) + 上一波的進度條百分比
        //float finalPercent = percent * (progressPercent - lastProgressPercent) + lastProgressPercent;
        float finalPercent = percent * (progressPercent - lastProgressPercent) + lastProgressPercent;
        Debug.Log($"最終進度百分比公式 : {finalPercent} =  {percent} * ({progressPercent} - {lastProgressPercent}) + {lastProgressPercent}");
        Debug.Log($"最終進度百分比 : {finalPercent}");
        progressPanel.SetPrecent(finalPercent);

    }

    /// <summary>
    /// 開啟光線
    /// </summary>
    public void OpenBeam()
    {
        //有時候會抓不到LaserBeam，但明明unity面板上就是有東西，很奇怪
        //if (LaserBeam == null)
        //    LaserBeam = GameObject.Find("LaserBeam");
        LaserBeam.SetActive(true);
    }
}
