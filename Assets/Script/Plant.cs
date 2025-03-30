using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    /// <summary>
    /// 總血量
    /// </summary>
    public float health;
    /// <summary>
    /// 當前血量
    /// </summary>
    protected float currentHealth;
    protected bool start;
    protected Animator ani;
    protected BoxCollider2D boxCollider2D;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        start = false;
        health = 100;
        currentHealth = health;
        ani = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = false;
        ///設定動畫速度(不要撥放)
        ani.speed = 0;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    /// <summary>
    /// 改變血量
    /// </summary>
    /// <param name="num"></param>
    public float ChangeHealth(float num)
    {
        //改變血量後，當前血量介於0 ~ 總血量之間;
        currentHealth = Mathf.Clamp(currentHealth + num, 0, health);
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        return currentHealth;
    }

    /// <summary>
    /// 種植完成後啟動植物
    /// </summary>
    public void SetPlantStart()
    {
        start = true;
        ///設定動畫速度(撥放)
        ani.speed = 1;
        boxCollider2D.enabled = true;
    }
}
