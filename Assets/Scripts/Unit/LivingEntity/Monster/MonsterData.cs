﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CSVReader.Data("id")]
public class MonsterData
{
    [Header("몬스터 기본 정보")]
    public int id;
    public string name;
    public float hp;
    public float findrange;
    public float attackrange;
    public float LimitTraceRange;
    public float AttackSpeed;
    public float damage;
    public float speed;


}