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
    private Button btnSetting;

    #region �R�x
    public Image stageCharacter;
    public Sprite spriteData;
    private Button btnStage;
    private Dictionary<int, string> stageItemSpriteDic;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        btnChangeUserName = UITool.GetUIComponent<Button>(this.gameObject, "BtnChangeUserName");
        txtUserName = UITool.GetUIComponent<TMP_Text>(this.gameObject, "TxtUserName");
        txtSmallLevel = UITool.GetUIComponent<TMP_Text>(this.gameObject, "TxtSmallLevel");
        BtnAdventure = UITool.GetUIComponent<Button>(this.gameObject, "BtnAdventure");
        btnSetting = UITool.GetUIComponent<Button>(gameObject, "BtnSetting");
        btnStage = UITool.GetUIComponent<Button>(gameObject, "BtnStage");

        stageItemSpriteDic = new Dictionary<int, string>();
        stageItemSpriteDic.Add(0, Globals.PeashooterAllbody);
        stageItemSpriteDic.Add(1, Globals.SunflowerAllbody);
        stageItemSpriteDic.Add(2, Globals.WallNutAllbody);
        stageItemSpriteDic.Add(4, Globals.SquashAllbody);
        stageItemSpriteDic.Add(5, Globals.PurpleMushroomAllbody);
    }

    protected override void Start()
    {
        base.Start();
        Debug.Log("���o���s");
        
        btnChangeUserName.onClick.AddListener(() => OnBtnChangeUser());
        BtnAdventure.onClick.AddListener(() => OnBtnAdventure());
        btnSetting.onClick.AddListener(() => OnBtnSetting());
        btnStage.onClick.AddListener(() => OnBtnStage());
        //�q�\�ƥ�
        EventCenter.Instance.AddEventListener<UserData>(EventType.eventNewUserCreate, OnEventNewUserCreate);
        EventCenter.Instance.AddEventListener<string>(EventType.eventCurUserChange, OnEventCurUserChange);
        EventCenter.Instance.AddEventListener<int>(EventType.eventChangeStageCharacter, SetStageCharacter);

        //�O�_�إ߷s�Τ�
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

        //Ū���R�x������
        StageData stageData = LocalConfig.LoadStageData();
        spriteData = GetStageCharacterData(stageData.id); // Example to get the spriteData for Peashooter
        stageCharacter.sprite = spriteData;
    }

    private void OnBtnChangeUser()
    {
        BaseUIManager.Instance.OpenPanel(UIConst.userPanel);
    }

    private void OnBtnAdventure()
    {
        UserData userData = LocalConfig.LoadUserData(BaseManager.Instance.curUserName);

        if (userData.level > 3)
        {
            Debug.Log($"�ثe�u�}����3���A�q�д��ݤU����s�A����!!!");
            BasePanel panel =  BaseUIManager.Instance.OpenPanel(UIConst.promptMessagePanel);
            PromptMessagePanel promptMessagePanel = panel.GetComponent<PromptMessagePanel>();
            promptMessagePanel.SetMessageText("�ثe�u�}����3���A�q�д��ݤU����s�A����!!!");
            promptMessagePanel.SetUpTitleText("�T��");
            return;
        }
        SceneControl.LoadSceneAsync("Game");
        FadeManager.instance.FadeIn();
        FadeManager.instance.DarkBgStandby();
        
    }

    private void OnBtnSetting()
    {
        BaseUIManager.Instance.OpenPanel(UIConst.settingPanel);
    }

    private void OnBtnStage()
    {
        BaseUIManager.Instance.OpenPanel(UIConst.stagePanel);
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

    public Sprite GetStageCharacterData(int id)
    {
        if (stageItemSpriteDic.ContainsKey(id))
        {
            return Resources.Load<Sprite>(stageItemSpriteDic[id]);
        }
        else
        {
            Debug.Log("Stage character not found for id: " + id);
            return null;
        }
    }

    public void SetStageCharacter(int id)
    {
        if (stageItemSpriteDic.ContainsKey(id))
        {
            spriteData =  Resources.Load<Sprite>(stageItemSpriteDic[id]);
            stageCharacter.sprite = spriteData;
        }
        else
        {
            Debug.Log("Stage character not found for id: " + id);
        }
    }
}
