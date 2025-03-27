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
