////////////////////////////////////////////////////
/*
    File BossSkeletonKingAction.cs
    class BossSkeletonKingAction
    
    담당자 : 안영훈
    부 담당자 : 

    스켈레톤킹 보스 행동 스크립트
*/
////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
///
using UnityEngine;
using UnityEngine.AI;

public class BossSkeletonKingAction : MonsterAction
{
    enum AttackType { AIR_ATTACK, ATTACK1, ATTACK2, BLACKHOLE, JUMP_ATTACK, SUMMON }

    [SerializeField] private GameObject attackPos;

    AttackType attackType;
    [Header("스킬 이펙트, 범위")] 
    [SerializeField] private GameObject JumpSkillRange;
    [SerializeField] private GameObject AttackRange;
    [SerializeField] private GameObject AttackEffect;
    [SerializeField] private GameObject AirSkillRange;
    [SerializeField] private GameObject AirSkillEffect;
    [SerializeField] private GameObject BlackHoleRange;
    [SerializeField] private GameObject BlackHoleEffect;

    [Header("소환수")]
    [SerializeField] private GameObject skeleton_grunt;
    [SerializeField] private GameObject skeleton_sword;

    [Header("스킬 캐스팅 시간")]
    [SerializeField] private float defalutAtkCastingTime;
    [SerializeField] private float blackHoleCastingTime;
    [SerializeField] private float summonCastingTime;
    [SerializeField] private float AirSkillCastingTime;

    BossSpawnPoint SpawnPoints = null;
    bool IsSummonSpawn = false;

    List<Transform> ProjectileList = new List<Transform>();
    List<GameObject> monsterList = new List<GameObject>();

    private GameObject currentTarget;

    string currentAnimation = null;

    public override void InitObject()
    {
        base.InitObject();
        SpawnPoints = GameObject.FindWithTag("BossSpawnPoint").GetComponent<BossSpawnPoint>();
    }


    protected override void DoAttack() // 공격 마무리 행동
    {
        if (_attackCoroutine != null) StopCoroutine(_attackCoroutine);
        _readyCast = false;

        MakeEffect();

        _navMeshAgent.isStopped = false;
        _navMeshAgent.acceleration = 8f;
        CameraManager.Instance.ShakeCamera(3, 1, 0.5f);
        currentTarget = _target;
        ChangeState(MONSTER_STATE.STATE_TRACE);

    }
    /// <summary>
    /// 스킬 이펙트 생성 관련
    /// </summary>
    private void MakeEffect()
    {
        switch (attackType)
        {
            case AttackType.ATTACK1:
            case AttackType.ATTACK2:
            case AttackType.JUMP_ATTACK:
                DefalutAttackEffect();
                break;
            case AttackType.SUMMON:
                break;
            default:
                break;
        }
    }
    private void DefalutAttackEffect()
    {
        transform.LookAt(_target.transform.position);
        BossAttack atk = ObjectPoolManager.Instance.GetObject(AttackEffect).GetComponent<BossAttack>();
        atk.SetParent(gameObject, attackPos.transform);
        atk.PlayAttackTimer(0.2f);
        atk.OnLoad(gameObject, attackPos);

    }

    private void AirAttackEffect(Transform target)
    {
        BossAttack atk = ObjectPoolManager.Instance.GetObject(AirSkillEffect).GetComponent<BossAttack>();
        atk.SetParent(gameObject, target);
        atk.PlayAttackTimer(0.2f);
        atk.OnLoad(currentTarget, currentTarget);
    }

    private void BlackHoleAttackEffect(Transform target)
    {
        BossAttack atk = ObjectPoolManager.Instance.GetObject(BlackHoleEffect).GetComponent<BossAttack>();
        atk.SetParent(gameObject, target);
        atk.PlayAttackTimer(4.5f);
        atk.OnLoad(currentTarget, currentTarget);
    }

    protected void ComboAttack()
    {
        _monster.myAnimator.SetTrigger("Attack1");
        currentAnimation = "Attack1";
    }

