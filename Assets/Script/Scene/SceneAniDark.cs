using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        BaseUIManager.Instance.ClosePanel(UIConst.mainMenuPanel);
        operation.allowSceneActivation = true;
    }
}
