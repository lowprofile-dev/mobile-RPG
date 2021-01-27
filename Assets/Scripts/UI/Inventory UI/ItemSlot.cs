/*
    File ItemSlot.cs
    class ItemSlot
    
    담당자 : 김기정
    부 담당자 : 
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ItemSlot : MonoBehaviour, IPointerDownHandler/*, IPointerUpHandler*/
{
    [SerializeField] GameObject itemIcon;
    [SerializeField] GameObject itemQuantity;
    [SerializeField] GameObject itemDetail;
    [SerializeField] GameObject itemDetailPrefab;
    [SerializeField] GameObject itemDetailNew;
    [SerializeField] GameObject itemGrade;
    [SerializeField] GameObject isEquipped;
    bool isShowingItemInfo;
    ItemData itemData;
    ItemManager itemManager;

    private void Awake()
    {
        itemIcon = gameObject.transform.GetChild(0).gameObject;
        itemQuantity = gameObject.transform.GetChild(1).gameObject;
        itemGrade = gameObject.transform.GetChild(2).gameObject;
        isShowingItemInfo = false;
        itemManager = ItemManager.Instance;
    }

    private void Update()
    {
        UpdateEquippedStatus();
    }

    public void SetItemGrade(Color color)
    {
        itemGrade.GetComponent<Image>().color = color;
    }

    public void SetIcon(Transform model)
    {
        Texture2D texture = RuntimePreviewGenerator.GenerateModelPreview(model);
        Rect rect = new Rect(0, 0, texture.width, texture.height);
        itemIcon.GetComponent<Image>().sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
    }

    public void SetIcon(Sprite sprite)
    {
        itemIcon.GetComponent<Image>().sprite = sprite;
    }

    public void SetItemDetail(GameObject obj)
    {
        itemDetail = obj;
    }

    public void SetItemDetailNew(GameObject obj)
    {
        itemDetailNew = obj;
    }

    public void SetQuantity(int quantity)
    {
        itemQuantity.GetComponent<TextMeshProUGUI>().SetText(quantity.ToString());
    }

    public void SetItemData(ItemData id)
    {
        itemData = id;
    }

    private void UpdateEquippedStatus()
    {
        isEquipped.SetActive(false);
        switch (itemData.itemType)
        {
            case "Armor":
                if (itemManager.currentItemKeys.ArmorKey == itemData.id)
                    isEquipped.SetActive(true);
                break;
            case "Bottom":
                if (itemManager.currentItemKeys.BottomKey == itemData.id)
                    isEquipped.SetActive(true);
                break;
            case "Helmet":
                if (itemManager.currentItemKeys.HelmetKey == itemData.id)
                    isEquipped.SetActive(true);
                break;
            case "Gloves":
                if (itemManager.currentItemKeys.GlovesKey == itemData.id)
                    isEquipped.SetActive(true);
                break;
            case "Boot":
                if (itemManager.currentItemKeys.BootKey == itemData.id)
                    isEquipped.SetActive(true);
                break;
        }
    }

    private void EquipItem()
    {
        if (itemData == null)
            return;
        itemManager.SetItemToPlayer(itemData);
        SoundManager.Instance.PlayEffect(SoundType.UI, "UI/ItemEquip2", 0.9f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.clickCount == 2)
        {
            EquipItem();
        }

        itemDetailNew.GetComponent<ItemDetailNew>().LoadItemBasicInfo(itemData);
        itemDetailNew.GetComponent<ItemDetailNew>().LoadItemDetail(itemData);
        itemDetailNew.GetComponent<ItemDetailNew>().SetIcon(itemIcon.GetComponent<Image>().sprite);
        SoundManager.Instance.PlayEffect(SoundType.UI, "UI/ClickLightBase2", 0.9f);
    }
}
