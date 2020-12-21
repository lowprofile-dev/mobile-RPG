using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle_TestPlayer : State
{
    public StateIdle_TestPlayer(LivingEntity parent)
    {
        _parentEntity = parent;
        _myStateMachine = parent.MyStateMachine;
    }

    public override void EnterState()
    {
        Debug.Log("Idle Enter");

    }

    public override void UpdateState()
    {
        //Debug.Log("Idle Update");
        if (Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.D) ||
            Input.GetKeyDown(KeyCode.S))
        {
            _myStateMachine.SetState("MOVE");
        }

        if(Input.GetMouseButtonDown(0))
        {
            _myStateMachine.SetState("ATTACK");
        }
    }

    public override void EndState()
    {
        Debug.Log("Idle End");
    }
}

public class StateMove_TestPlayer : State
{
    public StateMove_TestPlayer(LivingEntity parent)
    {
        _parentEntity = parent;
        _myStateMachine = parent.MyStateMachine;
    }

    public override void EnterState()
    {
        Debug.Log("Move Enter");

    }

    public override void UpdateState()
    {
        //_action.Move();
        Debug.Log("Move Update");
        if (!(Input.GetKey(KeyCode.A) ||
          Input.GetKey(KeyCode.W) ||
          Input.GetKey(KeyCode.D) ||
          Input.GetKey(KeyCode.S)))
        {
            _myStateMachine.SetState("IDLE");
        }

        if (Input.GetMouseButtonDown(0))
        {
            _myStateMachine.SetState("ATTACK");
        }
    }

    public override void EndState()
    {
        Debug.Log("Move End");

    }
}

public class StateAttack_TestPlayer : State
{
    private float timer = 3.0f;
    private float counter = 0.0f;
    public StateAttack_TestPlayer(LivingEntity parent)
    {
        _parentEntity = parent;
        _myStateMachine = parent.MyStateMachine;
    }

    public override void EnterState()
    {
        counter = 0.0f;
    }

    public override void UpdateState()
    {
        counter += Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            counter = 0;
        }

        if(counter > timer)
        {
            _myStateMachine.SetState("IDLE");
            counter = 0f;
        }
    }

    public override void EndState()
    {
    }
}

public class StateDie_TestPlayer : State
{

    public StateDie_TestPlayer(LivingEntity parent)
    {
        _parentEntity = parent;
        _myStateMachine = parent.MyStateMachine;
    }

    public override void EnterState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }

    public override void EndState()
    {
        throw new System.NotImplementedException();
    }
}

public class StateAvoid_TestPlayer : State
{

    public StateAvoid_TestPlayer(LivingEntity parent)
    {
        _parentEntity = parent;
        _myStateMachine = parent.MyStateMachine;
    }

    public override void EnterState()
    {
        Debug.Log("Enter Avoid");
    }

    public override void UpdateState()
    {
        Debug.Log("Update Avoid");
    }

    public override void EndState()
    {
        Debug.Log("End Avoid");
    }
}

public class StateSkillA_TestPlayer : State
{

    public StateSkillA_TestPlayer(LivingEntity parent)
    {
        _parentEntity = parent;
        _myStateMachine = parent.MyStateMachine;
    }

    public override void EnterState()
    {
        Debug.Log("Enter SkillA");
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }

    public override void EndState()
    {
        throw new System.NotImplementedException();
    }
}

public class StateSkillB_TestPlayer : State
{

    public StateSkillB_TestPlayer(LivingEntity parent)
    {
        _parentEntity = parent;
        _myStateMachine = parent.MyStateMachine;
    }

    public override void EnterState()
    {
        Debug.Log("Enter SkillB");
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }

    public override void EndState()
    {
        throw new System.NotImplementedException();
    }
}

public class StateSkillC_TestPlayer : State
{

    public StateSkillC_TestPlayer(LivingEntity parent)
    {
        _parentEntity = parent;
        _myStateMachine = parent.MyStateMachine;
    }

    public override void EnterState()
    {
        Debug.Log("Enter SkillC");
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }

    public override void EndState()
    {
        throw new System.NotImplementedException();
    }
}

