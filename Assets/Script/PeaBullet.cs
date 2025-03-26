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
