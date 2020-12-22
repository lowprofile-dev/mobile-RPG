using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StateIdle_TestPlayer : State
{
    public StateIdle_TestPlayer(LivingEntity parent)
    {
        _parentEntity = parent;
        _myStateMachine = parent.MyStateMachine;
        _myAnimator = parent.MyAnimator;
    }

    public override void EnterState()
    {
        Debug.Log("Idle Enter");

    }

    public override void UpdateState()
    {

        if (Player.Instance.IsMove())
        {
            _myStateMachine.SetState("MOVE");
            _myAnimator.SetTrigger("Move");
        }
        else if(Player.Instance.IsAttack())
        {
            _myStateMachine.SetState("ATTACK");
            _myAnimator.SetTrigger("Attack");
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
        _myAnimator = parent.MyAnimator;

    }

    public override void EnterState()
    {
        Debug.Log("Move Enter");
 

    }
    public override void UpdateState()
    {
        //_action.Move();
        Debug.Log("Move Update");
        if (!Player.Instance.IsMove())
        {
            _myStateMachine.SetState("IDLE");
            _myAnimator.SetTrigger("Idle");
        }
        else
        {
     
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
        _myAnimator = parent.MyAnimator;

    }

    public override void EnterState()
    {
        counter = 0.0f;
    }

    public override void UpdateState()
    {
    //        counter += Time.deltaTime;
        if(_myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime 
            >= _myAnimator.runtimeAnimatorController.animationClips[2].length/2)
        {
            _myStateMachine.SetState("IDLE");
            _myAnimator.SetTrigger("Idle");
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
        _myAnimator = parent.MyAnimator;

    }

    public override void EnterState()
    {
        _myAnimator.SetBool("Die",true);
    }

    public override void UpdateState()
    {
        if (_myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime
            >= _myAnimator.runtimeAnimatorController.animationClips[2].length / 2)
        {
            _myAnimator.SetBool("Die", false);
        }
    }

    public override void EndState()
    {
    }
}

public class StateAvoid_TestPlayer : State
{

    public StateAvoid_TestPlayer(LivingEntity parent)
    {
        _parentEntity = parent;
        _myStateMachine = parent.MyStateMachine;
        _myAnimator = parent.MyAnimator;
    }

    public override void EnterState()
    {
        Debug.Log("Enter Avoid");
        _myAnimator.SetTrigger("Avoid");
    }

    public override void UpdateState()
    {
        //if (_myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime
        //    >= _myAnimator.runtimeAnimatorController.animationClips[3].length)
        //{
            _myStateMachine.SetState("IDLE");
            _myAnimator.SetTrigger("Idle");
        //}
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

