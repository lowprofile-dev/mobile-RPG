using System;
using Cinemachine;
using SimpleInputNamespace;
using UnityEngine;

public enum PLAYERSTATE
{
    PS_IDLE, PS_MOVE, PS_ATTACK, PS_EVADE, PS_DIE, PS_SKILL, PS_INTERACTING
}

/// <summary>
/// 플레이어를 관리하는 클래스
/// </summary>
public class Player : LivingEntity
{
    public static Player Instance;

    private bool avoidButtonClick = false;
    [SerializeField] private float _initEvadeTime;
    private float _evadeTime;
    [SerializeField] private GameObject _playerAvatar; public GameObject playerAvater { get { return _playerAvatar; } }
    private PLAYERSTATE _cntState;

    [Header("상태이상")]
    private bool isStun = false;
    private bool isFall = false;
    private bool isRigid = false;

    [Header("버튼 입력")]
    private bool AttackButtonClick = false;
    private bool SkillA_ButtonClick = false;
    private bool SkillB_ButtonClick = false;
    private bool SkillC_ButtonClick = false;


    private float skillA_Counter = 0f;
    private float skillB_Counter = 0f;


    private float skillC_Counter = 0f;

    [SerializeField] public Transform firePoint;
    [SerializeField] public Transform skillPoint;

    [SerializeField] private CharacterController characterController;
    [SerializeField] private float speed = 6f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    [SerializeField] private Transform cam;
    [SerializeField] private GameObject playerFollowCam;
    [SerializeField] private CinemachineFreeLook playerFreeLook;
    [SerializeField] private CinemachineFreeLook minimapFreeLook;

    public bool weaponChanged = false;

    private Vector3 direction;
    private Vector3 moveDir; public Vector3 getMoveDir { get { return moveDir; } }
    [SerializeField] private Quaternion rotateAngle;

    public Joystick joystick = null;

    float horizontal;
    float vertical;
    float vSpeed;
    bool _isdead = false; public bool isdead { get { return _isdead; } }
    private int _cntSkillType;

    public float dashSpeed; // 대쉬 스피드
    public int currentCombo; // 현재 콤보 수
    private bool _canConnectCombo; public bool canconnectCombo { get { return _canConnectCombo; } } // 콤보를 더 이을 수 있는지

    PartSelection selection;
    FaceCam faceCam;

    public WeaponManager weaponManager;
    ItemManager itemManager;
    StatusManager statusManager;

    private void Awake()
    {
        Instance = this;
    }

    protected override void Start()
    {
        itemManager = ItemManager.Instance;
        statusManager = StatusManager.Instance;

        base.Start();

        _evadeTime = _initEvadeTime;
        SetUpPlayerCamera();
        moveDir = Vector3.forward;
    }

    protected override void InitObject()
    {
        initHp = statusManager.finalStatus.maxHp;
        initMp = statusManager.finalStatus.maxStamina;
        base.InitObject();

        selection = GetComponent<PartSelection>();
        selection.Start();
        selection.Init();

        myAnimator = GetComponent<Animator>();

        _cntState = PLAYERSTATE.PS_IDLE;
        EnterState();

        weaponManager = WeaponManager.Instance;
        weaponManager.SetWeapon("SWORD");
    }

    protected override void Update()
    {
        TestCode();
        UpdateAll();
        UpdateState();
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
                //Debug.Log("IDLE");
                IdleUpdate();
                break;
            case PLAYERSTATE.PS_MOVE:
                //Debug.Log("MOVE");
                MoveUpdate();
                break;
            case PLAYERSTATE.PS_ATTACK:
                //Debug.Log("ATTACK");
                AttackUpdate();
                break;
            case PLAYERSTATE.PS_EVADE:
                //Debug.Log("EVADE");
                EvadeUpdate();
                break;
            case PLAYERSTATE.PS_DIE:
                //Debug.Log("DIE");
                DieUpdate();
                break;
            case PLAYERSTATE.PS_SKILL:
                //Debug.Log("SKILL");
                SkillUpdate();
                break;
            case PLAYERSTATE.PS_INTERACTING:
                //Debug.Log("INTERACTING");
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
        if (Input.GetKeyDown(KeyCode.L))
        {
            weaponManager.GetWeapon().exp += 10.0f;
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
        myAnimator.SetTrigger("Move");
    }

    public void MoveUpdate()
    {
        PlayerMove();
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
            characterController.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        if (characterController.isGrounded)
        {
            vSpeed = 0;
        }

        vSpeed = vSpeed - Time.deltaTime * 9.8f;
        characterController.Move(Vector3.up * vSpeed * Time.deltaTime);
        //_myStateMachine.UpdateState();
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
        InitAttack();
        ChangeState(PLAYERSTATE.PS_IDLE);
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
            case 2: weaponManager.GetWeapon().Attack(); break;
            case 3: weaponManager.GetWeapon().Attack(); break;
        }
    }

