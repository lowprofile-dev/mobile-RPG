using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Armor, Bottom, Helmet, Gloves, Boot }

public class Item : MonoBehaviour
{
    public int id;
    public ItemType type;
    public string itemName;
    public string itemDescription;
}
