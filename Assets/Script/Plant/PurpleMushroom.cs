using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleMushroom : Plant
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
    public GameObject bulletPrefab;

    public Transform bulletPos;
    /// <summary>
    /// �l�u�����
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
        //�q�\�ƥ�
        //EventCenter.Instance.AddEventListener(EventType.eventGameVictory, GameOver);

        ani = GetComponentInChildren<Animator>();
        ///�]�w�ʵe�t��(���n����)
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
            //�ͦ��l�u
            //GameObject curBullet =  Instantiate(peaBulletPrefab, bulletPos.position, Quaternion.identity);
            ani.SetTrigger("att");
            Bullet bullet = bulletPool.Spawn(bulletPos.position, Quaternion.identity);
            bullet.transform.rotation = Quaternion.Euler(0, 0, -90);
            bullet.ResetTimer();
            ///�]�w�l�u��������
            //curBullet.transform.parent = bulletPos;
        }
    }

}
