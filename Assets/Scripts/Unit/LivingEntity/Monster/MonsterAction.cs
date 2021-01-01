using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;
public class MonsterAction : MonoBehaviour
{
    protected GameObject _target;
    protected Monster _monster;
    protected STATE _currentState;

    [SerializeField] protected float _findRange;
    [SerializeField] protected float _attackRange;
    [SerializeField] protected float _limitTraceRange;
    [SerializeField] protected float _speed;
    [SerializeField] protected float _distance;
    [SerializeField] protected float _attackSpeed;
    [SerializeField] private LayerMask layerMask;
    protected float _traceTimer;
    protected NavMeshAgent _navMeshAgent;

    public virtual void InitObject()
    {
        _monster = GetComponent<Monster>();
        _target = GameObject.FindGameObjectWithTag("Player");
        _currentState = STATE.STATE_IDLE;
        _traceTimer = 0;
        _navMeshAgent = GetComponent<NavMeshAgent>();
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

        Vector3 targetDir = new Vector3(_target.transform.position.x - transform.position.x, 0f, _target.transform.position.z - transform.position.z);

        Physics.Raycast(new Vector3(transform.position.x, 0.5f, transform.position.z), targetDir, out RaycastHit hit, 30f, layerMask);

        _distance = Vector3.Distance(_target.transform.position, transform.position);

        if (hit.transform.CompareTag("Player") && _distance <= _findRange)
        {
            ChangeState(STATE.STATE_FIND);
        }
       
    }

    public virtual void Attack()
    {
       

    }
    protected bool CanAttackState()
    {
        Vector3 targetDir = new Vector3(_target.transform.position.x - transform.position.x, 0f, _target.transform.position.z - transform.position.z);

        Physics.Raycast(new Vector3(transform.position.x, 0.5f, transform.position.z), targetDir, out RaycastHit hit, 30f, layerMask);

        _distance = Vector3.Distance(_target.transform.position, transform.position);

        if(hit.transform == null)
        {
            return false;
        }
        if(hit.transform.CompareTag("Player") && _distance <= _attackRange)
        {

            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void Move()
    {
    }

    public virtual void Die()
    {
       
    }

    public virtual void Damaged(float dmg)
    {
        _monster.Damaged(dmg);
    }

    public virtual void FindStart()
    {

    }

    public virtual void AttackStart()
    {

    }

    public virtual void AttackExit()
    {

    }
    
    public virtual IEnumerator DoFindAction()
    {
        yield return null;
    }

    public virtual IEnumerator AttackTarget()
    {
        yield return null;
    }

    public virtual IEnumerator DoAttackAction()
    {
        yield return null;
    }

    public virtual IEnumerator DoCastingAction()
    {
        yield return null;
    }

    public virtual void CastStart()
    {
        
    }

    public virtual void CastExit()
    {

    }

    public virtual void Cast()
    {

    }

    protected virtual void UpdateMonster()
    {

    }

    public virtual void DeathCheck()
    {

    }
    public virtual void PlayerDeathCheck()
    {

    }

    public bool NoHPCheck()
    {
        if(_monster.Hp <= 0)
        {
            return true;
        }

        return false;
    }

    public virtual void CheckLimitPlayerDistance()
    {
        // 캐릭터와 적의 거리가 limit 이상일때
        if(Vector3.Distance(_target.transform.position, transform.position) >= _limitTraceRange)
        {
            _traceTimer += Time.deltaTime;
            // 타이머가 재생된다.
        
            if(_traceTimer > 2)
            {
                // 상태를 바꾼다.
                ChangeState(STATE.STATE_IDLE);
                _traceTimer = 0;
            }
        }
        // 거리가 가깝다면
        else
        {
            _traceTimer = 0;
        }
    }

    public virtual void DeathStart()
    {

    }

    public virtual IEnumerator DoDeathAction()
    {
        yield return null;
    }

    protected virtual void IdleStart()
    {

    }
}
