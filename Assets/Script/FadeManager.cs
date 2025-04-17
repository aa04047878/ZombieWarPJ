using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    public static FadeManager instance;
    private GameObject sceneAniDarkPre;
    private GameObject sceneAniDarkObj;
    private GameObject sceneAniLightPre;
    private GameObject sceneAniLightObj;
    public Transform parent;
    public GameObject darkBgPre;
    public GameObject darkBgObj;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        
    }

    private void Start()
    {
        darkBgPre = Resources.Load<GameObject>("Prefab/SceneAni/DarkBg");
        sceneAniDarkPre = Resources.Load<GameObject>("Prefab/SceneAni/SceneAniDark");
        sceneAniLightPre = Resources.Load<GameObject>("Prefab/SceneAni/SceneAniLight");
        DarkBgStandby();
    }

    public void SetParent(string name)
    {
        parent = GameObject.Find(name).transform;
    }

    /// <summary>
    /// 變暗
    /// </summary>
    public void FadeIn()
    {
        sceneAniDarkObj = Instantiate(sceneAniDarkPre);
        sceneAniDarkObj.transform.SetParent(parent, false);
        //sceneAniDarkObj.transform.parent = parent;
    }

    /// <summary>
    /// 變亮
    /// </summary>
    public void FadeOut()
    {
        sceneAniLightObj = Instantiate(sceneAniLightPre);
        sceneAniLightObj.transform.SetParent(parent, false);
    }

    /// <summary>
    /// 暗背景準備
    /// </summary>
    public void DarkBgStandby()
    {
        darkBgObj = Instantiate(darkBgPre);
        darkBgObj.transform.SetParent(parent, false);
        darkBgObj.SetActive(false);
    }
}
