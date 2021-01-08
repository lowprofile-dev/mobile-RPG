using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using System;

public class SkeletonAction : MonsterAction
{
    enum ATTACKSTATE {Attack1 , Attack2 , Attack3 };

    [SerializeField] GameObject[] AttackEffect;
    [SerializeField] Transform FirePoint;
    Coroutine _attackCoroutine;
    Coroutine _castCoroutine;
    Coroutine _hitCoroutine;
    private float idleCnt = 0;
    private bool panic = false;
    Vector3 spawnPostion;
    Vector3 aroundPos;
    ATTACKSTATE currentAttack;
    string AtkState;
    private float castingTime = 0f;

    
    public override void InitObject()
    {
        base.InitObject();
        panic = true;
        spawnPostion = transform.position;
    }

    private void Update()
    {
        UpdateState();
    }

    protected override void UpdateMonster()
    {
        DeathCheck();
        TargetDeathCheck();
    }

    public override void UpdateState()
    {
        // 반드시 실행되는 업데이트 내용
        UpdateMonster();

        // 스테이트별 업데이트 내용
        switch (_currentState)
        {
            case MONSTER_STATE.STATE_SPAWN:
                SpawnAction();
                break;
            case MONSTER_STATE.STATE_IDLE:
                IdleUpdate();
                break;
            case MONSTER_STATE.STATE_STIRR:
                GroundSearch();
                break;
            case MONSTER_STATE.STATE_FIND:
                FindPlayer();
                break;
            case MONSTER_STATE.STATE_TRACE:
                MoveToTarget();
                CheckLimitPlayerDistance();
                break;
            case MONSTER_STATE.STATE_ATTACK:
                AttackUpdate();
                break;
            case MONSTER_STATE.STATE_KILL:
                KillPlayer();        
                break;
            case MONSTER_STATE.STATE_DEBUFF:
                break;
            case MONSTER_STATE.STATE_DIE:
                break;
            default:
                break;
        }
    }

    public override void EnterState(MONSTER_STATE targetState)
    {
        switch (targetState)
        {
            case MONSTER_STATE.STATE_SPAWN:
                SpawnStart();
                break;
            case MONSTER_STATE.STATE_IDLE:
                IdleStart();
                break;
            case MONSTER_STATE.STATE_STIRR:
                StirrStart();
                break;
            case MONSTER_STATE.STATE_FIND:
                FindStart();
                break;
            case MONSTER_STATE.STATE_TRACE:
                TraceStart();
                break;
            case MONSTER_STATE.STATE_DEBUFF:
                break;
            case MONSTER_STATE.STATE_ATTACK:
                AttackStart();
                break;
            case MONSTER_STATE.STATE_KILL:
                KillStart();
                break;
            case MONSTER_STATE.STATE_DIE:
                DeathStart();
                break;
            case MONSTER_STATE.STATE_CAST:
                CastStart();
                break;
            default:
                break;
        }

    }

    private void KillStart()
    {
        _monster.myAnimator.SetTrigger("Laugh");
        _navMeshAgent.enabled = false;
    }

    public override void ExitState(MONSTER_STATE targetState)
    {
        switch (targetState)
        {
            case MONSTER_STATE.STATE_IDLE:
                IdleExit();
                break;
            case MONSTER_STATE.STATE_STIRR:
                StirrExit();
                break;
            case MONSTER_STATE.STATE_FIND:
                FindExit();
                break;
            case MONSTER_STATE.STATE_TRACE:
                TraceExit();
                break;
            case MONSTER_STATE.STATE_DEBUFF:
                break;
            case MONSTER_STATE.STATE_ATTACK:
                AttackExit();
                break;
            case MONSTER_STATE.STATE_CAST:
                CastExit();
                break;
            case MONSTER_STATE.STATE_KILL:
                break;
            case MONSTER_STATE.STATE_DIE:
                break;
            default:
                break;
        }
    }

    private void FindExit()
    {
        
    }

    private void StirrExit()
    {
        _navMeshAgent.isStopped = false;
    }

    private void IdleExit()
    {
    }

    private void TraceExit()
    {
        _navMeshAgent.speed = _moveSpeed;
       // _monster.MyAnimator.ResetTrigger("Walk");
    }

    /// <summary>
    /// 추적 행동
    /// </summary>
    public override void MoveToTarget()
    {
        base.MoveToTarget();
        //_navMeshAgent.isStopped = false;
        
        _navMeshAgent.SetDestination(_target.transform.position);
        // Run StepSound 재생

        // 사거리 내에 적 존재 시 발동
        if (Vector3.Distance(_target.transform.position, _monster.transform.position) < _attackRange)
        {
            ChangeState(MONSTER_STATE.STATE_ATTACK);
        }
    }

    public override void CheckLimitPlayerDistance()
    {
        base.CheckLimitPlayerDistance();
    }
    
    protected override void AttackUpdate()
    {
        base.AttackUpdate();

        if(castingTime <= 6f)
        castingTime += Time.deltaTime;

        // 타겟과의 거리가 공격 범위보다 커지면
        if (Vector3.Distance(_target.transform.position, _monster.transform.position) > _attackRange)
        {       
            ChangeState(MONSTER_STATE.STATE_IDLE);
        }
    }

    protected override void AttackStart()
    {
        _attackCoroutine = StartCoroutine(AttackTarget());
    }

    protected override void AttackExit()
    {
        StopCoroutine(_attackCoroutine);
    }

