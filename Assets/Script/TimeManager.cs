using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �Ȱ��C��
    /// </summary>
    public static void PauseGame()
    {
        
        Time.timeScale = 0;
    }

    /// <summary>
    /// ��_�C��
    /// </summary>
    public static void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
