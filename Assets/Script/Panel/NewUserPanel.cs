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
        //�ˬd�Τ�W�٬O�_����
        if (inputString.Trim() == "")
        {
            Debug.Log("�п�J�W�l�A����!!!");
            BasePanel panel =  BaseUIManager.Instance.OpenPanel(UIConst.promptMessagePanel);
            PromptMessagePanel promptMessagePanel = panel as PromptMessagePanel;
            promptMessagePanel.SetMessageText("�п�J�W�l�A����!!!");
            promptMessagePanel.SetUpTitleText("�s�Τ�");
            return;
        }

        //�ˬd�Τ�W�٬O�_�w�s�b
        if (LocalConfig.LoadUserData(inputString) != null)
        {
            Debug.Log("�Τ�W�٤w�s�b!!!");
            BasePanel panel = BaseUIManager.Instance.OpenPanel(UIConst.promptMessagePanel);
            PromptMessagePanel promptMessagePanel = panel as PromptMessagePanel;
            promptMessagePanel.SetMessageText("�Τ�W�٤w�s�b!!!");
            promptMessagePanel.SetUpTitleText("�s�Τ�");
            return;
        }

        //�Ыطs�Τ�
        UserData userData = new UserData(inputString, 1);
        LocalConfig.SaveUserData(userData);
        //��s��e�Τ�W��
        BaseManager.Instance.SetCurUserName(userData.name);
        //�q���ƥ�w�o��
        EventCenter.Instance.EventTrigger(EventType.eventNewUserCreate, userData);
        ClosePanel();
    }

    private void OnBtnCancel()
    {
        if (BaseManager.Instance.curUserName == "")
        {
            Debug.Log("�п�J�W�l�A����!!!");
            BasePanel panel = BaseUIManager.Instance.OpenPanel(UIConst.promptMessagePanel);
            PromptMessagePanel promptMessagePanel = panel as PromptMessagePanel;
            promptMessagePanel.SetMessageText("�п�J�W�l�A����!!!");
            promptMessagePanel.SetUpTitleText("�s�Τ�");
            return;
        }

        ClosePanel();
    }

    private void OnInputFieldChange(string value)
    {
        //��Τ��J�ɡA��s��e�Τ�W��
        inputString = value;
    }
}
