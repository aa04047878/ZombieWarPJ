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
            BasePanel panel = BaseUIManager.Instance.OpenPanel(UIConst.promptMessagePanel);
            PromptMessagePanel promptMessagePanel = panel as PromptMessagePanel;
            promptMessagePanel.SetMessageText("�п�J�W�l�A����!!!");
            return;
        }
        //if (inputString == "")
        //{
        //    Debug.Log("�Τ�W�٤��ର��");
        //    return;
        //}



        //�ˬd�Τ�W�٬O�_�w�s�b
        if (LocalConfig.LoadUserData(inputString) != null)
        {
            Debug.Log("�Τ�W�٤w�s�b");
            BasePanel panel = BaseUIManager.Instance.OpenPanel(UIConst.promptMessagePanel);
            PromptMessagePanel promptMessagePanel = panel as PromptMessagePanel;
            promptMessagePanel.SetMessageText("�Τ�W�٤w�s�b");
            return;
        }

        //this.oldUserData.name = inputString;
        //�Ыطs�Τ�
        UserData userData = new UserData(inputString, oldUserData.level);
        LocalConfig.SaveUserData(userData);

        //�M���ª��Ȥ�
        bool isSuccess =  LocalConfig.ClearUserData(oldUserData.name);
        if (isSuccess)
        {
            //��s��e�Τ�W��
            BaseManager.Instance.SetCurUserName(userData.name);
        }
        //�q���ƥ�w�o��
        EventCenter.Instance.EventTrigger(EventType.eventNewUserCreate, userData);
        BaseUIManager.Instance.ClosePanel(UIConst.reNameUserPanel);
    }

    private void OnBtnCancel()
    {
        BaseUIManager.Instance.ClosePanel(UIConst.reNameUserPanel);
    }

    
    private void OnInputFieldChange(string value)
    {
        //value = �A�binputfield�̭��ҿ�J����r
        inputString = value;
    }
}
