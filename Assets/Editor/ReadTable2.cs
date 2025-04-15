using OfficeOpenXml;
using System;
using System.IO;
using System.Reflection; // �Ϯg�R�W�Ŷ�
using UnityEditor;
using UnityEngine;

// �ϥ� [InitializeOnLoad] �S�ʡA�������b�s�边�Ұʮɥ[��
[InitializeOnLoad]
public class CreateTable2
{
    public static bool needRead = false;

    // �R�A�غc�禡�A�b���Q�[���ɦ۰ʰ���
    static CreateTable2()
    {
        // �p�G needRead �� false�A�N������^�A���i��H�U�ާ@
        if (!needRead)
        {
            return;
        }

        //excel�ɮ׸�Ƹ��|(excel�ɮצb����)
        string path = Application.dataPath + "/Editor/���d�޲z.xlsx";
        //�]�wScriptableObject�W��
        string assetName = "Level";
        Debug.Log("path, " + path);
        // �ϥΫ��w���|�Ы� FileInfo ����A�ΨӾާ@���
        FileInfo fileInfo = new FileInfo(path);
        //�Ы�ScriptableObject
        LevelData levelData = (LevelData)ScriptableObject.CreateInstance(typeof(LevelData));
        // �ϥ� EPPlus �}�� Excel ���AŪ��Excel���
        using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
        {
            // ����W�� "�L��" ���u�@��
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Level2"];
            // �M���u�@�����Ħ�d��]���L�e���A�]�����i��O���D�^
            for (int i = worksheet.Dimension.Start.Row + 2; i <= worksheet.Dimension.End.Row; i++)
            {
                // �Ыؤ@�ӷs�� LevelItem ��ҡA�ΨӦs��C��ƾ�
                LevelItem levelItem = new LevelItem();
                // ��� LevelItem ������T�A�Ω�Ϯg
                Type type = typeof(LevelItem);
                // �M���Ӧ檺�C�C�ƾ�
                for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                {
                    // �ھڼ��D��]�ĤG��^������W��
                    FieldInfo variable = type.GetField(worksheet.GetValue(2, j).ToString());
                    // ����椸��ƾڡA���ഫ���r��
                    string tableValue = worksheet.GetValue(i, j).ToString();
                    // �ϥΤϮg�N�ƾڽ�ȵ� LevelItem ���������
                    variable.SetValue(levelItem, Convert.ChangeType(tableValue, variable.FieldType));
                }
                //��e�檺�ƾڪ��ȧ����A�[�J��levelData��
                levelData.levelDataList.Add(levelItem);
            }
        }

        //��ЫإX�Ӫ�ScriptableObject�O�s����w���| (�N LevelData �s�x�� Unity �겣�].asset ���))
        AssetDatabase.CreateAsset(levelData, "Assets/Resources/Data/Level2/" + assetName + ".asset");
        //�O�s�겣�ܧ�
        AssetDatabase.SaveAssets();
        // ��s�겣�ƾڮw�A���ܧ�Y�ɤϬM�b�s�边��(�л\�쥻���)
        AssetDatabase.Refresh();

    }

}

#region �ɥR����
/*
1. [InitializeOnLoad] :
   �o���ݩ��� Unity �s�边�b�Ұʮɦ۰ʥ[�������A�ð�����R�A�c�y�禡

2. needRead �P�_ : 
   �o���ܶq�Ω󱱨�O�_�ݭnŪ�� Excel ���A�p�G�� false�A�h������^�A���i��Ū���ާ@

3. path �M assetName : 
   path �O Excel ��檺���|(��Ʀb����)�AassetName �O�]�w ScriptableObject ���W��
 
4. �Ϯg (Reflection) : 
   �ϥΤϮgŪ�����D�檺���W�١A�M��N�������ƾڽ�ȵ��C�� LevelItem �����C 

5. ScriptableObject �겣�O�s : 
   �N�ƾڦs�x�� Unity ���겣�ɮסA��K�b�C�������ƨϥΩζi��t�m�C

���}�����ت��OŪ�� Excel ��椤���ƾڡA�ñN��O�s�� ScriptableObject�A��K�b�C�����ϥΡC
 */
#endregion
