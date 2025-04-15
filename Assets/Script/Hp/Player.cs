using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private float curHp; // �ثe�ͩR��
    private float maxHp; // �`��q
    private Image hpBar; // �ͩR��
    // Start is called before the first frame update
    void Start()
    {
        curHp = 100f; // ��l�ƥͩR��
        maxHp = curHp;
        hpBar = UITool.GetUIComponent<Image>(gameObject, "HpBar"); // ����ͩR����UI�ե�
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeHealth(float hp)
    {
        curHp = Mathf.Clamp(curHp + hp, 0, maxHp);
        hpBar.fillAmount = curHp / maxHp;
        Debug.Log($"���a�ثe��q : {curHp}");
        if (curHp <= 0)
        {
            //�q���C������
            EventCenter.Instance.EventTrigger(EventType.eventGameFail);

        }
    }
}
