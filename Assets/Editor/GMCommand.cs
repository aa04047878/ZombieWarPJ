using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GMCommand 
{
    [MenuItem("GMCommand/���}�D��椶��")]
    public static void OpenMainMenu()
    {
        BaseUIManager.Instance.OpenPanel(UIConst.mainMenuPanel);
    }
}
