using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using UnityEngine.SocialPlatforms;


public class LocalConfig : MonoBehaviour
{
    #region 說明
    /*
    使用Json儲存資料，但不用JsonUtility，因為JsonUtility只能儲存Inspector可見的資料，字典無法儲存。
    這裡使用Newtonsoft.Json來儲存資料，這個套件可以在Unity的Package Manager中找到。
    */
    #endregion

    /// <summary>
    /// 資料緩存字典(key : 用戶名, value : 用戶數據)
    /// </summary>
    public static Dictionary<string, UserData> userDataDic = new Dictionary<string, UserData>();

    /// <summary>
    /// 儲存資料
    /// </summary>
    public static void SaveUserData(UserData userData)
    {
        //在persistentDataPath底下創建一個文件夾，方便管理
        if (!File.Exists(Application.persistentDataPath + "/users"))
        {
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/users");
        }
        //新增資料緩存數據(優化1)
        userDataDic[userData.name] = userData;
        //將玩家資料轉換成Json格式
        string jsonData = JsonConvert.SerializeObject(userData);
        //將Json格式的資料寫入文件
        File.WriteAllText(Application.persistentDataPath +  string.Format("/users/{0}.json",userData.name) , jsonData);   
    }

    /// <summary>
    /// 讀取資料
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public static UserData LoadUserData(string userName)
    {
        //讀取資料之前先判斷資料是否已經存在於緩存中(優化1)，已存在直接從緩存中讀取資料即可
        if (userDataDic.ContainsKey(userName))
        {
            return userDataDic[userName];
        }

        //讀取Json格式的資料
        string path = Application.persistentDataPath + string.Format("/users/{0}.json", userName);
        if (File.Exists(path))
        {
            //從此路徑中讀取所有資料
            string jsonData = File.ReadAllText(path);
            //將Json格式的資料轉換成玩家資料(用戶的內存數據)
            UserData userData = JsonConvert.DeserializeObject<UserData>(jsonData);
            return userData;
        }
        else
        {
            Debug.LogError("找不到檔案");
            return null;
        }
    }

    /// <summary>
    /// 讀取所有資料
    /// </summary>
    /// <returns></returns>
    public static List<UserData> LoadAllUserData()
    {
        string folderPath = Application.persistentDataPath + "/users";
        DirectoryInfo folder = new DirectoryInfo(folderPath);
        List<UserData> userDataList = new List<UserData>();
        //從路徑取得取得所有檔案
        FileInfo[] allFiles = folder.GetFiles("*.json");
        //先檢查內存
        if (allFiles.Length == userDataDic.Count)
        {
            foreach (UserData userData in userDataDic.Values)
            {
                userDataList.Add(userData);
            }
            return userDataList;
        }

        // 再从硬盘加载
        foreach (FileInfo file in allFiles)
        {
            UserData userData = LoadUserData(file.Name.Split('.')[0]);
            if (userData != null)
            {
                userDataList.Add(userData);
            }
        }
        return userDataList;
    }

    public static bool ClearUserData(string userName)
    {
        //先找出檔案的路徑
        string path = Application.persistentDataPath + string.Format("/users/{0}.json", userName);
        //如果路徑裡有檔案就刪除
        if (File.Exists(path))
        {
            UserData oldUserData = LoadUserData(userName);
            //從此路徑刪除檔案
            File.Delete(path);
            //從緩存中刪除檔案
            if (userDataDic.ContainsKey(userName))
            {
                userDataDic.Remove(userName);
            }
            return true; //刪除成功
        }
        else
        {
            Debug.LogError("找不到檔案");
            return false; //刪除失敗
        }
    }
}
