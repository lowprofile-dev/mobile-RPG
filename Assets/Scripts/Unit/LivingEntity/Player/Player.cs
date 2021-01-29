////////////////////////////////////////////////////
/*
    File Player.cs
    Class Player
    Enum PLAYERSTATE

    담당자 : 이신홍, 김의겸

    플레이어의 행동 및 입력 등을 관리하는 클래스.
*/
////////////////////////////////////////////////////

using Cinemachine;
using SimpleInputNamespace;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PLAYERSTATE
{
    PS_IDLE, PS_MOVE, PS_ATTACK, PS_EVADE, PS_DIE, PS_SKILL, PS_INTERACTING , PS_STUN , PS_RIGID , PS_FALL
}

/// <summary>
/// 플레이어를 관리하는 클래스
/// </summary>
public class Player : LivingEntity
{
    // 싱글톤
    public static Player Instance;

    [Header("베이스")]
    private PLAYERSTATE _cntState;                      // 현재 상태

    [Header("캐싱")]
    [SerializeField] private GameObject _playerAvatar;                  // 플레이어의 아바타 (껍데기)
    [SerializeField] private CharacterController characterController;   // 캐릭터 컨트롤러
    public Joystick joystick = null;                                    // 조이스틱
    public PartSelection selection;                                     // 아바타 부품
    private WeaponManager _weaponManager;                               // 무기 매니저
    private ItemManager _itemManager;                                   // 아이템 매니저
    private StatusManager _statusManager;                               // 스테이터스 매니저
    private EPOOutline.Outlinable playerOutlinable;                     // 아웃라이너


    [Header("외견 관련 (BASE)")]
    [SerializeField] private int headId;
    [SerializeField] private int hairId;
    [SerializeField] private int headAccId;


    [Header("상태 관련")]
    bool _isdead = false;                                   // 사망 여부
    private float _slowingFactor = 0f; public float SlowingFactor { get { return _slowingFactor; } set { _slowingFactor = value; } }

    [Header("움직임")]
    [SerializeField] private float turnSmoothTime = 0.1f;   // 부드럽게 움직이는 시간
    float turnSmoothVelocity;                               // 부드럽게 움직이는 속도
    private Vector3 direction;                              // 입력받은 움직임 방향
    private Vector3 moveDir;                                // 변환된 움직임 방향
    private float horizontal;                               // 조이스틱 X 입력
    private float vertical;                                 // 조이스틱 Y 입력
    private float vSpeed;                                   // 움직임 속도

    [Header("카메라")]
    [SerializeField] private Transform cam;                          // 메인 카메라
    [SerializeField] private GameObject playerFollowCam;             // 플레이어를 쫓는 카메라
    [SerializeField] private CinemachineFreeLook playerFreeLook;     // playerFollowCam에서의 캐싱
    [SerializeField] private CinemachineFreeLook minimapFreeLook;    // 미니맵에 사용되는 카메라
    [SerializeField] private FaceCam faceCam;                        // 얼굴 카메라

    [Header("UI")]
    [SerializeField] private GameObject _systemPanel;   // 시스템 알림 UI

    [Header("회피 관련")]
    [SerializeField] private float _initEvadeTime;      // 회피에 사용되는 시간
    private float _evadeTime;                           // 현재 회피 시간

    [Header("마스터리 관련")]
    private bool resurrection = false;                  // 부활
    private bool rage = false;                  // 부활

    [Header("공격 관련")]
    [SerializeField] public Transform skillPoint;       // 스킬 발사 지점
    private int _cntSkillType;                          // 현재 사용하는 스킬 정보
    public int currentCombo;                            // 현재 콤보 수
    private bool _canConnectCombo;                      // 콤보를 더 이을 수 있는지
    private bool _canWalkAttackFrezzing;                // 공격상태에서 걸을 수 있게 되는 시점

    [Header("무기 관련")]
    public bool weaponChanged = false;                  // 무기가 바뀌었는지 여부

    [Header("대쉬 관련")]
    public float dashSpeed;                             // 대쉬 스피드
    private bool _isRushing;                            // 대쉬하고 있는지 여부
    private Vector3 _prevRushPos;                       // 대쉬를 시작했을 때의 위치
    private float _rushTime;                            // 대쉬를 시작한 후의 시간

    [Header("던전 관련")]
    public int currentDungeonArea;                      // 현재 위치한 던전의 구역

    [Header("사운드 관련")]
    private float _footstepSoundTime = 0.3f;            // 발소리 간격
    private float _cntFootStepSound = 0f;               // 현재의 발소리 진행 
    private float _rushSoundTime = 0.24f;               // 대쉬소리 간격

    [Header("시각 관련")]
    [SerializeField] private ParticleSystem trailParticle1; // 횡 파티클 (회피 / 돌진)
    [SerializeField] private ParticleSystem trailParticle2; // 종 파티클 (회피 / 돌진)
    private List<Renderer> lRenderers;                      // 아웃라이너 목록

