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
    /// 提示訊息
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
    #region 測試數據
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
        //訂閱事件
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
            //更新當前用戶名稱
            BaseManager.Instance.SetCurUserName(curUserName);
            ClosePanel();
        }
        else
        {
            Debug.Log("請選擇用戶!!!");
            BasePanel basePanel =  BaseUIManager.Instance.OpenPanel(UIConst.promptMessagePanel);
            PromptMessagePanel promptMessagePanel = basePanel as PromptMessagePanel;
            promptMessagePanel.SetMessageText("請選擇用戶!!!");
            promptMessagePanel.SetUpTitleText("選擇使用者");
            //Debug.Log("測試");
            //promptMessagePanel.txtTitle.text = "請選擇用戶";
        }
    }

    private void OnBtnCancel()
    {
        if (BaseManager.Instance.curUserName == "")
        {
            Debug.Log("請選擇用戶!!!");
            BasePanel basePanel = BaseUIManager.Instance.OpenPanel(UIConst.promptMessagePanel);
            PromptMessagePanel promptMessagePanel = basePanel as PromptMessagePanel;
            promptMessagePanel.SetMessageText("請選擇用戶!!!");
            promptMessagePanel.SetUpTitleText("選擇使用者");
            //Debug.Log("測試");
            //promptMessagePanel.txtTitle.text = "請選擇用戶";
            return;
        }

        ClosePanel();
    }

    private void OnBtnDelete()
    {
        //沒選擇用戶就不刪除
        if (curUserName == "")
        {
            Debug.Log("請選擇要刪除的用戶!!!");
            BasePanel basePanel = BaseUIManager.Instance.OpenPanel(UIConst.promptMessagePanel);
            PromptMessagePanel promptMessagePanel = basePanel as PromptMessagePanel;
            promptMessagePanel.SetMessageText("請選擇要刪除的用戶!!!");
            promptMessagePanel.SetUpTitleText("選擇使用者");
            //promptMessagePanel.txtTitle.text = "請選擇要刪除的用戶";
            return;
        }

        //刪除用戶
        bool isSuccess = LocalConfig.ClearUserData(curUserName);
        if (isSuccess)
        {
            curUserName = "";
            BaseManager.Instance.SetCurUserName(curUserName);   
            Debug.Log("刪除用戶成功");
            btnCancel.gameObject.SetActive(false);
            //重新整理用戶列表
            //RefreshMainPanel(); 等別人通知我，我再從新整理
        }
        else
        {
            Debug.Log("刪除用戶失敗");
        }
    }

    private void OnBtnReName()
    {
        if (curUserName == "")
        {
            Debug.Log("請選擇要變更名子的用戶!!!");
            BasePanel basePanel = BaseUIManager.Instance.OpenPanel(UIConst.promptMessagePanel);
            PromptMessagePanel promptMessagePanel = basePanel as PromptMessagePanel;
            promptMessagePanel.SetMessageText("請選擇要變更名子的用戶!!!");
            promptMessagePanel.SetUpTitleText("選擇使用者");
            //promptMessagePanel.txtTitle.text = "請選擇要變更名子的用戶";
            return;
        }
        GameObject modifyUserPanel = BaseUIManager.Instance.OpenPanel(UIConst.reNameUserPanel).gameObject;
        modifyUserPanel.GetComponent<ModifyUserPanel>().oldUserData = LocalConfig.LoadUserData(curUserName);
    }

    /// <summary>
    /// 從新整理所有UserNameItem的選中狀態
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
        //先把原本所有的userNameItem全部砍掉
        //Debug.Log($"顯示長度 : {scroll.content}");
        foreach (Transform child in scroll.content)
        {
            //Debug.Log(child);
            Destroy(child.gameObject);
        }

        //根據有多少資料產生出多少userNameItem
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

        //建立新玩家
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
        //更新當前用戶名稱
        curUserName = curName;
    }
    public override void ClosePanel()
    {
        base.ClosePanel();
        //介面關閉以後，不需要做事情，所以取消訂閱
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
