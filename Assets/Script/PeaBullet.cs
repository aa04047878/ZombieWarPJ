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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}
