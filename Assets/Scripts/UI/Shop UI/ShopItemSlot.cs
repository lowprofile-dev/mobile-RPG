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
    public int shopItemCardIndex;
    ItemData itemData;
    ItemManager itemManager;
    GameObject itemBag;
    GameObject ShopUI;

    private void Awake()
    {
        itemIcon = gameObject.transform.GetChild(0).gameObject;
        itemQuantity = gameObject.transform.GetChild(1).gameObject;
        itemGrade = gameObject.transform.GetChild(2).gameObject;
        ItemButton = gameObject.GetComponent<Button>();
        itemManager = ItemManager.Instance;
        itemBag = transform.parent.parent.parent.parent.gameObject;
        ShopUI = transform.parent.parent.parent.parent.parent.gameObject;
        shopItemCardIndex = ShopUI.GetComponent<ShopUIView>().currentCardIndex;
        //ItemButton.onClick.AddListener(delegate { OnClickItemSlot(); });
    }

    public void OnClickItemSlot()
    {
        if (itemManager == null) itemManager = ItemManager.Instance;
        itemManager.AddToCart(shopItemCardIndex, this.itemData);
        Debug.Log("아이템 ID: " + this.itemData.id);
        ShopUI.GetComponent<ShopUIView>().LoadPlayerInventory();
        ShopUI.GetComponent<ShopUIView>().LoadItemCards();
        ShopUI.GetComponent<ShopUIView>().SetTotalPrice();

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
    }

    public void SetItemData(ItemData id)
    {
        itemData = id;
    }
}
