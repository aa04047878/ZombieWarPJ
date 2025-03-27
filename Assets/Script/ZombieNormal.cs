using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieNormal : MonoBehaviour
{
    /// <summary>
    /// 殭屍移動方向
    /// </summary>
    private Vector3 direction;
    /// <summary>
    /// 殭屍移動速度
    /// </summary>
    public float speed;
    /// <summary>
    /// 是否可以移動
    /// </summary>
    private bool isWalk;
    private Animator ani;
    // Start is called before the first frame update
    void Start()
    {
        isWalk = true;
        direction = Vector3.left;
        ani = GetComponent<Animator>();
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
}
