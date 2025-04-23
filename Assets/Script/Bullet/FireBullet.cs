using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : Bullet
{
    public ObjectPool<FireBullet> fireBulletPool;
    protected override void Start()
    {
        base.Start();
        fireBulletPool = ObjectPool<FireBullet>.Instance;
    }

    protected override void Update()
    {
        base.Update();
        
        if (timer >= existTime)
        {
            fireBulletPool.Recycle(this);
        }
    }

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener(EventType.eventGameVictory, RecycleBullet);
        EventCenter.Instance.AddEventListener(EventType.eventGameFail, RecycleBullet);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener(EventType.eventGameVictory, RecycleBullet);
        EventCenter.Instance.RemoveEventListener(EventType.eventGameFail, RecycleBullet);
    }

    public override void DestroyBullet()
    {
        //base.DestroyBullet();
        //�^������
        fireBulletPool.Recycle(this);
        //���J���K�S��
        GameObject firePre = Resources.Load<GameObject>("Prefab/Fire");
        //�ͦ����K�S��
        GameObject fireObj = Instantiate(firePre, transform.position, Quaternion.identity);
    }

    public void RecycleBullet()
    {
        //�^������
        fireBulletPool.Recycle(this);
    }
}
