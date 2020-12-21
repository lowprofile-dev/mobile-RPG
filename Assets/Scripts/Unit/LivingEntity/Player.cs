using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntity
{
    private Rigidbody _rigidbody;
    protected override void Start()
    {
        base.Start();

        _rigidbody = GetComponent<Rigidbody>();
    }

    protected override void Update()
    {
       _rigidbody.transform.Translate(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Time.deltaTime);
        MyStateMachine.UpdateState();
        if (_hp <= 0) MyStateMachine.SetState("DIE");
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

}
