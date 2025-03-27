using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peashooter : MonoBehaviour
{
    /// <summary>
    /// ���j�ɶ�
    /// </summary>
    public float interval;
    /// <summary>
    /// �p�ɾ�
    /// </summary>
    public float timer;
    /// <summary>
    /// �l�u���w�m��
    /// </summary>
    public GameObject peaBulletPrefab;

    public Transform bulletPos;
    // Start is called before the first frame update
    private void OnEnable()
    {
        bulletPos = transform.Find("BulletPos");
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0;
            //�ͦ��l�u
            GameObject curBullet =  Instantiate(peaBulletPrefab, bulletPos.position, Quaternion.identity);
            ///�]�w�l�u��������
            //curBullet.transform.parent = bulletPos;
        }
    }
}
