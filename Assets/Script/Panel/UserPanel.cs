using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class UserPanel : BasePanel
{
    public Button btnOk;
    public Button btnCancel;
    public Button btnDelete;
    private Button BtnReName;
    /// <summary>
    /// ���ܰT��
    /// </summary>
    private GameObject promptMessage;
    private string _curUserName;
    public string curUserName
    {
        get {  return _curUserName; }
        set
        {
            _curUserName = value;
            ReflashSelectState();
        }
    }
    public ScrollRect scroll;
    public GameObject userNamePrefab;
    private Dictionary<string, UserNameItem> menuNameItems;
    #region ���ռƾ�
    private List<UserData> testData;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        btnOk = UITool.GetUIComponent<Button>(this.gameObject, "BtnOk");
        btnCancel = UITool.GetUIComponent<Button>(this.gameObject, "BtnCancel");
        btnDelete = UITool.GetUIComponent<Button>(this.gameObject, "BtnDelete");
        BtnReName = UITool.GetUIComponent<Button>(gameObject, "BtnReName");
        promptMessage = UITool.GetUIComponent<GameObject>(this.gameObject, "PromptMessage");
    }

    protected override void Start()
    {
        base.Start();
       
        if (BaseManager.Instance.curUserName != "")
            btnCancel.gameObject.SetActive(true);

        btnOk.onClick.AddListener(() => OnBtnOk());
        btnCancel.onClick.AddListener(() => OnBtnCancel());
        btnDelete.onClick.AddListener(() => OnBtnDelete());
        BtnReName.onClick.AddListener(() => OnBtnReName());
        //�q�\�ƥ�
        EventCenter.Instance.AddEventListener<UserData>(EventType.eventNewUserCreate, OnEventNewUserCreate);
        EventCenter.Instance.AddEventListener<UserData>(EventType.eventUserDelete, OnEventUserDelete);
        EventCenter.Instance.AddEventListener<string>(EventType.eventCurUserChange, OnEventCurUserChange);
        //EventCenter.Instance.AddEventListener<string>(EventType.eventModifyUser, OnEventCurUserChange);
        EventCenter.Instance.AddEventListener<UserData>(EventType.eventModifyUser, OnEventModifyUser);
        RefreshMainPanel();
        curUserName = BaseManager.Instance.curUserName;
    }

    private void OnBtnOk()
    {
        if (curUserName != "")
        {
            //��s��e�Τ�W��
            BaseManager.Instance.SetCurUserName(curUserName);
            ClosePanel();
        }
        else
        {
            Debug.Log("�п�ܥΤ�");
            BasePanel basePanel =  BaseUIManager.Instance.OpenPanel(UIConst.promptMessagePanel);
            PromptMessagePanel promptMessagePanel = basePanel as PromptMessagePanel;
            promptMessagePanel.SetMessageText("�п�ܥΤ�");
            //promptMessagePanel.txtTitle.text = "�п�ܥΤ�";
        }
    }

    private void OnBtnCancel()
    {
        if (BaseManager.Instance.curUserName == "")
        {
            Debug.Log("�п�ܥΤ�");
            BasePanel basePanel = BaseUIManager.Instance.OpenPanel(UIConst.promptMessagePanel);
            PromptMessagePanel promptMessagePanel = basePanel as PromptMessagePanel;
            promptMessagePanel.SetMessageText("�п�ܥΤ�");
            //promptMessagePanel.txtTitle.text = "�п�ܥΤ�";
            return;
        }

        ClosePanel();
    }

    private void OnBtnDelete()
    {
        //�S��ܥΤ�N���R��
        if (curUserName == "")
        {
            Debug.Log("�п�ܭn�R�����Τ�");
            BasePanel basePanel = BaseUIManager.Instance.OpenPanel(UIConst.promptMessagePanel);
            PromptMessagePanel promptMessagePanel = basePanel as PromptMessagePanel;
            promptMessagePanel.SetMessageText("�п�ܭn�R�����Τ�");
            //promptMessagePanel.txtTitle.text = "�п�ܭn�R�����Τ�";
            return;
        }

        //�R���Τ�
        bool isSuccess = LocalConfig.ClearUserData(curUserName);
        if (isSuccess)
        {
            curUserName = "";
            BaseManager.Instance.SetCurUserName(curUserName);   
            Debug.Log("�R���Τᦨ�\");
            btnCancel.gameObject.SetActive(false);
            //���s��z�Τ�C��
            //RefreshMainPanel(); ���O�H�q���ڡA�ڦA�q�s��z
        }
        else
        {
            Debug.Log("�R���Τᥢ��");
        }
    }

    private void OnBtnReName()
    {
        if (curUserName == "")
        {
            Debug.Log("�п�ܭn�ܧ�W�l���Τ�");
            BasePanel basePanel = BaseUIManager.Instance.OpenPanel(UIConst.promptMessagePanel);
            PromptMessagePanel promptMessagePanel = basePanel as PromptMessagePanel;
            promptMessagePanel.SetMessageText("�п�ܭn�ܧ�W�l���Τ�");
            //promptMessagePanel.txtTitle.text = "�п�ܭn�ܧ�W�l���Τ�";
            return;
        }
        GameObject modifyUserPanel = BaseUIManager.Instance.OpenPanel(UIConst.reNameUserPanel).gameObject;
        modifyUserPanel.GetComponent<ModifyUserPanel>().oldUserData = LocalConfig.LoadUserData(curUserName);
    }

    /// <summary>
    /// �q�s��z�Ҧ�UserNameItem���襤���A
    /// </summary>
    public void ReflashSelectState()
    {
        foreach (var item in menuNameItems.Values)
        {
            item.RefreshSelect();
        }
    }

    private void RefreshMainPanel()
    {
        //����쥻�Ҧ���userNameItem�����屼
        //Debug.Log($"��ܪ��� : {scroll.content}");
        foreach (Transform child in scroll.content)
        {
            //Debug.Log(child);
            Destroy(child.gameObject);
        }

        //�ھڦ��h�ָ�Ʋ��ͥX�h��userNameItem
        menuNameItems = new Dictionary<string, UserNameItem>();
        foreach (UserData userData in LocalConfig.LoadAllUserData())
        {
            Debug.Log($"userData.name : {userData.name}");
            Transform prefab = Instantiate(userNamePrefab).transform;
            prefab.SetParent(scroll.content, false);
            prefab.localPosition = Vector3.zero;
            prefab.localScale = Vector3.one;
            prefab.localRotation = Quaternion.identity;
            prefab.GetComponent<UserNameItem>().InitItem(userData, this);
            menuNameItems.Add(userData.name, prefab.GetComponent<UserNameItem>());
        }

        //�إ߷s���a
        Transform newPrefab = Instantiate(userNamePrefab).transform;
        newPrefab.SetParent(scroll.content, false);
        newPrefab.localPosition = Vector3.zero;
        newPrefab.localScale = Vector3.one;
        newPrefab.localRotation = Quaternion.identity;
        newPrefab.GetComponent<UserNameItem>().InitNewUserItem();
    }

    void OnEventNewUserCreate(UserData userData)
    {
        RefreshMainPanel();
        curUserName = userData.name;
    }

    void OnEventUserDelete(UserData userData)
    {
        RefreshMainPanel();
    }

    void OnEventCurUserChange(string curName)
    {
        //��s��e�Τ�W��
        curUserName = curName;
    }
    public override void ClosePanel()
    {
        base.ClosePanel();
        //���������H��A���ݭn���Ʊ��A�ҥH�����q�\
        EventCenter.Instance.RemoveEventListener<UserData>(EventType.eventNewUserCreate, OnEventNewUserCreate);
        EventCenter.Instance.RemoveEventListener<UserData>(EventType.eventUserDelete, OnEventUserDelete);
        EventCenter.Instance.RemoveEventListener<string>(EventType.eventCurUserChange, OnEventCurUserChange);
    }

    private void OnEventModifyUser(UserData userData)
    {
        RefreshMainPanel();
        curUserName = userData.name;

    }    
}
