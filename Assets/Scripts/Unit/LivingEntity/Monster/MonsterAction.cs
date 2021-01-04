using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// MonsterAction만 있어도 Monster는 기본적인 행동을 할 수 있다. 최소한의 단위로 함수를 쪼개놓았기에, 변경할 함수에 한해서만 함수를 오버라이딩하여 행동을 변경해주면 된다.
/// </summary>
public class MonsterAction : MonoBehaviour
{
    // 코루틴들
    protected Coroutine _attackCoroutine;   // AttackCoroutine 제어
    protected Coroutine _idleCoroutine;     // IdleCoroutine 제어
    protected Coroutine _castCoroutine;     // CastCoroutine 제어
    protected Coroutine _hitCoroutine;      // HitCoroutine 제어

    // 캐싱 대상
    protected Monster _monster; public Monster monster { get { return _monster; } }
    protected NavMeshAgent _navMeshAgent;

    // 오브젝트
    protected GameObject _target;           // 공격대상        
    protected GameObject _spawnEffect;      // 스폰용으로 사용된 이펙트

    // 기타 변수
    protected STATE _currentState;          // 현재 상태
    protected float _distance;              // 타겟과의 거리
    protected float _traceTimer;            // 추적 이후 경과한 시간
    protected Vector3 _spawnPosition;       // 스폰된 위치
    protected Vector3 _patrolPos;           // 정찰할 위치
    protected float _idleTime;              // Idle에서 경과한 시간
    protected bool _isImmune;               // 무적 여부


    [Header("범위")]
    [SerializeField] protected float _findRange;        // 타겟을 발견할 범위
    [SerializeField] protected float _attackRange;      // 공격 범위
    [SerializeField] protected float _limitTraceRange;  // 최대 쫓아오는 범위

    [Header("속도")]
    [SerializeField] protected float _moveSpeed;        // 이동속도
    [SerializeField] protected float _attackSpeed;      // 공격속도

    [Header("타겟 체크")]
    [SerializeField] private LayerMask _checkLayerMask; // 타겟 레이어 마스크 




    /////////// 기본 ////////////

    /// <summary>
    /// 오브젝트 초기화
    /// </summary>
    public virtual void InitObject()
    {
        _isImmune = false;

        CachingObject();

        _traceTimer = 0;
        _spawnPosition = transform.position;

        InitState();
    }

    protected virtual void CachingObject()
    {
        _monster = GetComponent<Monster>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _target = GameObject.FindGameObjectWithTag("Player");
    }

    /// <summary>
    /// 스테이트 초기화
    /// </summary>
    public void InitState()
    {
        _currentState = STATE.STATE_NULL;
        ChangeState(STATE.STATE_SPAWN);
    }

    /// <summary>
    /// 몬스터 업데이트
    /// </summary>
    protected virtual void UpdateMonster()
    {
        TargetDeathCheck();
    }

    private void Update()
    {
        UpdateState();
    }


    /////////// 상태 관련 ////////////

    /// <summary>
    /// 상태 변화 - Exit - current - Enter
    /// </summary>
    public virtual void ChangeState(STATE targetState)
    {
        if (_currentState != STATE.STATE_NULL)
        {
            ExitState(_currentState);
        }

        _currentState = targetState;
        EnterState(_currentState);
    }

    /// <summary>
    /// 현재 스테이트에 들어설때 일어나는 작업
    /// </summary>
    public virtual void EnterState(STATE targetState)
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

    /// <summary>
    /// 현재 스테이트를 업데이트한다.
    /// </summary>
    public virtual void UpdateState()
    {
        // 반드시 실행되는 업데이트 내용
        UpdateMonster();

        // 스테이트별 업데이트 내용
        switch (_currentState)
        {
            case STATE.STATE_SPAWN:
                SpawnUpdate();
                break;
            case STATE.STATE_IDLE:
                IdleUpdate();
                break;
            case STATE.STATE_STIRR:
                StirrUpdate();
                break;
            case STATE.STATE_FIND:
                FindPlayer();
                break;
            case STATE.STATE_TRACE:
                TraceUpdate();
                break;
            case STATE.STATE_ATTACK:
                AttackUpdate();
                break;
            case STATE.STATE_CAST:
                CastUpdate();
                break;
            case STATE.STATE_KILL:
                KillUpdate();
                break;
            case STATE.STATE_DIE:
                DeathUpdate();
                break;
            case STATE.STATE_DEBUFF:
                break;
            default:
                break;
        }
    }



