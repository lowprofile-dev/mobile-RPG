using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using System;

public class BossSkeletonPase2 : MonsterAction
{
    enum ATTACKSTATE {Attack0 , Attack1 , Attack2 , Attack3 };

    [SerializeField] GameObject[] AttackEffect;
    [SerializeField] Transform FirePoint;
    [SerializeField] GameObject castingBar;

    private float castingTime;
    Coroutine _attackCoroutine;
    Coroutine _castCoroutine;
    Coroutine _hitCoroutine;
    private bool panic = false;
    Vector3 spawnPostion;
    Vector3 aroundPos;
    ATTACKSTATE currentAttack;
    string AtkState;
    private float attackCoolTime;
    private bool canAttack = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _findRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
   
    public override void InitObject()
    {
        base.InitObject();
        panic = true;
        _navMeshAgent.stoppingDistance = _attackRange-1;
        spawnPostion = transform.position;
        attackCoolTime = _attackSpeed;
        StartCoroutine(AttackCoolCal());
        StartCoroutine(FSM());
    }
    private IEnumerator FSM()
    {
        yield return null;
        while (true)
        {
            Debug.Log(_currentState.ToString());
            Debug.Log(canAttack);
            yield return StartCoroutine(_currentState.ToString());
        }
    }

    private IEnumerator AttackCoolCal()
    {
        while (true)
        {
            yield return null;
            if (!canAttack)
            {
                attackCoolTime -= Time.deltaTime;
                if(attackCoolTime <= 0)
                {
                    attackCoolTime = _attackSpeed;
                    canAttack = true;
                }
            }
        }
    }
    private IEnumerator STATE_IDLE()
    {
        yield return null;
        if (!_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            _monster.MyAnimator.SetTrigger("Idle");
        }

        if (CanAttackState())
        {
            if (canAttack)
            {
                _currentState = STATE.STATE_ATTACK;
            }
            else
            {
                _currentState = STATE.STATE_IDLE;
                
            }
        }
        else if(_distance < _findRange)
        {
            _currentState = STATE.STATE_TRACE;
        }
    }

    private IEnumerator STATE_ATTACK()
    {
        yield return null;

        currentAttack = AttackPattern();
        
        yield return new WaitForSeconds(castingTime);
        castingBar.SetActive(false);
        canAttack = false;


        if (!_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).IsName(AtkState))
        {
            _monster.MyAnimator.SetTrigger(AtkState);
        }

        Debug.Log(_monster.monsterName + "의 공격!");
        _target.GetComponent<LivingEntity>().Damaged(_monster.attackDamage);

        GameObject effect = ObjectPoolManager.Instance.GetObject(AttackEffect[(int)currentAttack]);
        effect.transform.position = FirePoint.position;
        effect.transform.LookAt(_target.transform);

        yield return new WaitForSeconds(_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);

        _navMeshAgent.speed = _moveSpeed;
        //_navMeshAgent.stoppingDistance = _attackRange;

        _currentState = STATE.STATE_IDLE;
    }

    private IEnumerator STATE_TRACE()
    {
        yield return null;

        if (!_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            _monster.MyAnimator.SetTrigger("Walk");
        }

        if(CanAttackState() && canAttack)
        {
            _currentState = STATE.STATE_ATTACK;
        }
        else if (_distance >= _findRange)
        {
            //_navMeshAgent.SetDestination(spawnPostion);
            _currentState = STATE.STATE_IDLE;
        }
        else
        {
            _navMeshAgent.SetDestination(_target.transform.position);
        }
    }
    private void Update()
    {
        UpdateState();
    }

    protected override void UpdateMonster()
    {
        DeathCheck();
        TargetDeathCheck();
    }

    public override void UpdateState()
    {
        // 반드시 실행되는 업데이트 내용
        UpdateMonster();

        // 스테이트별 업데이트 내용
        switch (_currentState)
        {
            case STATE.STATE_SPAWN:
              
                break;
            case STATE.STATE_IDLE:
     
                break;
            case STATE.STATE_TRACE:
   
                break;
            case STATE.STATE_ATTACK:
   
                break;
            case STATE.STATE_KILL:
                KillPlayer();
                break;
            case STATE.STATE_DEBUFF:
                break;
            case STATE.STATE_DIE:
                break;
            default:
                break;
        }
    }
    

    private ATTACKSTATE AttackPattern()
    {
        _monster.Mp = 0f;
        int percent = UnityEngine.Random.Range(0, 100);

        if(percent <= 25)
        {
            castingBar.SetActive(true);
            castingTime = 1.5f;
            AtkState = "Attack0";
            return ATTACKSTATE.Attack0;
        }
        else if (percent <= 50)
        {
            castingBar.SetActive(true);
            castingTime = 1f;
            AtkState = "Attack1";
            return ATTACKSTATE.Attack1;
        }
        else if (percent <= 75)
        {

            AtkState = "Attack2";
            return ATTACKSTATE.Attack2;
        }
        else
        {
            castingBar.SetActive(true);
            castingTime = 1.5f;
            AtkState = "Attack3";
            return ATTACKSTATE.Attack3;
        }

    }

    
    public override void TargetDeathCheck()
    {
        if (_currentState != STATE.STATE_KILL && _target.GetComponent<LivingEntity>().Hp <= 0)
        {
            _currentState = STATE.STATE_KILL;
        }
    }
    private IEnumerator STATE_DIE()
    {
        // DeadSound를 재생한다.
        // 뭔가 사망 행동을 함
        _monster.MyAnimator.SetTrigger("Die");

        yield return new WaitForSeconds(2);

        ObjectPoolManager.Instance.ReturnObject(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAttack"))
        {

            if(!_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            _monster.MyAnimator.SetTrigger("Hit");

            _monster.Damaged(WeaponManager.Instance.GetWeapon().attackDamage);

            _currentState = STATE.STATE_HIT;
        }
    }
   
    private IEnumerator STATE_HIT()
    {
        yield return null;
        //데미지를 입는다.

        while (true)
        {
            yield return null;

            if (_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                _currentState = STATE.STATE_IDLE;
            }
        }
    }
    private IEnumerator STATE_KILL()
    {
        StopAllCoroutines();
        yield return null;
    }

    private void KillPlayer()
    {
        if (_monster.MyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            _monster.MyAnimator.ResetTrigger("Laugh");
            _monster.MyAnimator.SetTrigger("Laugh");
        }

        transform.RotateAround(_target.transform.position, Vector3.up, 5 * Time.deltaTime);
        transform.LookAt(_target.transform.position);
    }
   
}
