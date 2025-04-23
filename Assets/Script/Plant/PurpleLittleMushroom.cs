using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleLittleMushroom : Bullet
{
    public ObjectPool<PurpleLittleMushroom> purpleBulletPool;

    protected override void Start()
    {
        base.Start();
        purpleBulletPool = ObjectPool<PurpleLittleMushroom>.Instance;
    }

    protected override void Update()
    {
        base.Update();
       
        if (timer >= existTime)
        {
            purpleBulletPool.Recycle(this);
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
        purpleBulletPool.Recycle(this);
    }

    public void RecycleBullet()
    {
        purpleBulletPool.Recycle(this);
    }
}
