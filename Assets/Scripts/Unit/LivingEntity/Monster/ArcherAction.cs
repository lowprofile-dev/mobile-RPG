﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAction : MonsterAction
{
    bool canPanic;
    enum ARCHERATTACKTYPE { ATTACK, WHACK, SHOT, RAPID_SHOT};
    ARCHERATTACKTYPE atktype;

    [SerializeField] private Transform _baseRangeAttackPos;
    [SerializeField] private GameObject _baseRangeAttackPrefab;

    [SerializeField] private Transform _baseMeleeAttackPos;
    [SerializeField] private GameObject _baseMeleeAttackPrefab;

    Collider _baseAtkCollision;
    /////////// 탐색 관련 /////////////
    public override void InitObject()
    {
        base.InitObject();
        canPanic = true;
    }

    protected override void FindStart()
    {
        base.FindStart();

        if (canPanic)
        {
            _monster.myAnimator.SetTrigger("Panic");
        }
    }

    protected override bool CheckFindAnimationOver()
    {
        if (canPanic) return CheckAnimationOver("Panic", 1.0f);
        else return true;
    }

    protected override void FindPlayer()
    {
        _navMeshAgent.isStopped = false;
        ChangeState(MONSTER_STATE.STATE_TRACE);
        // 사거리 내에 적 존재 시 발동
    }

    private void CheckPlayerWithinRange()
    {
        if (Vector3.Distance(_target.transform.position, _monster.transform.position) < _attackRange)
        {
            ChangeState(MONSTER_STATE.STATE_ATTACK);
        }
    }

    protected override void FindExit()
    {
        base.FindExit();
        canPanic = false;
    }

    protected override void DoReturn()
    {
        base.DoReturn();
        canPanic = true;
    }

    protected override void DoAttack()
    {
        base.LookTarget();
        if (atktype == ARCHERATTACKTYPE.WHACK)
        {
            GameObject obj = ObjectPoolManager.Instance.GetObject(_baseMeleeAttackPrefab);
            obj.transform.SetParent(this.transform);
            obj.transform.position = _baseMeleeAttackPos.position;
            obj.transform.LookAt(_target.transform);

            Attack atk = obj.GetComponent<Attack>();
            atk.SetParent(gameObject);
            atk.PlayAttackTimer(0.3f);
        }
        else
        {
            GameObject obj = ObjectPoolManager.Instance.GetObject(_baseRangeAttackPrefab, _baseRangeAttackPos.position, _baseRangeAttackPos.rotation);
            obj.transform.SetParent(this.transform);
            //obj.transform.position = _baseRangeAttackPos.position;
            obj.transform.LookAt(_target.transform);
            obj.GetComponent<Arrow>().Launch();

            Attack atk = obj.GetComponent<Attack>();
            atk.SetParent(gameObject);
            atk.PlayAttackTimer(5f);
        }

    }

    protected override void CastStart()
    {
        int proc = Random.Range(0, 100);

        if (_distance < 3)
        {
            atktype = ARCHERATTACKTYPE.WHACK;
        }
        else
        {
            if (proc <= 50)
            {
                atktype = ARCHERATTACKTYPE.SHOT;
            }
            else
            {
                atktype = ARCHERATTACKTYPE.RAPID_SHOT;
            }
        }
    }
    protected override void DoCastingAction()
    {
        _cntCastTime += Time.deltaTime;
        _bar.CastUpdate();

        if (_cntCastTime >= _castTime)
        {
            _cntCastTime = 0;
            _readyCast = true;
            ChangeState(MONSTER_STATE.STATE_ATTACK);
        }
    }
    protected override void CastExit()
    {
        base.CastExit();
    }

    protected override void AttackStart()
    {
        int proc = Random.Range(0, 100);

        if (_distance < 3)
        {
            atktype = ARCHERATTACKTYPE.WHACK;
        }
        else if (_distance < 7)
        {
            if (proc <= 50)
            {
                atktype = ARCHERATTACKTYPE.ATTACK;
            }
            else if (proc <= 80)
            {
                atktype = ARCHERATTACKTYPE.SHOT;
            }
            else if (proc <= 100)
            {
                atktype = ARCHERATTACKTYPE.RAPID_SHOT;
            }
        }
        base.AttackStart();
    }

    protected override void AttackExit()
    {
        if (_attackCoroutine != null) StopCoroutine(_attackCoroutine);
    }
    protected override IEnumerator AttackTarget()
    {
        while (true)
        {
            yield return null;

            if (CanAttackState())
            {

                yield return new WaitForSeconds(_attackSpeed - _monster.myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);

                SetAttackAnimation();

                // 사운드 재생

                StartCoroutine(DoAttackAction());

                yield return new WaitForSeconds(_monster.myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime / 2);

                _readyCast = false;
                if (!_readyCast && ToCast()) break;
            }

        }
    }

    protected override void SetAttackType()
    {
        if (_readyCast) return;

    }

    protected override void SetAttackAnimation()
    {
        switch (atktype)
        {
            case ARCHERATTACKTYPE.ATTACK:
                _monster.myAnimator.SetTrigger("Attack1");
                break;
            case ARCHERATTACKTYPE.RAPID_SHOT:
                _monster.myAnimator.SetTrigger("Attack3");
                break;
            case ARCHERATTACKTYPE.SHOT:
                _monster.myAnimator.SetTrigger("Attack2");
                break;
            case ARCHERATTACKTYPE.WHACK:
                _monster.myAnimator.SetTrigger("Attack0");
                break;
            default:
                break;
        }
    }



    protected override void IdleStart()
    {
        base.IdleStart();
    }
    protected override void IdleUpdate()
    {
        base.IdleUpdate();
    }
    protected override void IdleExit()
    {
        base.IdleExit();
    }

    protected override IEnumerator SpawnDissolve()
    {
        yield return null;
        ChangeState(MONSTER_STATE.STATE_IDLE);
    }

    protected override void TraceStart()
    {
        base.TraceStart();
    }
    protected override void TraceUpdate()
    {
        base.TraceUpdate();
    }

    protected override void KillStart()
    {
        _monster.myAnimator.SetTrigger("Laugh");
    }
}