using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : BasePanel
{
    public Button btnChangeUserName;
    private Button BtnAdventure;
    private TMP_Text txtUserName;
    private TMP_Text txtSmallLevel;

    protected override void Awake()
    {
        base.Awake();
        
    }

    protected override void Start()
    {
        base.Start();
        Debug.Log("���o���s");
        btnChangeUserName = UITool.GetUIComponent<Button>(this.gameObject ,"BtnChangeUserName");
        txtUserName = UITool.GetUIComponent<TMP_Text>(this.gameObject, "TxtUserName");
        txtSmallLevel = UITool.GetUIComponent<TMP_Text>(this.gameObject, "TxtSmallLevel");
        BtnAdventure = UITool.GetUIComponent<Button>(this.gameObject, "BtnAdventure");
        btnChangeUserName.onClick.AddListener(() => OnBtnChangeUser());
        BtnAdventure.onClick.AddListener(() => OnBtnAdventure());
        //�q�\�ƥ�
        EventCenter.Instance.AddEventListener<UserData>(EventType.eventNewUserCreate, OnEventNewUserCreate);
        EventCenter.Instance.AddEventListener<string>(EventType.eventCurUserChange, OnEventCurUserChange);

        //�O�_�إ߷s�Τ�(����L�k�إ߷s�Τ�???)
        if (BaseManager.Instance.curUserName.Trim() == "")
        {
            BaseUIManager.Instance.OpenPanel(UIConst.newUserPanel);
            return;
        }
        txtUserName.text = BaseManager.Instance.curUserName;
        Debug.Log($"BaseManager.Instance.curUserName : {BaseManager.Instance.curUserName}");
        UserData userData = LocalConfig.LoadUserData(BaseManager.Instance.curUserName);
        if (userData != null)
        {
            txtUserName.text = userData.name;
            txtSmallLevel.text = userData.level.ToString();
        }
        
    }

    private void OnBtnChangeUser()
    {
        BaseUIManager.Instance.OpenPanel(UIConst.userPanel);
    }

    private void OnBtnAdventure()
    {
        BaseUIManager.Instance.ClosePanel(UIConst.mainMenuPanel);
        SceneControl.LoadScene("Game");
    }

    private void OnEventNewUserCreate(UserData userData)
    {
        //��s��e�Τ�W��
        txtUserName.text = userData.name;
        txtSmallLevel.text = userData.level.ToString();
    }

    private void OnEventCurUserChange(string curName)
    {
        //��s��e�Τ�W��
        txtUserName.text = curName;
        if (LocalConfig.LoadUserData(curName) == null)
        {
            return;
        }
        
        txtSmallLevel.text = LocalConfig.LoadUserData(curName).level.ToString();
    }
}
