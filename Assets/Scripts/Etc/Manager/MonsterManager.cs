////////////////////////////////////////////////////
/*
    File MonsterManager.cs
    class MonsterManager
    
    담당자 : 안영훈
    부 담당자 :
*/
////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSVReader;

public class MonsterManager : SingletonBase<MonsterManager>
{
    private Dictionary<int, MonsterData> monsterDictionary; public Dictionary<int, MonsterData> MonsterDictionary { get { return monsterDictionary; } }

    // 몬스터 CSV 데이터 로드
    public void InitMonsterManager()
    {
        monsterDictionary = new Dictionary<int, MonsterData>();

        Table monsterTable = Reader.ReadCSVToTable("CSVData/MonsterDataBase");
        monsterDictionary = monsterTable.TableToDictionary<int, MonsterData>();
    }
}
