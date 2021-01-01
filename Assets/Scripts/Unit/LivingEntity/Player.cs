using SimpleInputNamespace;
using UnityEngine;

public class Player : LivingEntity
{
    public static Player Instance;

    private bool avoidButtonClick = false;
    [SerializeField] private float avoid_power = 10f;
    private float count = 0f;
    private State saveState = null;

    [SerializeField] private GameObject _playerAvatar; public GameObject playerAvater { get { return _playerAvatar; } }

    private bool AttackButtonClick = false;
    private bool SkillA_ButtonClick = false;
    private bool SkillB_ButtonClick = false;
    private bool SkillC_ButtonClick = false;

    private float skillA_Counter = 0f;
    private float skillB_Counter = 0f;
    private float skillC_Counter = 0f;
    private float mpCounter = 0f;

    [SerializeField] public Transform firePoint;
    [SerializeField] public Transform skillPoint;

    [SerializeField] private CharacterController characterController;
    [SerializeField] private float speed = 6f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    [SerializeField] private Transform cam;

    private Vector3 direction;
    private Vector3 moveDir; public Vector3 getMoveDir { get { return moveDir; } }
    [SerializeField] private Quaternion rotateAngle;

    public Joystick joystick = null;

    float horizontal;
    float vertical;
    bool _isdead = false; public bool isdead { get { return _isdead; } }

    PartSelection selection;
    FaceCam faceCam;

    public WeaponManager weaponManager;
    public ItemManager itemManager;

    private void Awake()
    {
        Instance = this;
    }

    protected override void Start()
    {
        base.Start();
        faceCam = GameObject.Find("PlayerFaceCam").GetComponent<FaceCam>();
        faceCam.Init(transform.Find("PlayerAvatar").gameObject);
    }

    protected override void Update()
    {

        if (isdead)
        {
            //죽었을 때
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                weaponManager.SetWeapon("WAND");
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                _hp--;

            }

            if (joystick == null)
            {
                joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<Joystick>();
            }

            if (!GameManager.Instance.isInteracting) // 상호작용 중이지 않을 때
            {
                PlayerMove();

                PlayerSkillCheck();

                PlayerMpRecovery();

                weaponManager.UpdateWeapon();

                selection.Update();

                if (_hp <= 0 && !isdead)
                {
                    MyStateMachine.SetState("DIE");
                    _isdead = true;
                }
            }

            else
            {
                ContinueInteract();
            }
        }
    }

    private void PlayerMpRecovery()
    {
        mpCounter += Time.deltaTime;
        if (mpCounter >= 2f)
        {
            if (Mp <= 10)
            {
                _mp++;
            }

            mpCounter = 0f;
        }
    }

    private void PlayerSkillCheck()
    {
        if (SkillA_ButtonClick)
        {
            skillA_Counter += Time.deltaTime;
            if (skillA_Counter >= weaponManager.GetWeapon().coolTimeA)
            {
                SkillA_ButtonClick = false;
            }
        }
        if (SkillB_ButtonClick)
        {
            skillB_Counter += Time.deltaTime;
            if (skillB_Counter >= weaponManager.GetWeapon().coolTimeB)
            {
                SkillB_ButtonClick = false;
            }
        }
        if (SkillC_ButtonClick)
        {
            skillC_Counter += Time.deltaTime;
            if (skillC_Counter >= weaponManager.GetWeapon().coolTimeC)
            {
                SkillC_ButtonClick = false;
            }
        }
    }

    public void PlayerMove()
    {
        horizontal = joystick.GetX_axis().value;
        vertical = joystick.GetY_axis().value;

        direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        MyStateMachine.UpdateState();
    }

    public void CameraChange()
    {
        faceCam.Init(transform.Find("PlayerAvatar").gameObject);
    }
    public bool GetMove()
    {
        return joystick.getHold();
    }

    public void PlayerSkillA()
    {
        if (SkillA_ButtonClick == false)
        {
            MyStateMachine.SetState("SKILL_A");
            SkillA_ButtonClick = true;
            skillA_Counter = 0f;
            GameObject skill = ObjectPoolManager.Instance.GetObject(weaponManager.GetWeapon().SkillA());
            skill.transform.position = skillPoint.position;
            skill.transform.rotation = skillPoint.rotation;
        }
    }
    public void PlayerSkillB()
    {
        if (SkillB_ButtonClick == false)
        {
            MyStateMachine.SetState("SKILL_B");
            SkillB_ButtonClick = true;
            skillB_Counter = 0f;
            GameObject skill = ObjectPoolManager.Instance.GetObject(weaponManager.GetWeapon().SkillB());
            skill.transform.position = skillPoint.position;
            skill.transform.rotation = skillPoint.rotation;
        }
    }
    public void PlayerSkillC()
    {
        if (SkillC_ButtonClick == false)
        {
            MyStateMachine.SetState("SKILL_C");
            SkillC_ButtonClick = true;
            skillC_Counter = 0f;
            GameObject skill = ObjectPoolManager.Instance.GetObject(weaponManager.GetWeapon().SkillC());
            skill.transform.position = skillPoint.position;
            skill.transform.rotation = skillPoint.rotation;
        }
    }

    public bool GetAttackButton()
    {
        return AttackButtonClick;
    }

    public void SetAttackButton(bool attackbutton)
    {
        AttackButtonClick = attackbutton;
        if (AttackButtonClick == true)
        {
            GameObject skill = ObjectPoolManager.Instance.GetObject(weaponManager.GetWeapon().Attack());
            skill.transform.position = skillPoint.position;
            skill.transform.rotation = skillPoint.rotation;
        }
    }

    public void PlayerAvoidance()
    {
        if (avoidButtonClick)
        {
            if (direction == Vector3.zero)
            {
                characterController.Move(moveDir * speed * Time.deltaTime * avoid_power);
            }
            else
            {
                characterController.Move(moveDir * speed * Time.deltaTime * avoid_power);
            }
        }
    }
    public bool GetAvoidance()
    {
        return avoidButtonClick;
    }
    public void SetAvoidButton(bool avoidbutton)
    {
        avoidButtonClick = avoidbutton;
    }
    protected override void InitObject()
    {
        base.InitObject();

        selection = GetComponent<PartSelection>();
        selection.Start();
        selection.Init();

        MyAnimator = _playerAvatar.GetComponent<Animator>();

        MyStateMachine = new StateMachine();
        State Idle = new StateIdle_TestPlayer(this);
        State Move = new StateMove_TestPlayer(this);
        State Attack = new StateAttack_TestPlayer(this);
        State Die = new StateDie_TestPlayer(this);
        State Avoid = new StateAvoid_TestPlayer(this);
        State SkillA = new StateSkillA_TestPlayer(this);
        State SkillB = new StateSkillB_TestPlayer(this);
        State SkillC = new StateSkillC_TestPlayer(this);

        weaponManager = WeaponManager.Instance;

        MyStateMachine.AddState("IDLE", Idle);
        MyStateMachine.AddState("MOVE", Move);
        MyStateMachine.AddState("ATTACK", Attack);
        MyStateMachine.AddState("DIE", Die);
        MyStateMachine.AddState("AVOID", Avoid);
        MyStateMachine.AddState("SKILL_A", SkillA);
        MyStateMachine.AddState("SKILL_B", SkillB);
        MyStateMachine.AddState("SKILL_C", SkillC);

        MyStateMachine.SetState("IDLE");

        weaponManager.SetWeapon("SWORD");

    }

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

    public void ContinueInteract()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckInteractObject();
        }
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
