/*
    File StatusData.cs
    class StatusData
    
    담당자 : 김기정
    부 담당자 :
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CSVReader.Data("level")]
public class StatusData : ScriptableObject
{
    [Header("플레이어 기본 정보")]
    public int level;
    public int exp;
    public int stamina;
    public float staminaRecovery;

    [Header("공격 관련 스텟")]
    public int attackDamage;
    public int magicDamage;

    [Header("방어 관련 스텟")]
    public int armor;
    public int magicResistance;
    public int maxHp;
    public float hpRecovery;

    [Header("속도 관련 스텟")]
    public float moveSpeed;
    public float attackSpeed;
    public int tenacity;
}
