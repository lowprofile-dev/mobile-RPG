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
    [SerializeField] private GameObject JumpSkillRange;
    [SerializeField] private GameObject ShokeSkillRange;
    private GameObject currentTarget;

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
          
        GameObject obj = ObjectPoolManager.Instance.GetObject(_baseMeleeAttackPrefab);
        obj.transform.SetParent(this.transform);
        obj.transform.position = _baseMeleeAttackPos.position;

        Attack atk = obj.GetComponent<Attack>();
        atk.SetParent(gameObject);
        atk.PlayAttackTimer(1);

        
        StopCoroutine(_attackCoroutine);
        _attackCoroutine = null;
        _readyCast = false;

        CameraManager.Instance.ShakeCamera(3, 1, 0.5f);
        currentTarget = _target;
        ChangeState(STATE.STATE_TRACE);


    }
    protected override void SetAttackAnimation()
    {
        
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
    private void AttackCorotineInit()
    {
        _attackCoroutine = null;
        ChangeState(STATE.STATE_TRACE);
    }

    protected override void CastStart()
    {

        // 플레이어가 기절상태나 넘어짐 상태면 우선 공격 모션 2개 있음.
        // if(_target.getState? == 기절) attackType = AttackType.~~~~

        if (_attackCoroutine != null) Invoke("AttackCorotineInit", 1.5f);

        int proc = UnityEngine.Random.Range(0, 100);

        if (proc <= 50)
        {
            _castTime = 1.5f;
            attackType = AttackType.JUMP_ATTACK;
        }
        else if (proc <= 100)
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
        _navMeshAgent.speed = _moveSpeed;
        currentTarget = _target;

    }

    protected override void AttackExit()
    {
        _monster.MyAnimator.ResetTrigger(currentAnimation);
        //if (_attackCoroutine != null)
        //{
        //    StopCoroutine(_attackCoroutine);
        //    _attackCoroutine = null;
        //}
            
    }

    public override void MoveToTarget()
    {
        
        _navMeshAgent.SetDestination(currentTarget.transform.position);

        if (Vector3.Distance(_target.transform.position, _monster.transform.position) < _attackRange)
        {
            ChangeState(STATE.STATE_CAST);            
        }

    }

    protected override IEnumerator AttackTarget()
    {
        Debug.Log("AttackCorotine");
        //CastStart();

        while (true)
        {
            yield return null;

            AttackAction();
            
            //StartCoroutine(DoAttackAction());

            yield return new WaitForSeconds(_attackSpeed);
            SetAttackAnimation();

            // 사운드 재생

            //레인지 끄기
            
            
            yield return new WaitForSeconds(_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);

            //_monster.MyAnimator.ResetTrigger(currentAnimation); // 애니메이션의 재시작 부분에 Attack이 On이 되야함.

            _navMeshAgent.speed = _moveSpeed;
            _readyCast = false;
            if (!_readyCast && ToCast()) break;
            ChangeState(STATE.STATE_TRACE);

            break;
            
        }
    }
    private void AttackAction()
    {
        switch (attackType)
        {
            case AttackType.JUMP_ATTACK:
                StartCoroutine(JumpAction());
                break;
            case AttackType.SHOCK_WAVE:
                StartCoroutine(ShokeAction());
                break;
            case AttackType.SHOCK_WAVE2:
                break;
            case AttackType.DASH_ATTACK:
                break;
            case AttackType.LEFT_ATTACK:
                break;
            default:
                break;
        }

    }
    private IEnumerator JumpAction()
    {
        _navMeshAgent.speed = _moveSpeed * 1.5f;
        GameObject range = ObjectPoolManager.Instance.GetObject(JumpSkillRange);
        range.transform.position = _target.transform.position;
        currentTarget = range;
        transform.LookAt(range.transform.position);

        yield return new WaitForSeconds(_attackSpeed);

        ObjectPoolManager.Instance.ReturnObject(range);
    }

    private IEnumerator ShokeAction()
    {

        GameObject range = ObjectPoolManager.Instance.GetObject(ShokeSkillRange);

        range.GetComponent<BossSkillRange>().setFollow();
              
        yield return new WaitForSeconds(_attackSpeed);
          

        ObjectPoolManager.Instance.ReturnObject(range);
    }


    protected override void TraceStart()
    {
        _monster.MyAnimator.SetTrigger("Walk");
        _navMeshAgent.isStopped = false;
    }

    protected override void AttackStart()
    {
        if (!_readyCast && ToCast()) return;
        else
        {
            if (_attackCoroutine == null)
                _attackCoroutine = StartCoroutine(AttackTarget());
            else
                ChangeState(STATE.STATE_IDLE);
        }
        
    }

    public override void Damaged(float dmg, bool SetAnimation = false)
    {
        base.Damaged(dmg, false);
    }

}
