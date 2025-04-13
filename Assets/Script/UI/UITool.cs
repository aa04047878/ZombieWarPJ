using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITool : MonoBehaviour
{
    public static UITool Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static T GetUIComponent<T>(GameObject mySelf, string findBtnName) 
    {
        Transform[] transforms = mySelf.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in transforms)
        {
            if (t.name == findBtnName)
            {
                return t.GetComponent<T>();
            }
        }
        return default;
    }
}
