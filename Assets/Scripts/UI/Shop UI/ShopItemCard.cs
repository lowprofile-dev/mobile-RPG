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
    public Sprite defaultimage;
    public int index;

    GameObject ShopUI;

    private void Awake()
    {
        itemImage = transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>();
        itemPrice = transform.GetChild(1).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        itemBag = transform.parent.parent.GetChild(8).gameObject;
        cardBtn.onClick.AddListener(delegate { OnClickItemCard(); });
    }

    private void OnClickItemCard()
    {
        itemBag.transform.position = transform.position;
        itemBag.transform.parent.gameObject.GetComponent<ShopUIView>().currentCardIndex = index;
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
