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
    /// ���B���J����
    /// </summary>
    /// <param name="loadSceneName"></param>
    /// <returns></returns>
    public static AsyncOperation LoadSceneAsync(string loadSceneName)
    {
        return SceneManager.LoadSceneAsync(loadSceneName);
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
                SoundManager.instance.PlayBGM(Globals.BGM2);
                break;
            case "Menu":
                // �b�o�̳B�z Menu ���������J�����޿�
                BaseUIManager.Instance.OpenPanel(UIConst.mainMenuPanel);
                SoundManager.instance.PlayBGM(Globals.BGM5);
                break;
            case "Game":
                // �b�o�̳B�z Game ���������J�����޿�
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