    /// <summary>
    /// 현재 스테이트를 나갈때 일어나는 작업
    /// </summary>
    public virtual void ExitState(STATE targetState)
    {
        switch (targetState)
        {
            case STATE.STATE_SPAWN:
                SpawnExit();
                break;
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
            case STATE.STATE_ATTACK:
                AttackExit();
                break;
            case STATE.STATE_CAST:
                CastExit();
                break;
            case STATE.STATE_KILL:
                KillExit();
                break;
            case STATE.STATE_DIE:
                DeathExit();
                break;
            case STATE.STATE_DEBUFF:
                break;
            default:
                break;
        }
    }




    /////////// 스폰 관련////////////

    protected virtual void SpawnStart()
    {
        _isImmune = true; // 스폰 중 무적
        _monster.MyAnimator.SetTrigger("Spawn");
        AddSpawnEffect();
    }

    protected virtual void SpawnUpdate()
    {
    }

    /// <summary>
    /// 스폰 이펙트를 생성한다.
    /// </summary>
    protected virtual void AddSpawnEffect()
    {
        _spawnEffect = ResourceManager.Instance.Instantiate("Prefab/Effect/Spawn aura", Vector3.zero, Quaternion.identity, transform);
        _spawnEffect.transform.localPosition = Vector3.zero;

        StartCoroutine(SpawnDissolve());
    }

    /// <summary>
    /// 생성할때 디졸브 머테리얼을 재생한다.
    /// </summary>
    protected virtual IEnumerator SpawnDissolve()
    {
        _monster.avatarObject.GetComponent<Renderer>().material = _monster.dissolveMaterial; // 디졸브 Material로 변경
        Material mat = _monster.avatarObject.GetComponent<Renderer>().material;

        float height = 0;

        DOTween.To(() => height, x => height = x, 10, 3).SetEase(Ease.InSine);

        while (true)
        {
            mat.SetFloat("_CutoffHeight", height);

            yield return null;

            if (height >= 10) break;
        }

        _monster.avatarObject.GetComponent<Renderer>().material = _monster.nonDissolveMaterial; // 기본 Material로 변경
        ChangeState(STATE.STATE_IDLE);
    }

    protected virtual void SpawnExit()
    {
        _isImmune = false; // 스폰 끝날 시 무적 해제
        _monster.MyAnimator.ResetTrigger("Spawn");
    }



    /////////// 대기 관련////////////

    protected virtual void IdleStart()
    {
        SetNewPatrolPos();
        _idleCoroutine = StartCoroutine(PatrolAround());
    }

    /// <summary>
    /// 주변을 정찰 시, 정찰 포인트를 정해주어 이동시킨다. (하지 않으면 내용을 지운채로 오버라이딩하면 됨)
    /// </summary>
    protected virtual void SetNewPatrolPos()
    {
        _navMeshAgent.isStopped = false;
        _monster.MyAnimator.SetTrigger("Idle");

        _patrolPos = new Vector3(UnityEngine.Random.Range(_spawnPosition.x - 3f, _spawnPosition.x + 3f), _spawnPosition.y, UnityEngine.Random.Range(_spawnPosition.z - 3f, _spawnPosition.z + 3f)); // 주변을 둘러볼 수 있도록
        _navMeshAgent.SetDestination(_patrolPos);
    }

    /// <summary>
    /// 주변을 정찰하도록 한다.
    /// </summary>
    protected IEnumerator PatrolAround()
    {
        while (true)
        {
            yield return null;

            if (Vector3.Distance(transform.position, _patrolPos) <= 0.5f)
            {
                yield return new WaitForSeconds(2f);
                SetNewPatrolPos();
            }
        }
    }

