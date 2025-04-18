using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    /// <summary>
    /// 持續時間
    /// </summary>
    public float duration;
    /// <summary>
    /// 計時器
    /// </summary>
    private float timer;
    private Vector3 targetPos;
    public ObjectPool<Sun> sunPool;
    private void OnEnable()
    {
        targetPos = Vector3.zero;
        Debug.Log("太陽位置初始化");
    }

    //private void OnDisable()
    //{
    //    DontDestroyOnLoad(gameObject);
    //}

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        sunPool = ObjectPool<Sun>.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetPos != Vector3.zero && Vector3.Distance(targetPos, transform.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 1);
            return;
        }

        timer += Time.deltaTime;
        if (timer >= duration)
        {
            //Destroy(gameObject);
            sunPool.Recycle(this);
        }
    }

    public void SetTargetPos(Vector3 pos)
    {
        targetPos = pos;
    }

    public void ResetTimer()
    {
        timer = 0;
    }

    //private void OnMouseDown()
    //{
    //    //方滑鼠點擊此物件，就會執行(一次)

    //    //改變太陽數量
    //    GameManager.instance.ChangeSunNum(25);
    //    //點擊完後，太陽消失
    //    Destroy(gameObject);
    //}
}
