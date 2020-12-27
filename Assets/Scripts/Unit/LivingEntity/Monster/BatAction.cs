using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using System;

public class BatAction : MonsterAction
{

    NavMeshAgent _navMeshAgent;
    Coroutine _attackCoroutine;
    Coroutine _castCoroutine;

    public override void InitObject()
    {
        base.InitObject();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        UpdateState();
    }

    protected override void UpdateMonster()
    {
        DeathCheck();
    }

    public override void UpdateState()
    {
        // 반드시 실행되는 업데이트 내용
        UpdateMonster();

        // 스테이트별 업데이트 내용
        switch (_currentState)
        {
            case STATE.STATE_SPAWN:
                
                break;
            case STATE.STATE_IDLE:
                Search();
                break;
            case STATE.STATE_STIRR:
                
                break;
            case STATE.STATE_FIND:
               
                break;
            case STATE.STATE_TRACE:
                Move();
                CheckLimitPlayerDistance();
                break;
            case STATE.STATE_ATTACK:
                Attack();
                break;
            case STATE.STATE_KILL:
                break;
            case STATE.STATE_DEBUFF:
                break;
            case STATE.STATE_DIE:
                break;
            default:
                break;
        }
    }

    public override void EnterState(STATE targetState)
    {
        switch (targetState)
        {
            case STATE.STATE_SPAWN:
                
                break;
            case STATE.STATE_IDLE:
                
                break;
            case STATE.STATE_STIRR:
                
                break;
            case STATE.STATE_FIND:
                FindStart();
                break;
            case STATE.STATE_TRACE:
                
                break;
            case STATE.STATE_DEBUFF:
                break;
            case STATE.STATE_ATTACK:
                AttackStart();
                break;
            case STATE.STATE_KILL:
                break;
            case STATE.STATE_DIE:
                DeathStart();
                break;
            case STATE.STATE_CAST:
                CastStart();
                break;
            default:
                break;
        }

    }

    public override void ExitState(STATE targetState)
    {
        switch (targetState)
        {
            case STATE.STATE_IDLE:
                
                break;
            case STATE.STATE_STIRR:
                
                break;
            case STATE.STATE_FIND:
                
                break;
            case STATE.STATE_TRACE:
                
                break;
            case STATE.STATE_DEBUFF:
                break;
            case STATE.STATE_ATTACK:
                AttackExit();
                break;
            case STATE.STATE_CAST:
                CastExit();
                break;
            case STATE.STATE_KILL:
                break;
            case STATE.STATE_DIE:
                break;
            default:
                break;
        }
    }
}
