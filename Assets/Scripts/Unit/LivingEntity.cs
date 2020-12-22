using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum STATE
{
    STATE_IDLE, STATE_FIND, STATE_TRACE, STATE_ATTACK, STATE_CAST, STATE_DEBUFF, STATE_KILL, STATE_DIE
}

public class LivingEntity : Unit
{
    [SerializeField] protected float _initHp; public float initHp { get { return _initHp; } }
    protected float _hp; public float hp { get { return _hp; } }

    public StateMachine MyStateMachine;
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
    }

    public virtual void Damaged(float damage)
    {
        _hp -= damage;
        Debug.Log(name + "이 " + damage + "만큼 피해를 입었습니다.");
    }
}
