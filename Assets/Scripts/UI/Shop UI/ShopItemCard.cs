/*
    File ShopItemCard.cs
    class ShopItemCard
    
    담당자 : 김기정
    부 담당자 : 
 */

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

    public ShopUIView shopUIView;

    public Image itemImage;
    public TextMeshProUGUI itemPrice;
    public ItemData itemData;
    public Sprite defaultimage;
    public int index;

    GameObject ShopUI;

    private void Awake()
    {
        cardBtn.onClick.AddListener(delegate { OnClickItemCard(); });
    }

    public void InitShopItemCard()
    {
        itemImage = transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>();
        itemPrice = transform.GetChild(1).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        itemBag = shopUIView.itemBag;
    }

    private void OnClickItemCard()
    {
        itemBag.transform.position = transform.position;
        shopUIView.currentCardIndex = index;
        itemBag.SetActive(true);
    }

    public void ResetItemCard()
    {
        itemImage.sprite = defaultimage;
    }

    public void SetIcon(Transform model)
    {
        Texture2D texture = RuntimePreviewGenerator.GenerateModelPreview(model);
        Rect rect = new Rect(0, 0, texture.width, texture.height);
        itemImage.GetComponent<Image>().sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
    }
}
