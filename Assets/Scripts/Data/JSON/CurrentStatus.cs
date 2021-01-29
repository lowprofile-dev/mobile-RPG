/*
    File CurrentStatus.cs
    class CurrentStatus
    
    담당자 : 김기정
    부 담당자 :
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CurrentStatus : ICloneable
{
    [Header("플레이어 기본 정보")]
    //향후 서버 or 멀티플레이 구축시 필요한 데이터
    //public int id;
    //public string name;

    [Header("체력 스텟")]
    public float maxHp;                 // 최대 체력
    public float hpRecovery;            // 체력 회복량

    [Header("스태미너 스텟")]
    public float maxStamina;            // 최대 스태미너
    public float staminaRecovery;       // 스태미너 회복량

    [Header("공격 관련 스텟")]
    public float attackDamage;          // 공격력
    public float attackCooldown;        // 공격스킬 쿨타임 % 감소
    public float minDamagePer;          // 최종 데미지 랜덤 수치
    public float maxDamagePer;          // 최종 데미지 랜덤 수치
    public float criticalDamage;        // 크리티컬 데미지 (Default 1)
    public float criticalPercent;       // 크리티컬 확률 (Default 15)

    [Header("방어 및 이동 관련 스텟")]
    public float armor;                 //방어력
    public float rigidresistance;       //경직 cc 저항
    public float stunresistance;        //스턴 cc 저항
    public float fallresistance;        //넘어짐 cc 저항
    public float moveSpeed;             //이동 속도
    public float maxSpeed;
    public float dashCooldown;          //대쉬 스킬 쿨타임 % 감소
    public float dashStamina;           //대쉬 스킬 스태미너 % 감소

    public CurrentStatus()
    {
        maxHp = 150;
        hpRecovery = 1f;
        maxStamina = 10;
        staminaRecovery = 0.5f;
        attackDamage = 250;
        attackCooldown = 0;
        armor = 0;
        rigidresistance = 0;
        stunresistance = 0;
        fallresistance = 0;
        moveSpeed = 8;
        maxSpeed = moveSpeed;
        dashCooldown = 1;
        dashStamina = 2;
        minDamagePer = 0.8f;
        maxDamagePer = 1.2f;
        criticalDamage = 1.5f;
        criticalPercent = 15;
    }

    /// <summary>
    /// 깊은 복사
    /// </summary>
    /// <returns></returns>
    public object Clone()
    {
        CurrentStatus copy = this.MemberwiseClone() as CurrentStatus;
        return copy;
    }
}
