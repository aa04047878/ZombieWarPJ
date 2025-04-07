using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingHandle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //DORotate(旋轉角度, 旋轉時間, 旋轉模式).SetEase(旋轉方式(線性旋轉)).SetLoops(循環次數(永久循環) 重新開始);
        transform.DORotate(new Vector3(0, 0, -360), 1f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
