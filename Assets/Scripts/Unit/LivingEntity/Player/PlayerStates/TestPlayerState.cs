////////////////////////////////////////////////////
/*
    File TestPlayerState.cs
    class StateIdle_TestPlayer
    class StateMove_TestPlayer
    class StateAttack_TestPlayer
    class StateAvoid_TestPlayer
    class StateDie_TestPlayer
    class StateSkillA_TestPlayer
    class StateSkillB_TestPlayer
    class StateSkillC_TestPlayer
    
    플레이어의 다양한 상태를 서술해놓은 클래스

    담당자 : 김의겸
    부 담당자 : 
*/
////////////////////////////////////////////////////

/*
 * 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StateIdle_TestPlayer : State
{
    public StateIdle_TestPlayer(LivingEntity parent)
    {
        _parentEntity = parent;
        _myStateMachine = parent._myStateMachine;
        _myAnimator = parent.myAnimator;
    }

    public override void EnterState()
    {
        _myAnimator.SetTrigger("Idle");
        if (_myAnimator != Player.Instance.myAnimator) _myAnimator = Player.Instance.myAnimator;
    }

    public override void UpdateState()
    {
        if (Player.Instance.GetMove()) _myStateMachine.SetState("MOVE");

        if (Player.Instance.CheckCanAttack()) _myStateMachine.SetState("ATTACK");

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
        _myStateMachine = parent._myStateMachine;
        _myAnimator = parent.myAnimator;

    }

    public override void EnterState()
    {
        _myAnimator.SetTrigger("Move");
        if (_myAnimator != Player.Instance.myAnimator) _myAnimator = Player.Instance.myAnimator;

    }
    public override void UpdateState()
    {
        if (Player.Instance.CheckCanAttack()) _myStateMachine.SetState("ATTACK");

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
        _myStateMachine = parent._myStateMachine;
        _myAnimator = parent.myAnimator;
    }

    public override void EnterState()
    {
        Player.Instance.ToNextCombo();
        SetAttackAnimationTrigger();
        Player.Instance.SetAttackButton(false);
        if (_myAnimator != Player.Instance.myAnimator) _myAnimator = Player.Instance.myAnimator;
    }

    /// <summary>
    /// 현재 콤보 상태에 따라 공격 애니메이션을 재생한다.
    /// </summary>
    private void SetAttackAnimationTrigger()
    {
        switch (Player.Instance.currentCombo)
        {
            case 1: _myAnimator.SetTrigger("Attack01"); break;
            case 2: _myAnimator.SetTrigger("Attack02"); break;
            case 3: _myAnimator.SetTrigger("Attack03"); break;
        }
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
        _myStateMachine = parent._myStateMachine;
        _myAnimator = parent.myAnimator;
    }

    public override void EnterState()
    {
        if(Player.Instance.Mp >= 2)
        {
            _myAnimator.SetTrigger("Avoid");
            Player.Instance.UseMp(2);
            Player.Instance.EndAttack();
        }
        if (_myAnimator != Player.Instance.myAnimator) _myAnimator = Player.Instance.myAnimator;
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