    /// <summary>
    /// 상대를 거리에 따라 반응하도록 탐색한다.
    /// </summary>
    protected virtual void FindTargetInIdle()
    {
        Vector3 targetDir = (_target.transform.position - transform.position).normalized; // y축의 영향 받지 않음
        Physics.Raycast(transform.position, targetDir, out RaycastHit hit, 30f, _checkLayerMask);
        _distance = Vector3.Distance(_target.transform.position, transform.position);

        if (hit.transform.CompareTag("Player") && _distance <= _findRange) DoSomethingIdleSearchFind();
    }

    /// <summary>
    /// 주변을 응시하는 상태로 잠깐 넘어가게 한다 (일부만 사용하므로 추가할 애들은 IdleUpdate를 오버라이딩하도록 한다)
    /// </summary>
    protected virtual void ToStirr()
    {
        _idleTime += Time.deltaTime;

        if (_idleTime > 3)
        {
            _idleTime = 0;
            if (UnityEngine.Random.Range(1, 100) <= 30)
            {
                ChangeState(STATE.STATE_STIRR);
            }
        }
    }

    protected virtual void IdleUpdate()
    {
        FindTargetInIdle();
    }

    /// <summary>
    /// IdleSearch가 이뤄진 순간 어떤 행동을 할지 정해준다.
    /// </summary>
    protected virtual void DoSomethingIdleSearchFind()
    {
        ChangeState(STATE.STATE_FIND);
    }

    protected virtual void IdleExit()
    {
        if (_idleCoroutine != null) StopCoroutine(_idleCoroutine);
        _monster.MyAnimator.ResetTrigger("Idle");
    }



    /////////// 두리번거리기 관련////////////

    protected virtual void StirrStart()
    {
        _navMeshAgent.isStopped = true;
        _monster.MyAnimator.SetTrigger("Stirr");
    }

    private void StirrUpdate()
    {
        if (CheckAnimationOver("Stirr", 1.0f))
        {
            ChangeState(STATE.STATE_IDLE);
        }
    }

    protected virtual void StirrExit()
    {
        _navMeshAgent.isStopped = false;
        _monster.MyAnimator.ResetTrigger("Stirr");
    }



    /////////// 탐색 관련////////////

    public virtual void FindStart()
    {
        _navMeshAgent.isStopped = true;
        transform.LookAt(_target.transform.position);
    }

    /// <summary>
    /// 탐색 행동이 끝나면 Trace로 넘어가도록 함.
    /// </summary>
    protected virtual void FindPlayer()
    {
        if (CheckFindAnimationOver()) // 애니메이션이 끝나면 Trace로 넘어감
        {
            _navMeshAgent.isStopped = false;
            ChangeState(STATE.STATE_TRACE);
        }
    }

    /// <summary>
    /// 탐색 애니메이션이 끝났는지 체크
    /// </summary>
    protected virtual bool CheckFindAnimationOver()
    {
        return CheckAnimationOver(0, 1.0f);
    }

    protected virtual void FindExit()
    {

    }



    /////////// 이동 관련////////////

    protected virtual void TraceStart()
    {
        _monster.MyAnimator.SetTrigger("Walk");
    }

    private void TraceUpdate()
    {
        MoveToTarget();
        CheckLimitPlayerDistance();
    }

    /// <summary>
    /// 타겟을 향해 움직인다.
    /// </summary>
    public virtual void MoveToTarget()
    {
        _navMeshAgent.SetDestination(_target.transform.position);
        // Run StepSound 재생

        // 사거리 내에 적 존재 시 발동
        if (Vector3.Distance(_target.transform.position, _monster.transform.position) < _attackRange)
        {
            ChangeState(STATE.STATE_ATTACK);
        }
    }

    /// <summary>
    /// 플레이어와 거리가 일정 이상일 때, 일정 시간 이후에 원래 상태로 돌아간다.
    /// </summary>
    public virtual void CheckLimitPlayerDistance()
    {
        // 캐릭터와 적의 거리가 limit 이상일때
        if (Vector3.Distance(_target.transform.position, transform.position) >= _limitTraceRange)
        {
            _traceTimer += Time.deltaTime;
            // 타이머가 재생된다.

            if (_traceTimer > 2)
            {
                DoReturn();
            }
        }

        // 거리가 가깝다면
        else
        {
            _traceTimer = 0;
        }
    }

