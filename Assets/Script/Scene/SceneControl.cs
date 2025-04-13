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


    // �B�z�������J�������޿�
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"���� {scene.name} �w�g���J�����I");
        // �z�i�H�b�o�̰���@�Ǫ�l�ƾާ@
        // �Ҧp�G��l�Ƴ�����������B�]�m��v����m����
        switch (scene.name)
        {
            case "Menu":
                // �b�o�̳B�z Menu ���������J�����޿�
                BaseUIManager.Instance.OpenPanel(UIConst.mainMenuPanel);
                break;
            case "Game":
                // �b�o�̳B�z Game ���������J�����޿�
                break;
            default:
                break;
        }
    }

}
