using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torchwood : Plant
{
    //private GameObject fireBulletPre;
    public ObjectPool<FireBullet> fireBulletPool;

    protected override void Start()
    {
        base.Start();
        fireBulletPool = ObjectPool<FireBullet>.Instance;
        //fireBulletPre = Resources.Load<GameObject>("Prefab/FireBullet");
    }

    protected override void Update()
    {
        base.Update();
    }


    private void OnTriggerEnter2D(Collider2D hit)
    {
        //Debug.Log($"hit.name : {hit.name}");
        //�I��l�u�A�l�u�|�ܦ����K�l�u
        if (hit.tag == "Bullet")
        {
            Bullet bullet = hit.GetComponent<Bullet>();
            //�����N�O���K�l�u�A��������ާ@
            if (bullet.touchWoodCreate)
                return;

            bullet.DestroyBullet();
            //CreateBullet(transform.position);
            CreateBullet(hit.bounds.ClosestPoint(transform.position));
        }
    }


    private void CreateBullet(Vector3 bornPos)
    {
        //GameObject fireBullet = Instantiate(fireBulletPre, bornPos, Quaternion.identity);
        //fireBullet.transform.position = bornPos;
        //fireBullet.transform.parent = transform.parent;
        //fireBullet.GetComponent<Bullet>().touchWoodCreate = true;
        
        FireBullet fireBullet =  fireBulletPool.Spawn(bornPos, Quaternion.identity);
        fireBullet.ResetTimer();
        //fireBullet.transform.parent = transform.parent;
        fireBullet.GetComponent<Bullet>().touchWoodCreate = true;
    }
}
