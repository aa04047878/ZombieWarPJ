using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : Bullet
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void DestroyBullet()
    {
        base.DestroyBullet();
        //載入火焰特效
        GameObject firePre = Resources.Load<GameObject>("Prefab/Fire");
        //生成火焰特效
        GameObject fireObj = Instantiate(firePre, transform.position, Quaternion.identity);
    }
}
