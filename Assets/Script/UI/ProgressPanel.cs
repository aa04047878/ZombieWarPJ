using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.UI;

public class ProgressPanel : MonoBehaviour
{
    private GameObject progress;
    private GameObject head;
    private GameObject levelTest;
    private GameObject Bg;
    private GameObject flag;
    private GameObject flagPrefab;
    // Start is called before the first frame update
    void Start()
    {
        progress = transform.Find("Progress").gameObject;
        head = transform.Find("Head").gameObject;
        levelTest = transform.Find("LevelTest").gameObject;
        Bg = transform.Find("Bg").gameObject;
        flag = transform.Find("Flag").gameObject;
        flagPrefab = Resources.Load<GameObject>("Prefab/Flag");
        //SetPrecent(0.5f);
        //SetFlagPercent(0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetPrecent(float per)
    {
        //進度條
        progress.GetComponent<Image>().fillAmount = per;
        //進度條最右邊的位置 = 背景的世界座標位置 + 背景的寬度的一半
        float originPosX = Bg.GetComponent<RectTransform>().position.x + Bg.GetComponent<RectTransform>().sizeDelta.x / 2;
        //Debug.Log($"Bg.position.x : {Bg.GetComponent<RectTransform>().position.x} , Bg.sizeDelta.x / 2 : {Bg.GetComponent<RectTransform>().sizeDelta.x / 2}");
        //Debug.Log($"originPosX : {originPosX}");
        //進度條寬度 = 背景的寬度 
        float width = Bg.GetComponent<RectTransform>().sizeDelta.x;
        //進度條頭部的偏移量
        float offect = 0;
        //設置頭的位置 = 最右邊的位置 + 偏移量
        //head.GetComponent<RectTransform>().position = new Vector2(originPosX + offect, head.GetComponent<RectTransform>().position.y);
        //設置頭的位置 = 最右邊的位置 - 進度百分比換算而得的長度 + 偏移量
        head.GetComponent<RectTransform>().position = new Vector2(originPosX - per * width + offect, head.GetComponent<RectTransform>().position.y);

    }

    public void SetFlagPercent(float per)
    {
        flag.SetActive(false);
        //進度條最右邊的位置 = 背景的世界座標位置 + 背景的寬度的一半
        float originPosX = Bg.GetComponent<RectTransform>().position.x + Bg.GetComponent<RectTransform>().sizeDelta.x / 2;
        //進度條寬度 = 背景的寬度 
        float width = Bg.GetComponent<RectTransform>().sizeDelta.x;
        //進度條旗子的偏移量
        float offect = 0;
        //創造新旗子
        GameObject newFlag = Instantiate(flagPrefab);
        //設置旗子的父物件
        newFlag.transform.SetParent(gameObject.transform, false);
        //設置旗子的位置
        newFlag.GetComponent<RectTransform>().position = flag.GetComponent<RectTransform>().position;
        //設置旗子位置 = 最右邊的位置 - 進度條寬度的一半 + 偏移量
        newFlag.GetComponent<RectTransform>().position = new Vector2(originPosX - per * width + offect, newFlag.GetComponent<RectTransform>().position.y);
    }
}