    [Header("낙사 관련")]
    private bool _isFalling = false;                        // 떨어졌는지 여부
    [HideInInspector] public Vector3 lastRoomTransformPos;  // 최근에 있던 방의 위치

    // property
    public GameObject playerAvater { get { return _playerAvatar; } }
    public bool canconnectCombo { get { return _canConnectCombo; } }
    public Vector3 getMoveDir { get { return moveDir; } }
    public bool isdead { get { return _isdead; } set { _isdead = value; } }
    public PLAYERSTATE cntState { get { return _cntState; } }
    public GameObject GetPlayer { get { return this.gameObject; } }
    public FaceCam FaceCam { get { return faceCam; } }
    public WeaponManager weaponManager { get { return _weaponManager; } }
    public ItemManager itemManager { get { return _itemManager; } }
    public StatusManager statusManager { get { return _statusManager; } }

    [Header("카드 관련")]
    public float fullHealthDamage;
    public float proportionalDamage;
    public float giantDamage;
    public float annoyedDamage;
    public float spellStrikeDamage;
    public bool canDealFullHealth;              // 플레이어의 체력이 100%일 때 데미지 증가
    public bool canDealProportional = false;    // 몬스터 남은 체력 비례 데미지
    public bool canDealGiant = false;           // 몬스터 체력 80% 이상일 때 데미지 증가
    public bool canDealAnnoyed = false;         // 피격 받았을 때 플레이어 가하는 데미지 2초간 증가
    public bool canDealSpellStrike = false;
    public bool hasSpellStrike = false;         // 스킬 사용 이후 1회 평타 강화
    public bool isAnnoyed = false;
    public float annoyedTime = 0f;


    ///////////////// 베이스 //////////////////

    private void Awake()
    {
        Instance = this;
    }

    protected override void InitObject()
    {
        base.InitObject();

        var _player = this;

        UILoaderManager.Instance.LoadPlayerUI();

        _itemManager = ItemManager.Instance;
        _statusManager = StatusManager.Instance;

        _CCManager = new CCManager(ref _player, "player");
        lRenderers = new List<Renderer>();
        _rushTime = 0.6f;
        _evadeTime = _initEvadeTime;
        _prevRushPos = Vector3.zero;
        moveDir = Vector3.forward;

        OffTrailParticles();

        _hp = StatusManager.Instance.finalStatus.maxHp;
        _stemina = StatusManager.Instance.finalStatus.maxStamina;

        selection = GetComponent<PartSelection>();
        selection.Start();
        selection.Init();
        SetParts();
        itemManager.SetItemToPlayer(this);

        myAnimator = GetComponent<Animator>();

        _canWalkAttackFrezzing = true;
        _cntState = PLAYERSTATE.PS_IDLE;
        EnterState();

        _weaponManager = WeaponManager.Instance;
        if (weaponManager.GetWeaponName() == null) weaponManager.SetWeapon("SWORD");
        else weaponManager.SetWeapon(weaponManager.GetWeaponName());

        SetUpPlayerCamera();
        MasteryManager.Instance.UpdateCurrentExp();
        playerOutlinable = gameObject.GetComponent<EPOOutline.Outlinable>();
        InitOutline();
    }

    protected override void Update()
    {
        base.Update();
        _CCManager.Update();
        SetUpPlayerCamera();
        TestCode();
        UpdateAll();
        UpdateState();
        ApplyGravity();
        if (isAnnoyed)
            annoyedTime += Time.deltaTime;

    }

    ///////////////// 외견 관련 //////////////////

    /// <summary>
    /// 파트를 변경해준다.
    /// </summary>
    private void SetParts()
    {
        if (headId != 0) selection.ChangeHeadPart(headId);
        if (hairId != 0) selection.ChangeHairPart(hairId);
        if (headAccId != 0) selection.ChangeHeadAccesoriesPart(headAccId);
    }

    ///////////////// 상태 관련 //////////////////

    /// <summary>
    /// 상태를 바꾼다.
    /// </summary>
    public void ChangeState(PLAYERSTATE cntState)
    {
        ExitState();
        _cntState = cntState;
        EnterState();
    }

    /// <summary>
    /// 상태에 돌입할때 실행되는 함수이다.
    /// </summary>
    public void EnterState()
    {
        switch (_cntState)
        {
            case PLAYERSTATE.PS_IDLE:
                IdleEnter();
                break;
            case PLAYERSTATE.PS_MOVE:
                MoveEnter();
                break;
            case PLAYERSTATE.PS_ATTACK:
                AttackEnter();
                break;
            case PLAYERSTATE.PS_EVADE:
                EvadeEnter();
                break;
            case PLAYERSTATE.PS_DIE:
                DieEnter();
                break;
            case PLAYERSTATE.PS_SKILL:
                SkillEnter();
                break;
            case PLAYERSTATE.PS_INTERACTING:
                InteractEnter();
                break;
        }
    }


