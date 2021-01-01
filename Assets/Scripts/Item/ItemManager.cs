﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;
using CSVReader;

[System.Serializable]
public class CurrentItems
{
    //투구
    public int headAccesoriesIndex = 0;
    //장갑
    public int leftElbowIndex = 0;
    public int rightElbowIndex = 0;
    //갑옷
    public int chestIndex = 0;
    public int spineIndex = 0;
    //하체
    public int lowerSpineIndex = 0;
    //신발
    public int leftKneeIndex = 0;
    public int rightKneeIndex = 0;

    public CurrentItems()
    {
        headAccesoriesIndex = 0;
        leftElbowIndex = 0;
        rightElbowIndex = 0;
        chestIndex = 0;
        spineIndex = 0;
        lowerSpineIndex = 0;
        leftKneeIndex = 0;
        rightKneeIndex = 0;
    }
}

public class ItemManager : SingletonBase<ItemManager>
{
    //착용중인 아이템 인덱스
    public CurrentItems currentItems;
    //Dictionary<ItemData, int> playerInventory;
    public Dictionary<int, int> playerInventory;
    public Dictionary<int, ItemData> itemDictionary;
    Player player;
    PartSelection playerPartSelection;

    public int inventorySize;

    private void Start()
    {
        currentItems = new CurrentItems();
        itemDictionary = new Dictionary<int, ItemData>();
        playerInventory = new Dictionary<int, int>();
        Table itemTable = CSVReader.Reader.ReadCSVToTable("CSVData/itemDatabase");
        itemDictionary = itemTable.TableToDictionary<int, ItemData>();
        LoadCurrentItems();
        LoadInventoryData();
    }

    private void Update()
    {
        inventorySize = playerInventory.Count;
        if (player == null)
        {
            player = Player.Instance;
            playerPartSelection = player.gameObject.GetComponent<PartSelection>();
        }
    }

    /// <summary>
    /// 현재 플레이어가 착용하고 있는 아이템 저장
    /// </summary>
    private void SaveCurrentItems()
    {
        string jsonData = JsonUtility.ToJson(currentItems, true);
        string path = Path.Combine(Application.persistentDataPath, "playerCurrentItems.json");
        File.WriteAllText(path, jsonData);
        Debug.Log(jsonData);
    }

    /// <summary>
    /// 마지막으로 플레이어가 착용하고 있던 아이템 로드
    /// </summary>
    private void LoadCurrentItems()
    {
        PlayerPrefs.SetInt("LoadCurrentItemCount", PlayerPrefs.GetInt("LoadCurrentItemCount", 0));
        TextAsset jsonRawData;
        if (PlayerPrefs.GetInt("LoadCurrentItemCount") == 0)
        {
            Debug.Log("최초 실행입니다.");
            PlayerPrefs.SetInt("LoadCurrentItemCount", 1);

            jsonRawData = Resources.Load("Data/playerCurrentItems") as TextAsset;
            string jsonFirstData = jsonRawData.ToString();
            string pathFirst = Path.Combine(Application.persistentDataPath, "playerCurrentItems.json");
            File.WriteAllText(pathFirst, jsonFirstData);

            PlayerPrefs.Save();
        }

        string path = Path.Combine(Application.persistentDataPath, "playerCurrentItems.json");
        string jsonData = File.ReadAllText(path);
        if (jsonData == null) return;
        currentItems = JsonUtility.FromJson<CurrentItems>(jsonData);
    }

    /// <summary>
    /// 획득한 아이템 인벤토리에 추가!
    /// </summary>
    /// <param name="item">아이템</param>
    public void AddItem(Item item)
    {
        if (playerInventory.ContainsKey(item.itemData.id))
        {
            playerInventory[item.itemData.id] += 1;
        }
        else
        {
            playerInventory.Add(item.itemData.id, 1);
        }
        SaveInventoryData();
    }

    public void RemoveItem(Item item)
    {
        playerInventory.Remove(item.itemData.id);
        SaveInventoryData();
    }

    public void SetItemData(int id, out ItemData itemData)
    {
        itemData = itemDictionary[id];
    }

    [ContextMenu("Set Item to Player")]
    public void SetItemToPlayer()
    {
        playerPartSelection.ChangeChestPart(currentItems.chestIndex);
        playerPartSelection.ChangeSpinePart(currentItems.spineIndex);
        playerPartSelection.ChangeLowerSpinePart(currentItems.lowerSpineIndex);
        playerPartSelection.ChangeHeadAccesoriesPart(currentItems.headAccesoriesIndex);
        playerPartSelection.ChangeLeftElbowPart(currentItems.leftElbowIndex);
        playerPartSelection.ChangeRightElbowPart(currentItems.rightElbowIndex);
        playerPartSelection.ChangeLeftKneePart(currentItems.leftKneeIndex);
        playerPartSelection.ChangeRightKneePart(currentItems.rightKneeIndex);
    }

