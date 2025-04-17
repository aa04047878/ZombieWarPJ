using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FailPanel : BasePanel
{
    private Button btnGoToMenu;
    AsyncOperation operation;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        btnGoToMenu = UITool.GetUIComponent<Button>(gameObject, "BtnGoToMenu");
        btnGoToMenu.onClick.AddListener(() => OnBtnGoToMenu());
        operation = SceneControl.operation;
    }

    private void OnBtnGoToMenu()
    {
        operation.allowSceneActivation = true;
    }

    public void OnEventFailAni()
    {
        operation = SceneControl.LoadSceneAsync("Menu");
    }
}
