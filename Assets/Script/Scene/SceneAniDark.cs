using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAniDark : MonoBehaviour
{
    AsyncOperation operation;

    // Start is called before the first frame update
    void Start()
    {
        operation = SceneControl.operation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnEventDarkAni()
    {
        FadeManager.instance.darkBgObj.SetActive(true);
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            BaseUIManager.Instance.ClosePanel(UIConst.mainMenuPanel);
        }
                
        if (operation != null)
            operation.allowSceneActivation = true;
        else
            SceneControl.LoadScene("Menu");
    }
}
