using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CurrentStatus
{
    [Header("플레이어 기본 정보")]
    //향후 서버 or 멀티플레이 구축시 필요한 데이터
    //public int id;
    //public string name;
    public int level;              //숙련도의 평균
    public int exp;                     //경험치

    [Header("체력 스텟")]
    public float hp;                    //현재 체력량
    public float maxHp;                 //최대 체력
    public float hpRecovery;            //체력 회복량

    [Header("스태미너 스텟")]
    public float maxStamina;            //최대 스태미너
    public float staminaRecovery;       //스태미너 회복량

    [Header("공격 관련 스텟")]
    public float attackDamage;          //공격력
    public float attackSpeed;           //공격속도 (= 공격속도 쿨타임)
    public float attackCooldown;        //공격스킬 쿨타임 % 감소
    public float magicDamage;

    [Header("방어 및 이동 관련 스텟")]
    public float armor;                 //방어력
    public float magicResistance;       //마법 방어력
    public float tenacity;              //cc저항
    public float moveSpeed;             //이동 속도
    public float dashCooldown;          //대쉬 스킬 쿨타임 % 감소
    public float dashStamina;           //대쉬 스킬 스태미너 % 감소

    [Header("무기 숙련도")]
    public float swordLevel;            //한손검 숙련도
    public float daggerLevel;           //단검 숙련도
    public float greatSwordLevel;       //대검 숙련도
    public float bluntLevel;            //둔기 숙련도
    public float staffLevel;            //지팡이(근거리) 숙련도
    public float wandLevel;             //지팡이(원거리) 숙련도

    public CurrentStatus()
    {
        level = 1;
        exp = 0;
        hp = 150;
        maxHp = 150;
        hpRecovery = 1f;
        maxStamina = 100;
        staminaRecovery = 3f;
        attackDamage = 10;
        attackSpeed = 0.5f;
        attackCooldown = 0;
        armor = 3;
        magicResistance = 4;
        tenacity = 40;
        moveSpeed = 6;
        dashCooldown = 0;
        dashStamina = 0;
        swordLevel = 1;
        daggerLevel = 1;
        greatSwordLevel = 1;
        bluntLevel = 1;
        staffLevel = 1;
        wandLevel = 1;
    }
}
