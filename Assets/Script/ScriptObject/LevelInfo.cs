using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelInfo", menuName = "ScriptObject/LevelInfo" ,order = 0)]
public class LevelInfo : ScriptableObject
{
    public List<LevelInfoItem> levelInfoList = new List<LevelInfoItem>();
}

[System.Serializable]
public class LevelInfoItem
{
    [Header("關卡Id")]
    public int levelId;
    [Header("關卡名稱")]
    public string levelName;
    [Header("進度條百分比")]
    public float[] progressPrecent;

    public override string ToString()
    {
        return $"id : {levelId}";
    }
}
