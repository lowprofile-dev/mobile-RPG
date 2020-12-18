using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private State _currentState;
    public Dictionary<string, State> _statesDic;

    public StateMachine()
    {
        _currentState = null;
        _statesDic = new Dictionary<string, State>();
    }

    public void UpdateState()
    {
        if(_currentState != null) _currentState.UpdateState();
    }

    public void AddState(string stateName, State state)
    {
        _statesDic.Add(stateName, state);
    }

    public void RemoveState(string stateName)
    {
        _statesDic.Remove(stateName);
    }

    public void SetState(string stateName)
    {
        if(_currentState != null) _currentState.EndState();
        _currentState = _statesDic[stateName];
        _currentState.EnterState();
    }
}


