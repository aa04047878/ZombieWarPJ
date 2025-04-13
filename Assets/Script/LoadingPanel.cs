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
    ���B���J������2�ӱ���A���n�F���~�|�ഫ����
    1. operation.progress = 1 (�i�ױ���1)
        ���Ȫ��̤j��0.9f�A�N��[���w�����A�Q�n�E�������A�N�n�� allowSceneActivation �]�� true (�]��true�H��Aoperation.progress���Ȥ~�|�ܦ�1)
        �A���d�ҬO������0.9f�A���i�ױ������ܦ�1�C

    2. operation.allowSceneActivation = true (���\���J�������������)
    */

    public Slider slider;
    public GameObject BtnStart;
    /// <summary>
    /// ��e�i�ױ�
    /// </summary>
    public float curProgress;
    /// <summary>
    /// �������J�ɶ�
    /// </summary>
    public float loadingTime;
    /// <summary>
    /// �O�_�ϥίu�����J�覡
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
            //���B���J����
            operation = SceneControl.LoadSceneAsync("Menu");
            //���J����������A�O�_������������
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
            //operation.progress ���ȳ̤j��0.9f�A�N��[���w�����A�Q�n�E�������A�N�n�� allowSceneActivation �]�� true (�]��true�H��Aoperation.progress���ȴN�|�ܦ�1)
            curProgress = Mathf.Clamp01(operation.progress / 0.9f);
            OnSliderValueChange(curProgress);
        }
        
    }

    private void OnButtonStart()
    {
        //Ū�����y�|�b�ഫ�������L�{�������A��DOTween�٬O�|����A�ҥH�n��DOTween����
        DOTween.Clear();

        if (!really)
        {
            //�ഫ����
            SceneControl.LoadScene("Menu");
        }
        else
        {
            //���J����������A�O�_������������
            operation.allowSceneActivation = true;
        }
        
    }

    private void OnSliderValueChange(float value)
    {
        slider.value = value;
        if (value >= 1)
        {
            //���J����
            BtnStart.SetActive(true);
        }
       
    }
}