    protected override void SetAttackAnimation()
    {
        switch (attackType)
        {
            case AttackType.ATTACK1:
                _monster.myAnimator.SetTrigger("Attack0");
                currentAnimation = "Attack0";
                break;
            case AttackType.ATTACK2:
                _monster.myAnimator.SetTrigger("Attack1");
                currentAnimation = "Attack1";
                break;
            default:
                break;
        }
    }

    protected override void SpawnStart()
    {
        transform.LookAt(_target.transform.position);
        UILoaderManager.Instance.NameText.text = _monster.monsterName.ToString();
        _monster.myAnimator.SetTrigger("Spawn");
        _bar.gameObject.SetActive(false);
    }
    protected override void SpawnUpdate()
    {
        if (_monster.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Spawn") && _monster.myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            _monster.myAnimator.SetTrigger("Idle");
            _navMeshAgent.isStopped = true;
            Invoke("TimeDelay", 5f);
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

    /// <summary>
    /// 걷는 소리 (애니메이션 이벤트로 호출)
    /// </summary>
    private void WalkSound()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Monster Big Footstep " + UnityEngine.Random.Range(1, 4), 1f);
    }

    private void CastSoundStart()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/KingSkeleton/SkillCast", 0.7f);
    }

    protected override void CastStart()
    {
        if (Vector3.Distance(transform.position, _target.transform.position) <= _navMeshAgent.stoppingDistance)
        {
            _navMeshAgent.isStopped = true;
            _monster.myAnimator.SetTrigger("Idle");
        }
        else
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_target.transform.position);
            _monster.myAnimator.SetTrigger("Walk");
        }

        //캐스팅을 시작할 때 공격 패턴을 정하고 그에 따른 공격타입과 캐스팅 시간을 정해준다.
        int proc = UnityEngine.Random.Range(0, 100);

        if (monsterList.Count == 0 && !IsSummonSpawn) // 필드에 소환된 소환수들이 없을경우
        {
            _castTime = summonCastingTime;
            attackType = AttackType.SUMMON;
            IsSummonSpawn = true;
            return;
        }

        if (proc <= 25)
        {
            _castTime = defalutAtkCastingTime;
            attackType = AttackType.ATTACK1;
        }
        else if (proc <= 50)
        {
            _castTime = defalutAtkCastingTime;
            attackType = AttackType.ATTACK2;
        }
        else if (proc <= 75)
        {
            _castTime = AirSkillCastingTime;
            attackType = AttackType.AIR_ATTACK;
        }
        else if (proc <= 100)
        {
            _castTime = blackHoleCastingTime;
            attackType = AttackType.BLACKHOLE;
        }

    }

    protected override void DoCastingAction()
    {
        _cntCastTime += Time.deltaTime;
        _bar.CastUpdate();

        if (attackType != AttackType.ATTACK2 && attackType != AttackType.ATTACK1)
        {
            _navMeshAgent.isStopped = true;
            if (!_monster.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("HoldAttack"))
            {
                _monster.myAnimator.SetTrigger("HoldAttack");
            }
        }

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
        foreach (var item in monsterList)
        {
            if (item.activeSelf == false)
            {
                monsterList.Remove(item);
                break;
            }
        }
    }

    public override void MoveToTarget()
    {

        _navMeshAgent.SetDestination(_target.transform.position);

        if (Vector3.Distance(_target.transform.position, _monster.transform.position) < _attackRange)
        {
            ChangeState(MONSTER_STATE.STATE_CAST);
        }

    }

    protected override IEnumerator AttackTarget()
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
        
    }

    private void AttackAction()
    {
        if (attackType != AttackType.JUMP_ATTACK)
            _monster.myAnimator.SetTrigger("Walk");
        _navMeshAgent.SetDestination(_target.transform.position);

        switch (attackType)
        {
            case AttackType.AIR_ATTACK:
                StartCoroutine(AirAction());
                break;
            case AttackType.ATTACK1: case AttackType.ATTACK2:
                StartCoroutine(DefalutAttackAction());
                break;
            case AttackType.BLACKHOLE:
                StartCoroutine(BlackHoleAction());
                break;
            case AttackType.JUMP_ATTACK:
                StartCoroutine(JumpAttackAction());
                break;
            case AttackType.SUMMON:
                StartCoroutine(SummonAction());
                break;
            default:
                break;
        }
    }

    private void DoAttackVoice()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/KingSkeleton/AttackVoice" + UnityEngine.Random.Range(1, 4), 0.8f);
    }

    private void DoBlackholeSound()
    {
        StartCoroutine(BlackholeSoundCoroutine());
    }

    private IEnumerator BlackholeSoundCoroutine()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/KingSkeleton/Blackhole1", 0.7f);
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/KingSkeleton/Blackhole2", 0.7f);
        AudioSource source = SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/KingSkeleton/BlackholeLoop", 1f, 0, true);
        yield return new WaitForSeconds(8);
        source.Stop();
    }

    /// <summary>
    /// 블리딩 어택 시작 사운드
    /// </summary>
    private void StartStunAttackSound()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/KingSkeleton/BleedingSkillStart", 0.7f);
    }

    /// <summary>
    /// 블리딩 어택 땅에 충돌 사운드
    /// </summary>
    private void EndStunAttackSound()
    {
        DoAttackVoice();
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/KingSkeleton/BleedingSkillEnd", 0.7f);
    }

    /// <summary>
    /// AirAttack 폭발 사운드
    /// </summary>
    private void DoExplosionSound()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/KingSkeleton/Explosion1", 0.5f);
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/KingSkeleton/Explosion2", 0.5f);
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/KingSkeleton/Explosion3", 0.5f);
    }

    /// <summary>
    /// 부하 스폰 사운드
    /// </summary>
    private void DoSpawnSound()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/KingSkeleton/Spawn", 0.5f);
    }

    /// <summary>
    /// 패턴 공격 관련
    /// </summary>
    private IEnumerator SummonAction() // 소환 스킬 행동
    {
        yield return null;

        _monster.myAnimator.SetTrigger("Summon");
        currentAnimation = "Summon";

        DoSpawnSound();

        for (int i = 0; i < SpawnPoints.Points.Length; i++)
        {
            GameObject eft = ObjectPoolManager.Instance.GetObject(SpawnPoints.SpawnEffect);
            eft.transform.position = new Vector3(SpawnPoints.Points[i].position.x, transform.position.y, SpawnPoints.Points[i].position.z);
        }

        yield return new WaitForSeconds(3f);

        for (int i = 0; i < SpawnPoints.Points.Length; i++)
        {
            int rnd = UnityEngine.Random.Range(0, 1);
            GameObject mon;
            if (rnd == 0) mon = ObjectPoolManager.Instance.GetObject(skeleton_grunt);
            else mon = ObjectPoolManager.Instance.GetObject(skeleton_sword);

            mon.transform.position = SpawnPoints.Points[i].TransformPoint(0, 0, 0);
            monster.GetComponent<MonsterAction>().parentRoom = parentRoom;
            mon.GetComponent<Monster>().InitMonster();
            mon.GetComponent<MonsterAction>().SpawnPos = SpawnPoints.Points[i].TransformPoint(0, 0, 0);
            monsterList.Add(mon);
        }
        IsSummonSpawn = false;
    }

    private IEnumerator JumpAttackAction() // 내려치기 공격 행동
    {
        _navMeshAgent.stoppingDistance = 0f;
        currentTarget = _target;
        _navMeshAgent.SetDestination(_target.transform.position);
        _navMeshAgent.isStopped = false;
        _navMeshAgent.speed = _monster.speed * 1.5f;
        _navMeshAgent.acceleration = 500f;
        transform.LookAt(_target.transform.position);

        _monster.myAnimator.SetTrigger("Jump");
        currentAnimation = "Jump";

        yield return new WaitForSeconds(_monster.myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        _navMeshAgent.acceleration = 8f;
        _navMeshAgent.speed = _monster.speed;
        _navMeshAgent.stoppingDistance = 3f;
    }

    private IEnumerator AirAction() // 폭발 공격 행동
    {
        _monster.myAnimator.SetTrigger("HoldAttack");

        yield return null;

        for (int i = 0; i < 10; i++) // 경고 범위 설정
        {
            GameObject range = ObjectPoolManager.Instance.GetObject(AirSkillRange);
            range.GetComponent<BossSkillRange>().RemovedRange(gameObject, _attackSpeed);
            range.transform.position =
                new Vector3(UnityEngine.Random.Range(transform.position.x - 20, transform.position.x + 20), transform.position.y, UnityEngine.Random.Range(transform.position.z - 20, transform.position.z + 20));
            ProjectileList.Add(range.transform);
        }

        yield return new WaitForSeconds(_attackSpeed);

        DoExplosionSound();
        foreach (Transform item in ProjectileList) AirAttackEffect(item);

        ProjectileList.Clear();

        DoAttack();
    }

    private IEnumerator DefalutAttackAction() //기본 공격 행동
    {
        yield return null;

        _navMeshAgent.SetDestination(_target.transform.position);

        GameObject range = ObjectPoolManager.Instance.GetObject(AttackRange);
        range.GetComponent<BossSkillRange>().RemovedRange(gameObject, _attackSpeed);
        range.GetComponent<BossSkillRange>().setFollow();
    }

    private IEnumerator BlackHoleAction() // 블랙홀 공격 행동
    {
        _monster.myAnimator.SetTrigger("HoldAttack");
        yield return null;

        GameObject range = ObjectPoolManager.Instance.GetObject(BlackHoleRange);
        range.GetComponent<BossSkillRange>().RemovedRange(_target, _attackSpeed);
        range.transform.position = new Vector3(_target.transform.position.x, transform.position.y, _target.transform.position.z);

        yield return new WaitForSeconds(_attackSpeed);

        DoBlackholeSound();
        BlackHoleAttackEffect(range.transform);

        DoAttack();
    }

    /// <summary>
    /// 타겟 추적 관련
    /// </summary>
    protected override void TraceStart()
    {
        _navMeshAgent.SetDestination(_target.transform.position);

        _monster.myAnimator.ResetTrigger("Walk");

        if (Vector3.Distance(transform.position, _target.transform.position) >= _navMeshAgent.stoppingDistance)
        {
            _monster.myAnimator.SetTrigger("Idle");
        }
        else
        {
            _monster.myAnimator.SetTrigger("Walk");
        }
        _navMeshAgent.speed = _monster.speed * 1.5f;
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

    protected override void AttackUpdate()
    {
    }

    /// <summary>
    /// CC관련
    /// </summary>
    protected override void RigidStart()
    {
    }
    protected override void StunStart()
    {
        GameObject txt = ObjectPoolManager.Instance.GetObject(_monster.DamageText);
        txt.transform.SetParent(transform);
        txt.transform.localPosition = Vector3.zero;
        txt.transform.rotation = Quaternion.identity;
        txt.GetComponent<DamageText>().PlayText("CC 면역!", "monster");
    }
    protected override void RigidExit()
    {
    }
    protected override void StunExit()
    {
    }
    protected override void FallStart()
    {
        GameObject txt = ObjectPoolManager.Instance.GetObject(_monster.DamageText);
        txt.transform.SetParent(transform);
        txt.transform.localPosition = Vector3.zero;
        txt.transform.rotation = Quaternion.identity;
        txt.GetComponent<DamageText>().PlayText("CC 면역!", "monster");
    }
    protected override void FallExit() { }

    protected override void KillStart()
    {
        StopAllCoroutines();
        _monster.myAnimator.SetTrigger("Laugh");
        transform.LookAt(_target.transform.position);
    }

    protected override void KillUpdate()
    {
        if (!_monster.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Laugh"))
        {
            _monster.myAnimator.SetTrigger("Laugh");
        }
    }
    protected override void IdleStart()
    {
    }

    public override void DeathSound()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/KingSkeleton/Die", 0.7f);
    }
}
