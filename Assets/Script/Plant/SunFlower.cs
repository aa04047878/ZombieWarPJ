using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SunFlower : Plant
{
    /// <summary>
    /// 生成太陽的準備時間
    /// </summary>
    public float readyTime;
    /// <summary>
    /// 計時器
    /// </summary>
    private float timer;
    public GameObject sunPrefab;
    private int sunNum;
    public ObjectPool<Sun> sunPool;
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        timer = 0;
        sunPool = ObjectPool<Sun>.Instance;
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
    /// 生成太陽結束(動畫事件)
    /// </summary>
    public void SunFlowerOver()
    {
        BornSun();
        ani.SetBool("Ready", false);
        timer = 0;
    }

    /// <summary>
    /// 生成太陽
    /// </summary>
    public void BornSun()
    {
        //生成太陽
        //GameObject sun =  Instantiate(sunPrefab);
        
        sunNum++;
        float randomX;

        //基數的話太陽就生成在左邊
        if (sunNum % 2 == 1)
        {
            randomX = Random.Range(transform.position.x - 30, transform.position.x - 20);
        }
        else
        {
            //偶數的話太陽就生成在右邊
            randomX = Random.Range(transform.position.x + 20, transform.position.x + 30);
        }

        float randomY = Random.Range(transform.position.y + 20, transform.position.y + 30);

        //設定太陽的位置
        //sun.transform.position = new Vector2(randomX, randomY);

        Vector2 sunPos = new Vector2(randomX, randomY); ;
        Sun sun = sunPool.Spawn(sunPos, Quaternion.identity);
        sun.ResetTimer();
        sun.transform.position = sunPos;
    }
}
