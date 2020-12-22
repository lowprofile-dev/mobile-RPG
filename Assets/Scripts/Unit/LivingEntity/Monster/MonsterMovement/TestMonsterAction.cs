using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using System;

public class TestMonsterAction : MonsterAction
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
        Debug.Log("CURRENT STATE : " + _currentState);
    }

    public override void UpdateState()
    {
        // 반드시 실행되는 업데이트 내용
        UpdateMonster();

        // 스테이트별 업데이트 내용
        switch (_currentState)
        { 
            case STATE.STATE_IDLE:
                Search();
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
            case STATE.STATE_IDLE:
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

    /// <summary>
    /// 추적 행동
    /// </summary>
    public override void Move()
    {
        base.Move();
        _navMeshAgent.SetDestination(_target.transform.position);

        // Run StepSound 재생

        // 사거리 내에 적 존재 시 발동
        if (Vector3.Distance(_target.transform.position, _monster.transform.position) < _attackRange)
        {
            ChangeState(STATE.STATE_ATTACK);
        }
    }

    public override void CheckLimitPlayerDistance()
    {
        base.CheckLimitPlayerDistance();
    }

    public override void Damaged(float dmg)
    {
        base.Damaged(dmg);
        ChangeState(STATE.STATE_FIND);
    }

    public override void Attack()
    {
        base.Attack();

        // 타겟과의 거리가 공격 범위보다 커지면
        if (Vector3.Distance(_target.transform.position, _monster.transform.position) > _attackRange)
        {
            ChangeState(STATE.STATE_TRACE);
        }
    }

    public override void AttackStart()
    {
        _attackCoroutine = StartCoroutine(AttackTarget());
    }

    public override void AttackExit()
    {
        StopCoroutine(_attackCoroutine);
    }

    public override IEnumerator AttackTarget()
    {
        while (true)
        {
            yield return new WaitForSeconds(_monster.attackTime);

            // 사운드 재생
            Debug.Log(_monster.monsterName + "의 공격!");
            _target.GetComponent<LivingEntity>().Damaged(_monster.attackDamage);

            // 공격 행동 한다.
            StartCoroutine(DoAttackAction());

            // 일정 확률로 캐스팅 상태로 바꾼다.
            int toCastRandomValue = UnityEngine.Random.Range(0, 100);
            if (toCastRandomValue < _monster.castPercent)
            {
                ChangeState(STATE.STATE_CAST);
            }
        }
    }

    public override void CastStart()
    {
        _castCoroutine = StartCoroutine(DoCastingAction());
    }
    
    public override void Cast()
    {
        base.Cast();

        // 타겟과의 거리가 공격 범위보다 커지면
        if (Vector3.Distance(_target.transform.position, _monster.transform.position) > _attackRange)
        {
            ChangeState(STATE.STATE_TRACE);
        }
    }

    public override IEnumerator DoCastingAction()
    {
        Debug.Log("캐스팅 중입니다...");
        // 사운드 재생
        yield return new WaitForSeconds(2);
        ChangeState(STATE.STATE_ATTACK);
    }

    public override void CastExit()
    {
        StopCoroutine(_castCoroutine);
    }

    public override IEnumerator DoAttackAction()
    {
        // 아무튼 뭔가를 한다
        yield return null;
    }

    public override void Search()
    {
        base.Search();

        if(Vector3.Distance(_target.transform.position, _monster.transform.position) < _findRange)
        {
            ChangeState(STATE.STATE_FIND);
        }
    }

    public override void FindStart()
    {
        base.FindStart();

        // 위에 느낌표가 뜬다.
        // 소리를 낸다.

        // 인식 행동 진행
        StartCoroutine(DoFindAction());
    }
    
    public override IEnumerator DoFindAction()
    {
        yield return new WaitForSeconds(1);
        ChangeState(STATE.STATE_TRACE);
        yield return null;
    }

    public override void DeathCheck()
    {
        if (_currentState != STATE.STATE_DIE && NoHPCheck())
        {
            ChangeState(STATE.STATE_DIE);
        }
    }

    public override void DeathStart()
    {
        StartCoroutine(DoDeathAction());
    }

    public override IEnumerator DoDeathAction()
    {
        // DeadSound를 재생한다.
        // 뭔가 사망 행동을 함
        Debug.Log("사망했다! 사망행동으로 폭탄 떨구고 가기~");
        yield return new WaitForSeconds(2);

        DestroyImmediate(this.gameObject);
    }

}
