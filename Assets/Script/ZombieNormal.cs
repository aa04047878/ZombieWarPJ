using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieNormal : MonoBehaviour
{
    /// <summary>
    /// 殭屍移動方向
    /// </summary>
    private Vector3 direction;
    /// <summary>
    /// 殭屍移動速度
    /// </summary>
    public float speed;
    /// <summary>
    /// 是否可以移動
    /// </summary>
    private bool isWalk;
    public int damage;
    /// <summary>
    /// 傷害間隔
    /// </summary>
    public float damageInterval;
    /// <summary>
    /// 傷害計時器
    /// </summary>
    private float damageTimer;

    private Animator ani;
    private const string walk = "Walk";
    // Start is called before the first frame update
    void Start()
    {
        isWalk = true;
        direction = Vector3.left;
        ani = GetComponent<Animator>();
        damageTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (isWalk)
        {
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D hit) //hit為碰到的對方
    {
        if (hit.tag == "Plant")
        {
            //碰到植物就停止移動
            isWalk = false;

            //攻擊植物
            ani.SetBool(walk, isWalk);
        }
    }

    private void OnTriggerStay2D(Collider2D hit)
    {

        if (hit.tag == "Plant")
        {
            Debug.Log("OnTriggerStay2D");
            //過了傷害間隔時間，就造成一次傷害
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                Peashooter peashooter = hit.transform.GetComponent<Peashooter>();
                float newHealth = peashooter.ChangeHealth(-damage);
                if (newHealth <= 0)
                {
                    isWalk = true;
                    ani.SetBool(walk, isWalk);
                }
                //重新計時
                damageTimer = 0;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D hit)
    {
        //植物被消滅
        if (hit.tag == "Plant")
        {
            isWalk = true;
            ani?.SetBool(walk, isWalk);
        }
    }
}
