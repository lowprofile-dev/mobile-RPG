using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipSlot : MonoBehaviour , IPointerClickHandler
{
    [SerializeField] GameObject itemIcon;
    [SerializeField] GameObject itemDetail;
    [SerializeField] GameObject itemDetailPrefab;

    bool isShowingItemInfo;
    ItemData itemData;
    WeaponData weaponData;
    ItemManager itemManager;

    private void Awake()
    {
        itemIcon = gameObject.transform.GetChild(0).gameObject;
        isShowingItemInfo = false;
        itemManager = ItemManager.Instance;
    }

    public void SetIcon(Transform model)
    {
        Texture2D texture = RuntimePreviewGenerator.GenerateModelPreview(model);
        Rect rect = new Rect(0, 0, texture.width, texture.height);
        itemIcon.GetComponent<Image>().sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
    }

    public void SetItemDetail(GameObject obj)
    {
        itemDetail = obj;
    }

    public void SetItemData(ItemData id)
    {
        itemData = id;
    }
    public void setWeaponData(WeaponData id)
    {
        weaponData = id;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //itemDetail.SetActive(true);
        Debug.Log("장비템 로드");
        itemDetail.GetComponent<ItemDetail>().LoadItemDetail(itemData);
    }
}
