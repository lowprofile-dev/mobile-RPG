﻿////////////////////////////////////////////////////
/*
    File BossSkeletonWarrior.cs
    class BossSkeletonWarrior
    
    담당자 : 안영훈
    부 담당자 : 
*/
////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using System;

public class BossSkeletonWarrior : MonsterAction
{
    enum AttackType { JUMP_ATTACK , SHOCK_WAVE, SHOCK_WAVE2, DASH_ATTACK , LEFT_ATTACK}

    [SerializeField] private Transform _baseMeleeAttackPos;
    [SerializeField] private GameObject _baseMeleeAttackPrefab;

    AttackType attackType;
    [SerializeField] private GameObject JumpSkillRange;
    [SerializeField] private GameObject ShokeSkillRange;
    [SerializeField] private GameObject ShokeSkillRange2;

    [SerializeField] private GameObject ShokeSkillEffect2;
    [SerializeField] private GameObject ShokeSkillEffect1;
    [SerializeField] private GameObject JumpSkillEffect;
    private GameObject currentTarget;
    [SerializeField] private Transform _ShokeWavePoint;

    private float velocity;
    private float angle;

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
        if (_attackCoroutine != null) StopCoroutine(_attackCoroutine);
        //_attackCoroutine = null;
        _readyCast = false;

