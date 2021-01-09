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
    public List<ItemData> itemList = new List<ItemData>();
    Player player;
    PartSelection playerPartSelection;
    StatusManager statusManager;

    public int inventorySize;

    private void Start()
    {
        //아이템 데이터 초기화를 원할시 주석 풀것!
        PlayerPrefs.DeleteAll();
        statusManager = StatusManager.Instance;
        currentItems = new CurrentItems();
        currentItemKeys = new CurrentItemKeys();
        itemDictionary = new Dictionary<int, ItemData>();
        playerInventory = new Dictionary<int, int>(); //ID,개수
        Table itemTable = CSVReader.Reader.ReadCSVToTable("CSVData/ItemDatabase");
        itemDictionary = itemTable.TableToDictionary<int, ItemData>();
        itemList = itemTable.TableToList<ItemData>();
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
            //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            if (player != null)
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
        if (PlayerPrefs.GetInt("LoadCurrentItemCount") == 0)
        {
            Debug.Log("최초 아이템 데이터 로드 실행입니다.");
            PlayerPrefs.SetInt("LoadCurrentItemCount", 1);
            SaveCurrentItems();
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

    public void SellItem(ItemData itemdata)
    {
        playerInventory[itemdata.id] -= 1;
        currentItems.gold += (int)itemdata.sellprice;
        SaveInventoryData();
        SaveCurrentItems();
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

    public void AddGold(int amount)
    {
        currentItems.gold += amount;
        SaveCurrentItems();
    }

    public void AddCoin(int amount)
    {
        currentItems.coin += amount;
        SaveCurrentItems();
    }

    public void AddGem(int amount)
    {
        currentItems.gem += amount;
        SaveCurrentItems();
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
        playerPartSelection.ChangeLeftShoulderPart(currentItems.leftShoulderIndex);
        playerPartSelection.ChangeRightShoulderPart(currentItems.rightShoulderIndex);
        playerPartSelection.ChangeLeftKneePart(currentItems.leftKneeIndex);
        playerPartSelection.ChangeRightKneePart(currentItems.rightKneeIndex);
        playerPartSelection.ChangeLeftHipPart(currentItems.leftHipIndex);
        playerPartSelection.ChangeRightHipPart(currentItems.rightHipIndex);
        //이거 풀면 초상화는 해결되는데 조이스틱이 망가짐
        //player.ChangeFaceCamera();
    }

    public void SetItemToPlayer(Player _player)
    {
        _player.selection.ChangeChestPart(currentItems.chestIndex);
        _player.selection.ChangeSpinePart(currentItems.spineIndex);
        _player.selection.ChangeLowerSpinePart(currentItems.lowerSpineIndex);
        _player.selection.ChangeHeadAccesoriesPart(currentItems.headAccesoriesIndex);
        _player.selection.ChangeLeftElbowPart(currentItems.leftElbowIndex);
        _player.selection.ChangeRightElbowPart(currentItems.rightElbowIndex);
        _player.selection.ChangeLeftShoulderPart(currentItems.leftShoulderIndex);
        _player.selection.ChangeRightShoulderPart(currentItems.rightShoulderIndex);
        _player.selection.ChangeLeftKneePart(currentItems.leftKneeIndex);
        _player.selection.ChangeRightKneePart(currentItems.rightKneeIndex);
        _player.selection.ChangeLeftHipPart(currentItems.leftHipIndex);
        _player.selection.ChangeRightHipPart(currentItems.rightHipIndex);
        //이거 풀면 초상화는 해결되는데 조이스틱이 망가짐
        //_player.ChangeFaceCamera();
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
                playerPartSelection.ChangeRightShoulderPart(itemData.itemIndex);
                playerPartSelection.ChangeLeftShoulderPart(itemData.itemIndex);
                currentItems.leftElbowIndex = itemData.itemIndex;
                currentItems.rightElbowIndex = itemData.itemIndex;
                currentItemKeys.GlovesKey = itemData.id;
                break;
            case "Boot":
                playerPartSelection.ChangeLeftKneePart(itemData.itemIndex);
                playerPartSelection.ChangeRightKneePart(itemData.itemIndex);
                playerPartSelection.ChangeLeftHipPart(itemData.itemIndex);
                playerPartSelection.ChangeRightHipPart(itemData.itemIndex);
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
        if (PlayerPrefs.GetInt("LoadInventoryDataCount") == 0)
        {
            Debug.Log("최초 인벤토리 데이터 로드 실행입니다.");
            PlayerPrefs.SetInt("LoadInventoryDataCount", 1);
            InitInventoryData();
            SaveInventoryData();
            PlayerPrefs.Save();
        }
        //List<KeyValuePair<ItemData, int>> temp = new List<KeyValuePair<ItemData, int>>();
        string path = Path.Combine(Application.persistentDataPath, "inventoryDB.json");
        string jsonData = File.ReadAllText(path);
        if (jsonData == null) return;
        playerInventory = JsonConvert.DeserializeObject<Dictionary<int, int>>(jsonData);
    }

    private void InitInventoryData()
    {
        playerInventory.Add(1, 1);
        playerInventory.Add(2, 1);
        playerInventory.Add(3, 1);
        playerInventory.Add(4, 1);
        playerInventory.Add(5, 1);
    }

    [ContextMenu("Save Item Data to Json")]
    public void SaveItemData()
    {
        string jsonData = JsonConvert.SerializeObject(itemDictionary, Formatting.Indented);
        string path = Path.Combine(Application.persistentDataPath, "itemDB.json");
        File.WriteAllText(path, jsonData);
    }

    private void EquipItems()
    {
        statusManager.itemMultiplicationStatus = new MultiplicationStatus();
        statusManager.itemAdditionStatus = new AdditionStatus();
        EquipArmor(currentItemKeys.ArmorKey);
        EquipBottom(currentItemKeys.BottomKey);
        EquipHelmet(currentItemKeys.HelmetKey);
        EquipGloves(currentItemKeys.GlovesKey);
        EquipBoot(currentItemKeys.BootKey);
        statusManager.UpdateFinalStatus();
        Debug.Log("curreunt status : " + statusManager.finalStatus);
    }

    private void EquipBoot(int bootKey)
    {
        statusManager.itemMultiplicationStatus.moveSpeed += itemDictionary[bootKey].moveSpeed;
        statusManager.itemMultiplicationStatus.dashCooldown +=  itemDictionary[bootKey].dashCooldown;
        statusManager.itemMultiplicationStatus.dashStamina += itemDictionary[bootKey].dashStamina;
    }

    private void EquipGloves(int glovesKey)
    {
        statusManager.itemAdditionStatus.attackDamage += itemDictionary[glovesKey].attackDamage;
        statusManager.itemMultiplicationStatus.attackSpeed += itemDictionary[glovesKey].attackSpeed;
        statusManager.itemMultiplicationStatus.attackCooldown += itemDictionary[glovesKey].attackCooldown;
    }

    private void EquipHelmet(int helmetKey)
    {
        statusManager.itemAdditionStatus.rigidresistance += itemDictionary[helmetKey].rigidresistance;
        statusManager.itemAdditionStatus.stunresistance += itemDictionary[helmetKey].stunresistance;
        statusManager.itemAdditionStatus.fallresistance += itemDictionary[helmetKey].fallresistance;
    }

    private void EquipBottom(int bottomKey)
    {
        statusManager.itemAdditionStatus.stamina += itemDictionary[bottomKey].stamina;
        statusManager.itemAdditionStatus.staminaRecovery += (1 + itemDictionary[bottomKey].staminaRecovery);
        statusManager.itemAdditionStatus.hp += itemDictionary[bottomKey].hp;
        statusManager.itemAdditionStatus.hpRecovery += itemDictionary[bottomKey].hpRecovery;
    }

    private void EquipArmor(int armorKey)
    {
        statusManager.itemAdditionStatus.hp += itemDictionary[armorKey].hpIncreaseRate;
        statusManager.itemAdditionStatus.armor += itemDictionary[armorKey].armor;
        statusManager.itemAdditionStatus.magicResistance += itemDictionary[armorKey].magicResistance;
    }
}
