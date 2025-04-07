using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingHandle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //DORotate(���ਤ��, ����ɶ�, ����Ҧ�).SetEase(����覡(�u�ʱ���)).SetLoops(�`������(�ä[�`��) ���s�}�l);
        transform.DORotate(new Vector3(0, 0, -360), 1f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
