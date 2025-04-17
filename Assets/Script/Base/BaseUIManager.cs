using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUIManager
{
    private static BaseUIManager _instance;
    private Transform _uiRoot;
    /// <summary>
    /// 路徑配置字典(物體名子, 路徑)
    /// </summary>
    private Dictionary<string, string> pathDict;
    /// <summary>
    /// 預製件緩存字典(物體名子, 預置物)
    /// </summary>
    private Dictionary<string, GameObject> prefabDict;

    /// <summary>
    /// 紀錄已打開的介面字典
    /// </summary>
    public Dictionary<string, BasePanel> panelDict;

    //單利模式
    public static BaseUIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BaseUIManager();
            }
            return _instance;
        }
    }

    /// <summary>
    /// Panel掛載的地方(這裡指最頂層的Canvas)
    /// </summary>
    public Transform UIRoot
    {
        get
        {
            if (_uiRoot == null)
            {
                _uiRoot = GameObject.Find("Canvas").transform;
            }
            return _uiRoot;
        }
    }

    private BaseUIManager()
    {
        InitDicts();
    }


    /// <summary>
    /// 初始化
    /// </summary>
    private void InitDicts()
    {
        prefabDict = new Dictionary<string, GameObject>();
        panelDict = new Dictionary<string, BasePanel>();

        pathDict = new Dictionary<string, string>()
        {
            {UIConst.userPanel, "Menu/UserPanel"},
            {UIConst.mainMenuPanel, "Menu/MainMenuPanel"},
            {UIConst.newUserPanel, "Menu/NewUserPanel"},
            {UIConst.victoryPanel, "Menu/VictoryPanel"},
            {UIConst.failPanel, "Menu/FailPanel"},
            {UIConst.reNameUserPanel, "Menu/ReNameUserPanel"},
            {UIConst.promptMessagePanel, "Menu/PromptMessagePanel"},
            {UIConst.settingPanel, "Menu/SettingPanel"},
            {UIConst.quitMessagePanel, "Menu/QuitMessagePanel"},
            
        };
    }

    /// <summary>
    /// 開啟介面
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public BasePanel OpenPanel(string name)
    {
        BasePanel panel = null;
        // 檢查是否已打開
        if (panelDict.TryGetValue(name, out panel))
        {
            Debug.LogError("介面已打開: " + name);
            return null;
        }

        // 檢查路徑是否配置
        string path = "";
        if (!pathDict.TryGetValue(name, out path))
        {
            Debug.LogError("介面名稱錯誤，或未配置路徑: " + name);
            return null;
        }

        // 使用緩存預製件()
        GameObject panelPrefab = null;
        //預置物件字典若沒資料，需先加載
        if (!prefabDict.TryGetValue(name, out panelPrefab)) //有key就會把value放入上面
        {
            string realPath = "Prefab/Panel/" + path;
            panelPrefab = Resources.Load<GameObject>(realPath) as GameObject;
            prefabDict.Add(name, panelPrefab);
        }

        // 打開介面
        GameObject panelObject = GameObject.Instantiate(panelPrefab, UIRoot, false);
        Debug.Log("打開介面: " + name);
        panel = panelObject.GetComponent<BasePanel>();
        panelDict.Add(name, panel);
        panel.OpenPanel(name);
        return panel;
    }

    /// <summary>
    /// 關閉介面
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool ClosePanel(string name)
    {
        BasePanel panel = null;
        //如果介面沒打開，就返回
        if (!panelDict.TryGetValue(name, out panel))
        {
            Debug.Log("介面未打開，不執行任何操作: " + name);
            return false;
        }

        panel.ClosePanel();
        // panelDict.Remove(name);
        return true;
    }
}

public class UIConst
{
    /*
    把字串寫入變數裡面，便於呼叫，且只要修改一次
    */
    // menu panels
    public const string userPanel = "UserPanel";
    public const string mainMenuPanel = "MainMenuPanel";
    public const string newUserPanel = "NewUserPanel";
    public const string victoryPanel = "VictoryPanel";
    public const string failPanel = "FailPanel";
    public const string reNameUserPanel = "ReNameUserPanel";
    /// <summary>
    /// 提示訊息介面
    /// </summary>
    public const string promptMessagePanel = "PromptMessagePanel";
    public const string settingPanel = "SettingPanel";
    /// <summary>
    /// 離開訊息介面
    /// </summary>
    public const string quitMessagePanel = "QuitMessagePanel";
    
}

