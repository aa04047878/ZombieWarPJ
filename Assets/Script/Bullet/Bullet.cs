using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    /// <summary>
    /// 子彈的方向
    /// </summary>
    [SerializeField , Header("子彈的方向")]
    protected Vector3 direction;
    /// <summary>
    /// 子彈的速度
    /// </summary>
    [SerializeField , Header("子彈的速度")]
    protected float speed;
    /// <summary>
    /// 子彈的傷害
    /// </summary>
    [SerializeField , Header("子彈的傷害")]
    protected float damage;
    /// <summary>
    /// 子彈的存活時間
    /// </summary>
    [SerializeField , Header("存活時間")]
    protected float existTime;
    /// <summary>
    /// 計時器
    /// </summary>
    protected float timer;
    public bool touchWoodCreate;
    public ObjectPool<Bullet> bulletPool;

    //private bool isDestroyed;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        timer = 0;
        existTime = 10;
        bulletPool = ObjectPool<Bullet>.Instance;
        
        //10秒後自動銷毀子彈(因為已超出畫面)
        //Destroy(gameObject, 10);
        //訂閱事件
        //EventCenter.Instance.AddEventListener(EventType.eventGameVictory, GameOver);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        timer += Time.deltaTime;
        if (timer >= existTime)
        {
            bulletPool.Recycle(this);
        }
    }

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener(EventType.eventGameVictory, DestroyBullet);
        EventCenter.Instance.AddEventListener(EventType.eventGameFail, DestroyBullet);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener(EventType.eventGameVictory, DestroyBullet);
        EventCenter.Instance.RemoveEventListener(EventType.eventGameFail, DestroyBullet);
    }

    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.tag == "Zombie")
        {
            Debug.Log($"打到怪物 : {hit.name}");
            //取得Zombie腳本
            ZombieNormal zombie = hit.GetComponent<ZombieNormal>();
            //呼叫Zombie腳本的ChangeHealth方法
            zombie.ChangeHealth(-damage);
            DestroyBullet();
        }
    }

    /// <summary>
    /// 破壞子彈
    /// </summary>
    public virtual void DestroyBullet()
    {
        //銷毀子彈
        //Destroy(gameObject);
        bulletPool.Recycle(this);
        //isDestroyed = true;
    }

    public void ResetTimer()
    {
        timer = 0;
    }

    
}
