using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ZombieNormal : MonoBehaviour
{
    /// <summary>
    /// �L�Ͳ��ʤ�V
    /// </summary>
    private Vector3 direction;
    /// <summary>
    /// �L�Ͳ��ʳt��
    /// </summary>
    public float speed;
    /// <summary>
    /// �O�_�i�H����
    /// </summary>
    private bool isWalk;
    public int damage;
    /// <summary>
    /// �ˮ`���j
    /// </summary>
    public float damageInterval;
    /// <summary>
    /// �ˮ`�p�ɾ�
    /// </summary>
    private float damageTimer;
    /// <summary>
    /// ��q
    /// </summary>
    public float health;
    /// <summary>
    /// ��e��q
    /// </summary>
    private float currentHealth;
    /// <summary>
    /// �Y������
    /// </summary>
    private GameObject head;
    /// <summary>
    /// ���˨�@�w�{��
    /// </summary>
    private bool lostHead;
    /// <summary>
    /// �O�_���`
    /// </summary>
    private bool isDie;
    /// <summary>
    /// �Y���U�ɪ���q����
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

    private void OnTriggerEnter2D(Collider2D hit) //hit���I�쪺���
    {
        //���`�������
        if (isDie)
            return;

        if (hit.tag == "Plant")
        {
            //�I��Ӫ��N�����
            isWalk = false;

            //�����Ӫ�
            ani.SetBool(walk, isWalk);
        }
    }

    private void OnTriggerStay2D(Collider2D hit)
    {
        //���`�������
        if (isDie)
            return;

        if (hit.tag == "Plant")
        {
            Debug.Log("OnTriggerStay2D");
            //�L�F�ˮ`���j�ɶ��A�N�y���@���ˮ`
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                //�����Ӫ�
                Plant plant = hit.transform.GetComponent<Plant>();
                float newHealth = plant.ChangeHealth(-damage);
                Debug.Log($"{hit.name}����e��q : {newHealth}");
                if (newHealth <= 0)
                {
                    isWalk = true;
                    ani.SetBool(walk, isWalk);
                }
                //���s�p��
                damageTimer = 0;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D hit)
    {
        //�Ӫ��Q����
        if (hit.tag == "Plant")
        {
            isWalk = true;
            ani?.SetBool(walk, isWalk);
        }
    }

    /// <summary>
    /// ���ܦ�q
    /// </summary>
    /// <param name="num"></param>
    public void ChangeHealth(float num)
    {
        if (currentHealth <= 0) //A�l�u�w���ڦ����AB�l�u����A�եΦ���k�A�_�h�ʵe�|�A���@��
            return;

        //Debug.Log($"����ˮ` : {num}");
        //���ܦ�q��A��e��q����0 ~ �`��q����;
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
    /// ���`�ʵe����(�ʵe�ƥ�)
    /// </summary>
    public void DieAinOver()
    {
        ani.enabled = false;
        //Debug.Log("�����L�ͦ��X��");
        GameManager.instance.curZombieTotalNum--;
        GameManager.instance.ZombieDie(gameObject, id);
        Destroy(gameObject);
    }

    /// <summary>
    /// �]�w�ݩ�
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
                Debug.Log("�ثe�S�٦������d");
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