    /// <summary>
    /// 해당 상태에 있을때 작동되는 함수이다.
    /// </summary>
    public void UpdateState()
    {
        switch (_cntState)
        {
            case PLAYERSTATE.PS_IDLE:
                IdleUpdate();
                break;
            case PLAYERSTATE.PS_MOVE:
                MoveUpdate();
                break;
            case PLAYERSTATE.PS_ATTACK:
                AttackUpdate();
                break;
            case PLAYERSTATE.PS_EVADE:
                EvadeUpdate();
                break;
            case PLAYERSTATE.PS_DIE:
                DieUpdate();
                break;
            case PLAYERSTATE.PS_SKILL:
                SkillUpdate();
                break;
            case PLAYERSTATE.PS_INTERACTING:
                InteractUpdate();
                break;
        }
    }

    /// <summary>
    /// 해당 상태를 나갔을때 작동되는 함수이다.
    /// </summary>
    public void ExitState()
    {
        switch (_cntState)
        {
            case PLAYERSTATE.PS_IDLE:
                IdleExit();
                break;
            case PLAYERSTATE.PS_MOVE:
                MoveExit();
                break;
            case PLAYERSTATE.PS_ATTACK:
                AttackExit();
                break;
            case PLAYERSTATE.PS_EVADE:
                EvadeExit();
                break;
            case PLAYERSTATE.PS_DIE:
                DieExit();
                break;
            case PLAYERSTATE.PS_SKILL:
                SkillExit();
                break;
            case PLAYERSTATE.PS_INTERACTING:
                InteractExit();
                break;
        }
    }



    ///////////////// 공통 관련 //////////////////

    private void UpdateAll()
    {
        CachingJoyStick();
        ChangeToMoveCheck();
        CheckToDie();
        PlayerHpRecovery();
        PlayerMpRecovery();
    }

    /// <summary>
    /// 조이스틱 캐싱
    /// </summary>
    public void CachingJoyStick()
    {
        if (joystick == null)
            joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<Joystick>();
    }


    /// <summary>
    /// 주기적으로 HP를 회복한다.
    /// </summary>
    private void PlayerHpRecovery()
    {
        if (Hp <= statusManager.finalStatus.maxHp)
        {
            if (Hp >= statusManager.finalStatus.maxHp)
            {
                _hp = statusManager.finalStatus.maxHp;
            }
            else _hp += statusManager.finalStatus.hpRecovery * Time.deltaTime;
        }
    }

    /// <summary>
    /// 주기적으로 MP를 회복한다.
    /// </summary>
    private void PlayerMpRecovery()
    {
        if (Stemina <= statusManager.finalStatus.maxStamina)
        {
            if (Stemina >= statusManager.finalStatus.maxStamina)
            {
                _stemina = statusManager.finalStatus.maxStamina;
            }
            else _stemina += statusManager.finalStatus.staminaRecovery * Time.deltaTime;
        }
    }



    ///////////////// 대기 관련 //////////////////

    private void IdleEnter()
    {
        myAnimator.SetTrigger("Idle");
        IdleRushOutCheck();
    }

    /// <summary>
    /// 버그 막기용. Idle에서 Rush가 살아있을 경우를 체크한다.
    /// </summary>
    private void IdleRushOutCheck()
    {
        if (_isRushing) RushExit();
    }

    public void IdleUpdate()
    {
    }


    public void IdleExit()
    {
        myAnimator.ResetTrigger("Idle");
    }




    ///////////////// 이동 관련 //////////////////

    private void MoveEnter()
    {
        _cntFootStepSound = _footstepSoundTime / 2;
        myAnimator.SetTrigger("Move");
    }

    public void MoveUpdate()
    {
        PlayerMove();
        FootstepSound();
    }

    /// <summary>
    /// 발자국 소리, 던전일 시 울림 효과
    /// </summary>
    private void FootstepSound()
    {
        _cntFootStepSound -= Time.deltaTime;
        if(_cntFootStepSound < 0)
        {
            _cntFootStepSound = _footstepSoundTime;

            AudioSource source = SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Footsteps/LightArmorRun" + UnityEngine.Random.Range(1, 7), 0.2f);
            if (UILoaderManager.Instance.IsSceneDungeon()) SoundManager.Instance.SetAudioReverbEffect(source, AudioReverbPreset.Cave);
        }
    }

