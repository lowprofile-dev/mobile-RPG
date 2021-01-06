using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using System;

public class BossSkeletonPase2 : MonsterAction
{
    enum AttackType { JUMP_ATTACK , SHOCK_WAVE, SHOCK_WAVE2, DASH_ATTACK , LEFT_ATTACK}

    [SerializeField] private Transform _baseMeleeAttackPos;
    [SerializeField] private GameObject _baseMeleeAttackPrefab;

    AttackType attackType;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _findRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }


    protected override void DoAttack()
    {
        base.DoAttack();

        if (_attackType == 0 || _attackType == 1)
        {
            GameObject obj = ObjectPoolManager.Instance.GetObject(_baseMeleeAttackPrefab);
            obj.transform.SetParent(this.transform);
            obj.transform.position = _baseMeleeAttackPos.position;

            Attack atk = obj.GetComponent<Attack>();
            atk.SetParent(gameObject);
            atk.PlayAttackTimer(1);
        }


    }
    protected override void SetAttackAnimation()
    {
        transform.LookAt(_target.transform);

        if (_attackType == 0)
        {
            _monster.MyAnimator.SetTrigger("Attack0");
        }
        else if (_attackType == 1)
        {
            _monster.MyAnimator.SetTrigger("Attack1");
        }
        else
        {
            _monster.MyAnimator.SetTrigger("Attack2");
        }
    }
    protected override void SpawnStart()
    {
        ChangeState(STATE.STATE_IDLE);
    }

    protected override void SpawnExit()
    {
        base.SpawnExit();
    }

    protected override void CastStart()
    {

        // 플레이어가 기절상태나 넘어짐 상태면 우선 공격 모션 2개 있음.
        // if(_target.getState? == 기절) attackType = AttackType.~~~~

        int proc = UnityEngine.Random.Range(0, 100);

        if (proc <= 25)
        {
            _castTime = 1.5f;
            attackType = AttackType.JUMP_ATTACK;
        }
        else if (proc <= 50)
        {
            _castTime = 1f;
            attackType = AttackType.SHOCK_WAVE;
        }
        else if (proc <= 75)
        {
            _castTime = 0f;
            attackType = AttackType.SHOCK_WAVE2;
        }
        else
        {
            _castTime = 1.5f;
            attackType = AttackType.DASH_ATTACK;
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
            ChangeState(STATE.STATE_ATTACK);
        }

    }

    protected override void LookTarget()
    {

    }

    protected override void SetAttackType()
    {
        if (_readyCast) return;
    }


}
