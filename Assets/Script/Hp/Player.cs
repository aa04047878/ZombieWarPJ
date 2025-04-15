using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private float curHp; // 目前生命值
    private float maxHp; // 總血量
    private Image hpBar; // 生命條
    // Start is called before the first frame update
    void Start()
    {
        curHp = 100f; // 初始化生命值
        maxHp = curHp;
        hpBar = UITool.GetUIComponent<Image>(gameObject, "HpBar"); // 獲取生命條的UI組件
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeHealth(float hp)
    {
        curHp = Mathf.Clamp(curHp + hp, 0, maxHp);
        hpBar.fillAmount = curHp / maxHp;
        Debug.Log($"玩家目前血量 : {curHp}");
        if (curHp <= 0)
        {
            //通知遊戲失敗
            EventCenter.Instance.EventTrigger(EventType.eventGameFail);

        }
    }
}
