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
        if (_myAnimator != Player.Instance.MyAnimator) _myAnimator = Player.Instance.MyAnimator;
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
        if (_myAnimator != Player.Instance.MyAnimator) _myAnimator = Player.Instance.MyAnimator;

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
    public StateAttack_TestPlayer(LivingEntity parent)
    {
        _parentEntity = parent;
        _myStateMachine = parent.MyStateMachine;
        _myAnimator = parent.MyAnimator;
    }

    public override void EnterState()
    {
        _myAnimator.SetTrigger("Attack");
        if (_myAnimator != Player.Instance.MyAnimator) _myAnimator = Player.Instance.MyAnimator;
    }

    public override void UpdateState()
    {

        _myStateMachine.SetState("IDLE");

    }

    public override void EndState()
    {
        Player.Instance.SetAttackButton(false);
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
        if(Player.Instance.Mp >= 2)
        {
            _myAnimator.SetTrigger("Avoid");
            Player.Instance.UseMp(2);
        }
        if (_myAnimator != Player.Instance.MyAnimator) _myAnimator = Player.Instance.MyAnimator;
    }

    public override void UpdateState()
    {
        Player.Instance.PlayerAvoidance();

        if(Player.Instance.GetAvoidance()== false) _myStateMachine.SetState("IDLE");

        if (Player.Instance.GetMove())
        {
            Player.Instance.SetAvoidButton(false);
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
        _myAnimator.SetTrigger("Die");
        if (_myAnimator != Player.Instance.MyAnimator) _myAnimator = Player.Instance.MyAnimator;
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
        if (_myAnimator != Player.Instance.MyAnimator) _myAnimator = Player.Instance.MyAnimator;
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
        if (_myAnimator != Player.Instance.MyAnimator) _myAnimator = Player.Instance.MyAnimator;

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
        if (_myAnimator != Player.Instance.MyAnimator) _myAnimator = Player.Instance.MyAnimator;
    }

    public override void UpdateState()
    {
        _myStateMachine.SetState("IDLE");
    }

    public override void EndState()
    {
    }
}

