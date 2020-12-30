using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class ItemManager : SingletonBase<ItemManager>
{
    public Transform head;
    public Transform hair;
    public Transform headAccesories;
    public Transform rightShoulder;
    public Transform rightElbow;
    public Transform rightWeapon;
    public Transform leftShoulder;
    public Transform leftElbow;
    public Transform leftWeapon;
    public Transform leftShield;
    public Transform chest;
    public Transform spine;
    public Transform lowerSpine;
    public Transform rightHip;
    public Transform rightKnee;
    public Transform leftHip;
    public Transform leftKnee;

    GameObject headObject;
    GameObject hairObject;
    GameObject headAccesoriesObject;
    GameObject rightShoulderObject;
    GameObject rightElbowObject;
    GameObject rightWeaponObject;
    GameObject leftShoulderObject;
    GameObject leftElbowObject;
    GameObject leftWeaponObject;
    GameObject leftShieldObject;
    GameObject chestObject;
    GameObject spineObject;
    GameObject lowerSpineObject;
    GameObject rightHipObject;
    GameObject rightKneeObject;
    GameObject leftHipObject;
    GameObject leftKneeObject;

    [SerializeField] Dictionary<ItemData, int> playerInventory;

    /// <summary>
    /// 획득한 아이템 추가!
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
    }

    public void RemoveItem(Item item)
    {
        playerInventory.Remove(item.itemData);
    }

    public void SetItem(Item item)
    {
        //  TODO : 플레이어 캐릭터와 데이터 연동.
    }

    /// <summary>
    /// 인벤토리 데이터 JSON 저장
    /// </summary>
    [ContextMenu("Save Inventory Data to Json")]
    public void SaveInventoryData()
    {
        string jsonData = JsonConvert.SerializeObject(playerInventory);
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
        playerInventory = JsonUtility.FromJson<Dictionary<ItemData, int>>(jsonData);
    }
}
