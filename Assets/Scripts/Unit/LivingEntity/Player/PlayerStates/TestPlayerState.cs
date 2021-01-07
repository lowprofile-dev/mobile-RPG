using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

public class StateAvoid_TestPlayer : State
{

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
        _myStateMachine = parent._myStateMachine;
        _myAnimator = parent.myAnimator;

    }

    public override void EnterState()
    {
        _myAnimator.SetTrigger("Die");
        if (_myAnimator != Player.Instance.myAnimator) _myAnimator = Player.Instance.myAnimator;
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
        _myStateMachine = parent._myStateMachine;
        _myAnimator = parent.myAnimator;

    }

    public override void EnterState()
    {
        _myAnimator.SetTrigger("SkillA");
        if (_myAnimator != Player.Instance.myAnimator) _myAnimator = Player.Instance.myAnimator;
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
        _myStateMachine = parent._myStateMachine;
        _myAnimator = parent.myAnimator;

    }

    public override void EnterState()
    {
        _myAnimator.SetTrigger("SkillB");
        if (_myAnimator != Player.Instance.myAnimator) _myAnimator = Player.Instance.myAnimator;

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
        _myStateMachine = parent._myStateMachine;
        _myAnimator = parent.myAnimator;

    }

    public override void EnterState()
    {
        _myAnimator.SetTrigger("SkillC");
        if (_myAnimator != Player.Instance.myAnimator) _myAnimator = Player.Instance.myAnimator;
    }

    public override void UpdateState()
    {
        _myStateMachine.SetState("IDLE");
    }

    public override void EndState()
    {
    }
}
*/