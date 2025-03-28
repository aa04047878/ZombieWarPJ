using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*
    GameManager : 
        �����ݩ� : �v�T�j����������A�j���������󳣻ݭn�{�Ѫ����@�ݩ�
        1. �g�������ܼ�
        2. �g��������k
    */

    public static GameManager instance;
    /// <summary>
    /// �����ƶq
    /// </summary>
    public int sunNum;
    /// <summary>
    /// �L�ͥͦ�������
    /// </summary>
    public GameObject bornParent;
    /// <summary>
    /// �L�͹w�m��
    /// </summary>
    public GameObject zombiePre;
    public float createZombieTime;
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
        
    }

    /// <summary>
    /// ���ܤӶ��ƶq
    /// </summary>
    public void ChangeSunNum(int changeNum)
    {
        sunNum += changeNum;

        if (sunNum <= 0)
        {
            sunNum = 0;
        }
        //��sUI
        UIManager.instance.UpdateUI();
    }
    /// <summary>
    /// �гy�L��
    /// </summary>
    /// <returns></returns>
    IEnumerator CoCreateZombie()
    {
        while (true) //����ͦ��L��
        {
            yield return new WaitForSeconds(createZombieTime);
            GameObject zombie = Instantiate(zombiePre);
            int index = Random.Range(0, 5);
            Transform zombieLine = bornParent.transform.Find($"Born{index}");
            zombie.transform.parent = zombieLine;

            //�]�w�L�ͦ�m(�S�]�w��m���ܹw�]�Ȭ��@�ɮy��(0, 0, 0))
            zombie.transform.localPosition = Vector3.zero;
        }
    }
}
