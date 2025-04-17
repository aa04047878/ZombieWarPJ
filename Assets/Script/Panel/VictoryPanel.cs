using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryPanel : MonoBehaviour
{
    private Button btnGoToMenu;
    AsyncOperation operation;
    // Start is called before the first frame update
    void Start()
    {
        btnGoToMenu = UITool.GetUIComponent<Button>(gameObject, "BtnGoToMenu");
        btnGoToMenu.onClick.AddListener(() => OnBtnGoToMenu());
        operation = SceneControl.operation;
    }

    private void OnBtnGoToMenu()
    {
        operation.allowSceneActivation = true;
    }

    public void OnEventVictoryAni()
    {
        operation = SceneControl.LoadSceneAsync("Menu");
    }
}
