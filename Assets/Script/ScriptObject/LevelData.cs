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
    [Header("���dID(�ثe�ĴX��)")]
    public int levelId;
    [Header("���ݶi��(�ĴX�i)")]
    public int progressId;
    [Header("�ͦ��Ǫ��ɶ�")]
    public int createTime;
    [Header("�L������")]
    public int zombieType;
    [Header("�ͦ��Ǫ���m(�ĴX��)")]
    public int bornPos;
}
