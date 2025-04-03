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
        //���J���K�S��
        GameObject firePre = Resources.Load<GameObject>("Prefab/Fire");
        //�ͦ����K�S��
        GameObject fireObj = Instantiate(firePre, transform.position, Quaternion.identity);
    }
}
