using System.Collections;
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
}

public class ItemManager : SingletonBase<ItemManager>
{
    //착용중인 아이템 인덱스
    public CurrentItems currentItems;
    Dictionary<int, ItemData> itemDictionary;
    Dictionary<ItemData, int> playerInventory;
    public Player player;
    PartSelection playerPartSelection;

    private void Start()
    {
        Table itemTable = CSVReader.Reader.ReadCSVToTable("CSVData/ItemDatabase");
        itemDictionary = itemTable.TableToDictionary<int, ItemData>();
        playerPartSelection = player.gameObject.GetComponent<PartSelection>();
        LoadCurrentItems();
        LoadInventoryData();
    }

    /// <summary>
    /// 현재 플레이어가 착용하고 있는 아이템 저장
    /// </summary>
    private void SaveCurrentItems()
    {
        string jsonData = JsonUtility.ToJson(currentItems);
        string path = Path.Combine(Application.dataPath, "playerCurrentItems.json");
        File.WriteAllText(path, jsonData);
    }

    /// <summary>
    /// 마지막으로 플레이어가 착용하고 있던 아이템 로드
    /// </summary>
    private void LoadCurrentItems()
    {
        string path = Path.Combine(Application.dataPath, "playerCurrentItems.json");
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
        if (playerInventory.ContainsKey(item.itemData))
        {
            playerInventory[item.itemData] += 1;
        }
        else
        {
            playerInventory.Add(item.itemData, 1);
        }
        SaveInventoryData();
    }

    public void RemoveItem(Item item)
    {
        playerInventory.Remove(item.itemData);
        SaveInventoryData();
    }

    public void SetItemData(int id, out ItemData itemData)
    {
        itemData = itemDictionary[id];
    }

    /// <summary>
    /// 아이템에 맞춰 플레이어에게 장착
    /// </summary>
    /// <param name="item">갈아낄 아이템 객체</param>
    public void SetItemToPlayer(Item item)
    {
        switch (item.itemData.itemType)
        {
            case "Armor":
                playerPartSelection.ChangeChestPart(item.itemData.itemIndex);
                playerPartSelection.ChangeSpinePart(item.itemData.itemIndex);
                currentItems.chestIndex = item.itemData.itemIndex;
                currentItems.spineIndex = item.itemData.itemIndex;
                break;
            case "Bottom":
                playerPartSelection.ChangeLowerSpinePart(item.itemData.itemIndex);
                currentItems.lowerSpineIndex = item.itemData.itemIndex;
                break;
            case "Helmet":
                playerPartSelection.ChangeHeadAccesoriesPart(item.itemData.itemIndex);
                currentItems.headAccesoriesIndex = item.itemData.itemIndex;
                break;
            case "Gloves":
                playerPartSelection.ChangeLeftElbowPart(item.itemData.itemIndex);
                playerPartSelection.ChangeRightElbowPart(item.itemData.itemIndex);
                currentItems.leftElbowIndex = item.itemData.itemIndex;
                currentItems.rightElbowIndex = item.itemData.itemIndex;
                break;
            case "Boot":
                playerPartSelection.ChangeLeftKneePart(item.itemData.itemIndex);
                playerPartSelection.ChangeRightKneePart(item.itemData.itemIndex);
                currentItems.leftKneeIndex = item.itemData.itemIndex;
                currentItems.rightKneeIndex = item.itemData.itemIndex;
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
        string path = Path.Combine(Application.dataPath, "itemDatabase.json");
        File.WriteAllText(path, jsonData);
    }

    /// <summary>
    /// JSON형식의 인벤토리 데이터 로드 
    /// </summary>
    [ContextMenu("Load Inventory Data from Json")]
    public void LoadInventoryData()
    {
        string path = Path.Combine(Application.dataPath, "itemDatabase.json");
        string jsonData = File.ReadAllText(path);
        if (jsonData == null) return;
        playerInventory = JsonConvert.DeserializeObject<Dictionary<ItemData, int>>(jsonData);
    }
}
