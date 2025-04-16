using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    /*
   BasePanel : 
       �Ҧ������������O�A�]�t�F�}��Panel�M����Panel�C
   */

    /// <summary>
    /// �����W��
    /// </summary>
    protected new string name;
    /// <summary>
    /// ��e�����O�_�w�g�Q����
    /// </summary>
    protected bool isRemove = false;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
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
        //�w�g���������F�A�N�n��bpanelDict�̭����Ӥ�������
        if (BaseUIManager.Instance.panelDict.ContainsKey(name))
        {
            BaseUIManager.Instance.panelDict.Remove(name);
        }
        Destroy(gameObject);
    }
}
