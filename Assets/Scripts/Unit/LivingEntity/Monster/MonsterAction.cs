using UnityEngine;

public class MonsterAction : MonoBehaviour
{
    protected GameObject _target;
    protected Monster _monster;
    protected STATE _currentState;    

    public virtual void InitObject()
    {
        _monster = GetComponent<Monster>();
        _target = GameObject.FindGameObjectWithTag("Player");
        _currentState = STATE.STATE_IDLE;
    }

    public virtual void ChangeState(STATE targetState)
    {
        ExitState(_currentState);
        _currentState = targetState;
        EnterState(_currentState);
    }

    public virtual void UpdateState()
    {
        
    }

    public virtual void EnterState(STATE targetState)
    {
       
    }

    public virtual void ExitState(STATE targetState)
    {
        
    }

    public virtual void Search()
    {
    }

    public virtual void Attack()
    {
    }

    public virtual void Move()
    {
    }

    public virtual void Die()
    {

    }
}