    protected virtual void DoReturn()
    {
        // 상태를 바꾼다.
        ChangeState(STATE.STATE_IDLE);
        _traceTimer = 0;
    }

    private void TraceExit()
    {
        _monster.MyAnimator.ResetTrigger("Walk");
    }


    /////////// 공격 관련////////////

    protected virtual void AttackStart()
    {
        _attackCoroutine = StartCoroutine(AttackTarget());
    }

    protected virtual void AttackUpdate()
    {
        // 타겟과의 거리가 공격 범위보다 커지면
        if (Vector3.Distance(_target.transform.position, _monster.transform.position) > _attackRange)
        {
            ChangeState(STATE.STATE_IDLE);
        }
    }

    /// <summary>
    /// 대상에 대해 일정 주기마다 공격을 실행한다.
    /// </summary>
    protected virtual IEnumerator AttackTarget()
    {
        while (true)
        {
            yield return null;

            if (CanAttackState())
            {
                yield return new WaitForSeconds(_attackSpeed - _monster.MyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);

                _monster.MyAnimator.SetTrigger("Attack");
                _navMeshAgent.SetDestination(_target.transform.position);
                transform.LookAt(_target.transform);

                // 사운드 재생

                // 데미지 계산
                _target.GetComponent<LivingEntity>().Damaged(_monster.attackDamage);

                // 공격 행동 한다.
                StartCoroutine(DoAttackAction());

                yield return new WaitForSeconds(_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);

                _monster.MyAnimator.ResetTrigger("Attack"); // 애니메이션의 재시작 부분에 Attack이 On이 되야함.

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
                ChangeState(STATE.STATE_TRACE);
            }
        }
    }

    protected virtual void AttackExit()
    {
        _monster.MyAnimator.ResetTrigger("Attack");
        StopCoroutine(_attackCoroutine);
    }

    /// <summary>
    /// 공격할 때마다 일어나는 현상
    /// </summary>
    public virtual IEnumerator DoAttackAction()
    {
        yield return null;
    }

