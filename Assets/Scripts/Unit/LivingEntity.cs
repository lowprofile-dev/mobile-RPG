using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum STATE
{
    STATE_IDLE, STATE_MOVE, STATE_ATTACK, STATE_DIE
}

public class LivingEntity : Unit
{
    protected float _initHp;
    protected float _hp;

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
}
