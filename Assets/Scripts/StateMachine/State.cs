using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class State
{
    protected StateMachine _myStateMachine;
    protected LivingEntity _parentEntity;
    protected Animation _myAnimation;
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void EndState();
}

