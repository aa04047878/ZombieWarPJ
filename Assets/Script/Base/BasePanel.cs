using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    /*
   BasePanel : 
       所有介面的父類別，包含了開啟Panel和關閉Panel。
   */

    /// <summary>
    /// 介面名稱
    /// </summary>
    protected new string name;
    /// <summary>
    /// 當前介面是否已經被關閉
    /// </summary>
    protected bool isRemove = false;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {

    }

    public virtual void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public virtual void OpenPanel(string name)
    {
        this.name = name;
        SetActive(true);
    }

    public virtual void ClosePanel()
    {
        isRemove = true;
        SetActive(false);
        //已經關閉介面了，就要把在panelDict裡面的該介面移除
        if (BaseUIManager.Instance.panelDict.ContainsKey(name))
        {
            BaseUIManager.Instance.panelDict.Remove(name);
        }
        Destroy(gameObject);
    }
}
