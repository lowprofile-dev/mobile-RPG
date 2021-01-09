using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ShopItemSlot : MonoBehaviour
{
    [SerializeField] GameObject itemIcon;
    [SerializeField] GameObject itemQuantity;
    [SerializeField] GameObject itemGrade;
    [SerializeField] Button ItemButton;
    ItemData itemData;
    ItemManager itemManager;
    GameObject itemBag;
    GameObject ShopUI;
    int itemquantity;

    private void Awake()
    {
        itemIcon = gameObject.transform.GetChild(0).gameObject;
        itemQuantity = gameObject.transform.GetChild(1).gameObject;
        itemGrade = gameObject.transform.GetChild(2).gameObject;
        ItemButton = gameObject.GetComponent<Button>();
        itemManager = ItemManager.Instance;
        itemBag = transform.parent.parent.parent.parent.gameObject;
        ShopUI = itemBag.transform.parent.gameObject;
        ItemButton.onClick.AddListener(delegate { OnClickItemSlot(); });
    }

    private void OnClickItemSlot()
    {
        if (itemquantity == 0)
        {
            itemBag.SetActive(false);
            return;
        }
        //if (itemquantity == 1)
        //{

        //    if (itemManager.currentItemKeys.ArmorKey == itemData.id ||
        //        itemManager.currentItemKeys.BottomKey == itemData.id ||
        //        itemManager.currentItemKeys.HelmetKey == itemData.id ||
        //        itemManager.currentItemKeys.GlovesKey == itemData.id ||
        //        itemManager.currentItemKeys.BootKey == itemData.id)
        //    {
        //        itemBag.SetActive(false);
        //        return;
        //    }
        //}
        if (ShopUI == null)
            ShopUI = transform.parent.parent.parent.parent.parent.gameObject;
        ShopUI.GetComponent<ShopUIView>().shopItemCard.itemData = itemData;
        ShopUI.GetComponent<ShopUIView>().shopItemCard.itemImage.sprite = itemIcon.GetComponent<Image>().sprite;
        ShopUI.GetComponent<ShopUIView>().shopItemCard.itemPrice.text = itemData.sellprice.ToString();
        ShopUI.GetComponent<ShopUIView>().totalPrice += itemData.sellprice;
        ShopUI.GetComponent<ShopUIView>().SetTotalPrice();
        ShopUI.GetComponent<ShopUIView>().bucketList.Add(itemData);
        itemquantity--;
        SetQuantity(itemquantity);
        itemBag.SetActive(false);
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

    public void SetQuantity(int quantity)
    {
        itemQuantity.GetComponent<TextMeshProUGUI>().SetText(quantity.ToString());
        itemquantity = quantity;
    }

    public void SetItemData(ItemData id)
    {
        itemData = id;
    }
}
