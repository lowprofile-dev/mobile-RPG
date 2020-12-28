using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using System;

public class DragonAction : MonsterAction
{
    NavMeshAgent _navMeshAgent;
    Coroutine _attackCoroutine;
    Coroutine _idleCoroutine;
    Coroutine _castCoroutine;
    Coroutine _hitCoroutine;
    private float idleCnt = 0;
    private bool panic = false;
    Vector3 spawnPostion;
    Vector3 aroundPos;

    public override void InitObject()
    {
        base.InitObject();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        panic = true;
        spawnPostion = transform.position;
        _navMeshAgent.stoppingDistance = _attackRange;
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
                SpawnAction();
                break;
            case STATE.STATE_IDLE:
                Search();
                break;
            case STATE.STATE_STIRR:
                GroundSearch();
                break;
            case STATE.STATE_FIND:
                FindPlayer();
                break;
            case STATE.STATE_TRACE:
                Move();
                CheckLimitPlayerDistance();
                break;
            case STATE.STATE_ATTACK:
                Attack();
                break;
            case STATE.STATE_KILL:
                KillPlayer();        
                break;
            case STATE.STATE_DEBUFF:
                break;
            case STATE.STATE_DIE:
                break;
            default:
                break;
        }
    }

    public override void EnterState(STATE targetState)
    {
        switch (targetState)
        {
            case STATE.STATE_SPAWN:
                SpawnStart();
                break;
            case STATE.STATE_IDLE:
                IdleStart();
                break;
            case STATE.STATE_STIRR:
                StirrStart();
                break;
            case STATE.STATE_FIND:
                FindStart();
                break;
            case STATE.STATE_TRACE:
                TraceStart();
                break;
            case STATE.STATE_DEBUFF:
                break;
            case STATE.STATE_ATTACK:
                AttackStart();
                break;
            case STATE.STATE_KILL:
                KillStart();
                break;
            case STATE.STATE_DIE:
                DeathStart();
                break;
            case STATE.STATE_CAST:
                CastStart();
                break;
            default:
                break;
        }

    }

    private void KillStart()
    {
        _monster.MyAnimator.SetTrigger("Laugh");
        _navMeshAgent.enabled = false;
    }

    public override void ExitState(STATE targetState)
    {
        switch (targetState)
        {
            case STATE.STATE_IDLE:
                IdleExit();
                break;
            case STATE.STATE_STIRR:
                StirrExit();
                break;
            case STATE.STATE_FIND:
                FindExit();
                break;
            case STATE.STATE_TRACE:
                TraceExit();
                break;
            case STATE.STATE_DEBUFF:
                break;
            case STATE.STATE_ATTACK:
                AttackExit();
                break;
            case STATE.STATE_CAST:
                CastExit();
                break;
            case STATE.STATE_KILL:
                break;
            case STATE.STATE_DIE:
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
        if(_idleCoroutine != null)
        StopCoroutine(_idleCoroutine);
    }

    private void TraceExit()
    {
        _monster.MyAnimator.ResetTrigger("Walk");
    }

    /// <summary>
    /// 추적 행동
    /// </summary>
    public override void Move()
    {
        base.Move();
        //_navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(_target.transform.position);
        // Run StepSound 재생

        // 사거리 내에 적 존재 시 발동
        if (Vector3.Distance(_target.transform.position, _monster.transform.position) < _attackRange)
        {
            ChangeState(STATE.STATE_ATTACK);
        }
    }

    public override void CheckLimitPlayerDistance()
    {
        base.CheckLimitPlayerDistance();
    }

    public override void Damaged(float dmg)
    {
        base.Damaged(dmg);
        //ChangeState(STATE.STATE_FIND);
    }

    public override void Attack()
    {
        base.Attack();

        // 타겟과의 거리가 공격 범위보다 커지면
        if (Vector3.Distance(_target.transform.position, _monster.transform.position) > _attackRange)
        {       
            ChangeState(STATE.STATE_IDLE);
        }
    }

    public override void AttackStart()
    {
        _attackCoroutine = StartCoroutine(AttackTarget());
    }

    public override void AttackExit()
    {
        StopCoroutine(_attackCoroutine);
    }

    public override IEnumerator AttackTarget()
    {

        while (true)
        {
            yield return null;
           // yield return new WaitForSeconds(_attackSpeed);

            if (CanAttackState())
            {

                yield return new WaitForSeconds(_attackSpeed - _monster.MyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);

                _monster.MyAnimator.SetTrigger("Attack");
                _navMeshAgent.SetDestination(_target.transform.position);
                transform.LookAt(_target.transform);

                // 사운드 재생
                Debug.Log(_monster.monsterName + "의 공격!");
                _target.GetComponent<LivingEntity>().Damaged(_monster.attackDamage);

                // 공격 행동 한다.
                //StartCoroutine(DoAttackAction());
                // 공격 애니메이션이 끝났을때 자동으로 Idle로 넘어가도록
                yield return new WaitForSeconds(_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);

                _monster.MyAnimator.ResetTrigger("Attack"); // off가 되어있으므로 바로 돌아오진 않음.
                // 애니메이션의 재시작 부분에 Attack이 On이 되야함.

                
                //ChangeState(STATE.STATE_IDLE);
                
                // 일정 확률로 캐스팅 상태로 바꾼다.
                //int toCastRandomValue = UnityEngine.Random.Range(0, 100);

                //if (toCastRandomValue < _monster.castPercent)
                //{
                //    ChangeState(STATE.STATE_CAST);
                //}
            }
            else
            {
                ChangeState(STATE.STATE_IDLE);
            }
        }

    }

    public override void CastStart()
    {
        _castCoroutine = StartCoroutine(DoCastingAction());
    }

    public override void Cast()
    {
        base.Cast();

        // 타겟과의 거리가 공격 범위보다 커지면
        if (Vector3.Distance(_target.transform.position, _monster.transform.position) > _attackRange)
        {
            ChangeState(STATE.STATE_TRACE);
        }
    }

    public override IEnumerator DoCastingAction()
    {
        Debug.Log("캐스팅 중입니다...");
        // 사운드 재생
        yield return new WaitForSeconds(2);
        ChangeState(STATE.STATE_ATTACK);
    }

    public override void CastExit()
    {
        StopCoroutine(_castCoroutine);
    }

    public override IEnumerator DoAttackAction()
    {
        // 아무튼 뭔가를 한다

        yield return null;
    }

    public override void Search()
    {
        base.Search();

        idleCnt += Time.deltaTime;

        if (idleCnt > 3)
        {
            panic = true;
            idleCnt = 0;
            if (UnityEngine.Random.Range(1, 100) <= 30)
                ChangeState(STATE.STATE_STIRR);
        }
       
    }
    private IEnumerator Around()
    {
        while (true)
        {
            yield return null;

          if (Vector3.Distance(transform.position, aroundPos) >= 0.5f)
          {
                yield return new WaitForSeconds(2f);
                aroundPos = new Vector3(UnityEngine.Random.Range(spawnPostion.x - 3f, spawnPostion.x + 3f), spawnPostion.y, UnityEngine.Random.Range(spawnPostion.z - 3f, spawnPostion.z + 3f));
                _monster.MyAnimator.SetTrigger("Idle");
                _navMeshAgent.isStopped = false;
                _navMeshAgent.SetDestination(aroundPos);
          }
        }
    }

    public override void FindStart()
    {
        base.FindStart();

        if (panic)
        {
            _monster.MyAnimator.SetTrigger("Panic");
            panic = false;
        }

        _navMeshAgent.isStopped = true;
        transform.LookAt(_target.transform.position);
        // 위에 느낌표가 뜬다.
        // 소리를 낸다.

        // 인식 행동 진행
        StartCoroutine(DoFindAction());
    }

    public override IEnumerator DoFindAction()
    {
        yield return null;
    }

    public override void DeathCheck()
    {
        if (_currentState != STATE.STATE_DIE && NoHPCheck())
        {
            ChangeState(STATE.STATE_DIE);
            _monster.MyAnimator.SetTrigger("doDie");
        }
    }
    public override void PlayerDeathCheck()
    {
        if (_currentState != STATE.STATE_KILL && _target.GetComponent<LivingEntity>().Hp <= 0)
        {
            ChangeState(STATE.STATE_KILL);         
        }
    }

    public override void DeathStart()
    {
        StartCoroutine(DoDeathAction());
    }

    public override IEnumerator DoDeathAction()
    {
        // DeadSound를 재생한다.
        // 뭔가 사망 행동을 함
        _monster.MyAnimator.SetTrigger("Die");

        Debug.Log("사망했다! 사망행동으로 폭탄 떨구고 가기~");
        yield return new WaitForSeconds(2);

        DestroyImmediate(this.gameObject);
    }

    private void IdleStart()
    {
        _monster.MyAnimator.SetTrigger("Idle");

        aroundPos = new Vector3(UnityEngine.Random.Range(spawnPostion.x - 3f, spawnPostion.x + 3f), spawnPostion.y, UnityEngine.Random.Range(spawnPostion.z - 3f, spawnPostion.z + 3f));
        _navMeshAgent.SetDestination(aroundPos);

        _idleCoroutine = StartCoroutine(Around());
    }
    private void SpawnAction()
    {

        if (_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            ChangeState(STATE.STATE_IDLE);
        }
    }

    private void FindPlayer()
    {

        if (_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            _navMeshAgent.isStopped = false;
            ChangeState(STATE.STATE_TRACE);
        }

    }

    private void GroundSearch()
    {
        if (_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            ChangeState(STATE.STATE_IDLE);
        }
    }

    private void SpawnStart()
    {
        _monster.MyAnimator.SetTrigger("Spawn");
    }

    private void TraceStart()
    {
        _monster.MyAnimator.SetTrigger("Walk");
    }

    private void StirrStart()
    {
        _navMeshAgent.isStopped = true;
        _monster.MyAnimator.SetTrigger("Stirr");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("데미지입은 몬스터");

            if(!_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            _monster.MyAnimator.SetTrigger("Hit");

            _monster.Damaged(1);

            if (_hitCoroutine == null)
                _hitCoroutine = StartCoroutine(DoHitAction());
            else
                ChangeState(STATE.STATE_IDLE);
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
