using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantInfo", menuName = "ScriptObject/PlantInfo", order = 0)]
public class PlantInfo : ScriptableObject
{
    public List<PlantInfoItem> plantInfoList = new List<PlantInfoItem>();
}

[System.Serializable]
public class PlantInfoItem
{
    [Header("�Ӫ�Id")]
    public int plantId;
    [Header("�Ӫ��W��")]
    public string plantName;
    [Header("�y�z")]
    public string Description;
    [Header("�w�m��")]
    public GameObject cardPrefab;

    public override string ToString()
    {
        return $"id : {plantId}";
    }
}