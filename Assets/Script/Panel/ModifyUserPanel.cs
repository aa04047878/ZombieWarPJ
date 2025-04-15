using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModifyUserPanel : BasePanel
{
    private Button btnOk;
    private Button btnCancel;
    private TMP_InputField inputField;
    private string inputString;
    public UserData oldUserData;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        btnOk = UITool.GetUIComponent<Button>(gameObject, "BtnOk");
        btnCancel = UITool.GetUIComponent<Button>(gameObject, "BtnCancel");
        inputField = UITool.GetUIComponent<TMP_InputField>(gameObject, "InputField");
        btnOk.onClick.AddListener(() => OnBtnOk());
        btnCancel.onClick.AddListener(() => OnBtnCancel());
        inputField.onValueChanged.AddListener((value) => OnInputFieldChange(value));
    }


    private void OnBtnOk()
    {
        //this.oldUserData.name = inputString;
        //創建新用戶
        UserData userData = new UserData(inputString, oldUserData.level);
        LocalConfig.SaveUserData(userData);
       
        //清除舊的客戶
        bool isSuccess =  LocalConfig.ClearUserData(oldUserData.name);
        if (isSuccess)
        {
            //更新當前用戶名稱
            BaseManager.Instance.SetCurUserName(userData.name);
        }
        //通知事件已發生
        //EventCenter.Instance.EventTrigger(EventType.eventNewUserCreate, userData);
        BaseUIManager.Instance.ClosePanel(UIConst.reNameUserPanel);
    }

    private void OnBtnCancel()
    {
        BaseUIManager.Instance.ClosePanel(UIConst.reNameUserPanel);
    }

    
    private void OnInputFieldChange(string value)
    {
        //value = 你在inputfield裡面所輸入的文字
        inputString = value;
    }
}
