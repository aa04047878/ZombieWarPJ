using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour 
{
    /// <summary>
    /// �ܨ��l�u(�w�m��)
    /// </summary>
    private GameObject peashooterBulletPre;
    /// <summary>
    /// �Ӷ�(�w�m��)
    /// </summary>
    private GameObject sunPre;
    /// <summary>
    /// ����Ĩۣ�l�u
    /// </summary>
    private GameObject purpleLittleMushroomPre;

    private static ObjectPoolManager instance;
    public static ObjectPoolManager Instance
    {
        get
        {
            return instance;
        }
    }
    /// <summary>
    /// �l�u�����
    /// </summary>
    public ObjectPool<Bullet> bulletPool;
    public ObjectPool<Sun> sunPool;
    public ObjectPool<PurpleLittleMushroom> purpleMushroomBulletPool;
    public Transform bulletParent;
    public Transform purpleMushroomBulletParent;
    public Transform sunParent;
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
    }

    private void Start()
    {
        bulletParent = GameObject.Find("BulletParent").transform;
        sunParent = GameObject.Find("SunParent").transform;
        purpleMushroomBulletParent = GameObject.Find("PurpleMushroomBulletParent").transform;
        bulletPool = ObjectPool<Bullet>.Instance;
        sunPool = ObjectPool<Sun>.Instance;
        purpleMushroomBulletPool = ObjectPool<PurpleLittleMushroom>.Instance;

        //�w���l�u
        bulletPool.InitPool(peashooterBulletPre, 300);
        foreach (Bullet bullet in bulletPool.GetInventory())
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
    }

    
}
