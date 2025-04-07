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
    [Header("植物Id")]
    public int plantId;
    [Header("植物名稱")]
    public string plantName;
    [Header("描述")]
    public string Description;
    [Header("預置物")]
    public GameObject cardPrefab;

    public override string ToString()
    {
        return $"id : {plantId}";
    }
}