using UnityEngine;
using UnityEngine.AI;

public class TestMonsterAction : MonsterAction
{
    [SerializeField] protected float _findRange;
    [SerializeField] protected float _attackRange;

    NavMeshAgent _navMeshAgent;


    public override void InitObject()
    {
        base.InitObject();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        UpdateState();
    }

    public override void UpdateState()
    {
        switch (_currentState)
        { 
            case STATE.STATE_IDLE:
                Search();
                break;
            case STATE.STATE_MOVE:
                Move();
                break;
            case STATE.STATE_ATTACK:
                break;
            case STATE.STATE_DIE:
                break;
            default:
                break;
        }
    }

    public override void EnterState(STATE targetState)
    {
        switch (targetState)
        {
            case STATE.STATE_IDLE:
                break;
            case STATE.STATE_MOVE:
                break;
            case STATE.STATE_ATTACK:
                break;
            case STATE.STATE_DIE:
                break;
            default:
                break;
        }

    }

    public override void ExitState(STATE targetState)
    {
        switch (targetState)
        {
            case STATE.STATE_IDLE:
                break;
            case STATE.STATE_MOVE:
                break;
            case STATE.STATE_ATTACK:
                break;
            case STATE.STATE_DIE:
                break;
            default:
                break;
        }
    }

    public override void Move()
    {
        base.Move();
        _navMeshAgent.SetDestination(_target.transform.position);

        if (Vector3.Distance(_target.transform.position, _monster.transform.position) < _attackRange)
        {
            ChangeState(STATE.STATE_ATTACK);
        }
    }

    public override void Attack()
    {
        base.Attack();
    }

    public override void Search()
    {
        base.Search();

        if(Vector3.Distance(_target.transform.position, _monster.transform.position) < _findRange)
        {
            ChangeState(STATE.STATE_MOVE);
        }
    }
}
