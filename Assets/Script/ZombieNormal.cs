using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Start is called before the first frame update
    void Start()
    {
        isWalk = true;
        direction = Vector3.left;
        ani = GetComponent<Animator>();
        damageTimer = 0;
        health = 100;
        currentHealth = health;
        lostHeadHealth = 50;
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
        Debug.Log($"����ˮ` : {num}");
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
        }
    }

    /// <summary>
    /// ���`�ʵe����(�ʵe�ƥ�)
    /// </summary>
    public void DieAinOver()
    {
        ani.enabled = false;
        Destroy(gameObject);
    }
}
