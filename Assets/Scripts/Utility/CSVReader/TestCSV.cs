using CSVReader;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CSVReader.Data("Name")]
public class TestData
{
    public string Name;
    public int MaxHP;
    public int MaxMP;
    public string PrefabName;
}

public class TestCSV : MonoBehaviour
{
    void Start()
    {
        //데이터 불러오기 => 파일을 열어서 데이터가 담겨있는 테이블로 변환해줌
        Table table = CSVReader.Reader.ReadCSVToTable("Data/MonsterData");
        //테이블에 있는 데이터를 배열로 변환
        TestData[] arrayData = table.TableToArray<TestData>();
        //테이블에 있는 데이터를 List로 변환
        List<TestData> listData = table.TableToList<TestData>();
        //테이블에 있는 데이터를 Dictionary로 변환
        Dictionary<string, TestData> dictionaryData = table.TableToDictionary<string, TestData>();

      //  Debug.Log("SucceededLoad");

        System.GC.Collect();
    }

}
