using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    /// <summary>
    /// �l�u����V
    /// </summary>
    [SerializeField , Header("�l�u����V")]
    protected Vector3 direction;
    /// <summary>
    /// �l�u���t��
    /// </summary>
    [SerializeField , Header("�l�u���t��")]
    protected float speed;
    /// <summary>
    /// �l�u���ˮ`
    /// </summary>
    [SerializeField , Header("�l�u���ˮ`")]
    protected float damage;
    /// <summary>
    /// �l�u���s���ɶ�
    /// </summary>
    [SerializeField , Header("�s���ɶ�")]
    protected float existTime;
    /// <summary>
    /// �p�ɾ�
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

        //10���۰ʾP���l�u(�]���w�W�X�e��)
        //Destroy(gameObject, 10);
        //�q�\�ƥ�
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

    //private void OnDisable()
    //{
    //    DontDestroyOnLoad(gameObject);
    //}

    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.tag == "Zombie")
        {
            Debug.Log($"����Ǫ� : {hit.name}");
            //���oZombie�}��
            ZombieNormal zombie = hit.GetComponent<ZombieNormal>();
            //�I�sZombie�}����ChangeHealth��k
            zombie.ChangeHealth(-damage);
            DestroyBullet();
        }
    }

    public virtual void DestroyBullet()
    {
        //�P���l�u
        //Destroy(gameObject);
        bulletPool.Recycle(this);
        //isDestroyed = true;
    }

    public void ResetTimer()
    {
        timer = 0;
    }

    //protected void GameOver()
    //{
    //    //�C�������A�P���l�u
    //    Destroy(gameObject);
    //}

    //protected void OnTimeDestroy()
    //{
    //    if (isDestroyed)
    //        return;

    //    //�P���l�u
    //    Destroy(gameObject);
    //    isDestroyed = true;
    //}
}
