using DG.Tweening;
using EPOOutline;
using System;
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
    protected Rigidbody _rigidBody;
    protected Outlinable _outlinable;

    // 오브젝트
    protected GameObject _target; public GameObject Target { get { return _target; } }               // 공격대상        
    protected GameObject _spawnEffect;              // 스폰용으로 사용된 이펙트

    // 기타 변수
    protected MONSTER_STATE _currentState;                  // 현재 상태
    public MONSTER_STATE currentState { get { return _currentState; } }

    protected float _distance;                      // 타겟과의 거리
    protected float _traceTimer;                    // 추적 이후 경과한 시간
    protected Vector3 _spawnPosition;               // 스폰된 위치
    protected Vector3 _patrolPos;                   // 정찰할 위치
    protected float _idleTime;                      // Idle에서 경과한 시간
    protected bool _isImmune;                       // 무적 여부
    protected int _attackType;                      // 현재 공격의 타입
    protected bool _readyCast;                      // 캐스트 준비가 되었는지
    [HideInInspector] public float _cntCastTime;    // 현재 캐스트 시간

    // 트위닝
    protected Tweener _fadeOutRedTween;     // 데미지 받았을때 트위닝
    protected Tweener _lookAtTween;         // 쳐다보는 트위닝

    [Header("범위")]
    [SerializeField] protected float _findRange;        // 타겟을 발견할 범위
    [SerializeField] protected float _attackRange;      // 공격 범위
    [SerializeField] protected float _limitTraceRange;  // 최대 쫓아오는 범위

    [Header("속도")]
    [SerializeField] protected float _moveSpeed;        // 이동속도
    [SerializeField] protected float _attackSpeed;      // 공격속도

    [Header("타겟 체크")]
    [SerializeField] private LayerMask _checkLayerMask; // 타겟 레이어 마스크 

    [Header("캐스팅")]
    [SerializeField] protected int _castChangePercnt;   // 캐스트 확률
    public float _castTime;                             // 캐스트에 걸리는 시간

    [Header("UI")]
    [SerializeField] protected EnemySliderBar _bar;
    
    private float attackedTime = 0.1f;
    private float counter = 0f;

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
        var _mon = this;
        _monster.CCManager = new CCManager(ref _mon , "monster");
    }

    protected virtual void CachingObject()
    {
        _monster = GetComponent<Monster>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _rigidBody = GetComponent<Rigidbody>();
        _outlinable = GetComponent<Outlinable>();
        _target = GameObject.FindGameObjectWithTag("Player");
    }

    /// <summary>
    /// 스테이트 초기화
    /// </summary>
    public virtual void InitState()
    {
        _currentState = MONSTER_STATE.STATE_NULL;
        ChangeState(MONSTER_STATE.STATE_SPAWN);
    }

    /// <summary>
    /// 몬스터 업데이트
    /// </summary>
    protected virtual void UpdateMonster()
    {
        TargetDeathCheck();
        if(_currentState != MONSTER_STATE.STATE_SPAWN && _isImmune)
        {
            counter += Time.deltaTime;
            if(counter >= attackedTime)
            {
                counter = 0f;
                _isImmune = false;
            }
        }
    }

    private void Update()
    {
        _monster.CCManager.Update();
        if (_target == null)
        {
            _target = GameObject.FindGameObjectWithTag("Player");
        }
        UpdateState();
    }


    /////////// 상태 관련 ////////////

    /// <summary>
    /// 상태 변화 - Exit - current - Enter
    /// </summary>
    public virtual void ChangeState(MONSTER_STATE targetState)
    {
        if (_currentState != MONSTER_STATE.STATE_NULL)
        {
            ExitState(_currentState);
        }

        _currentState = targetState;

        EnterState(_currentState);
    }

    /// <summary>
    /// 현재 스테이트에 들어설때 일어나는 작업
    /// </summary>
    public virtual void EnterState(MONSTER_STATE targetState)
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
                ItemManager.Instance.DropItem(transform);
                Debug.Log("아이템 드롭!");
                break;
            case MONSTER_STATE.STATE_CAST:
                CastStart();
                break;
            case MONSTER_STATE.STATE_FALL:
                FallStart();
                break;
            case MONSTER_STATE.STATE_STUN:
                StunStart();
                break;
            case MONSTER_STATE.STATE_RIGID:
                RigidStart();
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

        Debug.Log(_currentState);
        UpdateMonster();

        // 스테이트별 업데이트 내용
        switch (_currentState)
        {
            case MONSTER_STATE.STATE_SPAWN:
                SpawnUpdate();
                break;
            case MONSTER_STATE.STATE_IDLE:
                IdleUpdate();
                break;
            case MONSTER_STATE.STATE_STIRR:
                StirrUpdate();
                break;
            case MONSTER_STATE.STATE_FIND:
                FindPlayer();
                break;
            case MONSTER_STATE.STATE_TRACE:
                TraceUpdate();
                break;
            case MONSTER_STATE.STATE_ATTACK:
                AttackUpdate();
                break;
            case MONSTER_STATE.STATE_CAST:
                CastUpdate();
                break;
            case MONSTER_STATE.STATE_KILL:
                KillUpdate();
                break;
            case MONSTER_STATE.STATE_DIE:
                DeathUpdate();
                break;
            case MONSTER_STATE.STATE_DEBUFF:
                break;
            case MONSTER_STATE.STATE_FALL:
                FallUpdate();
                break;
            case MONSTER_STATE.STATE_STUN:
                StunUpdate();
                break;
            case MONSTER_STATE.STATE_RIGID:
                RigidUpdate();
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 현재 스테이트를 나갈때 일어나는 작업
    /// </summary>
    public virtual void ExitState(MONSTER_STATE targetState)
    {
        switch (targetState)
        {
            case MONSTER_STATE.STATE_SPAWN:
                SpawnExit();
                break;
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
            case MONSTER_STATE.STATE_ATTACK:
                AttackExit();
                break;
            case MONSTER_STATE.STATE_CAST:
                CastExit();
                break;
            case MONSTER_STATE.STATE_KILL:
                KillExit();
                break;
            case MONSTER_STATE.STATE_DIE:
                DeathExit();
                break;
            case MONSTER_STATE.STATE_DEBUFF:
                break;
            case MONSTER_STATE.STATE_FALL:
                FallExit();
                break;
            case MONSTER_STATE.STATE_STUN:
                StunExit();
                break;
            case MONSTER_STATE.STATE_RIGID:
                RigidExit();
                break;
            default:
                break;
        }
    }

    /////////// 스폰 관련////////////

    protected virtual void SpawnStart()
    {
        _isImmune = true; // 스폰 중 무적
        //_monster.myAnimator.SetTrigger("Spawn");
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
    /// Spawn Dissolve -> Spawn FadeIn
    /// </summary>
    protected virtual IEnumerator SpawnDissolve()
    {
        _monster.avatarObject.GetComponent<Renderer>().material = _monster.nonDissolveMaterial;

        Material mat = _monster.avatarObject.GetComponent<Renderer>().material;
        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0);

        mat.DOFade(0.5f, 1.5f);
        yield return new WaitForSeconds(1.5f);

        mat.DOFade(1, 0.5f);
        yield return new WaitForSeconds(0.5f);

        ChangeState(MONSTER_STATE.STATE_IDLE);
    }

    protected virtual void SpawnExit()
    {
        _isImmune = false; // 스폰 끝날 시 무적 해제
        _monster.myAnimator.ResetTrigger("Spawn");
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
        _monster.myAnimator.SetTrigger("Idle");

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
        //Vector3 targetDir = (_target.transform.position - transform.position).normalized; // y축의 영향 받지 않음
        //Physics.Raycast(transform.position, targetDir, out RaycastHit hit, 30f, _checkLayerMask);
        _distance = Vector3.Distance(_target.transform.position, transform.position);

        //if (hit.transform.CompareTag("Player") && _distance <= _findRange) DoSomethingIdleSearchFind();
        if (_distance <= _findRange) DoSomethingIdleSearchFind();
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
                ChangeState(MONSTER_STATE.STATE_STIRR);
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
        ChangeState(MONSTER_STATE.STATE_FIND);
    }

    protected virtual void IdleExit()
    {
        if (_idleCoroutine != null) StopCoroutine(_idleCoroutine);
        _monster.myAnimator.ResetTrigger("Idle");
    }



    /////////// 두리번거리기 관련////////////

    protected virtual void StirrStart()
    {
        _navMeshAgent.isStopped = true;
        _monster.myAnimator.SetTrigger("Stirr");
    }

    protected virtual void StirrUpdate()
    {
        if (CheckAnimationOver("Stirr", 1.0f))
        {
            ChangeState(MONSTER_STATE.STATE_IDLE);
        }
    }

    protected virtual void StirrExit()
    {
        _navMeshAgent.isStopped = false;
        _monster.myAnimator.ResetTrigger("Stirr");
    }



    /////////// 탐색 관련////////////

    protected virtual void FindStart()
    {
        _navMeshAgent.isStopped = true;
        LookTarget();
    }

    /// <summary>
    /// 탐색 행동이 끝나면 Trace로 넘어가도록 함.
    /// </summary>
    protected virtual void FindPlayer()
    {
        if (CheckFindAnimationOver()) // 애니메이션이 끝나면 Trace로 넘어감
        {
            _navMeshAgent.isStopped = false;
            ChangeState(MONSTER_STATE.STATE_TRACE);
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
        _monster.myAnimator.SetTrigger("Walk");
    }

    protected virtual void TraceUpdate()
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
            ChangeState(MONSTER_STATE.STATE_ATTACK);
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
        ChangeState(MONSTER_STATE.STATE_IDLE);
        _traceTimer = 0;
    }

    private void TraceExit()
    {
        _monster.myAnimator.ResetTrigger("Walk");
    }


    /////////// 공격 관련////////////

    protected virtual void AttackStart()
    {
        if(!_readyCast && ToCast()) return;
        else _attackCoroutine = StartCoroutine(AttackTarget());
    }

    protected virtual void AttackUpdate()
    {
        // 타겟과의 거리가 공격 범위보다 커지면
        if (Vector3.Distance(_target.transform.position, _monster.transform.position) > _attackRange)
        {
            ChangeState(MONSTER_STATE.STATE_IDLE);
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

                yield return new WaitForSeconds(_attackSpeed - _monster.myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
                SetAttackType();
                SetAttackAnimation();
                
                LookTarget();

                // 사운드 재생

                StartCoroutine(DoAttackAction());

                yield return new WaitForSeconds(_monster.myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);

                ResetAttackAnimation();
                _monster.myAnimator.ResetTrigger("Attack"); // 애니메이션의 재시작 부분에 Attack이 On이 되야함.

                _attackType = 0;
                _readyCast = false;
                if(!_readyCast && ToCast()) break;
            }
            //else
            //{
            //    ChangeState(MONSTER_STATE.STATE_TRACE);
            //    break;
            //}
        }
    }

    protected virtual void SetAttackType()
    {

        if (_readyCast) return;

        _attackType = 0;
    }

    protected virtual bool ToCast()
    {
        int toCastRandomValue = UnityEngine.Random.Range(0, 100);

        if (toCastRandomValue <= _castChangePercnt)
        {
            ChangeState(MONSTER_STATE.STATE_CAST);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 공격 타입에 따른 애니메이션 트리거 설정
    /// </summary>
    protected virtual void SetAttackAnimation()
    {
        _monster.myAnimator.SetTrigger("Attack");
    }

    /// <summary>
    /// 공격 타입에 따른 애니메이션 트리거 원상복귀
    /// </summary>
    protected virtual void ResetAttackAnimation()
    {
        _monster.myAnimator.ResetTrigger("Attack");
    }

    /// <summary>
    /// 애니메이션을 통해 불러와진다.
    /// </summary>
    protected virtual void DoAttack()
    {
        // DO NOTHING
    }

    protected virtual void AttackExit()
    {
        _monster.myAnimator.ResetTrigger("Attack");
        if(_attackCoroutine != null) StopCoroutine(_attackCoroutine);
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
        // Vector3 targetDir = new Vector3(_target.transform.position.x - transform.position.x, 0f, _target.transform.position.z - transform.position.z);
        // Physics.Raycast(new Vector3(transform.position.x, 0.5f, transform.position.z), targetDir, out RaycastHit hit, 30f, _checkLayerMask);

        _distance = Vector3.Distance(_target.transform.position, transform.position);
        
        LookTarget();

        /*
        if (hit.transform == null)
        {
            return false;
        }
        else if (hit.transform.CompareTag("Player") && _distance <= _attackRange)
        {
            return true;
        }
        */

        return _distance <= _attackRange;
    }

    protected virtual void LookTarget()
    {
        transform.LookAt(_target.transform);
    }

    /////////// 캐스트 관련////////////

    protected virtual void CastStart()
    {
    }

    protected virtual void CastUpdate()
    {
        // 타겟과의 거리가 공격 범위보다 커지면
        //if (Vector3.Distance(_target.transform.position, _monster.transform.position) > _attackRange)
        //{
        //    ChangeState(MONSTER_STATE.STATE_TRACE);
        //}

        DoCastingAction();
    }

    
    /// <summary>
    /// 캐스팅를 진행시킨다.
    /// </summary>
    protected virtual void DoCastingAction()
    {
        _cntCastTime += Time.deltaTime;
        _bar.CastUpdate();

        if(_cntCastTime >= _castTime)
        {
            _cntCastTime = 0;
            _readyCast = true;
            _attackType = 1;
            ChangeState(MONSTER_STATE.STATE_ATTACK);
        }
    }

    protected virtual void CastExit()
    {
    }

    /////////// 피해 입음 관련////////////

    /// <summary>
    /// dmg를 받아 피해를 입는다.
    /// </summary>
    public virtual void Damaged(float dmg, bool SetAnimation = true)
    {
        if (_currentState == MONSTER_STATE.STATE_DIE)
        {
            return;
        }

        else
        {
            DamagedProcess(dmg, SetAnimation);
        }
    }

    /// <summary>
    /// 데미지를 받았을때 진행할 알고리즘 목록
    /// </summary>
    protected virtual void DamagedProcess(float dmg, bool SetAnimation = true)
    {
        if(MasteryManager.Instance.currentMastery.currentMasteryChoices[5] == -1)
        {
            _monster.Damaged(dmg * 1.1f);

        }
        else
        {
            _monster.Damaged(dmg);
        }
        _bar.HpUpdate();

        bool isDeath = DeathCheck();
        

        if (isDeath) CheckDeathAndChange();
    }
    
    /// <summary>
    /// 데미지를 받았을때의 연출 목록
    /// </summary>
    protected virtual void ProductionDamaged()
    {
        if (_currentState == MONSTER_STATE.STATE_DIE)
        {
            return;
        }

        else
        {
            Knockback();
            FadeOutRedHitOutline();
            ShakeHitCamera();
        }
    }

    /// <summary>
    /// 연출 : 넉백
    /// </summary>
    protected virtual void Knockback()
    {
        _rigidBody.AddRelativeForce(_target.transform.forward * 10, ForceMode.Impulse);
    }

    /// <summary>
    /// 연출 : 몬스터 테두리 빨갛게
    /// </summary>
    protected virtual void FadeOutRedHitOutline()
    {
        Color c = _outlinable.FrontParameters.Color;
        _outlinable.FrontParameters.Color = new Color(c.r, c.g, c.b, 1f);
        if (_fadeOutRedTween == null) _fadeOutRedTween = _outlinable.FrontParameters.DOFade(0, 1).SetAutoKill(false);
        else _fadeOutRedTween.ChangeStartValue(_outlinable.FrontParameters.Color.a, 1).Restart();
    }

    /// <summary>
    /// 연출 : 카메라 흔들기
    /// </summary>
    protected virtual void ShakeHitCamera()
    {
        CameraManager.Instance.ShakeCamera(1, 0.3f, 0.2f);
    }

    /// <summary>
    /// [외부 호출] 데미지를 받았을때 이를 호출하도록 한다.
    /// </summary>
    public virtual float DamageCheck(float damage)
    {
        if (GetCanDamageCheck())
        {
            Debug.Log("Player -> " + gameObject.name);
            Damaged(damage);
            ProductionDamaged();
            _isImmune = true;

            return damage;
        }

        return 0;
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
        StopAllCoroutines();
        StartCoroutine(DoDeathAction());
    }

    /// <summary>
    /// 죽었을 때, 타이머를 통해 죽는 연출 및 이벤트 발생을 할 수 있도록 한다.
    /// </summary>
    protected virtual IEnumerator DoDeathAction()
    {
        _monster.myAnimator.SetTrigger("Die");
        gameObject.GetComponent<Collider>().enabled = false;

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
        _outlinable.enabled = false;

        transform.DOMoveY(transform.position.y - 10, 10).OnComplete(() => { DestroyImmediate(gameObject); });
        _monster.avatarObject.GetComponent<Renderer>().material.DOFade(0, 2);

        _bar.gameObject.SetActive(false);

        if (_fadeOutRedTween != null) _fadeOutRedTween.Kill();
    }

    /// <summary>
    /// 죽었는지 여부를 판단하여 적합한 행동을 실시한다.
    /// </summary>
    protected virtual bool CheckDeathAndChange()
    {
        if (DeathCheck())
        {
            switch (WeaponManager.Instance.GetWeapon().name)
            {
                case "sword":
                    MasteryManager.Instance.currentMastery.currentSwordMasteryExp += 10;
                    break;
                case "wand":
                    MasteryManager.Instance.currentMastery.currentWandMasteryExp += 10;
                    break;
                case "dagger":
                    MasteryManager.Instance.currentMastery.currentWandMasteryExp += 10;
                    break;
                case "blunt":
                    MasteryManager.Instance.currentMastery.currentWandMasteryExp += 10;
                    break;
                case "staff":
                    MasteryManager.Instance.currentMastery.currentWandMasteryExp += 10;
                    break;
            }
        
            Debug.Log(name + " Exp + 10");
            MasteryManager.Instance.UpdateCurrentExp();

            ChangeState(MONSTER_STATE.STATE_DIE);
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
        _monster.myAnimator.SetTrigger("Laugh");
        _navMeshAgent.enabled = false;
    }

    protected virtual void KillUpdate()
    {
        if (CheckAnimationOver("Laugh", 1.0f))
        {
            _monster.myAnimator.ResetTrigger("Laugh");
            _monster.myAnimator.SetTrigger("Laugh");
        }

        transform.RotateAround(_target.transform.position, Vector3.up, 5 * Time.deltaTime);
        LookTarget();
    }

    protected virtual void KillExit()
    {

    }

    /// <summary>
    /// 타겟의 사망 여부를 받고 상태를 전환한다.
    /// </summary>
    public virtual void TargetDeathCheck()
    {
        if (_currentState != MONSTER_STATE.STATE_KILL && _target.GetComponent<LivingEntity>().Hp <= 0)
        {
            ChangeState(MONSTER_STATE.STATE_KILL);
        }
    }


    /////////// 기타 ////////////

    /// <summary>
    /// 현재 진행중인 애니메이션이 time보다 더 많이 진행됐는지 여부를 체크한다.
    /// </summary>
    public bool CheckAnimationOver(int animNum, float time)
    {
        return _monster.myAnimator.GetCurrentAnimatorStateInfo(animNum).normalizedTime >= time;
    }

    /// <summary>
    /// 해당 이름의 애니메이션이 time보다 더 많이 진행됐는지 여부를 체크한다.
    /// </summary>
    public bool CheckAnimationOver(string name, float time)
    {
        AnimatorStateInfo info = _monster.myAnimator.GetCurrentAnimatorStateInfo(0);
        return info.IsName(name) && info.normalizedTime >= time;
    }

    /// <summary>
    /// 해당 애니메이션의 진행 결과를 체크한다.
    /// </summary>
    public float GetAnimationPercentTime(int animNum)
    {
        return _monster.myAnimator.GetCurrentAnimatorStateInfo(animNum).normalizedTime;
    }

    protected virtual void RigidStart()
    {
        _navMeshAgent.isStopped = true;
        Debug.Log("경직걸림");
        _monster.myAnimator.SetTrigger("Rigid");
    }

    protected virtual void StunStart()
    {
        _navMeshAgent.isStopped = true;
        Debug.Log("스턴걸림");
        StopAllCoroutines();
        _monster.myAnimator.SetTrigger("Stun");
    }

    protected virtual void FallStart()
    {
        _navMeshAgent.isStopped = true;
        Debug.Log("넘어짐걸림");
        StopAllCoroutines();
        _monster.myAnimator.SetTrigger("Fall");
    }

    protected virtual void RigidUpdate()
    {

    }

    protected virtual void StunUpdate()
    {

    }

    protected virtual void FallUpdate()
    {

    }
    protected virtual void RigidExit()
    {
        _monster.myAnimator.ResetTrigger("rigid");
        _navMeshAgent.isStopped = false;
        _monster.myAnimator.SetTrigger("Idle");
    }

    protected virtual void StunExit()
    {
        _monster.myAnimator.ResetTrigger("stun");
        _navMeshAgent.isStopped = false;
        _monster.myAnimator.SetTrigger("Idle");
    }

    protected virtual void FallExit()
    {
        _monster.myAnimator.ResetTrigger("fall");
        _navMeshAgent.isStopped = false;
        _monster.myAnimator.SetTrigger("Idle");
    }

}