    /// <summary>
    /// 현재 콤보 상태에 따라 공격 애니메이션을 재생한다.
    /// </summary>
    private void SetAttackAnimationTrigger()
    {
        switch (Player.Instance.currentCombo)
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
        InitAttack();
    }




    ///////////////// 회피 관련 //////////////////

    private void EvadeEnter()
    {
        UseStemina(2);
        myAnimator.SetTrigger("Avoid");
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
        _evadeTime = _initEvadeTime;
        myAnimator.ResetTrigger("Avoid");
    }




    ///////////////// 스킬 관련 //////////////////

    private void SkillEnter()
    {
        SetSkillAnimation();
        //PlayerSkillCheck();
    }
    
    /// <summary>
    /// 스킬 선택에 따른 스킬 재생
    /// </summary>
    private void SetSkillAnimation()
    {
        switch (_cntSkillType)
        {
            case 0: myAnimator.SetTrigger("SkillA"); PlayerSkillA(); break;
            case 1: myAnimator.SetTrigger("SkillB"); PlayerSkillB(); break;
            case 2: myAnimator.SetTrigger("SkillC"); PlayerSkillC(); break;
        }
    }


    public void PlayerSkillA()
    {
        if (SkillA_ButtonClick == false)
        {
            //_myStateMachine.SetState("SKILL_A");
            SkillA_ButtonClick = true;
            skillA_Counter = 0f;
            //GameObject skill = ObjectPoolManager.Instance.GetObject(weaponManager.GetWeapon().SkillA());
            //skill.transform.position = skillPoint.position;
            //skill.transform.rotation = skillPoint.rotation;
            weaponManager.GetWeapon().SkillA();
            EndAttack();
        }
    }

    public void PlayerSkillB()
    {
        if (SkillB_ButtonClick == false && weaponManager.GetWeapon().CheckSkillB())
        {
            //_myStateMachine.SetState("SKILL_B");
            SkillB_ButtonClick = true;
            skillB_Counter = 0f;
            GameObject skill = ObjectPoolManager.Instance.GetObject(weaponManager.GetWeapon().SkillB());
            skill.transform.position = skillPoint.position;
            skill.transform.rotation = skillPoint.rotation;
            weaponManager.GetWeapon().SkillB();
            EndAttack();
        }
    }

    public void PlayerSkillC()
    {
        if (SkillC_ButtonClick == false && weaponManager.GetWeapon().CheckSkillC())
        {
            //_myStateMachine.SetState("SKILL_C");
            SkillC_ButtonClick = true;
            skillC_Counter = 0f;
            GameObject skill = ObjectPoolManager.Instance.GetObject(weaponManager.GetWeapon().SkillC());
            skill.transform.position = skillPoint.position;
            skill.transform.rotation = skillPoint.rotation;
            weaponManager.GetWeapon().SkillC();
            EndAttack();
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
            case 0: break;
            case 1: break;
            case 2: break;
        }
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

    }

    /// <summary>
    /// 스킬 선택에 따른 스킬 애니메이션 취소
    /// </summary>
    public void SkillExit()
    {
        switch (_cntSkillType)
        {
            case 0: myAnimator.ResetTrigger("SkillA"); break;
            case 1: myAnimator.ResetTrigger("SkillB"); break;
            case 2: myAnimator.ResetTrigger("SkillC"); break;
        }
    }

    /// <summary>
    /// 스킬 쿨타임 체크
    /// </summary>
    private void PlayerSkillCheck()
    {
        switch(_cntSkillType)
        {
            case 0:
                skillA_Counter += Time.deltaTime;
                if (skillA_Counter >= weaponManager.GetWeapon().skillACool) SkillA_ButtonClick = false;
                break;
            case 1:
                skillB_Counter += Time.deltaTime;
                if (skillB_Counter >= weaponManager.GetWeapon().skillBCool) SkillB_ButtonClick = false;
                break;
            case 2:
                skillC_Counter += Time.deltaTime;
                if (skillC_Counter >= weaponManager.GetWeapon().skillCCool) SkillC_ButtonClick = false;
                break;
        }
    }

