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
    [Header("���dId")]
    public int levelId;
    [Header("���d�W��")]
    public string levelName;
    [Header("�i�ױ��ʤ���")]
    public float[] progressPrecent;

    public override string ToString()
    {
        return $"id : {levelId}";
    }
}
