using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
            SoundManager.instance.PlayBGM(Globals.LoadingMusic);
            GameManager.instance.canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            GameManager.instance.btnSetting = GameObject.Find("BtnSetting").GetComponent<UnityEngine.UI.Button>();
            //GameManager.instance.btnSetting.onClick.AddListener(() => BaseUIManager.Instance.OpenPanel(UIConst.settingPanel));
            GameManager.instance.btnSetting.onClick.AddListener(delegate () {
                BaseUIManager.Instance.OpenPanel(UIConst.settingPanel);
                GameManager.instance.OpenCanvasSetting();
            });
        }
    }

    private void Update()
    {

    }

    /// <summary>
    /// ���B���J����
    /// </summary>
    /// <param name="loadSceneName"></param>
    /// <returns></returns>
    public static AsyncOperation LoadSceneAsync(string loadSceneName)
    {
        operation = SceneManager.LoadSceneAsync(loadSceneName);
        //���J����������n���n���W����
        operation.allowSceneActivation = false;
        return operation;
    }

    /// <summary>
    /// ���J����
    /// </summary>
    /// <param name="loadSceneName"></param>
    public static void LoadScene(string loadSceneName)
    {
        SceneManager.LoadScene(loadSceneName);
    }


    /// <summary>
    /// �B�z�������J�������޿�
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"���� {scene.name} �w�g���J�����I");
        // �z�i�H�b�o�̰���@�Ǫ�l�ƾާ@
        // �Ҧp�G��l�Ƴ�����������B�]�m��v����m����
        switch (scene.name)
        {
            case "Loading":
                FadeManager.instance.SetParent("Canvas");
                SoundManager.instance.PlayBGM(Globals.LoadingMusic);
                break;
            case "Menu":
                // �b�o�̳B�z Menu ���������J�����޿�
                TimeManager.ResumeGame();
                BaseUIManager.Instance.OpenPanel(UIConst.mainMenuPanel);
                FadeManager.instance.SetParent("Canvas");
                FadeManager.instance.FadeOut();
                SoundManager.instance.PlayBGM(Globals.MainMenuMusic);
                break;
            case "Game":
                // �b�o�̳B�z Game ���������J�����޿�
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
                GameManager.instance.canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
                GameManager.instance.btnSetting = GameObject.Find("BtnSetting").GetComponent<UnityEngine.UI.Button>();
                //GameManager.instance.btnSetting.onClick.AddListener(() => BaseUIManager.Instance.OpenPanel(UIConst.settingPanel));
                GameManager.instance.btnSetting.onClick.AddListener(delegate () {
                    BaseUIManager.Instance.OpenPanel(UIConst.settingPanel);
                    GameManager.instance.OpenCanvasSetting();
                });
                break;
            default:
                break;
        }
    }

   
}
