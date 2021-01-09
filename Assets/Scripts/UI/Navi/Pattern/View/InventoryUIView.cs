using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUIView : View
{
    [SerializeField] GameObject itemSlot;
    [SerializeField] Transform content;
    [SerializeField] GameObject itemDetail;
    [SerializeField] Button quitBtn;
    [SerializeField] TextMeshProUGUI gold;
    [SerializeField] TextMeshProUGUI coin;
    [SerializeField] TextMeshProUGUI gem;

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

        quitBtn.onClick.AddListener( delegate{ OnClickQuitButton(); });
    }

    private void OnEnable()
    {
        LoadPlayerInventory();
        gold.text = ItemManager.Instance.currentItems.gold.ToString();
        coin.text = ItemManager.Instance.currentItems.coin.ToString();
        gem.text = ItemManager.Instance.currentItems.gem.ToString();
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
            switch(itemData.itemgrade)
            {
                case 1:
                    slot.GetComponent<ItemSlot>().SetItemGrade(new Color(1, 1, 1, 0.15f));
                    break;
                case 2:
                    slot.GetComponent<ItemSlot>().SetItemGrade(new Color(0, 1, 0, 0.15f));
                    break;
                case 3:
                    slot.GetComponent<ItemSlot>().SetItemGrade(new Color(0, 0, 1, 0.15f));
                    break;
                case 4:
                    slot.GetComponent<ItemSlot>().SetItemGrade(new Color(153, 50, 204, 0.15f));
                    break;
                case 5:
                    slot.GetComponent<ItemSlot>().SetItemGrade(new Color(1, 0.92f, 0.016f, 0.15f));
                    break;
            }
            
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
