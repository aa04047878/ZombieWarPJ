using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    /// <summary>
    /// �ܷt�I��(�N�o�����ϥ�)
    /// </summary>
    public GameObject darkBg;
    /// <summary>
    /// �i�ױ�
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
    /// ��s�i�ױ�
    /// </summary>
    private void UpdateProgress()
    {
        float per = Mathf.Clamp(timer / waitTime, 0, 1);

        //�i�ױ��Ѿl�ɶ����
        progressBar.GetComponent<Image>().fillAmount = 1 - per;
    }

    /// <summary>
    /// ��s�t�I��
    /// </summary>
    private void UpdateDarkBg()
    {
        //�ˬd�O�_�N�o����
        if (progressBar.GetComponent<Image>().fillAmount == 0)
        {
            //�N�o�����A���÷t�I��
            darkBg.SetActive(false);
        }
        else
        {
            //�N�o���A��ܷt�I��
            darkBg.SetActive(true);
        }
    }
}