    /// <summary>
    /// 아이템에 맞춰 플레이어에게 장착
    /// </summary>
    /// <param name="itemData">갈아낄 아이템 데이터</param>
    public void SetItemToPlayer(ItemData itemData)
    {
        switch (itemData.itemType)
        {
            case "Armor":
                playerPartSelection.ChangeChestPart(itemData.itemIndex);
                playerPartSelection.ChangeSpinePart(itemData.itemIndex);
                currentItems.chestIndex = itemData.itemIndex;
                currentItems.spineIndex = itemData.itemIndex;
                break;
            case "Bottom":
                playerPartSelection.ChangeLowerSpinePart(itemData.itemIndex);
                currentItems.lowerSpineIndex = itemData.itemIndex;
                break;
            case "Helmet":
                playerPartSelection.ChangeHeadAccesoriesPart(itemData.itemIndex);
                currentItems.headAccesoriesIndex = itemData.itemIndex;
                break;
            case "Gloves":
                playerPartSelection.ChangeLeftElbowPart(itemData.itemIndex);
                playerPartSelection.ChangeRightElbowPart(itemData.itemIndex);
                currentItems.leftElbowIndex = itemData.itemIndex;
                currentItems.rightElbowIndex = itemData.itemIndex;
                break;
            case "Boot":
                playerPartSelection.ChangeLeftKneePart(itemData.itemIndex);
                playerPartSelection.ChangeRightKneePart(itemData.itemIndex);
                currentItems.leftKneeIndex = itemData.itemIndex;
                currentItems.rightKneeIndex = itemData.itemIndex;
                break;
        }
        SaveCurrentItems();
    }

    /// <summary>
    /// 인벤토리 데이터 JSON 저장
    /// </summary>
    [ContextMenu("Save Inventory Data to Json")]
    public void SaveInventoryData()
    {
        string jsonData = JsonConvert.SerializeObject(playerInventory, Formatting.Indented);
        string path = Path.Combine(Application.persistentDataPath, "inventoryDB.json");
        File.WriteAllText(path, jsonData);
    }

    /// <summary>
    /// JSON형식의 인벤토리 데이터 로드
    /// </summary>
    [ContextMenu("Load Inventory Data from Json")]
    public void LoadInventoryData()
    {
        TextAsset jsonRawData;
        if (PlayerPrefs.GetInt("LoadInventoryDataCount") == 0)
        {
            Debug.Log("최초 실행입니다.");
            PlayerPrefs.SetInt("LoadInventoryDataCount", 1);

            jsonRawData = Resources.Load("Data/inventoryDB") as TextAsset;
            string jsonFirstData = jsonRawData.ToString();
            string pathFirst = Path.Combine(Application.persistentDataPath, "inventoryDB.json");
            File.WriteAllText(pathFirst, jsonFirstData);

            PlayerPrefs.Save();
        }
        //List<KeyValuePair<ItemData, int>> temp = new List<KeyValuePair<ItemData, int>>();
        string path = Path.Combine(Application.persistentDataPath, "inventoryDB.json");
        string jsonData = File.ReadAllText(path);
        if (jsonData == null) return;
        playerInventory = JsonConvert.DeserializeObject<Dictionary<int, int>>(jsonData);
    }

    [ContextMenu("Save Item Data to Json")]
    public void SaveItemData()
    {
        string jsonData = JsonConvert.SerializeObject(itemDictionary, Formatting.Indented);
        string path = Path.Combine(Application.persistentDataPath, "itemDB.json");
        File.WriteAllText(path, jsonData);
    }

    //private List<KeyValuePair<ItemData, int>> DictToList(Dictionary<ItemData, int> dict)
    //{
    //    List<KeyValuePair<ItemData, int>> result = new List<KeyValuePair<ItemData, int>>();
    //    foreach(var data in dict)
    //    {
    //        result.Add(data);
    //    }
    //    return result;
    //}
    //private Dictionary<ItemData, int> ListToDict(List<KeyValuePair<ItemData, int>> list)
    //{
    //    Dictionary<ItemData, int> result = new Dictionary<ItemData, int>();
    //    foreach(var data in list)
    //    {
    //        result.Add(data.Key, data.Value);
    //    }
    //    return result;
    //}
}
