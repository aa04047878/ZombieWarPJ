using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    /*
    使用到的API解釋 : 
    1. Mathf.Clamp(value, min, max) :
        Unity的Mathf.Clamp函數將指定的value限制在min ~ max之間。如果value小於min，它返回min；如果value大於max，它返回max；否則，返回value本身。
    2. Physics2D.OverlapPointAll : 
        在 Unity 中，OverlapPointAll是物理引擎的一部分，屬於Physics2D類。它的主要功能是檢查 2D 空間中的某個點是否與多個碰撞器重疊，並返回所有重疊的碰撞器。
    3. 碰撞trigger :
        OnTriggerEnter2D() 、 OnTriggerStay2D() 、 OnTriggerExit2D()
    4. OnMouseDown() :
        當滑鼠按下時，觸發此事件
    5. ClosestPoint() :
       計算和collider的接觸點
    */

    /*
    使用到的接口(介面) : 
    
    */
}
