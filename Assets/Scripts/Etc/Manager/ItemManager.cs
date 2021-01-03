using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;
using CSVReader;

public class ItemManager : SingletonBase<ItemManager>
{
    //착용중인 아이템 인덱스
    public CurrentItems currentItems;
    public CurrentItemKeys currentItemKeys;
    //Dictionary<ItemData, int> playerInventory;
    public Dictionary<int, int> playerInventory;
    public Dictionary<int, ItemData> itemDictionary;
    Player player;
    PartSelection playerPartSelection;
    StatusManager statusManager;

    public int inventorySize;

    private void Start()
    {
        statusManager = StatusManager.Instance;
        currentItems = new CurrentItems();
        currentItemKeys = new CurrentItemKeys();
        itemDictionary = new Dictionary<int, ItemData>();
        playerInventory = new Dictionary<int, int>();
        Table itemTable = CSVReader.Reader.ReadCSVToTable("CSVData/ItemDatabase");
        itemDictionary = itemTable.TableToDictionary<int, ItemData>();
        LoadCurrentItems();
        LoadInventoryData();
        EquipItems();
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
            Debug.Log("최초 아이템 데이터 로드 실행입니다.");
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

    private void SaveCurrentItemKeys()
    {
        string jsonData = JsonUtility.ToJson(currentItemKeys, true);
        string path = Path.Combine(Application.persistentDataPath, "playerCurrentItemKeys.json");
        File.WriteAllText(path, jsonData);
    }

    private void LoadCurrentItemKeys()
    {
        PlayerPrefs.SetInt("LoadCurrentItemKeys", PlayerPrefs.GetInt("LoadCurrentItemKeys", 0));
        if (PlayerPrefs.GetInt("LoadCurrentItemKeys") == 0)
        {
            Debug.Log("최초 스테이터스 데이터 로드 실행입니다.");
            PlayerPrefs.SetInt("LoadCurrentItemKeys", 1);
            SaveCurrentItemKeys();
            PlayerPrefs.Save();
        }
        string path = Path.Combine(Application.persistentDataPath, "playerCurrentItemKeys.json");
        string jsonData = File.ReadAllText(path);
        if (jsonData == null) return;
        currentItemKeys = JsonUtility.FromJson<CurrentItemKeys>(jsonData);
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
                currentItemKeys.ArmorKey = itemData.id;
                break;
            case "Bottom":
                playerPartSelection.ChangeLowerSpinePart(itemData.itemIndex);
                currentItems.lowerSpineIndex = itemData.itemIndex;
                currentItemKeys.BottomKey = itemData.id;
                break;
            case "Helmet":
                playerPartSelection.ChangeHeadAccesoriesPart(itemData.itemIndex);
                currentItems.headAccesoriesIndex = itemData.itemIndex;
                currentItemKeys.HelmetKey = itemData.id;
                break;
            case "Gloves":
                playerPartSelection.ChangeLeftElbowPart(itemData.itemIndex);
                playerPartSelection.ChangeRightElbowPart(itemData.itemIndex);
                currentItems.leftElbowIndex = itemData.itemIndex;
                currentItems.rightElbowIndex = itemData.itemIndex;
                currentItemKeys.GlovesKey = itemData.id;
                break;
            case "Boot":
                playerPartSelection.ChangeLeftKneePart(itemData.itemIndex);
                playerPartSelection.ChangeRightKneePart(itemData.itemIndex);
                currentItems.leftKneeIndex = itemData.itemIndex;
                currentItems.rightKneeIndex = itemData.itemIndex;
                currentItemKeys.BootKey = itemData.id;
                break;
        }
        SaveCurrentItems();
        SaveCurrentItemKeys();
        EquipItems();
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
            Debug.Log("최초 인벤토리 데이터 로드 실행입니다.");
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
    private void EquipItems()
    {
        statusManager.finalStatus = (CurrentStatus) statusManager.playerStatus.Clone();
        EquipArmor(currentItemKeys.ArmorKey);
        EquipBottom(currentItemKeys.BottomKey);
        EquipHelmet(currentItemKeys.HelmetKey);
        EquipGloves(currentItemKeys.GlovesKey);
        EquipBoot(currentItemKeys.BootKey);
        Debug.Log("curreunt status : " + statusManager.finalStatus);
    }

    private void EquipBoot(int bootKey)
    {
        statusManager.finalStatus.moveSpeed *= (1 + itemDictionary[bootKey].moveSpeed);
        statusManager.finalStatus.dashCooldown *= (1 + itemDictionary[bootKey].dashCooldown);
        statusManager.finalStatus.dashStamina -= itemDictionary[bootKey].dashStamina;
    }

    private void EquipGloves(int glovesKey)
    {
        statusManager.finalStatus.attackDamage *= (1 + itemDictionary[glovesKey].attackDamage);
        statusManager.finalStatus.attackSpeed *= (1 + itemDictionary[glovesKey].attackSpeed);
        statusManager.finalStatus.attackCooldown += itemDictionary[glovesKey].attackCooldown;
    }

    private void EquipHelmet(int helmetKey)
    {
        statusManager.finalStatus.tenacity += itemDictionary[helmetKey].tenacity;
    }

    private void EquipBottom(int bottomKey)
    {
        statusManager.finalStatus.maxStamina += itemDictionary[bottomKey].stamina;
        statusManager.finalStatus.staminaRecovery += (1 + itemDictionary[bottomKey].staminaRecovery);
        statusManager.finalStatus.maxHp += (1 + itemDictionary[bottomKey].hp);
        statusManager.finalStatus.hpRecovery += (1 + itemDictionary[bottomKey].hpRecovery);
    }

    private void EquipArmor(int armorKey)
    {
        statusManager.finalStatus.maxHp *= (1 + itemDictionary[armorKey].hpIncreaseRate);
        statusManager.finalStatus.maxHp *= (1 + itemDictionary[armorKey].armor);
        statusManager.finalStatus.maxHp *= (1 + itemDictionary[armorKey].magicResistance);
    }
}
