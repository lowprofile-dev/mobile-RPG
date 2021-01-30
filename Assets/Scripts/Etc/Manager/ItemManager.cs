/*
    File ItemManager.cs
    class ItemManager
    
    담당자 : 김기정
    부 담당자 :
 */

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
    public Dictionary<int, int> playerInventory;
    public Dictionary<int, ItemData> itemDictionary;
    public List<ItemData> itemCart = new List<ItemData>();
    public GameObject dropItemPrefab;
    Player player;
    PartSelection playerPartSelection;
    StatusManager statusManager;

    public int inventorySize;
    public int dictionarySize;

    public List<ItemData> lowClassItems = new List<ItemData>();
    public List<ItemData> highClassItems = new List<ItemData>();
    List<ItemData> rareClassItems = new List<ItemData>();
    List<ItemData> heroicClassItems = new List<ItemData>();
    List<ItemData> legendaryClassItems = new List<ItemData>();
    List<List<ItemData>> allClassItems = new List<List<ItemData>>();

    //아이템 드랍률
    public float[] itemDropProbability = { 11f, 9f, 6.5f, 4.5f, 1.5f };
    public float[] stage1Probability = { 5, 3, 1.5f, 0.5f, 0 };
    public float[] stage2Probability = { 11f, 9f, 6.5f, 4.5f, 1.5f };
    public float[] bossProbability = { 0, 0, 60, 25, 15 };

    private void Update()
    {
        if (itemCart.Count == 0)
        {
            itemCart.Add(null);
            itemCart.Add(null);
            itemCart.Add(null);
            itemCart.Add(null);
        }
        inventorySize = 0;
        foreach (var item in playerInventory)
        {
            inventorySize += item.Value;
        }
        dictionarySize = itemDictionary.Count;
        if (player == null)
        {
            player = Player.Instance;
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
      //      Debug.Log("최초 아이템 데이터 로드 실행입니다.");
            PlayerPrefs.SetInt("LoadCurrentItemCount", 1);
            SaveCurrentItems();
            PlayerPrefs.Save();
        }

        string path = Path.Combine(Application.persistentDataPath, "playerCurrentItems.json");
        string jsonData = File.ReadAllText(path);
        if (jsonData == null) return;
        currentItems = JsonUtility.FromJson<CurrentItems>(jsonData);
    }

    /// <summary>
    /// 아이템매니저 초기화
    /// </summary>
    internal void InitItemManager()
    {
        statusManager = StatusManager.Instance;
        currentItems = new CurrentItems();
        currentItemKeys = new CurrentItemKeys();
        itemDictionary = new Dictionary<int, ItemData>();
        playerInventory = new Dictionary<int, int>(); //ID,개수
        Table itemTable = CSVReader.Reader.ReadCSVToTable("CSVData/ItemDatabase");
        itemDictionary = itemTable.TableToDictionary<int, ItemData>();
        LoadCurrentItemKeys();
        LoadCurrentItems();
        LoadItemsPerCategory();
        itemCart.Add(null);
        itemCart.Add(null);
        itemCart.Add(null);
        itemCart.Add(null);
        LoadCurrentItems();
        LoadInventoryData();
        EquipItems();
        dropItemPrefab = Resources.Load<GameObject>("Prefab/Items/Item Prefab");
    }

    void LoadItemsPerCategory()
    {
        foreach (var item in itemDictionary)
        {
            switch (item.Value.itemgrade)
            {
                case 1:
                    lowClassItems.Add(item.Value);
                    break;
                case 2:
                    highClassItems.Add(item.Value);
                    break;
                case 3:
                    rareClassItems.Add(item.Value);
                    break;
                case 4:
                    heroicClassItems.Add(item.Value);
                    break;
                case 5:
                    legendaryClassItems.Add(item.Value);
                    break;
            }
        }
        allClassItems.Add(lowClassItems);
        allClassItems.Add(highClassItems);
        allClassItems.Add(rareClassItems);
        allClassItems.Add(heroicClassItems);
        allClassItems.Add(legendaryClassItems);
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
            PlayerPrefs.SetInt("LoadCurrentItemKeys", 1);
            SaveCurrentItemKeys();
            PlayerPrefs.Save();
        }
        string path = Path.Combine(Application.persistentDataPath, "playerCurrentItemKeys.json");
        string jsonData = File.ReadAllText(path);
        if (jsonData == null) return;
        currentItemKeys = JsonUtility.FromJson<CurrentItemKeys>(jsonData);
    }

    public void AddToCart(int index, ItemData itemData)
    {
        if (itemCart[index] != null && itemCart[index].id != 0)
        {
            AddItem(itemCart[index]);
        }
        if (playerInventory[itemData.id] == 1)
        {
            if (currentItemKeys.ArmorKey == itemData.id ||
                currentItemKeys.BottomKey == itemData.id ||
                currentItemKeys.HelmetKey == itemData.id ||
                currentItemKeys.GlovesKey == itemData.id ||
                currentItemKeys.BootKey == itemData.id)
                return;
        }
        playerInventory[itemData.id] -= 1;
        if (playerInventory[itemData.id] == 0)
            playerInventory.Remove(itemData.id);
        itemCart[index] = (itemData);
        SaveInventoryData();
        LoadInventoryData();
    }

    public void ResetCart()
    {
        for (int i = 0; i < itemCart.Count; i++)
        {
            if (itemCart[i] != null)
                AddItem(itemCart[i]);
            itemCart[i] = null;
        }
        playerInventory.Remove(0);
        SaveInventoryData();
    }

    public int GetTotalCartPrice()
    {
        int totalPrice = 0;
        foreach (var itemData in itemCart)
        {
            if (itemData == null) continue;
            totalPrice += (int)itemData.sellprice;
        }
        return totalPrice;
    }

    public void SellItem()
    {
        foreach (var itemdata in itemCart) {
            if (itemdata == null) continue;
            currentItems.gold += (int)itemdata.sellprice;
        }
        itemCart.Clear();
        itemCart.Add(null);
        itemCart.Add(null);
        itemCart.Add(null);
        itemCart.Add(null);
        ResetCart();
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

    public void AddItem(ItemData itemData)
    {
        if (itemData.id == 0) return;
        if (playerInventory.ContainsKey(itemData.id))
        {
            playerInventory[itemData.id] += 1;
        }
        else
        {
            playerInventory.Add(itemData.id, 1);
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
    }

    /// <summary>
    /// 아이템에 맞춰 플레이어에게 장착
    /// </summary>
    /// <param name="itemData">갈아낄 아이템 데이터</param>
    public void SetItemToPlayer(ItemData itemData)
    {
        UnequipItems();
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
        player.InitOutline();
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
            PlayerPrefs.SetInt("LoadInventoryDataCount", 1);
            InitInventoryData();
            SaveInventoryData();
            PlayerPrefs.Save();
        }
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
        EquipItem(currentItemKeys.ArmorKey);
        EquipItem(currentItemKeys.BottomKey);
        EquipItem(currentItemKeys.HelmetKey);
        EquipItem(currentItemKeys.GlovesKey);
        EquipItem(currentItemKeys.BootKey);
        statusManager.UpdateFinalStatus();
    }

    private void UnequipItems()
    {
        UnEquipItem(currentItemKeys.ArmorKey);
        UnEquipItem(currentItemKeys.BottomKey);
        UnEquipItem(currentItemKeys.HelmetKey);
        UnEquipItem(currentItemKeys.GlovesKey);
        UnEquipItem(currentItemKeys.BootKey);
        statusManager.UpdateFinalStatus();
    }

    private void EquipItem(int key)
    {
        statusManager.additionStatus.hp += itemDictionary[key].hp;
        statusManager.multiplicationStatus.hpIncreaseRate += itemDictionary[key].hpIncreaseRate;
        statusManager.additionStatus.hpRecovery += itemDictionary[key].hpRecovery;
        statusManager.additionStatus.stamina += itemDictionary[key].stamina;
        statusManager.additionStatus.staminaRecovery += (itemDictionary[key].staminaRecovery);
        statusManager.additionStatus.attackDamage += itemDictionary[key].attackDamage;
        statusManager.multiplicationStatus.attackCooldown += itemDictionary[key].attackCooldown;
        statusManager.additionStatus.criticalDamage += itemDictionary[key].criticalDamage;
        statusManager.additionStatus.criticalPercent += itemDictionary[key].criticalPercent;
        statusManager.additionStatus.armor += itemDictionary[key].armor;
        statusManager.multiplicationStatus.moveSpeed += itemDictionary[key].moveSpeed;
        statusManager.multiplicationStatus.dashStamina += itemDictionary[key].dashStamina;
    }

    private void UnEquipItem(int key)
    {
        statusManager.additionStatus.hp -= itemDictionary[key].hp;
        statusManager.multiplicationStatus.hpIncreaseRate -= itemDictionary[key].hpIncreaseRate;
        statusManager.additionStatus.hpRecovery -= itemDictionary[key].hpRecovery;
        statusManager.additionStatus.stamina -= itemDictionary[key].stamina;
        statusManager.additionStatus.staminaRecovery -= (itemDictionary[key].staminaRecovery);
        statusManager.additionStatus.attackDamage -= itemDictionary[key].attackDamage;
        statusManager.multiplicationStatus.attackCooldown -= itemDictionary[key].attackCooldown;
        statusManager.additionStatus.criticalDamage -= itemDictionary[key].criticalDamage;
        statusManager.additionStatus.criticalPercent -= itemDictionary[key].criticalPercent;
        statusManager.additionStatus.armor -= itemDictionary[key].armor;
        statusManager.multiplicationStatus.moveSpeed -= itemDictionary[key].moveSpeed;
        statusManager.multiplicationStatus.dashStamina -= itemDictionary[key].dashStamina;
    }


    /*
    private void EquipBoot(int bootKey)
    {
        statusManager.multiplicationStatus.moveSpeed += itemDictionary[bootKey].moveSpeed;
        statusManager.multiplicationStatus.dashCooldown +=  itemDictionary[bootKey].dashCooldown;
        statusManager.multiplicationStatus.dashStamina += itemDictionary[bootKey].dashStamina;
    }

    private void EquipGloves(int glovesKey)
    {
        statusManager.additionStatus.attackDamage += itemDictionary[glovesKey].attackDamage;
        statusManager.multiplicationStatus.attackCooldown += itemDictionary[glovesKey].attackCooldown;
    }

    private void EquipHelmet(int helmetKey)
    {
        statusManager.additionStatus.rigidresistance += itemDictionary[helmetKey].rigidresistance;
        statusManager.additionStatus.stunresistance += itemDictionary[helmetKey].stunresistance;
        statusManager.additionStatus.fallresistance += itemDictionary[helmetKey].fallresistance;
    }

    private void EquipBottom(int bottomKey)
    {
        statusManager.additionStatus.stamina += itemDictionary[bottomKey].stamina;
        statusManager.additionStatus.staminaRecovery += (itemDictionary[bottomKey].staminaRecovery);
        statusManager.additionStatus.hp += itemDictionary[bottomKey].hp;
        statusManager.additionStatus.hpRecovery += itemDictionary[bottomKey].hpRecovery;
    }

    private void EquipArmor(int armorKey)
    {
        statusManager.additionStatus.hp += itemDictionary[armorKey].hpIncreaseRate;
        statusManager.additionStatus.armor += itemDictionary[armorKey].armor;
    }

    private void UnequipBoot(int bootKey)
    {
        statusManager.multiplicationStatus.moveSpeed -= itemDictionary[bootKey].moveSpeed;
        statusManager.multiplicationStatus.dashCooldown -= itemDictionary[bootKey].dashCooldown;
        statusManager.multiplicationStatus.dashStamina -= itemDictionary[bootKey].dashStamina;
    }

    private void UnequipGloves(int glovesKey)
    {
        statusManager.additionStatus.attackDamage -= itemDictionary[glovesKey].attackDamage;
        statusManager.multiplicationStatus.attackCooldown -= itemDictionary[glovesKey].attackCooldown;
    }

    private void UnequipHelmet(int helmetKey)
    {
        statusManager.additionStatus.rigidresistance -= itemDictionary[helmetKey].rigidresistance;
        statusManager.additionStatus.stunresistance -= itemDictionary[helmetKey].stunresistance;
        statusManager.additionStatus.fallresistance -= itemDictionary[helmetKey].fallresistance;
    }

    private void UnequipBottom(int bottomKey)
    {
        statusManager.additionStatus.stamina -= itemDictionary[bottomKey].stamina;
        statusManager.additionStatus.staminaRecovery -= (itemDictionary[bottomKey].staminaRecovery);
        statusManager.additionStatus.hp -= itemDictionary[bottomKey].hp;
        statusManager.additionStatus.hpRecovery -= itemDictionary[bottomKey].hpRecovery;
    }

    private void UnequipArmor(int armorKey)
    {
        statusManager.additionStatus.hp -= itemDictionary[armorKey].hpIncreaseRate;
        statusManager.additionStatus.armor -= itemDictionary[armorKey].armor;
    }
    */

    /// <summary>
    /// 아이템 드랍 함수
    /// </summary>
    /// <param name="monsterTransform"> 드랍할 좌표(몬스터가 사망한 지점) </param>
    public void DropItem(Transform monsterTransform)
    {
        var roll = UnityEngine.Random.Range(0, 100.0f);
        for (int i = 4; i >= 0; i--)
        {
            if (roll <= itemDropProbability[i]) // 현재 확률에 들어간다면 
            {
                GameObject dropItem = ObjectPoolManager.Instance.GetObject(dropItemPrefab);
                int itemTypeDice = UnityEngine.Random.Range(0, 5);
                string dropType;
                List<ItemData> possibleDropList = new List<ItemData>();
                switch (itemTypeDice)
                {
                    case 0:
                        dropType = "Armor";
                        break;
                    case 1:
                        dropType = "Bottom";
                        break;
                    case 2:
                        dropType = "Helmet";
                        break;
                    case 3:
                        dropType = "Gloves";
                        break;
                    case 4:
                        dropType = "Boot";
                        break;
                    default:
                        dropType = "Helmet";
                        break;
                }
                for (int j = 0; j < allClassItems[i].Count; j++)
                {
                    if (allClassItems[i][j].itemType == dropType)
                    {
                        possibleDropList.Add(allClassItems[i][j]);
                    }
                }
                dropItem.GetComponent<Item>().id = possibleDropList[UnityEngine.Random.Range(0, possibleDropList.Count)].id;
                dropItem.GetComponent<Item>().LoadItemData();
                dropItem.transform.position = monsterTransform.position;
                dropItem.transform.rotation = monsterTransform.rotation;
                dropItem.transform.SetParent(null);
                break; // 생성 후 break
            }
        }
    }
}