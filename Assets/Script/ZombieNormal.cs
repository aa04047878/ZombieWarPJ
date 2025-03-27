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

    private void OnTriggerEnter2D(Collider2D hit) //hit���I�쪺���
    {
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

        if (hit.tag == "Plant")
        {
            Debug.Log("OnTriggerStay2D");
            //�L�F�ˮ`���j�ɶ��A�N�y���@���ˮ`
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
}
