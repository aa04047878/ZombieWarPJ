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
        //Debug.Log($"select.name : {select.name}");
        btn = GetComponent<Button>();
        itemType = "Name";
        select.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(() => OnBtnNameItem());
       
        
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
            //打開新用戶介面
            BaseUIManager.Instance.OpenPanel(UIConst.newUserPanel);
        }
        
    }

    /// <summary>
    /// 重新整理選中
    /// </summary>
    public void RefreshSelect()
    {
        select.SetActive(userData.name == parent.curUserName);
        Debug.Log($"自己身上的資料名子 : {userData.name}, userPanel當前名子 : {parent.curUserName}");
        Debug.Log($"是否打開select : {userData.name == parent.curUserName}");
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
        txt.text = "創建新用戶";
    }
}
