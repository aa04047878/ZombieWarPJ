using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    /// <summary>
    /// 紆よ
    /// </summary>
    public Vector3 direction;
    /// <summary>
    /// 紆硉
    /// </summary>
    public float speed;
    /// <summary>
    /// 端甡
    /// </summary>
    public float damage;
    public bool touchWoodCreate;
    private bool isDestroyed;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        //10笆綪反紆(禬礶)
        Destroy(gameObject, 10);
        //璹綷ㄆン
        //EventCenter.Instance.AddEventListener(EventType.eventGameOver, GameOver);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.tag == "Zombie")
        {
            Debug.Log($"ゴ┣ : {hit.name}");
            //眔Zombie竲セ
            ZombieNormal zombie = hit.GetComponent<ZombieNormal>();
            //㊣Zombie竲セChangeHealthよ猭
            zombie.ChangeHealth(-damage);
            DestroyBullet();
        }
    }

    public virtual void DestroyBullet()
    {
        //綪反紆
        Destroy(gameObject);
        isDestroyed = true;
    }

    //protected void GameOver()
    //{
    //    //笴栏挡綪反紆
    //    Destroy(gameObject);
    //}

    //protected void OnTimeDestroy()
    //{
    //    if (isDestroyed)
    //        return;

    //    //綪反紆
    //    Destroy(gameObject);
    //    isDestroyed = true;
    //}
}
