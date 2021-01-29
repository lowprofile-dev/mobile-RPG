/*
    File ItemData.cs
    class ItemData
    
    담당자 : 김기정
    부 담당자 :
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum ItemType { Armor = 0, Bottom, Helmet, Gloves, Boot, Weapon }

[System.Serializable]
[CSVReader.Data("id")]
public class ItemData
{
    [Header("아이템 기본 정보")]
    public int id;
    public string itemType;
    public string itemName;
    public string itemDescription;
    public int itemIndex;
    public int itemgrade;

    [Header("체력 계수")]
    public float hp;                    //최대 체력 증가
    public float hpIncreaseRate;        //체력 % 증가
    public float hpRecovery;            //체력 회복량

    [Header("스태미너 계수")]
    public float stamina;               //최대 스태미너 증가
    public float staminaRecovery;       //스태미너 회복량

    [Header("공격 관련 계수")]
    public float attackDamage;          //공격력 증가
    public float attackCooldown;        //공격스킬 쿨타임 % 감소

    [Header("방어 및 이동 관련 계수")]
    public float armor;                 //방어력 증가
    public float moveSpeed;             //이동 속도 % 증가
    public float dashCooldown;          //대쉬 스킬 쿨타임 % 감소
    public float dashStamina;           //대쉬 스킬 스태미너 % 감소
    public float rigidresistance;       //경직 cc 저항
    public float stunresistance;        //스턴 cc 저항
    public float fallresistance;        //넘어짐 cc 저항
    public float buyprice;              //구매가
    public float sellprice;             //판매가
}
