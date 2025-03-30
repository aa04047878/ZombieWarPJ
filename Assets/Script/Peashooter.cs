using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peashooter : Plant
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

    override protected void Start()
    {
        base.Start();
        timer = 0;
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
        if (!start)
        {
            return;
        }

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
