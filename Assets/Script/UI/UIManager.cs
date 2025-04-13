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
        EventCenter.Instance.AddEventListener(EventType.eventGameStart, OpenBeam);
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
            if (levelItem.levelId == GameManager.instance.curLevelId && levelItem.progressId == GameManager.instance.curProgressId)
            {
                progressNum++;
            }
        }

        //取得當前波次的殭屍剩餘數量
        int remainNum = GameManager.instance.curProgressZombieList.Count;
        //當前波次進行多少百分比
        float percent = (float)(progressNum - remainNum) / progressNum;
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
        float finalPercent = percent * (progressPercent - lastProgressPercent) + lastProgressPercent;
        progressPanel.SetPrecent(finalPercent);

    }

    /// <summary>
    /// 開啟光線
    /// </summary>
    private void OpenBeam()
    {
        LaserBeam.SetActive(true);
    }
}
