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
    [SerializeField] private GameObject SkillRange;
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

        switch (attackType)
        {
            case AttackType.JUMP_ATTACK:
                _monster.MyAnimator.SetTrigger("Attack0");
                currentAnimation = "Attack0";
                break;
            case AttackType.SHOCK_WAVE:
                _monster.MyAnimator.SetTrigger("Attack1");
                currentAnimation = "Attack1";
                break;
            case AttackType.SHOCK_WAVE2:
                _monster.MyAnimator.SetTrigger("Attack2");
                currentAnimation = "Attack2";
                break;
            case AttackType.DASH_ATTACK:
                _monster.MyAnimator.SetTrigger("Attack3");
                currentAnimation = "Attack3";
                break;
            case AttackType.LEFT_ATTACK:
                break;
            default:
                break;
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

    public override void InitState()
    {
        _currentState = STATE.STATE_NULL;
        ChangeState(STATE.STATE_IDLE);
        _navMeshAgent.enabled = true;
    }

    protected override void AttackExit()
    {
        _monster.MyAnimator.ResetTrigger(currentAnimation);
        if (_attackCoroutine != null) StopCoroutine(_attackCoroutine);
    }

    public override void MoveToTarget()
    {
        _navMeshAgent.SetDestination(_target.transform.position);

        ChangeState(STATE.STATE_ATTACK);

    }

    protected override IEnumerator AttackTarget()
    {
        while (true)
        {
            yield return null;

            
                yield return new WaitForSeconds(_attackSpeed - _monster.MyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
                SetAttackType();
                SetAttackAnimation();

                LookTarget();

                // 사운드 재생

                StartCoroutine(DoAttackAction());

                yield return new WaitForSeconds(_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);

                _monster.MyAnimator.ResetTrigger(currentAnimation); // 애니메이션의 재시작 부분에 Attack이 On이 되야함.

                _readyCast = false;
                if (!_readyCast && ToCast()) break;
           
                ChangeState(STATE.STATE_TRACE);
                break;
            
        }
    }
    public override IEnumerator DoAttackAction()
    {
        yield return null;


    }
}
