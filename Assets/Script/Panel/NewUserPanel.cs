using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewUserPanel : BasePanel
{
    private Button btnOk;
    private Button btnCancel;
    private TMP_InputField inputField;
    private string inputString;
    protected override void Awake()
    {
        base.Awake();
        btnOk = UITool.GetUIComponent<Button>(gameObject, "BtnOk");
        btnCancel = UITool.GetUIComponent<Button>(gameObject, "BtnCancel");
        inputField = UITool.GetUIComponent<TMP_InputField>(gameObject, "InputField");
    }

    protected override void Start()
    {
        base.Start();
        inputString = "";
        btnOk.onClick.AddListener(() => OnBtnOk());
        btnCancel.onClick.AddListener(() => OnBtnCancel());
        inputField.onValueChanged.AddListener((value) => OnInputFieldChange(value));
    }

    private void OnBtnOk()
    {
        //檢查用戶名稱是否為空
        if (inputString.Trim() == "")
        {
            Debug.Log("請輸入名子，謝謝!!!");
            BasePanel panel =  BaseUIManager.Instance.OpenPanel(UIConst.promptMessagePanel);
            PromptMessagePanel promptMessagePanel = panel as PromptMessagePanel;
            promptMessagePanel.SetMessageText("請輸入名子，謝謝!!!");
            promptMessagePanel.SetUpTitleText("新用戶");
            return;
        }

        //檢查用戶名稱是否已存在
        if (LocalConfig.LoadUserData(inputString) != null)
        {
            Debug.Log("用戶名稱已存在!!!");
            BasePanel panel = BaseUIManager.Instance.OpenPanel(UIConst.promptMessagePanel);
            PromptMessagePanel promptMessagePanel = panel as PromptMessagePanel;
            promptMessagePanel.SetMessageText("用戶名稱已存在!!!");
            promptMessagePanel.SetUpTitleText("新用戶");
            return;
        }

        //創建新用戶
        UserData userData = new UserData(inputString, 1);
        LocalConfig.SaveUserData(userData);
        //更新當前用戶名稱
        BaseManager.Instance.SetCurUserName(userData.name);
        //通知事件已發生
        EventCenter.Instance.EventTrigger(EventType.eventNewUserCreate, userData);
        ClosePanel();
    }

    private void OnBtnCancel()
    {
        if (BaseManager.Instance.curUserName == "")
        {
            Debug.Log("請輸入名子，謝謝!!!");
            BasePanel panel = BaseUIManager.Instance.OpenPanel(UIConst.promptMessagePanel);
            PromptMessagePanel promptMessagePanel = panel as PromptMessagePanel;
            promptMessagePanel.SetMessageText("請輸入名子，謝謝!!!");
            promptMessagePanel.SetUpTitleText("新用戶");
            return;
        }

        ClosePanel();
    }

    private void OnInputFieldChange(string value)
    {
        //當用戶輸入時，更新當前用戶名稱
        inputString = value;
    }
}
