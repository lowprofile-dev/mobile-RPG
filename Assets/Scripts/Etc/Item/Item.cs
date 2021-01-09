using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Item : MonoBehaviour
{
    [SerializeField] int id;
    public ItemData itemData;
    ItemManager itemManager;
    //Transform itemModel;

    private void Start()
    {
        //itemModel = transform.GetChild(0);
        itemManager = ItemManager.Instance;
        LoadItemData();
    }

    [ContextMenu("Load item data")]
    public void LoadItemData()
    {
        ItemManager.Instance.SetItemData(id, out itemData);
    }

    //  COMMENT : 아이템 스크립트에서 모델링까지 불러오는 방식 (비용이 높아 폐기)
    //public void LoadItemModeling()
    //{
    //    string prefabPath;
    //    switch(itemData.itemType)
    //    {
    //        case "Armor":

    //            break;
    //        case "Bottom":

    //            break;
    //        case "Helmet":

    //            break;
    //        case "Gloves":

    //            break;
    //        case "Boot":

    //            break;
    //    }
    //    Instantiate(prefabPath, itemModel);
    //}
}
