using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using System;

public class BossSkeletonAction : MonsterAction
{
    enum ATTACKSTATE {Attack1 , Attack2 , Attack3 };

    [SerializeField] GameObject[] AttackEffect;
    [SerializeField] Transform FirePoint;
    Coroutine _attackCoroutine;
    Coroutine _castCoroutine;
    Coroutine _hitCoroutine;
    private bool panic = false;
    Vector3 spawnPostion;
    Vector3 aroundPos;
    ATTACKSTATE currentAttack;
    string AtkState;
    private float attackCoolTime;
    private bool canAttack = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _findRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
   
    public override void InitObject()
    {
        base.InitObject();
        panic = true;
        _navMeshAgent.stoppingDistance = _attackRange-1;
        spawnPostion = transform.position;
        attackCoolTime = _attackSpeed;
        StartCoroutine(AttackCoolCal());
        StartCoroutine(FSM());
    }
    private IEnumerator FSM()
    {
        yield return null;
        while (true)
        {
            yield return StartCoroutine(_currentState.ToString());
        }
    }

    private IEnumerator AttackCoolCal()
    {
        while (true)
        {
            yield return null;
            if (!canAttack)
            {
                attackCoolTime -= Time.deltaTime;
                if(attackCoolTime <= 0)
                {
                    attackCoolTime = _attackSpeed;
                    canAttack = true;
                }
            }
        }
    }
    private IEnumerator STATE_HIT()
    {
        yield return null;
        //데미지를 입는다.

        while (true)
        {
            yield return null;

            if (_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                _currentState = STATE.STATE_IDLE;
            }
        }
    }
    private IEnumerator STATE_KILL()
    {
        yield return null;
    }

    private IEnumerator STATE_IDLE()
    {
        yield return null;
        if (!_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            _monster.MyAnimator.SetTrigger("Idle");
        }

        if (CanAttackState())
        {
            if (canAttack)
            {
                //transform.LookAt(_target.transform.position);
                _currentState = STATE.STATE_ATTACK;
            }
            else
            {
                _currentState = STATE.STATE_IDLE;
                
            }
        }
        else if(_distance < _findRange)
        {
            _currentState = STATE.STATE_TRACE;
        }
    }

    private IEnumerator STATE_ATTACK()
    {
        yield return null;

        currentAttack = AttackPattern();
        //_navMeshAgent.stoppingDistance = 2f;
        //_navMeshAgent.isStopped = true;

        yield return new WaitForSeconds(0.5f);

        //_navMeshAgent.isStopped = false;
        //_navMeshAgent.speed = 30f;
        canAttack = false;

        if (!_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).IsName(AtkState))
        {
            _monster.MyAnimator.SetTrigger(AtkState);
        }

        Debug.Log(_monster.monsterName + "의 공격!");
        _target.GetComponent<LivingEntity>().Damaged(_monster.attackDamage);

        GameObject effect = ObjectPoolManager.Instance.GetObject(AttackEffect[(int)currentAttack]);
        effect.transform.position = FirePoint.position;
        effect.transform.LookAt(_target.transform);

        yield return new WaitForSeconds(_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);

