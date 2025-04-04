using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    /// <summary>
    /// �`��q
    /// </summary>
    public float health;
    /// <summary>
    /// ��e��q
    /// </summary>
    protected float currentHealth;
    protected bool start;
    protected Animator ani;
    protected BoxCollider2D boxCollider2D;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        start = false;
        currentHealth = health;
        ani = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = false;
        ///�]�w�ʵe�t��(���n����)
        ani.speed = 0;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    /// <summary>
    /// ���ܦ�q
    /// </summary>
    /// <param name="num"></param>
    public virtual float ChangeHealth(float num)
    {
        //���ܦ�q��A��e��q����0 ~ �`��q����;
        currentHealth = Mathf.Clamp(currentHealth + num, 0, health);
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        return currentHealth;
    }

    /// <summary>
    /// �شӧ�����ҰʴӪ�
    /// </summary>
    public virtual void SetPlantStart()
    {
        start = true;
        ///�]�w�ʵe�t��(����)
        ani.speed = 1;
        boxCollider2D.enabled = true;
    }
}
