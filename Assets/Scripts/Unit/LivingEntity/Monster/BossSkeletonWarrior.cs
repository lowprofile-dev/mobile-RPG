////////////////////////////////////////////////////
/*
    File BossSkeletonWarrior.cs
    class BossSkeletonWarrior
    
    담당자 : 안영훈
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

    AttackType attackType;
    [Header("스킬 이펙트 , 범위")]
    [SerializeField] private GameObject JumpSkillRange; //점프 스킬 경고선 범위
    [SerializeField] private GameObject ShokeSkillRange; //가르기 스킬 경고선 범위
    [SerializeField] private GameObject ShokeSkillRange2; //레이저 스킬 경고선 범위
    [SerializeField] private GameObject JumpSkillEffect;   //점프 스킬 이펙트
    [SerializeField] private GameObject ShokeSkillEffect1; //가르기 스킬 이펙트
    [SerializeField] private GameObject ShokeSkillEffect2; //레이저 스킬 이펙트

    private GameObject currentTarget; // 현재 타겟
    [SerializeField] private Transform _ShokeWavePoint; //레이저 나가는 pos

    protected override void DoAttack() // 공격 애니메이션 지점에서 공격이 ON됨
    {
        if (_attackCoroutine != null) StopCoroutine(_attackCoroutine);
        _readyCast = false;

        MakeEffect();
        _navMeshAgent.acceleration = 8f;
        CameraManager.Instance.ShakeCamera(3, 1, 0.5f);
        currentTarget = _target;
        ChangeState(MONSTER_STATE.STATE_TRACE);

    }

    private void MakeEffect() // 각 공격마다 나올 이펙트 선정
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
    private void ShockWave1Effect() //가르기 공격 이펙트
    {
        BossAttack atk = ObjectPoolManager.Instance.GetObject(ShokeSkillEffect1).GetComponent<BossAttack>();
        atk.SetParent(gameObject, _ShokeWavePoint);
        atk.PlayAttackTimer(0.5f);
        atk.OnLoad(gameObject , _ShokeWavePoint.gameObject);
    }


    /// <summary>
    /// 점프 어택 시 사운드 재생 (애니메이션 이벤트로 호출)
    /// </summary>
    private void JumpAttackSound()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/BossSkeleton/ExplosionAttack", 0.4f);
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/BossSkeleton/Attack" + UnityEngine.Random.Range(1,4), 1f);
    }

    private void JumpAttackEffect() // 내려찍기 스킬 이펙트
    {
        BossAttack atk = ObjectPoolManager.Instance.GetObject(JumpSkillEffect).GetComponent<BossAttack>();
        atk.SetParent(gameObject);
        atk.PlayAttackTimer(0.5f);
        atk.OnLoad(currentTarget, currentTarget);
    }

    /// <summary>
    /// 쇼크 어택 시 사운드 재생 (애니메이션 이벤트로 호출)
    /// </summary>
    private void Shock2AttackSound()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/BossSkeleton/LaserAttackElectro", 0.5f);
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/BossSkeleton/LaserAttackWhoosh", 0.5f);
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/BossSkeleton/Attack" + UnityEngine.Random.Range(1, 4), 1f);
    }

    /// <summary>
    /// 걷는 소리 (애니메이션 이벤트로 호출)
    /// </summary>
    private void WalkSound()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Monster Big Footstep " + UnityEngine.Random.Range(1, 4), 1f);
    }

    private void ShockWave2Effect() // 레이저 패턴 이펙트
    {
        BossAttack atk = ObjectPoolManager.Instance.GetObject(ShokeSkillEffect2).GetComponent<BossAttack>();
        atk.SetParent(gameObject,_ShokeWavePoint);
        atk.PlayAttackTimer(0.5f);
        atk.OnLoad(_ShokeWavePoint.gameObject, currentTarget);
    }

    /// <summary>
    /// 쇼크 어택 시 사운드 재생 (애니메이션 이벤트로 호출)
    /// </summary>
    private void ShockAttackSound()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/BossSkeleton/BigSlashWater", 0.5f);
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/BossSkeleton/BigSlashWhoosh", 0.5f);
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/BossSkeleton/Attack" + UnityEngine.Random.Range(1,4), 1f);
    }

    private void DoShokeWave() // 쇼크 스킬 발동
    {
        MakeEffect();

        if (_attackCoroutine != null) StopCoroutine(_attackCoroutine);
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
    }

    protected override void SetAttackAnimation() // 공격 애니메이션 선정
    {       
        switch (attackType)
        {
            case AttackType.JUMP_ATTACK:
                _monster.myAnimator.SetTrigger("Attack0");
                break;
            case AttackType.SHOCK_WAVE:
                _monster.myAnimator.SetTrigger("Attack1");
                break;
            case AttackType.SHOCK_WAVE2:
                _monster.myAnimator.SetTrigger("Attack2");
                break;
            case AttackType.DASH_ATTACK:
                _monster.myAnimator.SetTrigger("Combo1");
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
       if(_monster.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Spawn") && _monster.myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            _monster.myAnimator.SetTrigger("Idle");
            _navMeshAgent.isStopped = true;
            Invoke("TimeDelay" , 5f);
            ChangeState(MONSTER_STATE.STATE_IDLE);
        }

    }
    private void TimeDelay()
    {              
        _navMeshAgent.isStopped = false;
        ChangeState(MONSTER_STATE.STATE_TRACE);

    }

    protected override void SpawnExit()
    {
        base.SpawnExit();
    }

    private void AttackCorotineInit()
    {
        _attackCoroutine = null;
        ChangeState(MONSTER_STATE.STATE_TRACE);
    }

    protected override void CastStart() 
    {

        if(Vector3.Distance(transform.position , _target.transform.position) <= _navMeshAgent.stoppingDistance)
        {
            _monster.myAnimator.SetTrigger("Idle");
        }
        else
        {
        _monster.myAnimator.SetTrigger("Walk");
        }
       
        //캐스팅을 시작할 때 공격 패턴을 정하고 그에 따른 공격타입과 캐스팅 시간을 정해준다.
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
            _castTime = 1f;
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
            ChangeState(MONSTER_STATE.STATE_ATTACK);
        }

    }

    protected override void LookTarget() { }

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

    protected override IEnumerator AttackTarget() // 공격 액션
    {     
       yield return null;

       AttackAction();
       
       yield return new WaitForSeconds(_attackSpeed);
       SetAttackAnimation();
       
       // 사운드 재생

       yield return new WaitForSeconds(_monster.myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);

       _readyCast = false;

       ChangeState(MONSTER_STATE.STATE_TRACE);        
    }
    /// <summary>
    /// 공격 패턴 행동 관련
    /// </summary>
    private void AttackAction() // 공격 패턴 선정
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

    private IEnumerator DashAction() // 대쉬 후 가르기 패턴 동작
    {
        _navMeshAgent.stoppingDistance = 0f;
        currentTarget = _target;
        _navMeshAgent.SetDestination(_target.transform.position);
        _navMeshAgent.isStopped = false;
        _navMeshAgent.speed = _monster.speed * 1.5f;
        _navMeshAgent.acceleration = 250f;
        transform.LookAt(_target.transform.position);

        _monster.myAnimator.SetTrigger("Attack3");

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

    private IEnumerator JumpAction() // 점프 패턴 동작
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

    private IEnumerator ShokeAction() // 가르기 패턴 동작
    {       
        yield return null;

        _navMeshAgent.SetDestination(_target.transform.position);

        GameObject range = ObjectPoolManager.Instance.GetObject(ShokeSkillRange);
        range.GetComponent<BossSkillRange>().RemovedRange(gameObject, _attackSpeed);
        range.GetComponent<BossSkillRange>().setFollow();

    }

    private IEnumerator ShokeAction2() // 레이저 패턴 동작
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
    /// <summary>
    /// CC 관련
    /// </summary>
    protected override void FallStart()
    {
        GameObject txt = ObjectPoolManager.Instance.GetObject(_monster.DamageText);
        txt.transform.SetParent(transform);
        txt.transform.localPosition = Vector3.zero;
        txt.transform.rotation = Quaternion.identity;
        txt.GetComponent<DamageText>().PlayText("CC 면역!", "monster");
    }  
    protected override void RigidStart() { }
    protected override void StunStart() { }
    protected override void IdleStart() { }
    protected override void RigidExit() { }
    protected override void StunExit() { }
    protected override void FallExit() { }

    protected override void KillStart()
    {
        StopAllCoroutines();
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

    /// <summary>
    /// 죽었을 때 나오는 사운드
    /// </summary>
    public override void DeathSound()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/BossSkeleton/Die", 1f);
    }
}
