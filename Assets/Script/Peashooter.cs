using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peashooter : MonoBehaviour
{
    /// <summary>
    /// ���j�ɶ�
    /// </summary>
    public float interval;
    /// <summary>
    /// �p�ɾ�
    /// </summary>
    public float timer;
    /// <summary>
    /// �l�u���w�m��
    /// </summary>
    public GameObject peaBulletPrefab;

    public Transform bulletPos;
    /// <summary>
    /// �`��q
    /// </summary>
    public float health;
    /// <summary>
    /// ��e��q
    /// </summary>
    private float currentHealth;
    // Start is called before the first frame update
    private void OnEnable()
    {
        bulletPos = transform.Find("BulletPos");
    }

    void Start()
    {
        health = 100;
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0;
            //�ͦ��l�u
            GameObject curBullet =  Instantiate(peaBulletPrefab, bulletPos.position, Quaternion.identity);
            ///�]�w�l�u��������
            //curBullet.transform.parent = bulletPos;
        }
    }

    /// <summary>
    /// ���ܦ�q
    /// </summary>
    /// <param name="num"></param>
    public float ChangeHealth(float num)
    {
        //���ܦ�q��A��e��q����0 ~ �`��q����;
        currentHealth = Mathf.Clamp(currentHealth + num, 0, health);
        Debug.Log($"�ܨ��g���q : {currentHealth}");
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        return currentHealth;
    }
}
