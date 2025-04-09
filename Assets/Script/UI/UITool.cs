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

    public Button GetButton(GameObject mySelf, string findBtnName)
    {
        Button btn = null;
        Transform[] transforms = mySelf.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in transforms)
        {
            if (t.name == findBtnName)
            {
                btn = t.GetComponent<Button>();
                break;
            }
        }
        return btn;
    }
}