    /// <summary>
    /// 입력을 받아 플레이어를 움직인다.
    /// </summary>
    public void PlayerMove()
    {
        GetJoystickInput();

        if (direction.magnitude >= 0.1f) // 충분한 변화가 이뤄졌다면 이동
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDir.normalized * statusManager.finalStatus.moveSpeed * Time.deltaTime);
        }
    }

    public void MoveExit()
    {
        myAnimator.ResetTrigger("Move");
    }



    ///////////////// 공격 관련 //////////////////

    private void AttackEnter()
    {
        ToNextCombo();
        SetAttackAnimationTrigger();
    }

    public void AttackUpdate()
    {
        // 아무것도 하지 않는다.
    }

    /// <summary>
    /// [ANIMATION EVENT]자연적으로 공격이 끝난 케이스
    /// </summary>
    public void EndAttack()
    {
        hasSpellStrike = false;
        InitAttack();
        ChangeState(PLAYERSTATE.PS_IDLE);
        UnlockAttackFreeze();
    }

    /// <summary>
    /// 콤보 공격 및 애니메이션 정보를 초기화한다.
    /// </summary>
    public void InitAttack()
    {
        _canConnectCombo = false;
        currentCombo = 0;

        myAnimator.ResetTrigger("Attack01");
        myAnimator.ResetTrigger("Attack02");
        myAnimator.ResetTrigger("Attack03");
    }

    /// <summary>
    /// [ANIMATION EVENT] 아직 콤보를 이을 수 없는 상태. 애니메이션을 기다려야 한다.
    /// </summary>
    public void EndNextCombo()
    {
        _canConnectCombo = false;
    }

    /// <summary>
    /// [ANIMATION EVENT] 다음 콤보를 이을 수 있도록 조건을 ON한다.
    /// </summary>
    public void UnlockNextCombo()
    {
        _canConnectCombo = true;
    }

    /// <summary>
    /// [ANIMATION EVENT] 공격 이벤트를 실시한다.
    /// </summary>
    public void DoAttack()
    {
        switch (currentCombo)
        {
            case 1: weaponManager.GetWeapon().Attack(); break;
            case 2: weaponManager.GetWeapon().Attack2(); break;
            case 3: weaponManager.GetWeapon().Attack3(); break;
        }
    }

    /// <summary>
    /// 공격상태에서 다시 움직일 수 있게 해준다.
    /// </summary>
    public void UnlockAttackFreeze()
    {
        _canWalkAttackFrezzing = true;
    }

    /// <summary>
    /// 현재 콤보 상태에 따라 공격 애니메이션을 재생한다.
    /// </summary>
    private void SetAttackAnimationTrigger()
    {
        _canWalkAttackFrezzing = false;

        switch (currentCombo)
        {
            case 1: myAnimator.SetTrigger("Attack01"); break;
            case 2: myAnimator.SetTrigger("Attack02"); break;
            case 3: myAnimator.SetTrigger("Attack03"); break;
        }
    }

    /// <summary>
    /// 다음 콤보로 넘어간다.
    /// </summary>
    public void ToNextCombo()
    {
        currentCombo++;
        if (currentCombo > 3) currentCombo = 1;
    }

    public void AttackExit()
    {
        UnlockAttackFreeze();
        InitAttack();
    }



    ///////////////// 회피 관련 //////////////////

    private void EvadeEnter()
    {
        OnTrailparticles();
        UseStemina(statusManager.playerStatus.dashStamina);
        myAnimator.SetTrigger("Avoid");
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Player/Dash", 0.5f);
    }

    public void EvadeUpdate()
    {
        PlayerAvoidance();
        CheckEvadeOver();
    }

    /// <summary>
    /// 회피를 실행하고 체크한다.
    /// </summary>
    private void PlayerAvoidance()
    {
        _evadeTime -= Time.deltaTime;
        if (direction == Vector3.zero) characterController.Move(moveDir * dashSpeed * Time.deltaTime * (_evadeTime / _initEvadeTime));
        else characterController.Move(moveDir * dashSpeed * Time.deltaTime * (_evadeTime / _initEvadeTime));
    }

    /// <summary>
    /// 회피가 끝난 것을 체크한다.
    /// </summary>
    private void CheckEvadeOver()
    {
        if (_evadeTime <= 0f) ChangeState(PLAYERSTATE.PS_IDLE);
    }

    public void EvadeExit()
    {
        OffTrailParticles();
        _evadeTime = _initEvadeTime;
        myAnimator.ResetTrigger("Avoid");
    }



    ///////////////// 스킬 관련 //////////////////

    private void SkillEnter()
    {
        SetSkillAnimation();
        SkillStartSoundPlay();
    }

    /// <summary>
    /// 스킬 시전 즉시 애니메이션과 별도로 재생되어야 하는 사운드들을 넣는다.
    /// </summary>
    public void SkillStartSoundPlay()
    {
        if (weaponManager.GetWeaponName() == "WAND" && _cntSkillType == 2) SoundManager.Instance.PlayEffect(SoundType.EFFECT, "SkillEffect/Sword Skill 3 Holy", 0.6f);
    }

    /// <summary>
    /// 스킬 선택에 따른 스킬 재생
    /// </summary>
    private void SetSkillAnimation()
    {
        switch (_cntSkillType)
        {
            case 0: myAnimator.SetTrigger("SkillA"); break;
            case 1: myAnimator.SetTrigger("SkillB"); break;
            case 2: myAnimator.SetTrigger("SkillC"); break;
        }
    }

    /// <summary>
    /// [ANIMATION EVENT] 스킬에 대한 이벤트 발생 (파티클 생성 등) 1번째 케이스
    /// </summary>
    public void AddSkillAttackTime1()
    {
        switch (_cntSkillType)
        {
            case 0: PlayerSkillA(); break;
            case 1: PlayerSkillB(); break;
            case 2: PlayerSkillC(); break;
        }
    }

    /// <summary>
    /// [ANIMATION EVENT] 스킬에 대한 이벤트 발생 (파티클 생성 등) 2번째 케이스
    /// </summary>
    public void AddSkillAttackTime2()
    {
        switch (_cntSkillType)
        {
            case 0: PlayerSkillA2(); break;
            case 1: PlayerSkillB2(); break;
            case 2: PlayerSkillC2(); break;
        }
    }

    public void PlayerSkillA()
    {
        weaponManager.GetWeapon().SkillA();
        InitAttack();
    }

    public void PlayerSkillB()
    {
        if (weaponManager.GetWeapon().CheckSkillB())
        {
            weaponManager.GetWeapon().SkillB();
            InitAttack();
        }
    }

    public void PlayerSkillC()
    {
        if (weaponManager.GetWeapon().CheckSkillC())
        {
            weaponManager.GetWeapon().SkillC();
            InitAttack();
        }
    }

    private void PlayerSkillA2()
    {
        weaponManager.GetWeapon().SkillA2();
        InitAttack();
    }

    private void PlayerSkillB2()
    {
        weaponManager.GetWeapon().SkillB2();
        InitAttack();
    }

    private void PlayerSkillC2()
    {
        weaponManager.GetWeapon().SkillC2();
        InitAttack();
    }

    /// <summary>
    /// [ANIMATION EVENT] 스킬이 끝났을때 IDLE 상태로 돌림
    /// </summary>
    public void SkillOver()
    {
        ChangeState(PLAYERSTATE.PS_IDLE);
    }

    public void SkillUpdate()
    {
        Rush();
    }

    /// <summary>
    /// 스킬 선택에 따른 스킬 애니메이션 취소
    /// </summary>
    public void SkillExit()
    {
        myAnimator.ResetTrigger("SkillA");
        myAnimator.ResetTrigger("SkillB");
        myAnimator.ResetTrigger("SkillC");
        myAnimator.ResetTrigger("SkillBAttack");
        hasSpellStrike = true;
    }

    /// <summary>
    /// 돌진 상태를 시작한다. (돌진은 스킬 상태일때만 작동한다.)
    /// </summary>
    public void RushEnter()
    {
        OnTrailparticles();
        _isRushing = true;
        _rushTime = 0.6f;
        _cntFootStepSound = _rushSoundTime / 2;
    }

    /// <summary>
    /// 돌진 상태일때 돌진한다.
    /// </summary>
    public void Rush()
    {
        if (_isRushing) // 돌진상태일때
        {
            RushSound();

            characterController.Move(transform.forward * statusManager.finalStatus.moveSpeed * 5 * Time.deltaTime);
            speed = statusManager.finalStatus.moveSpeed;
            RaycastHit hit = new RaycastHit();

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 14; j++)
                {
                    if (Physics.Raycast(transform.position + new Vector3((i - 2) * 0.6f, (j - 4) * 0.6f, 0), transform.forward, out hit, 2, LayerMask.GetMask("Monster")))
                    {
                        RushExit();
                        return;
                    }
                }
            }

            _rushTime -= Time.deltaTime;

            if (_rushTime < 0 || Vector3.Distance(_prevRushPos, transform.position) < 0.3f) // 최대 시간 / 막혔을때의 탈출
            {
                RushExit();
            }

            _prevRushPos = transform.position;
        }
    }

    private void RushSound()
    {
        // 사운드 관련
        _cntFootStepSound -= Time.deltaTime;
        if (_cntFootStepSound < 0)
        {
            _cntFootStepSound = _rushSoundTime;
            AudioSource source = SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Footsteps/Heavy Armor Running " + UnityEngine.Random.Range(1, 5), 0.65f);
            SoundManager.Instance.SetAudioReverbEffect(source, AudioReverbPreset.Hallway);
        }
    }

    /// <summary>
    /// 돌진이 끝나며 일어나는 일들
    /// </summary>
    public void RushExit()
    {
        OffTrailParticles();
        _isRushing = false;
        _rushTime = 0.6f;

        if (weaponManager.GetWeaponName() == "SWORD" && _cntSkillType == 1)
        {
            myAnimator.SetTrigger("SkillBAttack");
            myAnimator.ResetTrigger("SkillB");
            SoundManager.Instance.PlayEffect(SoundType.EFFECT, "SkillEffect/Sword Skill 2 Shock", 0.6f);
        }
    }



    ///////////////// 상호작용 관련 //////////////////

    private void InteractEnter()
    {

    }

    public bool CheckThereisObject()
    {
        return Physics.OverlapSphere(transform.position, 3.0f, LayerMask.GetMask("NPC")).Length > 0;
    }

    /// <summary>
    /// 눌렀을 시 주변의 NPC와 상호작용한다.
    /// </summary>
    public void CheckInteractObject()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3.0f);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].GetComponent<NonLivingEntity>())
            {
                colliders[i].GetComponent<NonLivingEntity>().Interaction();
                break;
            }
        }
    }

    /// <summary>
    /// 이후에는 누를때마다 상호작용이 이루어진다.
    /// </summary>
    public void ContinueInteract()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckInteractObject();
        }
    }

    public void InteractUpdate()
    {
        ContinueInteract();
    }

    public void InteractExit()
    {

    }




    ///////////////// 사망 관련 //////////////////

    private void DieEnter()
    {
        if (_isdead) return;

        _isdead = true;

        myAnimator.SetTrigger("Die");

        _CCManager.Release();
        _DebuffManager.Release();
        _slowingFactor = 0f;

        if (MasteryManager.Instance.resurrection == false)
        {
            SoundManager.Instance.PlayBGM("FailBGM", 0.6f);
            Invoke("DieExit", 3f);
        }
    }

    public void DieUpdate()
    {
        if (MasteryManager.Instance.resurrection == true)
        {
            _isdead = false;
            //Debug.log("마스터리 특성 부활!");
            GameObject resurrectionEffect = ObjectPoolManager.Instance.GetObject("SakuraTargetBuff");
            resurrectionEffect.transform.position = transform.position + new Vector3(0f,-1f,0f);
            SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Player/Resurrection", 0.5f);

            _hp = statusManager.finalStatus.maxHp;
            ChangeState(PLAYERSTATE.PS_IDLE);
        }
    }

    public void DieExit()
    {
        _isdead = false;
        myAnimator.ResetTrigger("Die");
        if (MasteryManager.Instance.resurrection == true)
        {
            MasteryManager.Instance.resurrection = false;
        }
        else UILoaderManager.Instance.LoadVillage();
    }




    ///////////////// 전환 관련 //////////////////

    /// <summary>
    /// 조이스틱이 눌렸을때 조건에 맞는 상태라면 상태를 전환시킨다.
    /// </summary>
    public void ChangeToMoveCheck()
    {
        if (GetMove())
        {
            if (_cntState == PLAYERSTATE.PS_IDLE || (_cntState == PLAYERSTATE.PS_ATTACK && _canWalkAttackFrezzing))
            {
                ChangeState(PLAYERSTATE.PS_MOVE);
            }
        }

        else
        {
            if (_cntState == PLAYERSTATE.PS_MOVE)
            {
                ChangeState(PLAYERSTATE.PS_IDLE);
            }
        }
    }

    /// <summary>
    /// 공격 버튼이 눌렸을때 조건에 맞는 상태라면 상태를 전환시킨다.
    /// </summary>
    public void ChangeToAttackCheck()
    {
        if (_cntState == PLAYERSTATE.PS_IDLE ||
        _cntState == PLAYERSTATE.PS_MOVE
        )
        {
            ChangeState(PLAYERSTATE.PS_ATTACK);
        }

        // 공격 중일때는 Change로 하지 않고, 다시 한번 Attack에 들어선다.
        else if (_cntState == PLAYERSTATE.PS_ATTACK && _canConnectCombo)
        {
            AttackEnter();
        }
    }

    /// <summary>
    /// 회피 버튼이 눌렸을때 조건에 맞는 상태라면 상태를 전환시킨다.
    /// </summary>
    public void ChangeToEvadeChack()
    {
        if (_stemina >= 2)
        {
            if (_cntState == PLAYERSTATE.PS_IDLE ||
            _cntState == PLAYERSTATE.PS_MOVE ||
            _cntState == PLAYERSTATE.PS_ATTACK)
            {
                ChangeState(PLAYERSTATE.PS_EVADE);
            }
        }
    }

    /// <summary>
    /// 죽는 상태로 변환하게 한다.
    /// </summary>
    public void CheckToDie()
    {
        if (_hp <= 0 && !isdead && _cntState != PLAYERSTATE.PS_DIE)
        {
            ChangeState(PLAYERSTATE.PS_DIE);
        }
    }



    ///////////////// 입력 관련 //////////////////

    /// <summary>
    /// 조이스틱의 입력을 받아 변환한다.
    /// </summary>
    private void GetJoystickInput()
    {
        if (joystick == null) joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<Joystick>();
        horizontal = joystick.GetX_axis().value;
        vertical = joystick.GetY_axis().value;

        direction = new Vector3(horizontal, 0, vertical).normalized;
    }

    /// <summary>
    /// 조이스틱이 눌리고 있는지 여부를 반환한다.
    /// </summary>
    public bool GetMove()
    {
        return joystick.getHold();
    }

    /// <summary>
    /// 공격 버튼이 눌림
    /// </summary>
    public void AttackBtnClicked()
    {
        if(!_isRushing)
        {
            ChangeToAttackCheck();
        }
    }

    /// <summary>
    /// 회피 버튼이 눌림
    /// </summary>
    public void EvadeBtnClicked()
    {
        if(!_isRushing)
        {
            ChangeToEvadeChack();
        }
    }

    /// <summary>
    /// 스킬 A 버튼이 눌림
    /// </summary>
    public void SkillABtnClicked()
    {
        if (!_isRushing)
        {
            if (_cntState != PLAYERSTATE.PS_DIE)
            {
                _cntSkillType = 0;
                ChangeState(PLAYERSTATE.PS_SKILL);
            }
        }
    }

    /// <summary>
    /// 스킬 B 버튼이 눌림
    /// </summary>
    public void SkillBBtnClicked()
    {
        if (!_isRushing)
        {
            if (_cntState != PLAYERSTATE.PS_DIE)
            {
                _cntSkillType = 1;
                ChangeState(PLAYERSTATE.PS_SKILL);

                if (weaponManager.GetWeaponName() == "SWORD") PlayerSkillB(); // 애니메이션을 통해 반복적으로 불러오는 스킬이 아니기때문에 대쉬 스킬인 Sword B Skill을 눌렀을때 바로 실행되도록 함.
            }
        }
    }

    /// <summary>
    /// 스킬 C 버튼이 눌림
    /// </summary>
    public void SkillCBtnClicked()
    {
        if (_cntState != PLAYERSTATE.PS_DIE)
        {
            _cntSkillType = 2;
            ChangeState(PLAYERSTATE.PS_SKILL);
        }
    }

    /// <summary>
    ///  현재 스테이트 리턴
    /// </summary>
    /// <returns></returns>
    public PLAYERSTATE GetState()
    {
        return _cntState;
    }

    ///////////////// 기타 //////////////////

    /// <summary>
    /// 현재 진행중인 애니메이션이 time보다 더 많이 진행됐는지 여부를 체크한다.
    /// </summary>
    public bool CheckAnimationOver(int animNum, float time)
    {
        return myAnimator.GetCurrentAnimatorStateInfo(animNum).normalizedTime >= time;
    }

    /// <summary>
    /// 해당 이름의 애니메이션이 time보다 더 많이 진행됐는지 여부를 체크한다.
    /// </summary>
    public bool CheckAnimationOver(string name, float time)
    {
        AnimatorStateInfo info = myAnimator.GetCurrentAnimatorStateInfo(0);
        return info.IsName(name) && info.normalizedTime >= time;
    }

    /// <summary>
    /// 해당 애니메이션의 진행 결과를 체크한다.
    /// </summary>
    public float GetAnimationPercentTime(int animNum)
    {
        return myAnimator.GetCurrentAnimatorStateInfo(animNum).normalizedTime;
    }

    /// <summary>
    /// [연출] 따라다니는 트레일 파티클을 끈다.
    /// </summary>
    public void OffTrailParticles()
    {
        trailParticle1.Stop();
        trailParticle2.Stop();
    }

    /// <summary>
    /// [연출] 따라다니는 트레일 파티클을 켠다.
    /// </summary>
    public void OnTrailparticles()
    {
        trailParticle1.Play();
        trailParticle2.Play();
    }



    ///////////////// 카메라 관련 //////////////////

    /// <summary>
    /// 플레이어와 관련된 카메라들을 세팅한다.
    /// </summary>
    private void SetUpPlayerCamera()
    {
        SetPlayerFaceCam();
        SetPlayerFollowCam();
        SetMinimapFreeLook();
        if (cam != null) return;
        if (SceneManager.GetActiveScene().name == "DungeonScene") cam = GameObject.FindGameObjectWithTag("DungeonMainCamera").transform;
        else cam = GameObject.FindGameObjectWithTag("PlayerFollowCamera").transform;
    }

    /// <summary>
    /// 플레이어 얼굴을 비추는 카메라를 세팅한다.
    /// </summary>
    private void SetPlayerFaceCam()
    {
        if (faceCam != null) return;
        faceCam = GameObject.Find("PlayerFaceCam").GetComponent<FaceCam>();
        faceCam.InitFaceCam(_playerAvatar);
    }

    /// <summary>
    /// 플레이어를 추적하는 카메라를 세팅한다.
    /// </summary>
    private void SetPlayerFollowCam()
    {
        if (playerFollowCam != null)
            return;
        playerFollowCam = GameObject.FindGameObjectWithTag("PlayerFollowCamera");
        playerFreeLook = playerFollowCam.GetComponent<CinemachineFreeLook>();
        playerFreeLook.Follow = transform;
        playerFreeLook.LookAt = transform;
    }

    /// <summary>
    /// 미니맵 카메라를 세팅한다.
    /// </summary>
    private void SetMinimapFreeLook()
    {
        if (minimapFreeLook != null || SceneManager.GetActiveScene().name != "DungeonScene")
            return;
        minimapFreeLook = GameObject.FindGameObjectWithTag("MinimapCamera").GetComponent<CinemachineFreeLook>();
        minimapFreeLook.Follow = transform;
        minimapFreeLook.LookAt = transform;
    }

    /// <summary>
    /// FaceCam을 갱신한다.
    /// </summary>
    public void ChangeFaceCamera()
    {
        faceCam.InitFaceCam(_playerAvatar);
    }



    ///////////////// 아웃라인 관련 //////////////////

    public void InitOutline()
    {
        lRenderers.Clear();
        playerOutlinable.outlineTargets.Clear();
        GetRenderers(transform);
        for (int i = 0; i < lRenderers.Count; i++)
        {
            EPOOutline.OutlineTarget outline = new EPOOutline.OutlineTarget(lRenderers[i], 0);
            outline.CullMode = UnityEngine.Rendering.CullMode.Off;
            playerOutlinable.outlineTargets.Add(outline);
        }
    }

    private void GetRenderers(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.gameObject.GetComponent<Renderer>() != null)
            {
                lRenderers.Add(child.gameObject.GetComponent<Renderer>());
            }
            if (child.childCount > 0)
            {
                GetRenderers(child);
            }
        }
    }




    ///////////////// 기타 캐릭터 기능들 //////////////////

    public override void Damaged(float damage, bool noArmorDmg)
    {
        base.Damaged(damage, noArmorDmg);
        UIVignette.Instance.ShowDamagedAnimation();
        AudioSource source = SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Etc/Hit " + UnityEngine.Random.Range(1, 6), 0.6f);
        SoundManager.Instance.SetPitch(source, 1.5f);
        isAnnoyed = true;
        annoyedTime = 0f;
    }

    public override float GetArmorFromDamaged(float damage)
    {
        return damage * (float)((100 - (statusManager.finalStatus.armor)) / 100); // Armor만큼의 비율만큼 데미지를 덜 받는다 (armor 30 = 30% 데미지 감소)
    }

    public void RestoreHP(float restoreHp)
    {
        if(_cntState != PLAYERSTATE.PS_DIE && restoreHp != 0)
        {
            _hp += restoreHp;
            if (_hp > StatusManager.Instance.finalStatus.maxHp) _hp = StatusManager.Instance.finalStatus.maxHp;
            ObjectPoolManager.Instance.GetObject(DamageText, transform.position, Quaternion.identity).GetComponent<DamageText>().PlayRestore((int)restoreHp);
        }
    }


    private void ApplyGravity()
    {
        //낙사
        if (transform.position.y < -70 && !_isdead && _cntState != PLAYERSTATE.PS_DIE)
        {
            _isFalling = true;
            Damaged(statusManager.finalStatus.maxHp * 0.1f, true);
            ReturnToGround();
            return;
        }

        if (characterController.isGrounded)
        {
            vSpeed = 0;
        }

        vSpeed = vSpeed - Time.deltaTime * 274f;
        characterController.Move(Vector3.up * vSpeed * Time.deltaTime);
    }

    private void ReturnToGround()
    {
        _isFalling = false;
        transform.position = lastRoomTransformPos;
    }

    ///////////////// 테스트 관련 //////////////////

    /// <summary>
    /// 테스트를 실시하는 코드 모음
    /// </summary>
    private void TestCode()
    {
        ChangeWeaponTest();
        ChangeMasteryLevelTest();
    }

    /// <summary>
    /// 마스터리 레벨을 올리는 테스트
    /// </summary>
    private void ChangeMasteryLevelTest()
    {
        //weaponManager.GetWeapon().MasteryLevelUp();

        if (Input.GetKeyDown(KeyCode.L))
        {
            weaponManager.AddExpToCurrentWeapon(80);
            MasteryManager.Instance.SaveCurrentMastery();
        }
    }

    /// <summary>
    /// 무기 테스트를 위한 변경 함수
    /// </summary>
    private void ChangeWeaponTest()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            weaponManager.SetWeapon("WAND");
            weaponChanged = true;
        }
        else weaponChanged = false;
    }
}
