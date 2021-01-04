using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CSVReader.Data("id")]
public class WeaponData
{
    [Header("무기 기본 정보")]
    public int id;
    public int weaponModelIndex;
    public string weaponType;

    [Header("무기 공격 관련 스텟")]
    public float magicDamage;
    public float attackDamage;
    public float attackSpeed;

    public float skillACoef;
    public float skillBCoef;
    public float skillCCoef;

    public float skillACool;
    public float skillBCool;
    public float skillCCool;
}
