using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaBullet : Bullet
{
    public ObjectPool<PeaBullet> peaBulletPool;
    protected override void Start()
    {
        base.Start();
        peaBulletPool = ObjectPool<PeaBullet>.Instance;
    }

    protected override void Update()
    {
        base.Update();
        
        if (timer >= existTime)
        {
            peaBulletPool.Recycle(this);
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
        peaBulletPool.Recycle(this);
    }

    public void RecycleBullet()
    {
        peaBulletPool.Recycle(this);
    }
}