        MakeEffect();
        _navMeshAgent.acceleration = 8f;
        CameraManager.Instance.ShakeCamera(3, 1, 0.5f);
        currentTarget = _target;
        ChangeState(MONSTER_STATE.STATE_TRACE);

    }

    private void MakeEffect()
    {

        switch (attackType)
        {
            case AttackType.JUMP_ATTACK:
                JumpAttackEffect();
                break;
            case AttackType.SHOCK_WAVE:
                ShockWave1Effect();
                break;
            case AttackType.SHOCK_WAVE2:
                ShockWave2Effect();
                break;
            case AttackType.DASH_ATTACK:
                ShockWave1Effect();
                break;
            case AttackType.LEFT_ATTACK:
                break;
            default:
                break;
        }
    }
    private void ShockWave1Effect()
    {
        BossAttack atk = ObjectPoolManager.Instance.GetObject(ShokeSkillEffect1).GetComponent<BossAttack>();
        atk.SetParent(gameObject, _ShokeWavePoint);
        atk.PlayAttackTimer(0.5f);
        atk.OnLoad(gameObject , _ShokeWavePoint.gameObject);

    }

    private void JumpAttackEffect()
    {
        
        BossAttack atk = ObjectPoolManager.Instance.GetObject(JumpSkillEffect).GetComponent<BossAttack>();
        atk.SetParent(gameObject);
        atk.PlayAttackTimer(0.5f);
        atk.OnLoad(currentTarget, currentTarget);
    }

    private void ShockWave2Effect()
    {

        BossAttack atk = ObjectPoolManager.Instance.GetObject(ShokeSkillEffect2).GetComponent<BossAttack>();
        atk.SetParent(gameObject,_ShokeWavePoint);
        atk.PlayAttackTimer(0.5f);
        atk.OnLoad(_ShokeWavePoint.gameObject, currentTarget);

    }

    private void DoShokeWave()
    {

        MakeEffect();

        if (_attackCoroutine != null) StopCoroutine(_attackCoroutine);
        _attackCoroutine = null;
        _readyCast = false;

        _navMeshAgent.speed = _monster.speed;
        _navMeshAgent.acceleration = 8f;
        CameraManager.Instance.ShakeCamera(3, 1, 0.5f);
        currentTarget = _target;
        ChangeState(MONSTER_STATE.STATE_TRACE);
    }
    protected void ComboAttack()
    {
        //애니메이터 호출용
        _monster.myAnimator.SetTrigger("Combo1");
        currentAnimation = "Combo1";

    }
    protected override void SetAttackAnimation()
    {
        
        switch (attackType)
        {
            case AttackType.JUMP_ATTACK:
                _monster.myAnimator.SetTrigger("Attack0");
                currentAnimation = "Attack0";
                break;
            case AttackType.SHOCK_WAVE:
                _monster.myAnimator.SetTrigger("Attack1");
                currentAnimation = "Attack1";
                break;
            case AttackType.SHOCK_WAVE2:
                _monster.myAnimator.SetTrigger("Attack2");
                currentAnimation = "Attack2";
                break;
            case AttackType.DASH_ATTACK:
                _monster.myAnimator.SetTrigger("Combo1");
                currentAnimation = "Combo1";
                break;
            case AttackType.LEFT_ATTACK:
                break;
            default:
                break;
        }
       
    }
    protected override void SpawnStart()
    {
        UILoaderManager.Instance.NameText.text = _monster.monsterName.ToString();
        _monster.myAnimator.SetTrigger("Spawn");
        _bar.gameObject.SetActive(false);

    }
    protected override void SpawnUpdate()
    {
        //base.SpawnUpdate();
       if(_monster.myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            _monster.myAnimator.SetTrigger("Walk");
            StartCoroutine(TimeDelay());
            ChangeState(MONSTER_STATE.STATE_IDLE);
        }

    }
    private IEnumerator TimeDelay()
    {
        _navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(3f);
        _navMeshAgent.isStopped = false;
    }

    protected override void SpawnExit()
    {
        base.SpawnExit();
        //_monster.myAnimator.SetTrigger("Walk");
        //ChangeState(MONSTER_STATE.STATE_IDLE);
    }
    private void AttackCorotineInit()
    {
        _attackCoroutine = null;
        ChangeState(MONSTER_STATE.STATE_TRACE);
    }

    protected override void CastStart()
    {

        // 플레이어가 기절상태나 넘어짐 상태면 우선 공격 모션 2개 있음.
        // if(_target.getState? == 기절) attackType = AttackType.~~~~

        //if (_attackCoroutine != null) Invoke("AttackCorotineInit", 1.5f);

        if(Vector3.Distance(transform.position , _target.transform.position) <= _navMeshAgent.stoppingDistance)
        {
            _monster.myAnimator.SetTrigger("Idle");
        }
        else
        {
        _monster.myAnimator.SetTrigger("Walk");
        }
       
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

   //     Debug.Log("캐스팅" + attackType.ToString());
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

    protected override void LookTarget()
    {

    }

    protected override void SetAttackType()
    {
        if (_readyCast) return;
    }

    public override void InitState()
    {
        _currentState = MONSTER_STATE.STATE_NULL;
        ChangeState(MONSTER_STATE.STATE_SPAWN);
        _navMeshAgent.enabled = true;
        _navMeshAgent.speed = _monster.speed;
        currentTarget = _target;

    }

    protected override void AttackExit()
    {
        _navMeshAgent.acceleration = 8f;
    }

    public override void MoveToTarget()
    {
        _navMeshAgent.SetDestination(currentTarget.transform.position);

        if (Vector3.Distance(_target.transform.position, _monster.transform.position) < _attackRange)
        {
            ChangeState(MONSTER_STATE.STATE_CAST);            
        }

    }

    protected override IEnumerator AttackTarget()
    {

        while (true)
        {
            yield return null;

            AttackAction();
            
            yield return new WaitForSeconds(_attackSpeed);
            SetAttackAnimation();
            
            // 사운드 재생

            yield return new WaitForSeconds(_monster.myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);

            _readyCast = false;
            //if (!_readyCast && ToCast()) break;
            ChangeState(MONSTER_STATE.STATE_TRACE);

            break;
            
        }
    }

    private void AttackAction()
    {
        if (attackType != AttackType.DASH_ATTACK)
            _monster.myAnimator.SetTrigger("Walk");

        _navMeshAgent.SetDestination(_target.transform.position);

        switch (attackType)
        {
            case AttackType.JUMP_ATTACK:
                StartCoroutine(JumpAction());
                break;
            case AttackType.SHOCK_WAVE:
                StartCoroutine(ShokeAction());
                break;
            case AttackType.SHOCK_WAVE2:
                StartCoroutine(ShokeAction2());
                break;
            case AttackType.DASH_ATTACK:
                StartCoroutine(DashAction());
                break;
            case AttackType.LEFT_ATTACK:
                break;
            default:
                break;
        }

    }

    private IEnumerator DashAction()
    {
        _navMeshAgent.stoppingDistance = 0f;
        currentTarget = _target;
        _navMeshAgent.SetDestination(_target.transform.position);
        _navMeshAgent.isStopped = false;
        _navMeshAgent.speed = _monster.speed * 1.5f;
        _navMeshAgent.acceleration = 250f;
        transform.LookAt(_target.transform.position);

        _monster.myAnimator.SetTrigger("Attack3");
        currentAnimation = "Attack3";

        GameObject obj = ObjectPoolManager.Instance.GetObject(_baseMeleeAttackPrefab);
        obj.transform.SetParent(this.transform);
        obj.transform.position = _baseMeleeAttackPos.position;

        Attack atk = obj.GetComponent<Attack>();
        atk.SetParent(gameObject);
        atk.PlayAttackTimer(1);

        yield return new WaitForSeconds(_monster.myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        _navMeshAgent.acceleration = 8f;
        _navMeshAgent.speed = _monster.speed;
        _navMeshAgent.stoppingDistance = 3f;
    }

    private IEnumerator JumpAction()
    {
        yield return null;
        _navMeshAgent.acceleration = 25f;
        GameObject range = ObjectPoolManager.Instance.GetObject(JumpSkillRange);
        range.GetComponent<BossSkillRange>().RemovedRange(gameObject , _attackSpeed);
        range.transform.position = new Vector3(_target.transform.position.x, transform.position.y, _target.transform.position.z);
        currentTarget = range;
        _navMeshAgent.SetDestination(range.transform.position);
        transform.LookAt(range.transform.position);
    }

    private IEnumerator ShokeAction()
    {       
        yield return null;

        _navMeshAgent.SetDestination(_target.transform.position);

        GameObject range = ObjectPoolManager.Instance.GetObject(ShokeSkillRange);
        range.GetComponent<BossSkillRange>().RemovedRange(gameObject, _attackSpeed);
        range.GetComponent<BossSkillRange>().setFollow();

    }

    private IEnumerator ShokeAction2()
    {
        yield return null;
        //_navMeshAgent.isStopped = true;
        _navMeshAgent.speed = _monster.speed / 2f;
        _navMeshAgent.SetDestination(_target.transform.position);
        transform.LookAt(_target.transform.position);

        GameObject range = ObjectPoolManager.Instance.GetObject(ShokeSkillRange2);
        range.GetComponent<BossSkillRangeFill>().RemovedRange(gameObject, _target, _attackSpeed);
        range.GetComponent<BossSkillRangeFill>().setFollow();

    }


    protected override void TraceStart()
    {
        if(_attackCoroutine != null) StopCoroutine(_attackCoroutine);
        _attackCoroutine = null;
        _monster.myAnimator.ResetTrigger("Walk");       

        if (Vector3.Distance(transform.position, _target.transform.position) >= _navMeshAgent.stoppingDistance)
        {
            _monster.myAnimator.SetTrigger("Idle");
        }
        else
        {
            _monster.myAnimator.SetTrigger("Walk");
        }

        _navMeshAgent.speed = _monster.speed * 2f;
        _navMeshAgent.isStopped = false;
    }
    protected override void TraceUpdate()
    {
        MoveToTarget();
    }
    protected override void AttackStart()
    {
        if (!_readyCast && ToCast()) return;
        else
        {
            if (_attackCoroutine == null)
                _attackCoroutine = StartCoroutine(AttackTarget());
            //else
            //    ChangeState(MONSTER_STATE.STATE_IDLE);
        }
        
    }

    public override void Damaged(float dmg, bool SetAnimation = false)
    {
        if (MasteryManager.Instance.currentMastery.currentMasteryChoices[5] == 1)
        {
            base.Damaged(dmg * 1.2f, false);

        }
        else
        {
            base.Damaged(dmg, false);
        }
    }

    protected override void AttackUpdate() { }

    protected override void RigidExit()
    {
        //base.RigidExit();
        //_attackCoroutine = null;
    }
    protected override void StunExit()
    {
        //base.StunExit();
        //_attackCoroutine = null;

    }
    protected override void FallExit()
    {
        //base.FallExit();
        //_attackCoroutine = null;
    }

    protected override void FallStart()
    {
        GameObject txt = ObjectPoolManager.Instance.GetObject(_monster.DamageText);
        txt.transform.SetParent(transform);
        txt.transform.localPosition = Vector3.zero;
        txt.transform.rotation = Quaternion.identity;
        txt.GetComponent<DamageText>().PlayText("넘어짐 면역!", "monster");
    }

    protected override void RigidStart()
    {        
    }

    protected override void StunStart()
    {
        //base.StunStart();
    }
    protected override void IdleStart()
    {
        //ChangeState(MONSTER_STATE.STATE_TRACE);
    }

    //protected override void IdleUpdate()
    //{

    //}
    //protected override void IdleExit()
    //{

    //}
    protected override void KillStart()
    {
        _monster.myAnimator.SetTrigger("Laugh");
    }

    protected override void DeathStart()
    {
        TalkManager.Instance.SetQuestCondition(1, monster.id, 1);
        _navMeshAgent.isStopped = true;
        base.DeathStart();
    }

    protected override void DeathUpdate()
    {
        base.DeathUpdate();
    }

    protected override void DeathExit()
    {
        base.DeathExit();
    }
}