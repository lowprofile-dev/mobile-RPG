/*
    File Item.cs
    class Item
    
    담당자 : 김기정
    부 담당자 :
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Item : MonoBehaviour
{
    public int id;
    public ItemData itemData;
    public bool isDrop;
    ItemManager itemManager;

    private void Start()
    {
        itemManager = ItemManager.Instance;
        LoadItemData();
    }

    private void OnEnable()
    {
        Invoke("DisableItem", 10f);
    }

    [ContextMenu("Load item data")]
    public void LoadItemData()
    {
        ItemManager.Instance.SetItemData(id, out itemData);
    }

    private void DisableItem()
    {
        //gameObject.SetActive(false);
        ObjectPoolManager.Instance.ReturnObject(gameObject);
    }

    /// <summary>
    /// 플레이어와 충돌시 인벤토리에 아이템 추가
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            itemManager.AddItem(this);
            SystemPanel.instance.SetText(itemData.itemName + " 아이템 획득!");
            SystemPanel.instance.FadeOutStart();
            gameObject.SetActive(false);
        }
    }
}
