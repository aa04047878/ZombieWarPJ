using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager 
{


    /// <summary>
    /// ¼È°±¹CÀ¸
    /// </summary>
    public static void PauseGame()
    { 
        Time.timeScale = 0;
    }

    /// <summary>
    /// «ì´_¹CÀ¸
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
