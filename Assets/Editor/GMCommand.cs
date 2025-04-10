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

    [MenuItem("GMCommand/�x�s���a�ݸ��(���ե�)")]
    public static void SaveLocalConfig()
    {
        for (int i = 0; i < 5; i++)
        {
            UserData userData = new UserData();
            userData.name = "xiaoqi" + i.ToString();
            userData.level = i;
            LocalConfig.SaveUserData(userData);
        }
        Debug.Log("�x�s����");
    }

   [MenuItem("GMCommand/Ū�����a�ݸ��(���ե�)")]
    public static void LoadLocalConfig()
    {
        for (int i = 0; i < 5; i++)
        {
            string name = "xiaoqi" + i.ToString();
            UserData userData = LocalConfig.LoadUserData(name);
            Debug.Log("Ū������: " + userData.name + " " + userData.level);
        }
    }
}
