using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] GameObject itemIcon;
    [SerializeField] GameObject itemDetail;
    [SerializeField] GameObject itemDetailPrefab;
    [SerializeField] Transform pos;
    bool isShowingItemInfo;
    ItemData itemData;
    //WeaponData weaponData;
    ItemManager itemManager;
    Sprite icon;
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
        icon = itemIcon.GetComponent<Image>().sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
    }

    public void SetItemDetail(GameObject obj)
    {
        itemDetail = obj;
    }

    public void SetItemData(ItemData id)
    {
        itemData = id;
    }
    //public void setWeaponData(WeaponData id)
    //{
    //    weaponData = id;
    //}

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    itemDetail.SetActive(true);
    //    //Debug.Log("장비템 로드");
    //    //itemDetail.GetComponent<ItemDetail>().LoadItemDetail(itemData);
    //    itemDetail.GetComponent<EquipDetail>().LoadData(itemData, icon);
    //    itemDetail.transform.position = pos.position;
    //}

    public void OnPointerDown(PointerEventData eventData)
    {
        itemDetail.SetActive(true);
        itemDetail.GetComponent<EquipDetail>().LoadData(itemData, icon);
        itemDetail.transform.position = pos.position;
        SoundManager.Instance.PlayEffect(SoundType.UI, "UI/ClickLightBase2", 0.9f);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        itemDetail.SetActive(false);
    }

    //public void OnMouseEnter()
    //{
    //    itemDetail.SetActive(true);
    //    //Debug.Log("장비템 로드");
    //    //itemDetail.GetComponent<ItemDetail>().LoadItemDetail(itemData);
    //    itemDetail.GetComponent<EquipDetail>().LoadData(itemData, icon);
    //    itemDetail.transform.position = transform.position + new Vector3(80f,0f,0f);
    //}
    //
    //public void OnMouseExit()
    //{
    //    itemDetail.SetActive(false);
    //}
}
