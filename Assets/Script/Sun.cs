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

    //private void OnMouseDown()
    //{
    //    //��ƹ��I��������A�N�|����(�@��)

    //    //���ܤӶ��ƶq
    //    GameManager.instance.ChangeSunNum(25);
    //    //�I������A�Ӷ�����
    //    Destroy(gameObject);
    //}
}
