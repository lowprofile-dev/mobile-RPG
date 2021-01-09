using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;

public class ShopItemCard : MonoBehaviour
{
    [SerializeField] Button cardBtn;
    [SerializeField] GameObject itemBag;

    public Image itemImage;
    public TextMeshProUGUI itemPrice;
    public ItemData itemData;

    GameObject ShopUI;

    private void Awake()
    {
        cardBtn.onClick.AddListener(delegate { OnClickItemCard(); });
        ShopUI = transform.parent.parent.gameObject;
    }

    private void OnClickItemCard()
    {
        ShopUI.GetComponent<ShopUIView>().shopItemCard = this;
        itemBag.transform.position = transform.position;
        itemBag.SetActive(true);
    }
}
