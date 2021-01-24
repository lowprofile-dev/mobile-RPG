﻿////////////////////////////////////////////////////
/*
    File SkeletonAction.cs
    class SkeletonAction
    
    담당자 : 안영훈

*/
////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class SkeletonAction : MonsterAction
{
    [SerializeField] private Transform _baseMeleeAttackPos;
    [SerializeField] private GameObject _baseMeleeAttackPrefab;
    string currentAnimation;

    protected override void DoAttack()
    {
        //base.DoAttack();
        
        GameObject obj = ObjectPoolManager.Instance.GetObject(_baseMeleeAttackPrefab);
        obj.transform.SetParent(this.transform);
        obj.transform.position = _baseMeleeAttackPos.position;

        Attack atk = obj.GetComponent<Attack>();
        atk.SetParent(gameObject);
        atk.PlayAttackTimer(0.3f);
       
    }
    protected override IEnumerator AttackTarget()
    {

        while (true)
        {
            yield return null;

            if (CanAttackState())
            {

                yield return new WaitForSeconds(_attackSpeed - _monster.myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
                SetAttackType();
                SetAttackAnimation();

                LookTarget();

                StartCoroutine(DoAttackAction());

                yield return new WaitForSeconds(_monster.myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime/2);

                _monster.myAnimator.SetTrigger("Idle");

                _readyCast = false;
                if (!_readyCast && ToCast()) break;
            }

            else
            {
                ChangeState(MONSTER_STATE.STATE_TRACE);
                break;
            }
        }
    }

    protected override void SetAttackAnimation()
    {
        transform.LookAt(_target.transform);
        _navMeshAgent.isStopped = true;

        if (_attackType == 0)
        {
            _monster.myAnimator.SetTrigger("Attack0");
            currentAnimation = "Attack0";
        }
        else if(_attackType == 1)
        {
            _monster.myAnimator.SetTrigger("Attack1");
            currentAnimation = "Attack1";
        }
        else
        {
            _monster.myAnimator.SetTrigger("Attack2");
            currentAnimation = "Attack2";
        }
    }
    protected override void SpawnStart()
    {
        ChangeState(MONSTER_STATE.STATE_IDLE);
    }

    protected override void SpawnExit()
    {
        base.SpawnExit();
    }

    protected override void DoCastingAction()
    {
        _cntCastTime += Time.deltaTime;
        _bar.CastUpdate();

        if (_cntCastTime >= _castTime)
        {
            _cntCastTime = 0;
            _readyCast = true;
            _attackType = 2;
            ChangeState(MONSTER_STATE.STATE_ATTACK);
        }

    }
    protected override void AttackStart()
    {
        base.AttackStart();
       // _monster.myAnimator.SetTrigger("Idle");
    }
    protected override void LookTarget()
    {
       
    }

    protected override void SetAttackType()
    {
        if (_readyCast) return;

        int proc = UnityEngine.Random.Range(0, 2);

        if (proc == 0)
        {
            _attackType = 0;
        }
        else
        {
            _attackType = 1;
        }
    }

    protected override void AttackExit()
    {
        _navMeshAgent.isStopped = false;
        _monster.myAnimator.ResetTrigger(currentAnimation);
        if (_attackCoroutine != null) StopCoroutine(_attackCoroutine);
        
    }
    protected override void IdleStart()
    {
        ChangeState(MONSTER_STATE.STATE_TRACE);
    }

    protected override void KillStart()
    {
        _monster.myAnimator.SetTrigger("Laugh");
    }
}