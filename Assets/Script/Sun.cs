using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    /// <summary>
    /// ����ɶ�
    /// </summary>
    public float duration;
    /// <summary>
    /// �p�ɾ�
    /// </summary>
    private float timer;
    private Vector3 targetPos;

    private void OnEnable()
    {
        targetPos = Vector3.zero;
        Debug.Log("�Ӷ���m��l��");
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        
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
            Destroy(gameObject);
        }
    }

    public void SetTargetPos(Vector3 pos)
    {
        targetPos = pos;
    }

    //private void OnMouseDown()
    //{
    //    //��ƹ��I��������A�N�|����(�@��)

    //    //���ܤӶ��ƶq
    //    GameManager.instance.ChangeSunNum(25);
    //    //�I������A�Ӷ�����
    //    Destroy(gameObject);
    //}
}
