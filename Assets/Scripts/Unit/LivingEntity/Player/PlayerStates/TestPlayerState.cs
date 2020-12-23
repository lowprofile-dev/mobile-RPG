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
        _myAnimator.SetTrigger("Idle");
    }

    public override void UpdateState()
    {
        if (Player.Instance.GetMove()) _myStateMachine.SetState("MOVE");

        if (Player.Instance.GetAttackButton()) _myStateMachine.SetState("ATTACK");

        if (Player.Instance.GetAvoidance()) _myStateMachine.SetState("AVOID");
    }

    public override void EndState()
    {
        _myAnimator.ResetTrigger("Idle");
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
        _myAnimator.SetTrigger("Move");
    }
    public override void UpdateState()
    {
        if (Player.Instance.GetAttackButton()) _myStateMachine.SetState("ATTACK");

        if (Player.Instance.GetAvoidance()) _myStateMachine.SetState("AVOID");

        if (!Player.Instance.GetMove()) _myStateMachine.SetState("IDLE");
    }

    public override void EndState()
    {

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
       
        _myAnimator.SetTrigger("Attack");
        Player.Instance.SetAttackButton(false);
    }

    public override void UpdateState()
    {

        _myStateMachine.SetState("IDLE");

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
        _myAnimator.SetTrigger("Avoid");

    }

    public override void UpdateState()
    {
        _myStateMachine.SetState("IDLE");
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
    }

    public override void UpdateState()
    {
        
    }

    public override void EndState()
    {
    }
}

public class StateSkillA_TestPlayer : State
{

    public StateSkillA_TestPlayer(LivingEntity parent)
    {
        _parentEntity = parent;
        _myStateMachine = parent.MyStateMachine;
        _myAnimator = parent.MyAnimator;

    }

    public override void EnterState()
    {
        _myAnimator.SetTrigger("SkillA");
    }

    public override void UpdateState()
    {
        _myStateMachine.SetState("IDLE");
    }

    public override void EndState()
    {
    }
}

public class StateSkillB_TestPlayer : State
{

    public StateSkillB_TestPlayer(LivingEntity parent)
    {
        _parentEntity = parent;
        _myStateMachine = parent.MyStateMachine;
        _myAnimator = parent.MyAnimator;

    }

    public override void EnterState()
    {
        _myAnimator.SetTrigger("SkillB");
    }

    public override void UpdateState()
    {
        _myStateMachine.SetState("IDLE");
    }

    public override void EndState()
    {
    }
}

public class StateSkillC_TestPlayer : State
{

    public StateSkillC_TestPlayer(LivingEntity parent)
    {
        _parentEntity = parent;
        _myStateMachine = parent.MyStateMachine;
        _myAnimator = parent.MyAnimator;

    }

    public override void EnterState()
    {
        _myAnimator.SetTrigger("SkillC");

    }

    public override void UpdateState()
    {
        _myStateMachine.SetState("IDLE");
    }

    public override void EndState()
    {
    }
}

