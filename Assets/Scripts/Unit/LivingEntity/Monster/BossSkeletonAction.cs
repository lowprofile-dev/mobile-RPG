using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class BossSkeletonAction : MonsterAction
{
    [SerializeField] private Transform _baseMeleeAttackPos;
    [SerializeField] private GameObject _baseMeleeAttackPrefab;
    [SerializeField] private GameObject _bossSkeletonPase2;
    string currentAnimation;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _findRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }

    protected override void DoAttack()
    {
        //base.DoAttack();
        
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
            currentAnimation = "Attack0";
        }
        else if(_attackType == 1)
        {
            _monster.MyAnimator.SetTrigger("Attack1");
            currentAnimation = "Attack1";
        }
        else
        {
            _monster.MyAnimator.SetTrigger("Attack2");
            currentAnimation = "Attack2";
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

    protected override void DoCastingAction()
    {
        _cntCastTime += Time.deltaTime;
        _bar.CastUpdate();

        if (_cntCastTime >= _castTime)
        {
            _cntCastTime = 0;
            _readyCast = true;
            _attackType = 2;
            ChangeState(STATE.STATE_ATTACK);
        }

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

    protected override bool DeathCheck()
    {
        return _monster.Hp <= _monster.initHp / 2;
    }

    protected override IEnumerator DoDeathAction()
    {
        yield return null;
 
        GameObject pase2 = ObjectPoolManager.Instance.GetObject(_bossSkeletonPase2);
        pase2.transform.position = transform.localPosition;
        pase2.transform.LookAt(_target.transform.position);

        Debug.Log(SceneManager.GetActiveScene().name);
        ObjectPoolManager.Instance.ReturnObject(gameObject);
    }

    protected override void AttackExit()
    {
        _monster.MyAnimator.ResetTrigger(currentAnimation);
        if (_attackCoroutine != null) StopCoroutine(_attackCoroutine);
    }
}
