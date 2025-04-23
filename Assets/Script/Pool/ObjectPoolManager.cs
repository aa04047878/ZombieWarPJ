using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour 
{
    /// <summary>
    /// 豌豆子彈(預置物)
    /// </summary>
    private GameObject peashooterBulletPre;
    /// <summary>
    /// 太陽(預置物)
    /// </summary>
    private GameObject sunPre;
    /// <summary>
    /// 紫色蘑菇子彈
    /// </summary>
    private GameObject purpleLittleMushroomPre;
    private GameObject fireBulletPre;

    private static ObjectPoolManager instance;
    public static ObjectPoolManager Instance
    {
        get
        {
            return instance;
        }
    }
    /// <summary>
    /// 子彈物件池
    /// </summary>
    public ObjectPool<PeaBullet> peaBulletPool;
    public ObjectPool<Sun> sunPool;
    public ObjectPool<PurpleLittleMushroom> purpleMushroomBulletPool;
    public ObjectPool<FireBullet> fireObjectPool;
    public Transform bulletParent;
    public Transform purpleMushroomBulletParent;
    public Transform sunParent;
    public Transform fireBulletParent;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        peashooterBulletPre = Resources.Load<GameObject>("Prefab/Bullet/PeaBullet");
        sunPre = Resources.Load<GameObject>("Prefab/Sun");
        purpleLittleMushroomPre = Resources.Load<GameObject>("Prefab/Bullet/PurpleLittleMushroom");
        fireBulletPre = Resources.Load<GameObject>("Prefab/Bullet/FireBullet");
    }

    private void Start()
    {
        bulletParent = GameObject.Find("BulletParent").transform;
        sunParent = GameObject.Find("SunParent").transform;
        fireBulletParent = GameObject.Find("FireBulletParent").transform;
        purpleMushroomBulletParent = GameObject.Find("PurpleMushroomBulletParent").transform;
        peaBulletPool = ObjectPool<PeaBullet>.Instance;
        sunPool = ObjectPool<Sun>.Instance;
        purpleMushroomBulletPool = ObjectPool<PurpleLittleMushroom>.Instance;
        fireObjectPool = ObjectPool<FireBullet>.Instance;

        //預熱子彈
        peaBulletPool.InitPool(peashooterBulletPre, 300);
        foreach (Bullet bullet in peaBulletPool.GetInventory())
        {
            bullet.transform.parent = bulletParent;
        }

        sunPool.InitPool(sunPre, 30);
        foreach (Sun sun in sunPool.GetInventory())
        {
            sun.transform.parent = sunParent;
        }

        purpleMushroomBulletPool.InitPool(purpleLittleMushroomPre, 300);
        foreach(var bullet in purpleMushroomBulletPool.GetInventory())
        {
            bullet.transform.parent = purpleMushroomBulletParent;
        }

        fireObjectPool.InitPool(fireBulletPre, 300);
        foreach (var bullet in fireObjectPool.GetInventory())
        {
            bullet.transform.parent = fireBulletParent;
        }
    }

    
}
