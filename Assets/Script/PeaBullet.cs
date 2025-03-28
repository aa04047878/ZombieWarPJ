using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaBullet : MonoBehaviour
{
    /// <summary>
    /// 子彈的方向
    /// </summary>
    public Vector3 direction;
    /// <summary>
    /// 子彈的速度
    /// </summary>
    public float speed;
    /// <summary>
    /// 傷害
    /// </summary>
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        damage = 15;
        //10秒後自動銷毀子彈(因為已超出畫面)
        Destroy(gameObject, 10);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.tag == "Zombie")
        {
            Debug.Log($"打到怪物 : {hit.name}");
            //取得Zombie腳本
            ZombieNormal zombie = hit.GetComponent<ZombieNormal>();
            //呼叫Zombie腳本的ChangeHealth方法
            zombie.ChangeHealth(-damage);
            //銷毀子彈
            Destroy(gameObject);
        }
    }
}
