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
    }

    protected override void Start()
    {
        base.Start();
        btnOk = UITool.GetUIComponent<Button>(gameObject,"BtnOk");
        btnCancel = UITool.GetUIComponent<Button>(gameObject, "BtnCancel");
        inputField = UITool.GetUIComponent<TMP_InputField>(gameObject, "InputField");
        btnOk.onClick.AddListener(() => OnBtnOk());
        btnCancel.onClick.AddListener(() => OnBtnCancel());
        inputField.onValueChanged.AddListener((value) => OnInputFieldChange(value));
    }

    private void OnBtnOk()
    {
        //�ˬd�Τ�W�٬O�_����
        if (inputString.Trim() == "")
        {
            Debug.Log("�Τ�W�٤��ର��");
            return;
        }

        //�ˬd�Τ�W�٬O�_�w�s�b
        if (LocalConfig.LoadUserData(inputString) != null)
        {
            Debug.Log("�Τ�W�٤w�s�b");
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
            Debug.Log("�Τ�W�٤��ର��");
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