    ///////////////// 상호작용 관련 //////////////////

    private void InteractEnter()
    {
        
    }

    /// <summary>
    /// 눌렀을 시 주변의 NPC와 상호작용한다.
    /// </summary>
    public void CheckInteractObject()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.9f);
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
        _isdead = true;
        myAnimator.SetTrigger("Die");
    }

    public void DieUpdate()
    {

    }

    public void DieExit()
    {
        _isdead = false;
        myAnimator.ResetTrigger("Die");
    }




    ///////////////// 전환 관련 //////////////////

    /// <summary>
    /// 조이스틱이 눌렸을때 조건에 맞는 상태라면 상태를 전환시킨다.
    /// </summary>
    public void ChangeToMoveCheck()
    {
        if (GetMove())
        {
            if (_cntState == PLAYERSTATE.PS_IDLE)
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
        else if(_cntState == PLAYERSTATE.PS_ATTACK && _canConnectCombo)
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
        if (_hp <= 0 && !isdead)
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
        ChangeToAttackCheck();
    }

    /// <summary>
    /// 회피 버튼이 눌림
    /// </summary>
    public void EvadeBtnClicked()
    {
        ChangeToEvadeChack();
    }

    /// <summary>
    /// 스킬 A 버튼이 눌림
    /// </summary>
    public void SkillABtnClicked()
    {
        if(_cntState != PLAYERSTATE.PS_DIE)
        {
            _cntSkillType = 0;
            ChangeState(PLAYERSTATE.PS_SKILL);
        }
    }

    /// <summary>
    /// 스킬 B 버튼이 눌림
    /// </summary>
    public void SkillBBtnClicked()
    {
        if (_cntState != PLAYERSTATE.PS_DIE)
        {
            _cntSkillType = 1;
            ChangeState(PLAYERSTATE.PS_SKILL);
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





    ///////////////// 카메라 관련 //////////////////

    /// <summary>
    /// 플레이어와 관련된 카메라들을 세팅한다.
    /// </summary>
    private void SetUpPlayerCamera()
    {
        cam = GameObject.FindGameObjectWithTag("DungeonMainCamera").transform;
        SetPlayerFaceCam();
        SetPlayerFollowCam();
        SetMinimapFreeLook();
    }

    /// <summary>
    /// 플레이어 얼굴을 비추는 카메라를 세팅한다.
    /// </summary>
    private void SetPlayerFaceCam()
    {
        faceCam = GameObject.Find("PlayerFaceCam").GetComponent<FaceCam>();
        faceCam.InitFaceCam(transform.Find("PlayerAvatar").gameObject);
    }

    /// <summary>
    /// 플레이어를 추적하는 카메라를 세팅한다.
    /// </summary>
    private void SetPlayerFollowCam()
    {
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
        minimapFreeLook = GameObject.FindGameObjectWithTag("MinimapCamera").GetComponent<CinemachineFreeLook>();
        minimapFreeLook.Follow = transform;
        minimapFreeLook.LookAt = transform;
    }

    /// <summary>
    /// FaceCam을 갱신한다.
    /// </summary>
    public void ChangeFaceCamera()
    {
        faceCam.InitFaceCam(transform.Find("PlayerAvatar").gameObject);
    }



    ///////////////// 이전 사용 코드 //////////////////

    public bool GetAttackButton()
    {
        return AttackButtonClick;
    }

    public void SetAttackButton(bool attackbutton)
    {
        AttackButtonClick = attackbutton;
    }


    public bool CheckCanAttack()
    {
        return AttackButtonClick == true;
    }


    public bool GetAvoidance()
    {
        return avoidButtonClick;
    }

    public void SetAvoidButton(bool avoidbutton)
    {
        avoidButtonClick = avoidbutton;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            Item itemInfo = other.gameObject.GetComponent<Item>();
            itemManager.AddItem(itemInfo);
            Debug.Log(itemInfo.itemData.itemName + " 아이템 획득!");
            Destroy(other.gameObject);
        }
    }
}
