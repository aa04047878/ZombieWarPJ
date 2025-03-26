using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    /// <summary>
    /// 變暗背景(冷卻中的圖示)
    /// </summary>
    public GameObject darkBg;
    /// <summary>
    /// 進度條
    /// </summary>
    public GameObject progressBar;

    public int useSun;
    public float waitTime;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        darkBg = transform.Find("DarkBg").gameObject;
        progressBar = transform.Find("ProgressBar").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        UpdateProgress();
        UpdateDarkBg();
    }

    /// <summary>
    /// 更新進度條
    /// </summary>
    private void UpdateProgress()
    {
        float per = Mathf.Clamp(timer / waitTime, 0, 1);

        //進度條剩餘時間比例
        progressBar.GetComponent<Image>().fillAmount = 1 - per;
    }

    /// <summary>
    /// 更新暗背景
    /// </summary>
    private void UpdateDarkBg()
    {
        //檢查是否冷卻完成
        if (progressBar.GetComponent<Image>().fillAmount == 0)
        {
            //冷卻完成，隱藏暗背景
            darkBg.SetActive(false);
        }
        else
        {
            //冷卻中，顯示暗背景
            darkBg.SetActive(true);
        }
    }
}
