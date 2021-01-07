using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GoblinAction : MonsterAction
{
    bool _isDamaged = false;

    [SerializeField] private Transform _baseMeleeAttackPos;
    [SerializeField] private GameObject _baseMeleeAttackPrefab;

    Collider _baseAtkCollision;


    /////////// 기본 /////////////

    /////////// 상태 관련 /////////////

    /////////// 스폰 관련 /////////////

    /////////// 대기 관련 /////////////

    protected override void UpdateMonster()
    {
        base.UpdateMonster();
    }
    protected override void IdleUpdate()
    {
        base.IdleUpdate();
        ToStirr();
    }

    protected override void DoSomethingIdleSearchFind()
    {
        if (_isDamaged) base.DoSomethingIdleSearchFind();
        // 데미지를 안 받았다면 아무것도 하지 않는다.
    }

    /////////// 두리번거리기 관련 /////////////

    protected override void StirrStart()
    {
        _navMeshAgent.isStopped = true;
        _monster.myAnimator.SetTrigger("Sleep");
    }

    protected override void StirrUpdate()
    {
        if (CheckAnimationOver("Sleep", 1.0f))
        {
            ChangeState(STATE.STATE_IDLE);
        }
    }

    protected override void StirrExit()
    {
        _navMeshAgent.isStopped = false;
        _monster.myAnimator.ResetTrigger("Sleep");
    }

    /////////// 탐색 관련 /////////////

    protected override void FindStart()
    {
        ChangeState(STATE.STATE_TRACE);
    }

    protected override void FindExit()
    {
        // 아무것도 하지 않는다.
    }

    protected override void DoReturn()
    {
        Debug.Log("RETURN DO");
        base.DoReturn();
        _isDamaged = false;
    }

    /////////// 추적 관련 /////////////

    /////////// 공격 관련 /////////////

    protected override void DoAttack()
    {
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
        if (_attackType == 0) _monster.myAnimator.SetTrigger("Attack");
        else if (_attackType == 1) _monster.myAnimator.SetTrigger("AttackSpecial");
    }

    protected override void ResetAttackAnimation()
    {
        if (_attackType == 0) _monster.myAnimator.ResetTrigger("Attack");
        else if (_attackType == 1) _monster.myAnimator.ResetTrigger("AttackSpecial");
    }

    /////////// 캐스팅 관련 /////////////

    /////////// 피격 관련 /////////////

    protected override void SetHitAnimation(bool isDeath)
    {
        if (isDeath) _monster.myAnimator.ResetTrigger("Hit");
        else
        {
            _isDamaged = true;
            _monster.myAnimator.SetTrigger("Hit");
            if (_currentState == STATE.STATE_IDLE || _currentState == STATE.STATE_STIRR) ChangeState(STATE.STATE_TRACE);
        }
    }
}
