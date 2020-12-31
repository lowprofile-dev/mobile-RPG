using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] GameObject itemSlot;
    [SerializeField] Transform content;
    ItemManager itemManager;
    List<GameObject> itemSlots;
    public Transform[] iconList;

    public void OnClickQuitButton()
    {
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        itemSlots = new List<GameObject>();
        itemManager = ItemManager.Instance;
    }

    private void OnEnable()
    {
        LoadPlayerInventory();
    }

    private void LoadPlayerInventory()
    {
        ClearInventoryCache();
        foreach(var item in itemManager.playerInventory)
        {
            ItemData itemData = itemManager.itemDictionary[item.Key];
            GameObject slot = Instantiate(itemSlot, content);
            slot.GetComponent<ItemSlot>().SetIcon(iconList[item.Key-1]);
            slot.GetComponent<ItemSlot>().SetQuantity(item.Value);
            itemSlots.Add(slot);
        }
    }

    private void ClearInventoryCache()
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            Destroy(itemSlots[i]);
        }
        itemSlots.Clear();
    }
}
