using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : BasePanel
{
    public Button btnChangeUserName;

    protected override void Awake()
    {
        base.Awake();
        
    }

    protected override void Start()
    {
        base.Start();
        Debug.Log("¨ú±o«ö¶s");
        btnChangeUserName = UITool.Instance.GetButton(this.gameObject ,"BtnChangeUserName");
        btnChangeUserName.onClick.AddListener(() => OnBtnChangeUser());
    }

    private void OnBtnChangeUser()
    {
        BaseUIManager.Instance.OpenPanel(UIConst.userPanel);
    }
}
