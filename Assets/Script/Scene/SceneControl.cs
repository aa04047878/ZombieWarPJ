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

    public static AsyncOperation operation;
    private void Awake()
    {
        //FadeManager.instance.SetParent("Canvas");
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
        operation = SceneManager.LoadSceneAsync(loadSceneName);
        //載入場景完成後要不要馬上切換
        operation.allowSceneActivation = false;
        return operation;
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
                FadeManager.instance.SetParent("Canvas");
                SoundManager.instance.PlayBGM(Globals.BGM2);
                break;
            case "Menu":
                // 在這裡處理 Menu 場景的載入完成邏輯
                TimeManager.ResumeGame();
                BaseUIManager.Instance.OpenPanel(UIConst.mainMenuPanel);
                FadeManager.instance.SetParent("Canvas");
                FadeManager.instance.FadeOut();
                SoundManager.instance.PlayBGM(Globals.BGM5);
                break;
            case "Game":
                // 在這裡處理 Game 場景的載入完成邏輯
                FadeManager.instance.SetParent("Canvas");
                FadeManager.instance.FadeOut();
                GameManager.instance.curProgressZombieList = new List<GameObject>();
                GameManager.instance.curLevelId = 1;
                GameManager.instance.curProgressId = 1;
                GameManager.instance.sunNum = 0;
                GameManager.instance.curKillZombieCount = 0;
                GameManager.instance.killCountResult = new Queue<int>();
                GameManager.instance.dieZombieIdList = new List<int>();
                GameManager.instance.bornParent = GameObject.Find("Borns");
                GameManager.instance.victoryPanelObj = Instantiate(GameManager.instance.victoryPanelPre);
                GameManager.instance.victoryPanelObj.SetActive(false);
                GameManager.instance.failPanelObj = Instantiate(GameManager.instance.failPanelPre);
                GameManager.instance.failPanelObj.SetActive(false);
                StartCoroutine(GameManager.instance.CoLoadTable());
                break;
            default:
                break;
        }
    }

}
