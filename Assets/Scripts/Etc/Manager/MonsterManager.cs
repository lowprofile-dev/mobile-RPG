﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSVReader;

public class MonsterManager : SingletonBase<MonsterManager>
{
    private Dictionary<int, MonsterData> monsterDictionary; public Dictionary<int, MonsterData> MonsterDictionary { get { return monsterDictionary; } }



    public void InitMonsterManager()
    {
        monsterDictionary = new Dictionary<int, MonsterData>();

        Table monsterTable = CSVReader.Reader.ReadCSVToTable("CSVData/MonsterDataBase");
        monsterDictionary = monsterTable.TableToDictionary<int, MonsterData>();
    }
}