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
    /// �p�g���u
    /// </summary>
    public GameObject LaserBeam;

   
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        LaserBeam = GameObject.Find("LaserBeam");
        LaserBeam.SetActive(false);
        //�q�\�ƥ�
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
            if (levelItem.progressId == GameManager.instance.curProgressId)
            {
                progressNum++;
            }
        }
        Debug.Log($"��e�i���� {GameManager.instance.curProgressId} �i���L�ͼƶq : {progressNum}");
        //���o��e�i�����L�ͳѾl�ƶq(������o�ӭȡA�]���ĤG�i�}�l�S���������ͥX�L�ͨӡA�A�N��@�������L�ͥ����F)
        //int remainNum = GameManager.instance.curProgressZombieList.Count;

        //���o��e�i���������L�ͼƶq
        //int curKillNum = GameManager.instance.curKillZombieCount;

        int curKillNum = GameManager.instance.killCountResult.Dequeue();
        Debug.Log($"��e�����ƶq : {curKillNum}");
        //��e�i���i��h�֦ʤ���
        float percent = (float)curKillNum / progressNum;
        Debug.Log($"��e�i���i��h�֦ʤ��� : {percent}");
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
        //float finalPercent = percent * (progressPercent - lastProgressPercent) + lastProgressPercent;
        float finalPercent = percent * (progressPercent - lastProgressPercent) + lastProgressPercent;
        Debug.Log($"�̲׶i�צʤ��񤽦� : {finalPercent} =  {percent} * ({progressPercent} - {lastProgressPercent}) + {lastProgressPercent}");
        Debug.Log($"�̲׶i�צʤ��� : {finalPercent}");
        progressPanel.SetPrecent(finalPercent);

    }

    /// <summary>
    /// �}�ҥ��u
    /// </summary>
    public void OpenBeam()
    {
        //���ɭԷ|�줣��LaserBeam�A������unity���O�W�N�O���F��A�ܩ_��
        //if (LaserBeam == null)
        //    LaserBeam = GameObject.Find("LaserBeam");
        LaserBeam.SetActive(true);
    }
}
