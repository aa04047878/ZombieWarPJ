using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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
    /// <summary>
    /// 血量
    /// </summary>
    public float health;
    /// <summary>
    /// 當前血量
    /// </summary>
    private float currentHealth;
    /// <summary>
    /// 頭的物件
    /// </summary>
    private GameObject head;
    /// <summary>
    /// 受傷到一定程度
    /// </summary>
    private bool lostHead;
    /// <summary>
    /// 是否死亡
    /// </summary>
    private bool isDie;
    /// <summary>
    /// 頭落下時的血量條件
    /// </summary>
    private float lostHeadHealth;
    private Animator ani;
    private const string walk = "Walk";
    public int id;
    // Start is called before the first frame update
    void Start()
    {
        isWalk = true;
        direction = Vector3.left;
        ani = GetComponent<Animator>();
        damageTimer = 0;
        SetAttribute();
        head = transform.Find("Head").gameObject;
        isDie = false;
        lostHead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDie)
            return;
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
        //死亡不能攻擊
        if (isDie)
            return;

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
        //死亡不能攻擊
        if (isDie)
            return;

        if (hit.tag == "Plant")
        {
            Debug.Log("OnTriggerStay2D");
            //過了傷害間隔時間，就造成一次傷害
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                //攻擊植物
                Plant plant = hit.transform.GetComponent<Plant>();
                float newHealth = plant.ChangeHealth(-damage);
                Debug.Log($"{hit.name}的當前血量 : {newHealth}");
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

    /// <summary>
    /// 改變血量
    /// </summary>
    /// <param name="num"></param>
    public void ChangeHealth(float num)
    {
        if (currentHealth <= 0) //A子彈已讓我死掉，B子彈不能再調用此方法，否則動畫會再撥一次
            return;

        //Debug.Log($"受到傷害 : {num}");
        //改變血量後，當前血量介於0 ~ 總血量之間;
        currentHealth = Mathf.Clamp(currentHealth + num, 0, health);
        if (currentHealth < lostHeadHealth)
        {
            if (!lostHead)
            {
                lostHead = true;
                ani.SetBool("LostHead", true);
                head.SetActive(true);
                head.transform.GetComponent<Animator>().SetBool("LostHead", true);
            }
        }

        if (currentHealth <= 0)
        {
            ani.SetTrigger("Die");
            isDie = true;

            if (GameManager.instance.dieZombieIdList.Contains(id))
            {
                return;
            }

            GameManager.instance.curKillZombieCount++;
            int count = GameManager.instance.curKillZombieCount;
            GameManager.instance.killCountResult.Enqueue(count);
            GameManager.instance.AddIdList(id);
        }
    }

    /// <summary>
    /// 死亡動畫結束(動畫事件)
    /// </summary>
    public void DieAinOver()
    {
        ani.enabled = false;
        //Debug.Log("測試殭屍死幾次");
        GameManager.instance.curZombieTotalNum--;
        GameManager.instance.ZombieDie(gameObject, id);
        Destroy(gameObject);
    }

    /// <summary>
    /// 設定屬性
    /// </summary>
    public void SetAttribute()
    {
        UserData userData = LocalConfig.LoadUserData(BaseManager.Instance.curUserName);
        switch (userData.level)
        {
            case 1:
                health = 75;
                damage = 20;
                break;
            case 2:
                health = 100;
                damage = 30;
                break;
            case 3:
                health = 150;
                damage = 40;
                break;
            default:
                Debug.Log("目前沒還有此關卡");
                break;
        }
        currentHealth = health;
        lostHeadHealth = health / 2;
    }

    public void SetId(int id)
    {
        this.id = id;
    }
}
