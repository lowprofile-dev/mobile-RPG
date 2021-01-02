using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CSVReader.Data("Lv")]
public class StatusData
{
    [Header("플레이어 기본 정보")]
    public int level;
    public int exp;

    [Header("공격 관련 스텟")]
    public int attackDamage;
    public int magicDamage;

    [Header("방어 관련 스텟")]
    public int armor;
    public int magicResistance;
    public int maxHp;

    [Header("속도 관련 스텟")]
    public float moveSpeed;
    public float attackSpeed;
    public int tenacity;
}
