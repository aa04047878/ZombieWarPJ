using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PromptMessagePanel : BasePanel
{
    private Button btnOk;
    public TMP_Text txtTitle;
    public TMP_Text txtUpTitle;
    protected override void Awake()
    {
        base.Awake();
        btnOk = UITool.GetUIComponent<Button>(this.gameObject, "BtnOk");
        txtTitle = UITool.GetUIComponent<TMP_Text>(this.gameObject, "Title");
        txtUpTitle = UITool.GetUIComponent<TMP_Text>(this.gameObject, "UpTitle");
    }

    protected override void Start()
    {
        base.Start();
        btnOk.onClick.AddListener(() => OnBtnOk());
    }

    private void OnBtnOk()
    {
        BaseUIManager.Instance.ClosePanel(UIConst.promptMessagePanel);
    }

    public void SetMessageText(string newMessage)
    {
        txtTitle.text = newMessage;
        Debug.Log(newMessage);
    }

    public void SetUpTitleText(string newUpTitle)
    {
        txtUpTitle.text = newUpTitle;
        Debug.Log(txtUpTitle);
    }
}
