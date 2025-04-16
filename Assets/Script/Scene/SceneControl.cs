using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    private static SceneControl instance;
    public static SceneControl Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SceneControl>();
            }
            return instance;
        }
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        //EventCenter.Instance.AddEventListener
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (SceneManager.GetActiveScene().name == "Loading")
        {
            SoundManager.instance.PlayBGM(Globals.BGM2);
        
        }
    }

    private void Update()
    {

    }

    /// <summary>
    /// 異步載入場景
    /// </summary>
    /// <param name="loadSceneName"></param>
    /// <returns></returns>
    public static AsyncOperation LoadSceneAsync(string loadSceneName)
    {
        return SceneManager.LoadSceneAsync(loadSceneName);
    }

    /// <summary>
    /// 載入場景
    /// </summary>
    /// <param name="loadSceneName"></param>
    public static void LoadScene(string loadSceneName)
    {
        SceneManager.LoadScene(loadSceneName);
    }


    /// <summary>
    /// 處理場景載入完成的邏輯
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"場景 {scene.name} 已經載入完成！");
        // 您可以在這裡執行一些初始化操作
        // 例如：初始化場景中的物件、設置攝影機位置等等
        switch (scene.name)
        {
            case "Loading":
                SoundManager.instance.PlayBGM(Globals.BGM2);
                break;
            case "Menu":
                // 在這裡處理 Menu 場景的載入完成邏輯
                BaseUIManager.Instance.OpenPanel(UIConst.mainMenuPanel);
                SoundManager.instance.PlayBGM(Globals.BGM5);
                break;
            case "Game":
                // 在這裡處理 Game 場景的載入完成邏輯
                GameManager.instance.curProgressZombieList = new List<GameObject>();
                GameManager.instance.curLevelId = 1;
                GameManager.instance.curProgressId = 1;
                StartCoroutine(GameManager.instance.CoLoadTable());
                break;
            default:
                break;
        }
    }

}
