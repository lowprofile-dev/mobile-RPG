using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntity
{
    [SerializeField]
    private CharacterController characterController;
    [SerializeField]
    private float speed = 6f;
    [SerializeField]
    private float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    [SerializeField]
    private Transform cam;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if(!GameManager.Instance.isInteracting) // 상호작용 중이지 않을 때
        {
            //_rigidbody.transform.Translate(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Time.deltaTime);
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                characterController.Move(moveDir.normalized * speed * Time.deltaTime);
            }

            MyStateMachine.UpdateState();
            if (_hp <= 0) MyStateMachine.SetState("DIE");
        }

        CheckInteractObject();
    }

    protected override void InitObject()
    {
        //임의 값
        _initHp = 10;
        _hp = 10;

        MyStateMachine = new StateMachine();
        State Idle = new StateIdle_TestPlayer(this);
        State Move = new StateMove_TestPlayer(this);
        State Attack = new StateAttack_TestPlayer(this);
        State Die = new StateDie_TestPlayer(this);

        MyStateMachine.AddState("IDLE", Idle);
        MyStateMachine.AddState("MOVE", Move);
        MyStateMachine.AddState("ATTACK", Attack);
        MyStateMachine.AddState("DIE", Die);

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
