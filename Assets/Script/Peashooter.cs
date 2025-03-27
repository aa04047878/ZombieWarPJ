using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peashooter : MonoBehaviour
{
    /// <summary>
    /// 間隔時間
    /// </summary>
    public float interval;
    /// <summary>
    /// 計時器
    /// </summary>
    public float timer;
    /// <summary>
    /// 子彈的預置物
    /// </summary>
    public GameObject peaBulletPrefab;

    public Transform bulletPos;
    /// <summary>
    /// 總血量
    /// </summary>
    public float health;
    /// <summary>
    /// 當前血量
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
            //生成子彈
            GameObject curBullet =  Instantiate(peaBulletPrefab, bulletPos.position, Quaternion.identity);
            ///設定子彈的父物件
            //curBullet.transform.parent = bulletPos;
        }
    }

    /// <summary>
    /// 改變血量
    /// </summary>
    /// <param name="num"></param>
    public float ChangeHealth(float num)
    {
        //改變血量後，當前血量介於0 ~ 總血量之間;
        currentHealth = Mathf.Clamp(currentHealth + num, 0, health);
        Debug.Log($"豌豆射手血量 : {currentHealth}");
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        return currentHealth;
    }
}
