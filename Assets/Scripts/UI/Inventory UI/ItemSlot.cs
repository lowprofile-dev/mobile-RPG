using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] GameObject itemIcon;
    [SerializeField] GameObject itemQuantity;
    [SerializeField] GameObject itemDetail;
    [SerializeField] GameObject itemDetailPrefab;
    float btnClickTime;
    bool isShowingItemInfo;
    ItemData itemData;

    private void Awake()
    {
        itemIcon = gameObject.transform.GetChild(0).gameObject;
        itemQuantity = gameObject.transform.GetChild(1).gameObject;
        btnClickTime = 0.0f;
        isShowingItemInfo = false;
    }

    public void SetIcon(Transform model)
    {
        //Instantiate(RuntimePreviewGenerator.GenerateModelPreview(model), itemIcon);
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

    public void OnPointerDown(PointerEventData eventData)
    {
        //btnClickTime += Time.deltaTime;
        //if (btnClickTime > 1.5f)
        //{
        //    isShowingItemInfo = true;
        //}
        if (!isShowingItemInfo)
        {
            itemDetail = Instantiate(itemDetailPrefab, transform.parent.parent);
            itemDetail.GetComponent<ItemDetail>().LoadItemDetail(itemData);
        }
        isShowingItemInfo = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isShowingItemInfo = false;
        Destroy(itemDetail);
    }
}
