using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum STATE
{
    STATE_IDLE, STATE_FIND, STATE_TRACE, STATE_ATTACK, STATE_CAST, STATE_DEBUFF, STATE_KILL, STATE_DIE , STATE_STIRR , STATE_SPAWN , STATE_HIT
}

public class LivingEntity : Unit
{
    [SerializeField] protected float _initHp; public float initHp { get { return _initHp; } }
    [SerializeField] protected float _initMp; public float initMp { get { return _initMp; } }
    [SerializeField] protected float _hp; public float Hp { get { return _hp; } }
    [SerializeField] protected float _mp; public float Mp { get { return _mp; } set { _mp = value; } }
    public StateMachine MyStateMachine;
    public Animator MyAnimator;

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
        _mp = initMp;
    }

    public virtual void Damaged(float damage)
    {
        _hp -= damage;
        Debug.Log(name + "이 " + damage + "만큼 피해를 입었습니다.");
    }

    public virtual void UseMp(float skillmp)
    {
        _mp -= skillmp;
    }
}
