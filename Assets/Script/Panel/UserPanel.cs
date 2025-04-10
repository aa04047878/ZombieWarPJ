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
        btnOk = UITool.Instance.GetButton(this.gameObject, "BtnOk");
        btnCancel = UITool.Instance.GetButton(this.gameObject, "BtnCancel");
        btnDelete = UITool.Instance.GetButton(this.gameObject, "BtnDelete");
        btnOk.onClick.AddListener(() => OnBtnOk());
        btnCancel.onClick.AddListener(() => OnBtnCancel());
        btnDelete.onClick.AddListener(() => OnBtnDelete());
        //testData = new List<UserData>();
        //testData.Add(new UserData("�p��1", 1));
        //testData.Add(new UserData("�p��2", 2));
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
        //�S��ܥΤ�N���R��
        if (curName == "")
        {
            Debug.Log("�п�ܭn�R�����Τ�");
            return;
        }

        //�R���Τ�
        bool isSuccess = LocalConfig.ClearUserData(curName);
        if (isSuccess)
        {
            Debug.Log("�R���Τᦨ�\");
            //���s��z�Τ�C��
            RefreshMainPanel();
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
}
