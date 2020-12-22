using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleInputNamespace;
using UnityEngine.EventSystems;
public class Player : LivingEntity
{
    public static Player Instance;
    
    private bool avoidButtonClick = false;
    [SerializeField] private float avoid_power = 10f;
    private float count = 0f;
    private State saveState = null;

    private bool AttackButtonClick = false;
    private bool SkillA_ButtonClick = false;
    private bool SkillB_ButtonClick = false;
    private bool SkillC_ButtonClick = false;

    [SerializeField] private CharacterController characterController;
    [SerializeField] private float speed = 6f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    [SerializeField] private Transform cam;

    private Vector3 direction;
    private Vector3 moveDir;
    [SerializeField] private Quaternion rotateAngle;

    public Joystick joystick;
    float horizontal;
    float vertical;
    protected override void Start()
    {
        base.Start();
        Instance = this;
        joystick = GameObject.Find("Joystick").GetComponent<Joystick>();
    }

    protected override void Update()
    {
        if(!GameManager.Instance.isInteracting) // 상호작용 중이지 않을 때
        {
            PlayerAvoidance();

            PlayerMove();

            if(Input.GetKeyDown(KeyCode.P))
            {
                _hp = 0;
            }

            if (_hp <= 0) MyStateMachine.SetState("DIE");

            Debug.Log(MyStateMachine.GetState());
        }

        CheckInteractObject();
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

    public bool IsMove()
    {
        if (joystick.getHold()) return true;
        else return false;
    }

    public void PlayerSkillA()
    {
        MyStateMachine.SetState("SKILL_A");
    }
    public void PlayerSkillB()
    {
        MyStateMachine.SetState("SKILL_B");
    }
    public void PlayerSkillC()
    {
        MyStateMachine.SetState("SKILL_C");
    }

    public void PlayerAttack()
    {
        MyStateMachine.SetState("ATTACK");
    }

    public bool IsAttack()
    {
        return AttackButtonClick;
    }

    public void SetAttackButton(bool attackbutton)
    {
        AttackButtonClick = attackbutton;
    }
    public void PlayerAvoidance()
    {
        if (avoidButtonClick)
        {
            if(count == 0f) MyStateMachine.SetState("AVOID");

            count += 1f;

            if (count > avoid_power)
            {
                avoidButtonClick = false;
                count = 0f;
                MyStateMachine.SetState(saveState);
            }
            if (direction == Vector3.zero) characterController.Move(moveDir * speed * Time.deltaTime * avoid_power);

            else characterController.Move(moveDir * speed * Time.deltaTime * avoid_power);
        }
        else saveState = MyStateMachine.GetState();
    }
    public bool IsAvoidance()
    {
        if (avoidButtonClick) return true;
        else return false;
    }
    public void SetAvoidButton()
    {
        avoidButtonClick = true;
    }
    protected override void InitObject()
    {
        //임의 값
        _initHp = 10;
        _hp = 10;
        MyAnimator = new Animator();
        MyAnimator = GameObject.Find("Player").GetComponent<Animator>();

        MyStateMachine = new StateMachine();
        State Idle = new StateIdle_TestPlayer(this);
        State Move = new StateMove_TestPlayer(this);
        State Attack = new StateAttack_TestPlayer(this);
        State Die = new StateDie_TestPlayer(this);
        State Avoid = new StateAvoid_TestPlayer(this);
        State SkillA = new StateSkillA_TestPlayer(this);
        State SkillB = new StateSkillB_TestPlayer(this);
        State SkillC = new StateSkillC_TestPlayer(this);

        MyStateMachine.AddState("IDLE", Idle);
        MyStateMachine.AddState("MOVE", Move);
        MyStateMachine.AddState("ATTACK", Attack);
        MyStateMachine.AddState("DIE", Die);
        MyStateMachine.AddState("AVOID", Avoid);
        MyStateMachine.AddState("SKILL_A", SkillA);
        MyStateMachine.AddState("SKILL_B", SkillB);
        MyStateMachine.AddState("SKILL_C", SkillC);

        MyStateMachine.SetState("IDLE");
    }

    public void CheckInteractObject()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1.9f);
            for(int i=0; i<colliders.Length; i++)
            {
                if(colliders[i].GetComponent<NonLivingEntity>())
                {
                    colliders[i].GetComponent<NonLivingEntity>().Interaction();
                    break;
                }
            }
        }
    }

}
