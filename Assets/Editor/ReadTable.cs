using OfficeOpenXml;
using System;
using System.IO;
using System.Reflection; // 反射命名空間
using UnityEditor;
using UnityEngine;

// 使用 [InitializeOnLoad] 特性，讓此類在編輯器啟動時加載
[InitializeOnLoad]
public class Startup
{
    public static bool needRead = false;

    // 靜態建構函式，在類被加載時自動執行
    static Startup()
    { 
        // 如果 needRead 為 false，就直接返回，不進行以下操作
        if (!needRead)
        {
            return;
        }

        //excel檔案資料路徑(excel檔案在哪裡)
        string path = Application.dataPath + "/Editor/關卡管理.xlsx";
        //設定ScriptableObject名稱
        string assetName = "Level";
        Debug.Log("path, " + path);
        // 使用指定路徑創建 FileInfo 物件，用來操作文件
        FileInfo fileInfo = new FileInfo(path);
        //創建ScriptableObject
        LevelData levelData = (LevelData)ScriptableObject.CreateInstance(typeof(LevelData));
        // 使用 EPPlus 開啟 Excel 文件，讀取Excel資料
        using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
        {
            // 獲取名為 "Level1" 的工作表
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Level1"];
            // 遍歷工作表的有效行範圍（跳過前兩行，因為它可能是標題）
            for (int i = worksheet.Dimension.Start.Row + 2; i <= worksheet.Dimension.End.Row; i++)
            {
                // 創建一個新的 LevelItem 實例，用來存放每行數據
                LevelItem levelItem = new LevelItem();
                // 獲取 LevelItem 類型資訊，用於反射
                Type type = typeof(LevelItem);
                // 遍歷該行的每列數據
                for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                {
                    // 根據標題行（第二行）獲取欄位名稱
                    FieldInfo variable = type.GetField(worksheet.GetValue(2, j).ToString());
                    // 獲取單元格數據，並轉換為字串
                    string tableValue = worksheet.GetValue(i, j).ToString();
                    // 使用反射將數據賦值給 LevelItem 的對應欄位
                    variable.SetValue(levelItem, Convert.ChangeType(tableValue, variable.FieldType));
                }
                //當前行的數據附值完畢，加入到levelData中
                levelData.levelDataList.Add(levelItem);
            }
        }

        //把創建出來的ScriptableObject保存到指定路徑 (將 LevelData 存儲為 Unity 資產（.asset 文件))
        AssetDatabase.CreateAsset(levelData, "Assets/Resources/Data/Level1/" + assetName + ".asset");
        //保存資產變更
        AssetDatabase.SaveAssets();
        // 刷新資產數據庫，讓變更即時反映在編輯器中(覆蓋原本資料)
        AssetDatabase.Refresh();
        
    }
    
}

#region 補充說明
/*
1. [InitializeOnLoad] :
   這個屬性讓 Unity 編輯器在啟動時自動加載該類，並執行其靜態構造函式

2. needRead 判斷 : 
   這個變量用於控制是否需要讀取 Excel 表格，如果為 false，則直接返回，不進行讀取操作

3. path 和 assetName : 
   path 是 Excel 表格的路徑(資料在哪裡)，assetName 是設定 ScriptableObject 的名稱
 
4. 反射 (Reflection) : 
   使用反射讀取標題行的欄位名稱，然後將對應的數據賦值給每個 LevelItem 的欄位。 

5. ScriptableObject 資產保存 : 
   將數據存儲為 Unity 的資產檔案，方便在遊戲中重複使用或進行配置。

此腳本的目的是讀取 Excel 表格中的數據，並將其保存為 ScriptableObject，方便在遊戲中使用。
 */
#endregion
