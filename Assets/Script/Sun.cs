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
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }

    private void OnMouseDown()
    {
        //方滑鼠點擊此物件，就會執行(一次)

        //改變太陽數量
        GameManager.instance.ChangeSunNum(25);
        //點擊完後，太陽消失
        Destroy(gameObject);
    }
}
