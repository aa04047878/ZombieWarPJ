using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class GMCommand
{
    [MenuItem("GMCommand/打開主選單介面")]
    public static void OpenMainMenu()
    {
        BaseUIManager.Instance.OpenPanel(UIConst.mainMenuPanel);
    }

    [MenuItem("GMCommand/儲存本地端資料(測試用)")]
    public static void SaveLocalConfig()
    {
        for (int i = 0; i < 5; i++)
        {
            UserData userData = new UserData();
            userData.name = "xiaoqi" + i.ToString();
            userData.level = i;
            LocalConfig.SaveUserData(userData);
        }
        Debug.Log("儲存完成");
    }

   [MenuItem("GMCommand/讀取本地端資料(測試用)")]
    public static void LoadLocalConfig()
    {
        for (int i = 0; i < 5; i++)
        {
            string name = "xiaoqi" + i.ToString();
            UserData userData = LocalConfig.LoadUserData(name);
            Debug.Log("讀取完成: " + userData.name + " " + userData.level);
        }
    }

    [MenuItem("GMCommand/清除客戶端資料(測試用)")]
    public static void ClearClientData()
    {
        //先找出檔案的路徑
        string path = Application.persistentDataPath + "/clientData.json";
        //如果路徑裡有檔案就刪除
        if (File.Exists(path))
        {
            //從此路徑刪除檔案
            File.Delete(path);
            //從緩存中刪除檔案
            LocalConfig.clientData = null;
            Debug.Log("清除客戶端資料成功");
            return; //刪除成功
        }
        else
        {
            Debug.Log("清除客戶端資料失敗");
            return; //刪除失敗
        }
    }
}
