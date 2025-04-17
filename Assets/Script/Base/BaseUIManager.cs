using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUIManager
{
    private static BaseUIManager _instance;
    private Transform _uiRoot;
    /// <summary>
    /// ���|�t�m�r��(����W�l, ���|)
    /// </summary>
    private Dictionary<string, string> pathDict;
    /// <summary>
    /// �w�s��w�s�r��(����W�l, �w�m��)
    /// </summary>
    private Dictionary<string, GameObject> prefabDict;

    /// <summary>
    /// �����w���}�������r��
    /// </summary>
    public Dictionary<string, BasePanel> panelDict;

    //��Q�Ҧ�
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
    /// Panel�������a��(�o�̫��̳��h��Canvas)
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
    /// ��l��
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
    /// �}�Ҥ���
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public BasePanel OpenPanel(string name)
    {
        BasePanel panel = null;
        // �ˬd�O�_�w���}
        if (panelDict.TryGetValue(name, out panel))
        {
            Debug.LogError("�����w���}: " + name);
            return null;
        }

        // �ˬd���|�O�_�t�m
        string path = "";
        if (!pathDict.TryGetValue(name, out path))
        {
            Debug.LogError("�����W�ٿ��~�A�Υ��t�m���|: " + name);
            return null;
        }

        // �ϥνw�s�w�s��()
        GameObject panelPrefab = null;
        //�w�m����r��Y�S��ơA�ݥ��[��
        if (!prefabDict.TryGetValue(name, out panelPrefab)) //��key�N�|��value��J�W��
        {
            string realPath = "Prefab/Panel/" + path;
            panelPrefab = Resources.Load<GameObject>(realPath) as GameObject;
            prefabDict.Add(name, panelPrefab);
        }

        // ���}����
        GameObject panelObject = GameObject.Instantiate(panelPrefab, UIRoot, false);
        Debug.Log("���}����: " + name);
        panel = panelObject.GetComponent<BasePanel>();
        panelDict.Add(name, panel);
        panel.OpenPanel(name);
        return panel;
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool ClosePanel(string name)
    {
        BasePanel panel = null;
        //�p�G�����S���}�A�N��^
        if (!panelDict.TryGetValue(name, out panel))
        {
            Debug.Log("���������}�A���������ާ@: " + name);
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
    ��r��g�J�ܼƸ̭��A�K��I�s�A�B�u�n�ק�@��
    */
    // menu panels
    public const string userPanel = "UserPanel";
    public const string mainMenuPanel = "MainMenuPanel";
    public const string newUserPanel = "NewUserPanel";
    public const string victoryPanel = "VictoryPanel";
    public const string failPanel = "FailPanel";
    public const string reNameUserPanel = "ReNameUserPanel";
    /// <summary>
    /// ���ܰT������
    /// </summary>
    public const string promptMessagePanel = "PromptMessagePanel";
    public const string settingPanel = "SettingPanel";
    /// <summary>
    /// ���}�T������
    /// </summary>
    public const string quitMessagePanel = "QuitMessagePanel";
    
}

