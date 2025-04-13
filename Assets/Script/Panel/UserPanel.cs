using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserPanel : BasePanel
{
    public Button btnOk;
    public Button btnCancel;
    public Button btnDelete;
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
    }

    protected override void Start()
    {
        base.Start();
        btnOk = UITool.GetUIComponent<Button>(this.gameObject, "BtnOk");
        btnCancel = UITool.GetUIComponent<Button>(this.gameObject, "BtnCancel");
        btnDelete = UITool.GetUIComponent<Button>(this.gameObject, "BtnDelete");
        btnOk.onClick.AddListener(() => OnBtnOk());
        btnCancel.onClick.AddListener(() => OnBtnCancel());
        btnDelete.onClick.AddListener(() => OnBtnDelete());
        //testData = new List<UserData>();
        //testData.Add(new UserData("�p��1", 1));
        //testData.Add(new UserData("�p��2", 2));
        RefreshMainPanel();
        curUserName = BaseManager.Instance.curUserName;
        //�q�\�ƥ�
        EventCenter.Instance.AddEventListener<UserData>(EventType.eventNewUserCreate, OnEventNewUserCreate);
        EventCenter.Instance.AddEventListener<UserData>(EventType.eventUserDelete, OnEventUserDelete);
        EventCenter.Instance.AddEventListener<string>(EventType.eventCurUserChange, OnEventCurUserChange);
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
        }
    }

    private void OnBtnCancel()
    {
        ClosePanel();
    }

    private void OnBtnDelete()
    {
        //�S��ܥΤ�N���R��
        if (curUserName == "")
        {
            Debug.Log("�п�ܭn�R�����Τ�");
            return;
        }

        //�R���Τ�
        bool isSuccess = LocalConfig.ClearUserData(curUserName);
        if (isSuccess && curUserName == BaseManager.Instance.curUserName)
        {
            List<UserData> userDatas = LocalConfig.LoadAllUserData();
            if (userDatas.Count > 0)
            {
                //�p�G�٦���L�Τ�A�h��ܲĤ@�ӥΤ�
                curUserName = userDatas[0].name;
                BaseManager.Instance.SetCurUserName(curUserName);
            }
            else
            {
                //�p�G�S����L�Τ�A�h�M�ŷ�e�Τ�W��
                curUserName = "";
                BaseManager.Instance.SetCurUserName(curUserName);
            }

            Debug.Log("�R���Τᦨ�\");
            //���s��z�Τ�C��
            //RefreshMainPanel(); ���O�H�q���ڡA�ڦA�q�s��z
        }
        else
        {
            Debug.Log("�R���Τᥢ��");
        }
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
        foreach (Transform child in scroll.content)
        {
            Debug.Log(child);
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
}