    /// <summary>
    /// 현재가 공격할 수 있는 상태인지 측정한다. (거리, 대상 존재 여부)
    /// </summary>
    protected bool CanAttackState()
    {
        Vector3 targetDir = new Vector3(_target.transform.position.x - transform.position.x, 0f, _target.transform.position.z - transform.position.z);
        Physics.Raycast(new Vector3(transform.position.x, 0.5f, transform.position.z), targetDir, out RaycastHit hit, 30f, _checkLayerMask);

        _distance = Vector3.Distance(_target.transform.position, transform.position);

        if (hit.transform == null)
        {
            return false;
        }
        else if (hit.transform.CompareTag("Player") && _distance <= _attackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    /////////// 캐스트 관련////////////

    protected virtual void CastStart()
    {
        _castCoroutine = StartCoroutine(DoCastingAction());
    }

    protected virtual void CastUpdate()
    {
        // 타겟과의 거리가 공격 범위보다 커지면
        if (Vector3.Distance(_target.transform.position, _monster.transform.position) > _attackRange)
        {
            ChangeState(STATE.STATE_TRACE);
        }
    }

    /// <summary>
    /// 캐스팅를 진행시킨다.
    /// </summary>
    protected virtual IEnumerator DoCastingAction()
    {
        Debug.Log("캐스팅 중입니다...");
        // 사운드 재생
        yield return new WaitForSeconds(2);
        ChangeState(STATE.STATE_ATTACK);
    }

    protected virtual void CastExit()
    {
        StopCoroutine(_castCoroutine);
    }





    /////////// 피해 입음 관련////////////

    /// <summary>
    /// dmg를 받아 피해를 입는다.
    /// </summary>
    public virtual void Damaged(float dmg, bool SetAnimation = true)
    {
        if (_currentState == STATE.STATE_DIE)
        {
            return;
        }

        else
        {
            _monster.Damaged(dmg);

            bool isDeath = DeathCheck();

            if (SetAnimation)
            {
                if (isDeath) _monster.MyAnimator.ResetTrigger("Hit");
                else _monster.MyAnimator.SetTrigger("Hit");
            }

            if (isDeath) CheckDeathAndChange();
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            if (GetCanDamageCheck())
            {
                Damaged(WeaponManager.Instance.GetWeapon().attackDamage);
            }
        }
    }


    /// <summary>
    /// 데미지를 받을 조건을 체크한다.
    /// </summary>
    protected virtual bool GetCanDamageCheck()
    {
        return !_isImmune && !DeathCheck();
    }




    /////////// 사망 관련////////////

    protected virtual void DeathStart()
    {
        StartCoroutine(DoDeathAction());
    }

    /// <summary>
    /// 죽었을 때, 타이머를 통해 죽는 연출 및 이벤트 발생을 할 수 있도록 한다.
    /// </summary>
    protected virtual IEnumerator DoDeathAction()
    {
        _monster.MyAnimator.SetTrigger("Die");

        while (true)
        {
            yield return null;
            if (CheckAnimationOver("Die", 1.0f)) break;
        }

        SetDeathProduction();
    }

    /// <summary>
    /// 죽었을때의 연출
    /// </summary>
    protected virtual void SetDeathProduction()
    {
        transform.DOMoveY(transform.position.y - 10, 10).OnComplete(() => { DestroyImmediate(gameObject); });
        _monster.avatarObject.GetComponent<Renderer>().material.DOFade(0, 2);
        _monster.hpbarObject.gameObject.SetActive(false);
    }

    /// <summary>
    /// 죽었는지 여부를 판단하여 적합한 행동을 실시한다.
    /// </summary>
    protected virtual bool CheckDeathAndChange()
    {
        if (DeathCheck())
        {
            ChangeState(STATE.STATE_DIE);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 사망 여부를 체크한다.
    /// </summary>
    protected virtual bool DeathCheck()
    {
        return _monster.Hp <= 0;
    }

    protected virtual void DeathUpdate()
    {
    }

    protected virtual void DeathExit()
    {
    }




    /////////// 처치 관련////////////

    protected virtual void KillStart()
    {
        _monster.MyAnimator.SetTrigger("Laugh");
        _navMeshAgent.enabled = false;
    }

    protected virtual void KillUpdate()
    {
        if (CheckAnimationOver("Laugh", 1.0f))
        {
            _monster.MyAnimator.ResetTrigger("Laugh");
            _monster.MyAnimator.SetTrigger("Laugh");
        }

        transform.RotateAround(_target.transform.position, Vector3.up, 5 * Time.deltaTime);
        transform.LookAt(_target.transform.position);
    }

    protected virtual void KillExit()
    {

    }

    /// <summary>
    /// 타겟의 사망 여부를 받고 상태를 전환한다.
    /// </summary>
    public virtual void TargetDeathCheck()
    {
        if (_currentState != STATE.STATE_KILL && _target.GetComponent<LivingEntity>().Hp <= 0)
        {
            ChangeState(STATE.STATE_KILL);
        }
    }


    /////////// 기타 ////////////

    /// <summary>
    /// 현재 진행중인 애니메이션이 time보다 더 많이 진행됐는지 여부를 체크한다.
    /// </summary>
    public bool CheckAnimationOver(int animNum, float time)
    {
        return _monster.MyAnimator.GetCurrentAnimatorStateInfo(animNum).normalizedTime >= time;
    }

    /// <summary>
    /// 해당 이름의 애니메이션이 time보다 더 많이 진행됐는지 여부를 체크한다.
    /// </summary>
    public bool CheckAnimationOver(string name, float time)
    {
        AnimatorStateInfo info = _monster.MyAnimator.GetCurrentAnimatorStateInfo(0);
        return info.IsName(name) && info.normalizedTime >= time;
    }

    /// <summary>
    /// 해당 애니메이션의 진행 결과를 체크한다.
    /// </summary>
    public float GetAnimationPercentTime(int animNum)
    {
        return _monster.MyAnimator.GetCurrentAnimatorStateInfo(animNum).normalizedTime;
    }
}
