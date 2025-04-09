using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GMCommand 
{
    [MenuItem("GMCommand/打開主選單介面")]
    public static void OpenMainMenu()
    {
        BaseUIManager.Instance.OpenPanel(UIConst.mainMenuPanel);
    }
}
