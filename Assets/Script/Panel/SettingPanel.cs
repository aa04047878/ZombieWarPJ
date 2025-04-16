using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    public Scrollbar bgmScrollbar;
    private Button btnOk;
    protected override void Awake()
    {
        base.Awake();
        //UITool.GetUIComponent<Scrollbar>(gameObject, "BGMScrollbar");
        btnOk = UITool.GetUIComponent<Button>(gameObject, "BtnOk");
    }

    protected override void Start()
    {
        base.Start();
        bgmScrollbar.value = SoundManager.instance.GetVolume();     
        btnOk.onClick.AddListener(() => OnBtnOk());
    }

    protected override void Update()
    {
        base.Update();
        SoundManager.instance.SetVolume(bgmScrollbar.value);
    }

    private void OnBtnOk()
    {
        BaseUIManager.Instance.ClosePanel(UIConst.settingPanel);
    }
}
