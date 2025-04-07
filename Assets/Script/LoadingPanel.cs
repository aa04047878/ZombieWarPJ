using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{
    /*
    異步載入場景有2個條件，都要達成才會轉換場景
    1. operation.progress = 1 (進度條為1)
        此值的最大為0.9f，代表加載已完成，想要激活場景，就要把 allowSceneActivation 設為 true (設成true以後，operation.progress的值才會變成1)
        ，此範例是直接除0.9f，讓進度條的值變成1。

    2. operation.allowSceneActivation = true (允許載入完成後切換場景)
    */

    public Slider slider;
    public GameObject BtnStart;
    /// <summary>
    /// 當前進度條
    /// </summary>
    public float curProgress;
    /// <summary>
    /// 假的載入時間
    /// </summary>
    public float loadingTime;
    /// <summary>
    /// 是否使用真的載入方式
    /// </summary>
    public bool really;
    AsyncOperation operation;
    // Start is called before the first frame update
    void Start()
    {
        really = true;
        BtnStart.SetActive(false);
        BtnStart.GetComponent<Button>().onClick.AddListener(() => OnButtonStart());
        curProgress = 0;
        slider.value = curProgress;

        if (really)
        {
            //異步載入場景
            operation = SceneManager.LoadSceneAsync("Menu");
            //載入場景完成後，是否直接切換場景
            operation.allowSceneActivation = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!really)
        {
            curProgress += Time.deltaTime / loadingTime;
            if (curProgress >= 1)
            {
                curProgress = 1;
            }
            OnSliderValueChange(curProgress);
        }
        else
        {
            //operation.progress 的值最大為0.9f，代表加載已完成，想要激活場景，就要把 allowSceneActivation 設為 true (設成true以後，operation.progress的值就會變成1)
            curProgress = Mathf.Clamp01(operation.progress / 0.9f);
            OnSliderValueChange(curProgress);
        }
        
    }

    private void OnButtonStart()
    {
        //讀取的球會在轉換場景的過程中消失，但DOTween還是會執行，所以要把DOTween關掉
        DOTween.Clear();

        if (!really)
        {
            //轉換場景
            SceneManager.LoadScene("Menu");
        }
        else
        {
            //載入場景完成後，是否直接切換場景
            operation.allowSceneActivation = true;
        }
        
    }

    private void OnSliderValueChange(float value)
    {
        slider.value = value;
        if (value >= 1)
        {
            //載入完成
            BtnStart.SetActive(true);
        }
       
    }
}
