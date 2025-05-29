using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitMessagePanel : BasePanel
{
    private Button btnOk;
    private Button btnCancel;
    
    protected override void Awake()
    {
        base.Awake();
        btnOk = UITool.GetUIComponent<Button>(gameObject, "BtnOk");
        btnCancel = UITool.GetUIComponent<Button>(gameObject, "BtnCancel");
        
    }

    protected override void Start()
    {
        base.Start();
        btnOk.onClick.AddListener(() => OnBtnOk());
        btnCancel.onClick.AddListener(() => OnBtnCancel());
        
    }

    protected override void Update()
    {
        base.Update();
    }

    private void OnBtnOk()
    {
        Application.Quit();
    }

    private void OnBtnCancel()
    {
        BaseUIManager.Instance.ClosePanel(UIConst.quitMessagePanel);
    }

    
}
