using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaBullet : MonoBehaviour
{
    /// <summary>
    /// �l�u����V
    /// </summary>
    public Vector3 direction;
    /// <summary>
    /// �l�u���t��
    /// </summary>
    public float speed;
    /// <summary>
    /// �ˮ`
    /// </summary>
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        damage = 15;
        //10���۰ʾP���l�u(�]���w�W�X�e��)
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
            Debug.Log($"����Ǫ� : {hit.name}");
            //���oZombie�}��
            ZombieNormal zombie = hit.GetComponent<ZombieNormal>();
            //�I�sZombie�}����ChangeHealth��k
            zombie.ChangeHealth(-damage);
            //�P���l�u
            Destroy(gameObject);
        }
    }
}
