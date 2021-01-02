using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIView : View
{
    [SerializeField] GameObject itemSlot;
    [SerializeField] Transform content;
    [SerializeField] GameObject itemDetail;

    ItemManager itemManager;
    List<GameObject> itemSlots;
    public Transform[] iconList;

    public override void UIStart()
    {
        base.UIStart();
    }

    public override void UIUpdate()
    {
        base.UIUpdate();
    }

    public override void UIExit()
    {
        base.UIExit();
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
            slot.GetComponent<ItemSlot>().SetItemData(itemManager.itemDictionary[item.Key]);
            slot.GetComponent<ItemSlot>().SetItemDetail(itemDetail);
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

    public void OnClickQuitButton()
    {
        UINaviationManager.Instance.PopToNav("SubUI_Inventory");
    }
}
