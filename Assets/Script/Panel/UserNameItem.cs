using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserNameItem : MonoBehaviour
{
    private GameObject select;
    private TMP_Text txt;
    public UserData userData;
    private Button btn;
    private UserPanel parent;
    private string itemType;

    private void Awake()
    {
        txt = transform.Find("Name").GetComponent<TMP_Text>();
        select = transform.Find("Select").gameObject;
        Debug.Log($"select.name : {select.name}");
        btn = GetComponent<Button>();
        itemType = "Name";
    }

    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(() => OnBtnNameItem());
        select.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnBtnNameItem()
    {
        if (itemType == "Name")
        {
            parent.curUserName = userData.name;
        }
        else
        {
            //���}�s�Τᤶ��
            BaseUIManager.Instance.OpenPanel(UIConst.newUserPanel);
        }
        
    }

    /// <summary>
    /// ���s��z�襤
    /// </summary>
    public void RefreshSelect()
    {
        select.SetActive(userData.name == parent.curUserName);
        Debug.Log($"�ۤv���W����ƦW�l : {userData.name}, userPanel��e�W�l : {parent.curUserName}");
        Debug.Log($"�O�_���}select : {userData.name == parent.curUserName}");
    }

    public void InitItem(UserData userData, UserPanel userPanel)
    {
        this.userData = userData;
        txt.text = userData.name;
        parent = userPanel;
    }

    public void InitNewUserItem()
    {
        itemType = "New";
        txt.text = "�Ыطs�Τ�";
    }
}
