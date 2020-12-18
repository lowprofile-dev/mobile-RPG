using UnityEngine;
using System.Collections;
using GoogleSheetsToUnity;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using GoogleSheetsToUnity.ThirdPary;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class JSONData
{
    public List<string> columns;
    public Dictionary<string, string> dataList;
}

public class JSONGetter : MonoBehaviour
{
    public string associatedSheet = "";
    public string associatedWorksheet = "";

    public List<JSONData> jsonDatas = new List<JSONData>();

    private void Start()
    {
        UpdateStats(UpdateMethodOne);
    }

    private void SaveJSONData(List<GSTU_Cell> list, string name)
    {
        for(int i=0; i<list.Count; i++)
        {
            JSONData data = new JSONData();
            data.dataList.Add(list[i].columnId, list[i].value);
            data.columns.Add(list[i].columnId);
            jsonDatas.Add(data);
        }
    }

    void UpdateStats(UnityAction<GstuSpreadSheet> callback, bool mergedCells = false)
    {
        Debug.Log("TARGET : " + callback.Target.ToString());
        SpreadsheetManager.Read(new GSTU_Search(associatedSheet, associatedWorksheet), callback, mergedCells);
    }

    void UpdateMethodOne(GstuSpreadSheet ss)
    {
        Debug.Log("ROWS : " + ss.columns.GetValueFromPrimary("Math"));
        /*
        //data.UpdateStats(ss.rows["Jim"]);
        foreach (string dataName in data.Names)
            data.UpdateStats(ss.rows[dataName], dataName);
        EditorUtility.SetDirty(target);
        */
    }

}
