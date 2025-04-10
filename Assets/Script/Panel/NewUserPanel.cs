using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewUserPanel : BasePanel
{
    private Button btnOk;
    private Button btnCancel;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        btnOk = UITool.Instance.GetButton(gameObject,"BtnOk");
        btnCancel = UITool.Instance.GetButton(gameObject, "BtnCancel");
        btnOk.onClick.AddListener(() => OnBtnOk());
        btnCancel.onClick.AddListener(() => OnBtnCancel());
    }

    private void OnBtnOk()
    {
        //新增用戶明子
        ClosePanel();
    }

    private void OnBtnCancel()
    {
        //不做任何事
        ClosePanel();
    }
}
