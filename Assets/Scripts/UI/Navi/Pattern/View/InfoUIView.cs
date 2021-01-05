using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class InfoUIView : View
{
    [SerializeField] Button ExitBtn;

    [SerializeField] GameObject Hat_Slot;
    [SerializeField] GameObject Armor_Slot;
    [SerializeField] GameObject Glove_Slot;
    [SerializeField] GameObject Pant_Slot;
    [SerializeField] GameObject Shoes_Slot;

    [SerializeField] GameObject itemDetail;
    public Transform[] iconList;
    ItemManager itemManager;
    // Start is called before the first frame update

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

    private void OnEnable()
    {
        LoadCurrentEquip();
    }

    private void Awake()
    {
        itemManager = ItemManager.Instance;
        ExitBtn.onClick.AddListener(delegate { OnClick(); });
    }

    private void OnClick()
    {
        UINaviationManager.Instance.PopToNav(name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadCurrentEquip()
    {
        //Hat_Slot.GetComponent<EquipSlot>().SetIcon(ItemManager.Instance.currentItemKeys.HelmetKey)

        Debug.Log(itemManager.currentItemKeys.HelmetKey);
        Debug.Log(itemManager.currentItemKeys.ArmorKey);
        Debug.Log(itemManager.currentItemKeys.GlovesKey);
        Debug.Log(itemManager.currentItemKeys.BottomKey);
        Debug.Log(itemManager.currentItemKeys.BootKey);

        Hat_Slot.GetComponent<EquipSlot>().SetItemData(itemManager.itemDictionary[itemManager.currentItemKeys.HelmetKey]);
        Hat_Slot.GetComponent<EquipSlot>().SetItemDetail(itemDetail);

        Armor_Slot.GetComponent<EquipSlot>().SetItemData(itemManager.itemDictionary[itemManager.currentItemKeys.ArmorKey]);
        Armor_Slot.GetComponent<EquipSlot>().SetItemDetail(itemDetail);

        Glove_Slot.GetComponent<EquipSlot>().SetItemData(itemManager.itemDictionary[itemManager.currentItemKeys.GlovesKey]);
        Glove_Slot.GetComponent<EquipSlot>().SetItemDetail(itemDetail);

        Pant_Slot.GetComponent<EquipSlot>().SetItemData(itemManager.itemDictionary[itemManager.currentItemKeys.BottomKey]);
        Pant_Slot.GetComponent<EquipSlot>().SetItemDetail(itemDetail);

        Shoes_Slot.GetComponent<EquipSlot>().SetItemData(itemManager.itemDictionary[itemManager.currentItemKeys.BootKey]);
        Shoes_Slot.GetComponent<EquipSlot>().SetItemDetail(itemDetail);

    }
}
