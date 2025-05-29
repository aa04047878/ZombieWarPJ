using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    public Scrollbar bgmScrollbar;
    public Slider bgmSlider;
    private Button btnOk;
    private Button btnback;
    private Button btnQuit;
    protected override void Awake()
    {
        base.Awake();
        //UITool.GetUIComponent<Scrollbar>(gameObject, "BGMScrollbar");
        btnOk = UITool.GetUIComponent<Button>(gameObject, "BtnOk");
        btnback = UITool.GetUIComponent<Button>(gameObject, "BtnBack");
        btnQuit = UITool.GetUIComponent<Button>(gameObject, "BtnQuit");
    }

    protected override void Start()
    {
        base.Start();
        //bgmScrollbar.value = SoundManager.instance.GetVolume();  
        bgmSlider.value = SoundManager.instance.GetVolume();
        Debug.Log($"bgmSlider.value : {bgmSlider.value}");
        btnOk.onClick.AddListener(() => OnBtnOk());
        btnback.onClick.AddListener(() => OnBtnBack());
        btnQuit.onClick.AddListener(() => OnBtnQuit());
    }

    protected override void Update()
    {
        base.Update();
        SoundManager.instance.SetVolume(bgmSlider.value);
    }

    private void OnBtnOk()
    {
        //儲存音效設定
        AudioData audioData = LocalConfig.LoadAudioData();
        audioData.bgmVolume = bgmSlider.value;
        LocalConfig.SaveAudioData(audioData);
    }

    private void OnBtnBack()
    {
        BaseUIManager.Instance.ClosePanel(UIConst.settingPanel);
    }

    private void OnBtnQuit()
    {
        BaseUIManager.Instance.OpenPanel(UIConst.quitMessagePanel);
    }
}
