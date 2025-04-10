using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserPanel : BasePanel
{
    public Button btnOk;
    public Button btnCancel;
    public Button btnDelete;
    private string curUserName;
    public string curName
    {
        get {  return curUserName; }
        set
        {
            curUserName = value;
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
    }

    protected override void Start()
    {
        base.Start();
        btnOk = UITool.Instance.GetButton(this.gameObject, "BtnOk");
        btnCancel = UITool.Instance.GetButton(this.gameObject, "BtnCancel");
        btnDelete = UITool.Instance.GetButton(this.gameObject, "BtnDelete");
        btnOk.onClick.AddListener(() => OnBtnOk());
        btnCancel.onClick.AddListener(() => OnBtnCancel());
        btnDelete.onClick.AddListener(() => OnBtnDelete());
        //testData = new List<UserData>();
        //testData.Add(new UserData("小棋1", 1));
        //testData.Add(new UserData("小棋2", 2));
        RefreshMainPanel();

    }

    private void OnBtnOk()
    {
        ClosePanel();
    }

    private void OnBtnCancel()
    {
        ClosePanel();
    }

    private void OnBtnDelete()
    {
        //沒選擇用戶就不刪除
        if (curName == "")
        {
            Debug.Log("請選擇要刪除的用戶");
            return;
        }

        //刪除用戶
        bool isSuccess = LocalConfig.ClearUserData(curName);
        if (isSuccess)
        {
            Debug.Log("刪除用戶成功");
            //重新整理用戶列表
            RefreshMainPanel();
        }
        else
        {
            Debug.Log("刪除用戶失敗");
        }
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
        foreach (Transform child in scroll.content)
        {
            Debug.Log(child);
            Destroy(child.gameObject);
        }

        //根據有多少資料產生出多少userNameItem
        menuNameItems = new Dictionary<string, UserNameItem>();
        foreach (UserData userData in LocalConfig.LoadAllUserData())
        {
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
}
