using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager 
{


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

    public static bool IsGamePaused()
    {
        return Time.timeScale == 0;
    }
}
