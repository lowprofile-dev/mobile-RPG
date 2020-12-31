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
    //Dictionary<ItemData, int> playerInventory;
    Dictionary<int, int> playerInventory;
    Player player;
    PartSelection playerPartSelection;

    public int inventorySize;

    private void Start()
    {
        itemDictionary = new Dictionary<int, ItemData>();
        playerInventory = new Dictionary<int, int>();
        Table itemTable = CSVReader.Reader.ReadCSVToTable("CSVData/itemDatabase");
        itemDictionary = itemTable.TableToDictionary<int, ItemData>();
        SaveCurrentItems();
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
        string path = Path.Combine(Application.dataPath, "inventoryDB.json");
        File.WriteAllText(path, jsonData);
    }

    /// <summary>
    /// JSON형식의 인벤토리 데이터 로드
    /// </summary>
    [ContextMenu("Load Inventory Data from Json")]
    public void LoadInventoryData()
    {
        List<KeyValuePair<ItemData, int>> temp = new List<KeyValuePair<ItemData, int>>();
        string path = Path.Combine(Application.dataPath, "inventoryDB.json");
        string jsonData = File.ReadAllText(path);
        if (jsonData == null) return;
        playerInventory = JsonConvert.DeserializeObject<Dictionary<int, int>>(jsonData);
    }

    [ContextMenu("Save Item Data to Json")]
    public void SaveItemData()
    {
        string jsonData = JsonConvert.SerializeObject(itemDictionary, Formatting.Indented);
        string path = Path.Combine(Application.dataPath, "itemDB.json");
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
