using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : ScriptableObject
{
    public List<LevelItem> levelDataList = new List<LevelItem>();
}

[System.Serializable]
public class LevelItem
{
    [Header("ID")]
    public int id;
    [Header("關卡ID(目前第幾關)")]
    public int levelId;
    [Header("所屬進度(第幾波)")]
    public int progressId;
    [Header("生成怪物時間")]
    public int createTime;
    [Header("殭屍類型")]
    public int zombieType;
    [Header("生成怪物位置(第幾行)")]
    public int bornPos;
}
