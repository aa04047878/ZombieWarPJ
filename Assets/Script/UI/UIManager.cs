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
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
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
        //���o�����d��LevelInfoItem
        LevelInfoItem levelInfo = GameManager.instance.levelInfo.levelInfoList[GameManager.instance.curLevelId - 1];

        for (int i = 0; i < levelInfo.progressPrecent.Length; i++)
        {
            float percent = levelInfo.progressPrecent[i];
            if (percent == 1)
                continue;
            //�]�m�X�l����m
            progressPanel.SetFlagPercent(percent);
        }

        //��l�ƶi�ױ�����
        progressPanel.SetPrecent(0);
    }

    public void UpdateProgressPanel()
    {
        int progressNum = 0;
        //�p���e�i�����L�ͼƶq
        for (int i = 0; i < GameManager.instance.levelData.levelDataList.Count; i++)
        {
            LevelItem levelItem = GameManager.instance.levelData.levelDataList[i];
            if (levelItem.levelId == GameManager.instance.curLevelId && levelItem.progressId == GameManager.instance.curProgressId)
            {
                progressNum++;
            }
        }

        //���o��e�i�����L�ͳѾl�ƶq
        int remainNum = GameManager.instance.curProgressZombieList.Count;
        //��e�i���i��h�֦ʤ���
        float percent = (float)(progressNum - remainNum) / progressNum;
        LevelInfoItem levelInfo = GameManager.instance.levelInfo.levelInfoList[GameManager.instance.curLevelId - 1];
        //���o��e�i�����i�ױ��ʤ���
        float progressPercent = levelInfo.progressPrecent[GameManager.instance.curProgressId - 1];
        //���o�W�@�i���i�ױ��ʤ���
        float lastProgressPercent = 0;
        if (GameManager.instance.curProgressId > 1)
        {
            lastProgressPercent = levelInfo.progressPrecent[GameManager.instance.curProgressId - 2];
        }
        //�̲׶i�צʤ��� = ��e�i�����i�ױ��ʤ���(��e�i���i��h�֦ʤ��� * ��e�i���i�ױ�����) + �W�@�i���i�ױ��ʤ���
        float finalPercent = percent * (progressPercent - lastProgressPercent) + lastProgressPercent;
        progressPanel.SetPrecent(finalPercent);

    }
}
