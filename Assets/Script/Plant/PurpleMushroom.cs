using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleMushroom : Plant
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
    public GameObject bulletPrefab;

    public Transform bulletPos;
    /// <summary>
    /// 子彈物件池
    /// </summary>
    public ObjectPool<PurpleLittleMushroom> bulletPool;

    // Start is called before the first frame update
    private void OnEnable()
    {
        bulletPos = transform.Find("BulletPos");
    }

    override protected void Start()
    {
        //base.Start();
        start = false;
        currentHealth = health;
        //ani = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = false;
        //訂閱事件
        //EventCenter.Instance.AddEventListener(EventType.eventGameVictory, GameOver);

        ani = GetComponentInChildren<Animator>();
        ///設定動畫速度(不要撥放)
        ani.speed = 0;
        timer = 0;
        bulletPool = ObjectPool<PurpleLittleMushroom>.Instance;
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
            //GameObject curBullet =  Instantiate(peaBulletPrefab, bulletPos.position, Quaternion.identity);
            ani.SetTrigger("att");
            Bullet bullet = bulletPool.Spawn(bulletPos.position, Quaternion.identity);
            bullet.transform.rotation = Quaternion.Euler(0, 0, -90);
            bullet.ResetTimer();
            ///設定子彈的父物件
            //curBullet.transform.parent = bulletPos;
        }
    }

}
