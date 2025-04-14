using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryPanel : MonoBehaviour
{
    private Button btnGoToMenu;
    // Start is called before the first frame update
    void Start()
    {
        btnGoToMenu = UITool.GetUIComponent<Button>(gameObject, "BtnGoToMenu");
        btnGoToMenu.onClick.AddListener(() => OnBtnGoToMenu());
    }

    private void OnBtnGoToMenu()
    {
        SceneControl.LoadScene("Menu");
    }
}
