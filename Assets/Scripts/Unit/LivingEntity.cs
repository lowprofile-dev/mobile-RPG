﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum STATE
{
    STATE_NULL, STATE_IDLE, STATE_FIND, STATE_TRACE, STATE_ATTACK, STATE_CAST, STATE_DEBUFF, STATE_KILL, STATE_DIE , STATE_STIRR , STATE_SPAWN , STATE_HIT 
}

public class LivingEntity : Unit
{
    [SerializeField] protected float _initHp; public float initHp { get { return _initHp; } set { _initHp = value; } }
    [SerializeField] protected float _initMp; public float initMp { get { return _initMp; } set { _initMp = value; } }
    [SerializeField] protected float _hp; public float Hp { get { return _hp; } set { _hp = value; } }
    [SerializeField] protected float _stemina; public float Stemina { get { return _stemina; } set { _stemina = value; } }
    [SerializeField] protected GameObject _DamageText; public GameObject DamageText { get { return _DamageText; } }

    private StateMachine _myStateMachine; public StateMachine MyStateMachine { get { return _myStateMachine; } }
    public Animator myAnimator;

    protected virtual void Start()
    {
        InitObject();
    }

    protected virtual void Update()
    {

    }
    
    // 오브젝트에서 필요한 초기화들을 실시한다.
    protected virtual void InitObject()
    {
        _hp = initHp;
        _stemina = initMp;
        gameObject.GetComponent<Collider>().enabled = true;
    }

    public virtual void Damaged(float damage)
    {
        _hp -= damage;
        ObjectPoolManager.Instance.GetObject(DamageText, transform.position, Quaternion.identity).GetComponent<DamageText>().PlayDamage(damage, true);
    }

    public virtual void UseStemina(float skillmp)
    {
        _stemina -= skillmp;
    }
}
