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
    [SerializeField] GameObject itemDetailNew;

    ItemManager itemManager;
    List<GameObject> itemSlots;
    public Transform[] iconList;

    public override void UIStart()
    {
        base.UIStart();
        SoundManager.Instance.PlayEffect(SoundType.UI, "UI/OpenInventory", 0.9f);
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

    protected override void OnEnable()
    {
        base.OnEnable();
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
            try
            {
                ItemData itemData = itemManager.itemDictionary[item.Key];
                GameObject slot = Instantiate(itemSlot, content);
                slot.GetComponent<ItemSlot>().SetIcon(iconList[item.Key - 1]);
                slot.GetComponent<ItemSlot>().SetQuantity(item.Value);
                slot.GetComponent<ItemSlot>().SetItemData(itemManager.itemDictionary[item.Key]);
                slot.GetComponent<ItemSlot>().SetItemDetail(itemDetail);
                slot.GetComponent<ItemSlot>().SetItemDetailNew(itemDetailNew);
                itemSlots.Add(slot);
                switch (itemData.itemgrade)
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
            catch
            {
                if(!itemManager.playerInventory.ContainsKey(item.Key))
                {
                    itemManager.playerInventory.Remove(item.Key);
                }
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
        Player.Instance.FaceCam.InitFaceCam(Player.Instance.playerAvater);
        SoundManager.Instance.PlayEffect(SoundType.UI, "UI/ClickLightBase2", 1.0f);
    }
}
