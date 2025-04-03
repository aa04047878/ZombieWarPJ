using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peashooter : Plant
{
    /// <summary>
    /// 間隔時間
    /// </summary>
    public float interval;
    /// <summary>
    /// 計時器
    /// </summary>
    public float timer;
    /// <summary>
    /// 子彈的預置物
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
            //生成子彈
            GameObject curBullet =  Instantiate(peaBulletPrefab, bulletPos.position, Quaternion.identity);
            ///設定子彈的父物件
            //curBullet.transform.parent = bulletPos;
        }
    }

    
}
