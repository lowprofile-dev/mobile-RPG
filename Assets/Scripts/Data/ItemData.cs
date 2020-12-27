using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CSVReader.Data("id")]
public class ItemData
{
    public int id;
    public string itemName;
    public enum ItemType { Coin, Ammo, Potion, Weapon };
    public ItemType itemType;
    public string iconImg;
    public float itemPrice;
    public float value; //Coin : 코인의 값, Ammo : 데미지, Potion : 회복량. Weapon : 데미지
}