    protected override IEnumerator AttackTarget()
    {
        
        while (true)
        {
            yield return null;

            if (CanAttackState())
            {

                currentAttack = AttackPattern();

                yield return new WaitForSeconds(_monster.myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
                
                _navMeshAgent.SetDestination(_target.transform.position);
                transform.LookAt(_target.transform);
                _monster.myAnimator.SetTrigger(AtkState);
                // 사운드 재생
                Debug.Log(_monster.monsterName + "의 공격!");
                _target.GetComponent<LivingEntity>().Damaged(_monster.attackDamage);

                // 공격 행동 한다.
                //StartCoroutine(DoAttackAction());
                // 공격 애니메이션이 끝났을때 자동으로 Idle로 넘어가도록

                GameObject effect = ObjectPoolManager.Instance.GetObject(AttackEffect[(int)currentAttack]);
                effect.transform.position = FirePoint.position;
                effect.transform.LookAt(_target.transform);

                yield return new WaitForSeconds(_monster.myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);

                _monster.myAnimator.ResetTrigger(AtkState); // off가 되어있으므로 바로 돌아오진 않음.
                                                            // 애니메이션의 재시작 부분에 Attack이 On이 되야함.

                //int toCastRandomValue = UnityEngine.Random.Range(0, 100);

                //if (toCastRandomValue < _monster.castPercent)
                //{
                //    ChangeState(MONSTER_STATE.STATE_CAST);
                //}
            }
            else
            {
                ChangeState(MONSTER_STATE.STATE_IDLE);
            }
        }

    }
    
    private ATTACKSTATE AttackPattern()
    {

        if (castingTime >= 5f) {
            castingTime = 0f;
            AtkState = "Attack2";
            return ATTACKSTATE.Attack3;
        }
        int percent = UnityEngine.Random.Range(0, 100);

        if(percent <= 50)
        {
            AtkState = "Attack0";
            return ATTACKSTATE.Attack1;
        }
        else
        {
            AtkState = "Attack1";
            return ATTACKSTATE.Attack2;
        }

    }
    
    public override IEnumerator DoAttackAction()
    {
        // 아무튼 뭔가를 한다

        yield return null;
    }

    protected override void IdleUpdate()
    {
        base.IdleUpdate();

        idleCnt += Time.deltaTime;

        if (idleCnt > 3)
        {
            panic = true;
            idleCnt = 0;
            if (UnityEngine.Random.Range(1, 100) <= 30)
                ChangeState(MONSTER_STATE.STATE_STIRR);
        }
       
    }

    protected override void FindStart()
    {
        base.FindStart();

        if (panic)
        {
            _monster.myAnimator.SetTrigger("Panic");
            panic = false;
        }

        _navMeshAgent.isStopped = true;
        transform.LookAt(_target.transform.position);
        // 위에 느낌표가 뜬다.
        // 소리를 낸다.
    }
    
    public override void TargetDeathCheck()
    {
        if (_currentState != MONSTER_STATE.STATE_KILL && _target.GetComponent<LivingEntity>().Hp <= 0)
        {
            ChangeState(MONSTER_STATE.STATE_KILL);         
        }
    }

    protected override void DeathStart()
    {
        StartCoroutine(DoDeathAction());
    }

    protected override IEnumerator DoDeathAction()
    {
        // DeadSound를 재생한다.
        // 뭔가 사망 행동을 함
        _monster.myAnimator.SetTrigger("Die");

        yield return new WaitForSeconds(2);

        DestroyImmediate(this.gameObject);
    }

    protected override void IdleStart()
    {
        if (!_monster.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        _monster.myAnimator.SetTrigger("Idle");
    }

    private void SpawnAction()
    {

        if (_monster.myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            ChangeState(MONSTER_STATE.STATE_IDLE);
        }
    }

    private void FindPlayer()
    {

        if (_monster.myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            _navMeshAgent.isStopped = false;
            ChangeState(MONSTER_STATE.STATE_TRACE);
        }

    }

    private void GroundSearch()
    {
        if (_monster.myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            ChangeState(MONSTER_STATE.STATE_IDLE);
        }
    }

    private void SpawnStart()
    {
        _monster.myAnimator.SetTrigger("Spawn");
    }

    private void TraceStart()
    {
        _navMeshAgent.speed = _moveSpeed * 1.5f;
        _monster.myAnimator.SetTrigger("Walk");
    }

    private void StirrStart()
    {
        _navMeshAgent.isStopped = true;
        _monster.myAnimator.SetTrigger("Stirr");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("데미지입은 몬스터");

            if(!_monster.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            _monster.myAnimator.SetTrigger("Hit");

            _monster.Damaged(1);

            if (_hitCoroutine == null)
                _hitCoroutine = StartCoroutine(DoHitAction());
            else
                ChangeState(MONSTER_STATE.STATE_IDLE);
        }
    }
   

    private IEnumerator DoHitAction()
    {
        yield return null;
        //데미지를 입는다.

        while (true)
        {
            yield return null;

            if (_monster.myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                StopCoroutine(_hitCoroutine);
                _hitCoroutine = null;
                ChangeState(MONSTER_STATE.STATE_IDLE);
            }
        }
    }

    private void KillPlayer()
    {
        if (_monster.myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            _monster.myAnimator.ResetTrigger("Laugh");
            _monster.myAnimator.SetTrigger("Laugh");
        }

        transform.RotateAround(_target.transform.position, Vector3.up, 5 * Time.deltaTime);
        transform.LookAt(_target.transform.position);
    }
   
}
