using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserPanel : BasePanel
{
    public Button btnOk;
    public Button btnCancel;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        btnOk = UITool.Instance.GetButton(this.gameObject, "BtnOk");
        btnCancel = UITool.Instance.GetButton(this.gameObject, "BtnCancel");
        btnOk.onClick.AddListener(() => OnBtnOk());
        btnCancel.onClick.AddListener(() => OnBtnCancel());
    }

    private void OnBtnOk()
    {
        ClosePanel();
    }

    private void OnBtnCancel()
    {
        ClosePanel();
    }
}
