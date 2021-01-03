using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;
using CSVReader;

/// <summary>
/// 세이브 및 로드, 이런저런 상황에 사용되는 수치들을 종합한다.
/// </summary>
public class StatusManager : SingletonBase<StatusManager>
{
    [SerializeField] Player player;
    [SerializeField] Dictionary<int, StatusData> statusDictionary;
    public CurrentStatus playerStatus;

    // 카드 리롤
    public int cardRerollCoin;
    public int needToCardRerollCoin;
    public int rerollCount;

    /// <summary>
    /// 스테이터스 정보들을 초기화한다.
    /// </summary>
    public void InitStatusManager()
    {
        cardRerollCoin = 10000;
        needToCardRerollCoin = 0;
        rerollCount = 0;
    }

    void Start()
    {
        player = Player.Instance;
        playerStatus = new CurrentStatus();
        Table statusTable = CSVReader.Reader.ReadCSVToTable("CSVData/StatusDatabase");
        statusDictionary = statusTable.TableToDictionary<int, StatusData>();
        LoadCurrentStatus();
    }

    void Update()
    {
        if (player == null)
        {
            player = Player.Instance;
        }

        
    }

    private void LoadCurrentStatus()
    {
        string path = Path.Combine(Application.persistentDataPath, "playerCurrentStatus.json");
        string jsonData = File.ReadAllText(path);
        if (jsonData == null) return;
        playerStatus = JsonUtility.FromJson<CurrentStatus>(jsonData);
    }

    private void SaveCurrentStatus()
    {
        string jsonData = JsonUtility.ToJson(playerStatus, true);
        string path = Path.Combine(Application.persistentDataPath, "playerCurrentStatus.json");
        File.WriteAllText(path, jsonData);
    }

    public void AddExp(int _exp)
    {
        playerStatus.exp += _exp;
        if (statusDictionary[playerStatus.level].exp <= playerStatus.exp)
        {
            playerStatus.exp -= statusDictionary[playerStatus.level].exp;
            LevelUp();
        }
        SaveCurrentStatus();
    }

    private void LevelUp()
    {
        playerStatus.level += 1;
        playerStatus.maxHp = statusDictionary[playerStatus.level].maxHp;
        playerStatus.attackDamage = statusDictionary[playerStatus.level].attackDamage;
        playerStatus.magicDamage = statusDictionary[playerStatus.level].magicDamage;
        playerStatus.armor = statusDictionary[playerStatus.level].armor;
        playerStatus.magicResistance = statusDictionary[playerStatus.level].magicResistance;
        playerStatus.maxHp = statusDictionary[playerStatus.level].maxHp;
        playerStatus.moveSpeed = statusDictionary[playerStatus.level].moveSpeed;
        playerStatus.attackSpeed = statusDictionary[playerStatus.level].attackSpeed;
        playerStatus.tenacity = statusDictionary[playerStatus.level].tenacity;
    }
}
