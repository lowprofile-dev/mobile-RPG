////////////////////////////////////////////////////
/*
    File StateMachine.cs
    class StateMachine
    
    담당자 : 이신홍
    부 담당자 : 

    상태머신. (사용하지 않는다.)
*/
////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private State _currentState;                    // 현재 스테이트
    public Dictionary<string, State> _statesDic;    // 스테이트 목록

    public StateMachine()
    {
        _currentState = null;
        _statesDic = new Dictionary<string, State>();
    }

    public void UpdateState()
    {
        if(_currentState != null) _currentState.UpdateState();
    }

    /// <summary>
    /// 스테이트 추가
    /// </summary>
    public void AddState(string stateName, State state)
    {
        _statesDic.Add(stateName, state);
    }

    /// <summary>
    /// 스테이트 제거
    /// </summary>
    public void RemoveState(string stateName)
    {
        _statesDic.Remove(stateName);
    }
    
    public void SetState(string stateName)
    {
        if(_currentState != null) _currentState.EndState();
        if (_currentState == _statesDic[stateName]) return;
        else
        {
            _currentState = _statesDic[stateName];
            _currentState.EnterState();
        }
    }
    
    public void SetState(State inputState)
    {
        if (_currentState != null) _currentState.EndState();
        _currentState = inputState;
        _currentState.EnterState();
    }

    public State GetState()
    {
        return _currentState;
    }

}


