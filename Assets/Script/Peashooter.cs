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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0;
            //生成子彈
            Instantiate(peaBulletPrefab, bulletPos.position, Quaternion.identity);            
        }
    }
}
