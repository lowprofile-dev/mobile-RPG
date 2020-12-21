using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntity
{
    private Rigidbody _rigidbody;
    private Vector3 dir;
    private float count = 1f;
    private bool avoidButtonClick = false;
    private State saveState = null;
    [SerializeField] private float avoid_power = 10f;
    protected override void Start()
    {
        base.Start();

        _rigidbody = GetComponent<Rigidbody>();
    }

    protected override void Update()
    {
        if(!GameManager.Instance.isInteracting) // 상호작용 중이지 않을 때
        {
            dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            PlayerAvoidance();

            Debug.Log("State : " + MyStateMachine.GetState());

            PlayerSkill();

            MyStateMachine.UpdateState();
            if (_hp <= 0) MyStateMachine.SetState("DIE");
        }

        CheckInteractObject();
    }

    public void PlayerSkill()
    {
       if( Input.GetKeyDown(KeyCode.Alpha1))
       {
            MyStateMachine.SetState("SKILL_A");
       }
       else if(Input.GetKeyDown(KeyCode.Alpha2))
       {
            MyStateMachine.SetState("SKILL_B");
       }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            MyStateMachine.SetState("SKILL_C");
        }
    }

    public void PlayerAvoidance()
    {
        if (avoidButtonClick)
        {
            count += 1f;
            if (count > avoid_power)
            {
                avoidButtonClick = false;
                count = 1f;
                MyStateMachine.SetState(saveState);
            }
            if (dir == Vector3.zero) _rigidbody.transform.Translate(Vector3.forward * Time.deltaTime * avoid_power);
            else _rigidbody.transform.Translate(dir * Time.deltaTime * avoid_power);
        }
        else _rigidbody.transform.Translate(dir * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift) && avoidButtonClick == false)
        {
            avoidButtonClick = true;
            saveState = MyStateMachine.GetState();
            MyStateMachine.SetState("AVOID");
        }
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
