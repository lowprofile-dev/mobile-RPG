using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Item : MonoBehaviour
{
    [SerializeField] int id;
    public ItemData itemData;
    public ItemManager itemManager;
    private void Start()
    {
        LoadItemData();
    }

    [ContextMenu("Load item data")]
    public void LoadItemData()
    {
        itemManager.SetItemData(id, out itemData);
    }
}
