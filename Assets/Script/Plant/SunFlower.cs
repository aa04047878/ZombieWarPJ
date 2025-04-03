using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SunFlower : Plant
{
    /// <summary>
    /// �ͦ��Ӷ����ǳƮɶ�
    /// </summary>
    public float readyTime;
    /// <summary>
    /// �p�ɾ�
    /// </summary>
    private float timer;
    public GameObject sunPrefab;
    private int sunNum;
    // Start is called before the first frame update
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
        if (timer >= readyTime)
        {
            ani.SetBool("Ready", true);
        }
    }

    /// <summary>
    /// �ͦ��Ӷ�����(�ʵe�ƥ�)
    /// </summary>
    public void SunFlowerOver()
    {
        BornSun();
        ani.SetBool("Ready", false);
        timer = 0;
    }

    /// <summary>
    /// �ͦ��Ӷ�
    /// </summary>
    public void BornSun()
    {
        //�ͦ��Ӷ�
        GameObject sun =  Instantiate(sunPrefab);
        sunNum++;
        float randomX;

        //��ƪ��ܤӶ��N�ͦ��b����
        if (sunNum % 2 == 1)
        {
            randomX = Random.Range(transform.position.x - 30, transform.position.x - 20);
        }
        else
        {
            //���ƪ��ܤӶ��N�ͦ��b�k��
            randomX = Random.Range(transform.position.x + 20, transform.position.x + 30);
        }

        float randomY = Random.Range(transform.position.y + 20, transform.position.y + 30);

        //�]�w�Ӷ�����m
        sun.transform.position = new Vector2(randomX, randomY);
    }
}
