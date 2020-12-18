using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Reflection;
using System;
using System.Runtime.InteropServices;

using GoogleSheetsToUnity;
using GoogleSheetsToUnity.ThirdPary;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Table Data", menuName = "Scriptable Object/Table Data", order = int.MaxValue)]
public class Table : ScriptableObject
{
    public string associatedSheet = "";
    public string associatedWorksheet = "";

    //테이블 이름
    string mName;
    //테이블 데이터 이름들
    public string[] mArrayHeader;
    //데이터 행렬화
    string[,] mArrayData;
    //행 갯수
    int mColumnCount;
    //열 갯수
    int mRowCount;

    public string[,] Data { get { return mArrayData; } }
    public string[] Header { get { return mArrayHeader; } }
    public int ColumnCount { get { return mColumnCount; } }
    public int RowCount { get { return mRowCount; } }

    //-------------------------------------------------------------------------------------------------
    public Table(string name, string[,] arrayData, string[] arrayHeader, int rowCount, int columnCount)
    {
        mName = name;
        mArrayData = arrayData;
        mArrayHeader = arrayHeader;
        mRowCount = rowCount;
        mColumnCount = columnCount;
    }

    public void SetTable(string name, string[,] arrayData, string[] arrayHeader, int rowCount, int columnCount)
    {
        mName = name;
        mArrayData = arrayData;
        mArrayHeader = arrayHeader;
        mRowCount = rowCount;
        mColumnCount = columnCount;
    }

    //-------------------------------------------------------------------------------------------------
    //행렬데이터 => 배열로 변환
    public T[] TableToArray<T>() where T : class, new()
    {
        T[] result = new T[mRowCount];

        System.Type type = typeof(T);
        System.Reflection.FieldInfo[] arrayFieldInfo = type.GetFields();

        for (int i = 0; i < mRowCount; ++i)
        {
            result[i] = new T();
            for (int j = 0; j < arrayFieldInfo.Length; ++j)
            {
                System.Type fieldType = arrayFieldInfo[j].FieldType;
                arrayFieldInfo[j].SetValue(result[i], CSVReader.TypeConverter.ConvertType(mArrayData[i, j], fieldType));
            }
        }

        return result;
    }

    //-------------------------------------------------------------------------------------------------
    //행렬 데이터 List로 변환 
    public List<T> TableToList<T>() where T : class, new()
    {
        List<T> result = new List<T>(mRowCount);

        System.Type type = typeof(T);
        System.Reflection.FieldInfo[] arrayFieldInfo = type.GetFields();

        for (int i = 0; i < mRowCount; ++i)
        {
            T newData = new T();
            for (int j = 0; j < arrayFieldInfo.Length; ++j)
            {
                System.Type fieldType = arrayFieldInfo[j].FieldType;
                arrayFieldInfo[j].SetValue(newData, CSVReader.TypeConverter.ConvertType(mArrayData[i, j], fieldType));
            }
            result.Add(newData);
        }

        return result;
    }

    //-------------------------------------------------------------------------------------------------
    //행렬데이터 Dictionary로 변환
    public Dictionary<TKey, TData> TableToDictionary<TKey, TData>() where TData : class, new()
    {
        Dictionary<TKey, TData> result;
        //{{ 키 이름 찾아오기 ~ 
        string keyName = CSVReader.KeySelector.TryGetKey<TData>();
        // }}
        result = new Dictionary<TKey, TData>();

        System.Type type = typeof(TData);
        System.Reflection.FieldInfo[] arrayFieldInfo = type.GetFields();

        for (int i = 0; i < mRowCount; ++i)
        {
            TData newData = new TData();
            object key = null;

            if (typeof(string) == typeof(TKey))
            {
                key = "";
            }
            else
            {
                key = Activator.CreateInstance(typeof(TKey));
            }

            for (int j = 0; j < arrayFieldInfo.Length; ++j)
            {
                System.Type fieldType = arrayFieldInfo[j].FieldType;
                arrayFieldInfo[j].SetValue(newData, CSVReader.TypeConverter.ConvertType(mArrayData[i, j], fieldType));
                //DataAttribute를 사용해서 명시적으로 KeyName을 설정하였다면
                if (string.IsNullOrEmpty(keyName) == false)
                {
                    if (keyName == arrayFieldInfo[j].Name)
                    {
                        key = Convert.ChangeType(mArrayData[i, j], fieldType);
                    }
                }
                //KeyName을 설정을 안했다면 첫번째 Field를 KeyName으로
                else
                {
                    if (j == 0)
                    {
                        keyName = arrayFieldInfo[j].Name;
                    }
                }
            }
            result.Add((TKey)key, newData);
        }

        return result;
    }
}

[CustomEditor(typeof(Table))]
public class DataEditor : Editor
{
    Table data;

    void OnEnable()
    {
        data = (Table)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Label("Read Data Examples");

        if (GUILayout.Button("Pull Data Method One"))
        {
            UpdateStats(UpdateMethodOne);
        }
    }

    void UpdateStats(UnityAction<GstuSpreadSheet> callback, bool mergedCells = false)
    {
        SpreadsheetManager.Read(new GSTU_Search(data.associatedSheet, data.associatedWorksheet), callback, mergedCells);
    }

    void UpdateMethodOne(GstuSpreadSheet ss)
    {
        string name;
        string[] arrayHeader;
        string[,] arrayData;
        int columnCount;
        int rowCount;

        string firstColName = "";
        List<string> listHeader = new List<string>();
        List<string[]> listData = new List<string[]>();
        List<string> dictKeys = new List<string>(ss.Cells.Keys);

        //행 이름 저장
        for (int i = 0; i < dictKeys.Count; i++)
        {
            string k = dictKeys[i];
            if (i == 0)
                firstColName = ss.Cells[k].columnId;
            else if (i != 0 && ss.Cells[k].columnId == firstColName)
                break;
            listHeader.Add(ss.Cells[k].columnId);
        }

        name = data.associatedWorksheet;
        arrayHeader = listHeader.ToArray();
        columnCount = listHeader.Count;
        rowCount = ss.Cells.Count / columnCount;
        arrayData = new string[rowCount, columnCount];
        int r = 0, c = 0;
        for (int i = 0; i < dictKeys.Count; i++)
        {
            string k = dictKeys[i];
            if (i == 0)
                firstColName = ss.Cells[k].columnId;
            else if (ss.Cells[k].columnId.Equals(firstColName))
            {
                r++;
                c = 0;
            }
            arrayData[r, c] = ss.Cells[k].value;
            c++;
        }

        data.SetTable(name, arrayData, arrayHeader, rowCount, columnCount);
        EditorUtility.SetDirty(target);
    }
}