        _navMeshAgent.speed = _speed;
        //_navMeshAgent.stoppingDistance = _attackRange;
        _currentState = STATE.STATE_IDLE;
    }

    private IEnumerator STATE_TRACE()
    {
        yield return null;

        if (!_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            _monster.MyAnimator.SetTrigger("Walk");
        }

        if(CanAttackState() && canAttack)
        {
            _currentState = STATE.STATE_ATTACK;
        }
        else if (_distance >= _findRange)
        {
            //_navMeshAgent.SetDestination(spawnPostion);
            _currentState = STATE.STATE_IDLE;
        }
        else
        {
            _navMeshAgent.SetDestination(_target.transform.position);
        }
    }
    private void Update()
    {
        UpdateState();
    }

    protected override void UpdateMonster()
    {
        DeathCheck();
        PlayerDeathCheck();
    }

    public override void UpdateState()
    {
        // 반드시 실행되는 업데이트 내용
        UpdateMonster();

        // 스테이트별 업데이트 내용
        switch (_currentState)
        {
            case STATE.STATE_SPAWN:
                //SpawnAction();
                break;
            case STATE.STATE_IDLE:
                //Search();
                break;
            case STATE.STATE_TRACE:
                //Move();
                //CheckLimitPlayerDistance();
                break;
            case STATE.STATE_ATTACK:
                //Attack();
                break;
            case STATE.STATE_KILL:
                KillPlayer();
                break;
            case STATE.STATE_DEBUFF:
                break;
            case STATE.STATE_DIE:
                Die();
                break;
            default:
                break;
        }
    }

    public override void Die()
    {
        base.Die();

        if (!_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            _monster.MyAnimator.SetTrigger("Die");
        }

        if (_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && _monster.MyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            ObjectPoolManager.Instance.ReturnObject(gameObject);
        }
    }

    //public override void EnterState(STATE targetState)
    //{
    //    switch (targetState)
    //    {
    //        case STATE.STATE_SPAWN:
    //            SpawnStart();
    //            break;
    //        case STATE.STATE_IDLE:
    //            IdleStart();
    //            break;
    //        case STATE.STATE_TRACE:
    //            TraceStart();
    //            break;
    //        case STATE.STATE_DEBUFF:
    //            break;
    //        case STATE.STATE_ATTACK:
    //            AttackStart();
    //            break;
    //        case STATE.STATE_KILL:
    //            KillStart();
    //            break;
    //        case STATE.STATE_DIE:
    //            DeathStart();
    //            break;
    //        case STATE.STATE_CAST:
    //            CastStart();
    //            break;
    //        default:
    //            break;
    //    }

    //}

    //public override void ExitState(STATE targetState)
    //{
    //    switch (targetState)
    //    {
    //        case STATE.STATE_IDLE:
    //            IdleExit();
    //            break;
    //        case STATE.STATE_TRACE:
    //            TraceExit();
    //            break;
    //        case STATE.STATE_DEBUFF:
    //            break;
    //        case STATE.STATE_ATTACK:
    //            AttackExit();
    //            break;
    //        case STATE.STATE_CAST:
    //            CastExit();
    //            break;
    //        case STATE.STATE_KILL:
    //            break;
    //        case STATE.STATE_DIE:
    //            break;
    //        default:
    //            break;
    //    }
    //}

    //private void KillStart()
    //{
    //    _monster.MyAnimator.SetTrigger("Laugh");
    //    _navMeshAgent.enabled = false;
    //}

    //private void IdleExit()
    //{
    //}

    //private void TraceExit()
    //{
    //    _navMeshAgent.speed = _speed;
    //   // _monster.MyAnimator.ResetTrigger("Walk");
    //}

    //public override void Move()
    //{
    //    base.Move();
    //    //_navMeshAgent.isStopped = false;
    //    if (!_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
    //    {
    //        _monster.MyAnimator.SetTrigger("Walk");
    //    }

    //    if (CanAttackState() && canAttack)
    //    {
    //        ChangeState(STATE.STATE_ATTACK);
    //    }
    //    else if (Vector3.Distance(transform.position , _target.transform.position ) > _findRange)
    //    {
    //        ChangeState(STATE.STATE_IDLE);
    //    }
    //    else
    //    {
    //        Debug.Log("하시레");
    //        _navMeshAgent.SetDestination(_target.transform.position);
    //    }


    //    //_navMeshAgent.SetDestination(_target.transform.position);
    //    // Run StepSound 재생
    //}

    //public override void CheckLimitPlayerDistance()
    //{
    //    base.CheckLimitPlayerDistance();
    //}

    public override void Damaged(float dmg)
    {
        base.Damaged(dmg);
        //ChangeState(STATE.STATE_FIND);
    }

    //public override void Attack()
    //{
    //    base.Attack();

    //    if(castingTime <= 6f)
    //    castingTime += Time.deltaTime;

    //    // 타겟과의 거리가 공격 범위보다 커지면
    //    //if (Vector3.Distance(_target.transform.position, _monster.transform.position) > _attackRange)
    //    //{       
    //    //    ChangeState(STATE.STATE_IDLE);
    //    //}
    //}

    //public override void AttackStart()
    //{
    //    //if(_attackCoroutine == null)
    //    _attackCoroutine = StartCoroutine(AttackTarget());
    //}

    //public override void AttackExit()
    //{
    //    StopCoroutine(_attackCoroutine);
    //    //_attackCoroutine = null;
    //}

    //public override IEnumerator AttackTarget()
    //{
        
    //    while (true)
    //    {
    //        currentAttack = AttackPattern();
    //        _navMeshAgent.stoppingDistance = 0;
    //        _navMeshAgent.isStopped = true;
    //        _navMeshAgent.SetDestination(_target.transform.position);

    //        yield return new WaitForSeconds(0.5f);
    //        _navMeshAgent.isStopped = false;
    //        _navMeshAgent.speed = 100f;

    //        if (!_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).IsName(AtkState))
    //        {
    //            _monster.MyAnimator.SetTrigger(AtkState);
    //        }
    //            // 사운드 재생
    //            Debug.Log(_monster.monsterName + "의 공격!");
    //            _target.GetComponent<LivingEntity>().Damaged(_monster.attackDamage);

    //            // 공격 행동 한다.
    //            //StartCoroutine(DoAttackAction());
    //            // 공격 애니메이션이 끝났을때 자동으로 Idle로 넘어가도록

    //            GameObject effect = ObjectPoolManager.Instance.GetObject(AttackEffect[(int)currentAttack]);
    //            effect.transform.position = FirePoint.position;
    //            effect.transform.LookAt(_target.transform);

    //           _navMeshAgent.speed = _speed;
    //           _navMeshAgent.stoppingDistance = _attackRange - 1;
    //        // _monster.MyAnimator.ResetTrigger(AtkState); // off가 되어있으므로 바로 돌아오진 않음.
    //        // 애니메이션의 재시작 부분에 Attack이 On이 되야함.


    //        yield return new WaitForSeconds(_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
    //        canAttack = false;
    //        ChangeState(STATE.STATE_IDLE);
                
    //            // 일정 확률로 캐스팅 상태로 바꾼다.
    //            //int toCastRandomValue = UnityEngine.Random.Range(0, 100);

    //            //if (toCastRandomValue < _monster.castPercent)
    //            //{
    //            //    ChangeState(STATE.STATE_CAST);
    //            //}
            
    //    }

    //}

    private ATTACKSTATE AttackPattern()
    {

        if (_monster.Mp >= _monster.initMp) {
            _monster.Mp = 0f;
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

    //public override void CastStart()
    //{
    //    _castCoroutine = StartCoroutine(DoCastingAction());
    //}

    //public override void Cast()
    //{
    //    base.Cast();

    //    // 타겟과의 거리가 공격 범위보다 커지면
    //    if (Vector3.Distance(_target.transform.position, _monster.transform.position) > _attackRange)
    //    {
    //        ChangeState(STATE.STATE_TRACE);
    //    }
    //}

    //public override IEnumerator DoCastingAction()
    //{
    //    Debug.Log("캐스팅 중입니다...");
    //    // 사운드 재생
    //    yield return new WaitForSeconds(2);
    //    ChangeState(STATE.STATE_ATTACK);
    //}

    //public override void CastExit()
    //{
    //    StopCoroutine(_castCoroutine);
    //}

    //public override IEnumerator DoAttackAction()
    //{
    //    // 아무튼 뭔가를 한다

    //    yield return null;
    //}

    //public override void Search()
    //{
    //    base.Search();

    //    if (!_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
    //    {
    //        _monster.MyAnimator.SetTrigger("Idle");
    //    }

    //    if (CanAttackState())
    //    {
    //        if (canAttack)
    //        {
    //            ChangeState(STATE.STATE_ATTACK);
    //        }
    //        else
    //        {
    //            Debug.Log("공격불가");
    //            ChangeState(STATE.STATE_IDLE);
    //            //transform.LookAt(_target.transform.position);
    //        }
    //    }
    //    else if (Vector3.Distance(transform.position , _target.transform.position) <= _findRange)
    //    {
    //        ChangeState(STATE.STATE_TRACE);
    //    }

    //}

    //public override void FindStart()
    //{
    //    base.FindStart();

    //    if (panic)
    //    {
    //        _monster.MyAnimator.SetTrigger("Panic");
    //        panic = false;
    //    }

    //    _navMeshAgent.isStopped = true;
    //    transform.LookAt(_target.transform.position);
    //    // 위에 느낌표가 뜬다.
    //    // 소리를 낸다.

    //    // 인식 행동 진행
    //    StartCoroutine(DoFindAction());
    //}

    //public override IEnumerator DoFindAction()
    //{
    //    yield return null;
    //}

    public override void DeathCheck()
    {
        if (_currentState != STATE.STATE_DIE && NoHPCheck())
        {
            StopAllCoroutines();
            Debug.Log("사망");
            _currentState = STATE.STATE_DIE;

        }
    }
    public override void PlayerDeathCheck()
    {
        if (_currentState != STATE.STATE_KILL && _target.GetComponent<LivingEntity>().Hp <= 0)
        {
            //ChangeState(STATE.STATE_KILL);
            _currentState = STATE.STATE_KILL;
        }
    }

    //public override void DeathStart()
    //{
    //    StartCoroutine(DoDeathAction());
    //}

    //public override IEnumerator DoDeathAction()
    //{
    //    // DeadSound를 재생한다.
    //    // 뭔가 사망 행동을 함
    //    _monster.MyAnimator.SetTrigger("Die");

    //    Debug.Log("사망했다! 사망행동으로 폭탄 떨구고 가기~");
    //    yield return new WaitForSeconds(2);

    //    DestroyImmediate(this.gameObject);
    //}

    //protected override void IdleStart()
    //{
    //    //if (!_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
    //    //_monster.MyAnimator.SetTrigger("Idle");
    //}

    //private void SpawnAction()
    //{

    //    if (_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
    //    {
    //        ChangeState(STATE.STATE_IDLE);
    //    }
    //}

    //private void SpawnStart()
    //{
    //   // _monster.MyAnimator.SetTrigger("Spawn");
    //}

    //private void TraceStart()
    //{
    //    _navMeshAgent.speed = _speed * 1.5f;
    //    _navMeshAgent.isStopped = false;
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            Debug.Log("데미지입은 몬스터");

            if(!_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            _monster.MyAnimator.SetTrigger("Hit");

            Damaged(WeaponManager.Instance.GetWeapon().damage);

            _currentState = STATE.STATE_IDLE;
        }
    }
   

    private IEnumerator DoHitAction()
    {
        yield return null;
        //데미지를 입는다.

        while (true)
        {
            yield return null;

            if (_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                StopCoroutine(_hitCoroutine);
                _hitCoroutine = null;
                ChangeState(STATE.STATE_IDLE);
            }
        }
    }

    private void KillPlayer()
    {
        if (_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            _monster.MyAnimator.ResetTrigger("Laugh");
            _monster.MyAnimator.SetTrigger("Laugh");
        }

        transform.RotateAround(_target.transform.position, Vector3.up, 5 * Time.deltaTime);
        transform.LookAt(_target.transform.position);
    }
   
}
