////////////////////////////////////////////////////
/*
    File State.cs
    class State
    
    담당자 : 이신홍
    부 담당자 : 

    상태. (사용하지 않는다.)
*/
////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class State
{
    protected StateMachine _myStateMachine; // 이 스테이트가 속한 스테이트 머신
    protected LivingEntity _parentEntity;   // 이 스테이트가 속한 개체
    protected Animator _myAnimator;         // 애니메이터 캐싱

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void EndState();
}

