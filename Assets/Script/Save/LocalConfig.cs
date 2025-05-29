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

    #region 資料緩存區
    /// <summary>
    /// 資料緩存字典(key : 用戶名, value : 用戶數據)
    /// </summary>
    public static Dictionary<string, UserData> userDataDic = new Dictionary<string, UserData>();

    public static ClientData clientData;
    public static AudioData audioData;
    #endregion


    #region UserData部分
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
        ////如果名子為空就返回null
        //if (userName == null)
        //    return null;

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
            Debug.Log("找不到檔案");
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

    /// <summary>
    /// 清除使用者資料
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
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
            EventCenter.Instance.EventTrigger(EventType.eventUserDelete, oldUserData);
            return true; //刪除成功
        }
        else
        {
            Debug.LogError("找不到檔案");
            return false; //刪除失敗
        }
    }

    #endregion

    #region ClientData部分
    public static void SaveClientData(ClientData data)
    {
        clientData = data;
        //把資料轉換成Json格式
        string jsonData = JsonConvert.SerializeObject(clientData);
        //將Json格式的資料寫入文件
        File.WriteAllText(Application.persistentDataPath + "/clientData.json", jsonData);
    }

    public static ClientData LoadClientData()
    {
        //讀取資料之前先判斷資料是否已經存在於緩存中(優化1)，已存在直接從緩存中讀取資料即可
        if (clientData != null)
        {
            return clientData;
        }

        //資料路徑
        string path = Application.persistentDataPath + "/clientData.json";
        //檢查路徑裡是否有檔案
        if (File.Exists(path))
        {
            //從此路徑中讀取所有資料
            string jsonData = File.ReadAllText(path);
            //將Json格式的資料轉換成玩家資料(用戶的內存數據)
            ClientData clientData = JsonConvert.DeserializeObject<ClientData>(jsonData);
            return clientData;
        }
        else
        {
            //沒資料就新增一個
            ClientData clientData = new ClientData("");
            string jsonData = JsonConvert.SerializeObject(clientData);
            File.WriteAllText(Application.persistentDataPath + "/clientData.json", jsonData);
            return clientData;
        }
    }

    #endregion

    #region Audiodata部分
    public static void SaveAudioData(AudioData data)
    {
        audioData = data;
        //把資料轉換成Json格式
        string jsonData = JsonConvert.SerializeObject(audioData);
        //將Json格式的資料寫入文件
        File.WriteAllText(Application.persistentDataPath + "/audioData.json", jsonData);
    }

    public static AudioData LoadAudioData()
    {
        //讀取資料之前先判斷資料是否已經存在於緩存中(優化1)，已存在直接從緩存中讀取資料即可
        if (audioData != null)
        {
            return audioData;
        }

        //資料路徑
        string path = Application.persistentDataPath + "/audioData.json";
        //檢查路徑裡是否有檔案
        if (File.Exists(path))
        {
            //從此路徑中讀取所有資料
            string jsonData = File.ReadAllText(path);
            //將Json格式的資料轉換成玩家資料(用戶的內存數據)
            AudioData audioData = JsonConvert.DeserializeObject<AudioData>(jsonData);
            return audioData;
        }
        else
        {
            //沒資料就新增一個
            AudioData audioData = new AudioData();
            //先存檔於本地端
            string jsonData = JsonConvert.SerializeObject(audioData);
            File.WriteAllText(Application.persistentDataPath + "/audioData.json", jsonData);
            return audioData;
        }
    }
    #endregion
}
