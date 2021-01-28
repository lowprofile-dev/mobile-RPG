/*
    File ArcherAction.cs
    class ArcherAction
    
    담당자 : 김기정
    부 담당자 : 
 */

using System.Collections;
using UnityEngine;

public class ArcherAction : MonsterAction
{
    bool canPanic;
    enum ARCHERATTACKTYPE { ATTACK, WHACK, SHOT, RAPID_SHOT };
    ARCHERATTACKTYPE atktype;

    [SerializeField] private Transform _baseRangeAttackPos;
    [SerializeField] private GameObject _baseRangeAttackPrefab;

    Collider _baseAtkCollision;
    /////////// 탐색 관련 /////////////
    public override void InitObject()
    {
        base.InitObject();
        canPanic = true;
    }

    protected override void FindStart()
    {
        base.FindStart();

        if (canPanic)
        {
            _monster.myAnimator.SetTrigger("Panic");
        }
    }

    /// <summary>
    /// 패닉일때 소리
    /// </summary>
    private void DoPanicSound()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Monster Medium Panic " + UnityEngine.Random.Range(1, 4), 0.6f);
    }

    protected override bool CheckFindAnimationOver()
    {
        if (canPanic) return CheckAnimationOver("Panic", 1.0f);
        else return true;
    }

    protected override void FindPlayer()
    {
        _navMeshAgent.isStopped = false;
        ChangeState(MONSTER_STATE.STATE_TRACE);
        // 사거리 내에 적 존재 시 발동
    }

    private void CheckPlayerWithinRange()
    {
        if (Vector3.Distance(_target.transform.position, _monster.transform.position) < _attackRange)
        {
            ChangeState(MONSTER_STATE.STATE_CAST);
        }
    }

    protected override void FindExit()
    {
        base.FindExit();
        canPanic = false;
    }

    protected override void DoReturn()
    {
        base.DoReturn();
        canPanic = true;
    }

    protected override void DoAttack()
    {
        base.LookTarget();
        if (atktype == ARCHERATTACKTYPE.WHACK)
        {
            GameObject obj = ObjectPoolManager.Instance.GetObject(_baseMeleeAttackPrefab);
            obj.transform.SetParent(transform);
            obj.transform.position = _baseMeleeAttackPos.position;
            obj.transform.LookAt(_target.transform);

            Attack atk = obj.GetComponent<Attack>();
            atk.SetParent(gameObject);
            atk.PlayAttackTimer(0.3f);
        }
        else
        {
            GameObject obj = ObjectPoolManager.Instance.GetObject(_baseRangeAttackPrefab, _baseRangeAttackPos.position, _baseRangeAttackPos.rotation);
            obj.transform.SetParent(null);
            //obj.transform.position = _baseRangeAttackPos.position;
            obj.transform.LookAt(_target.transform);
            obj.GetComponent<Arrow>().Launch();

            Attack atk = obj.GetComponent<Attack>();
            atk.SetParent(gameObject);
            atk.PlayAttackTimer(5f);
        }
        _navMeshAgent.isStopped = false;
        ChangeState(MONSTER_STATE.STATE_TRACE);
    }

    protected override void CastStart()
    {
        int proc = Random.Range(0, 100);

        if (_distance < 3)
        {
            _castTime = 1f;
            atktype = ARCHERATTACKTYPE.WHACK;
        }
        else if (_distance < 7)
        {
            if (proc <= 50)
            {
                _castTime = 1f;
                atktype = ARCHERATTACKTYPE.ATTACK;
            }
            else if (proc <= 80)
            {
                _castTime = 1f;
                atktype = ARCHERATTACKTYPE.SHOT;
            }
            else if (proc <= 100)
            {
                _castTime = 1.5f;
                atktype = ARCHERATTACKTYPE.RAPID_SHOT;
            }
        }
    }

    protected override void SetAttackType()
    {
        if (_readyCast) return;
    }

    protected override void SetAttackAnimation()
    {
        switch (atktype)
        {
            case ARCHERATTACKTYPE.ATTACK:
                _monster.myAnimator.SetTrigger("Attack1");
                break;
            case ARCHERATTACKTYPE.RAPID_SHOT:
                _monster.myAnimator.SetTrigger("Attack3");
                break;
            case ARCHERATTACKTYPE.SHOT:
                _monster.myAnimator.SetTrigger("Attack2");
                break;
            case ARCHERATTACKTYPE.WHACK:
                _monster.myAnimator.SetTrigger("Attack0");
                break;
            default:
                break;
        }
    }

    private void BowWrackSound()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Monster Big Whoosh Start " + UnityEngine.Random.Range(1, 4), 1.0f);
    }

    private void BowShotSound()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Bow Fire " + UnityEngine.Random.Range(1, 4), 0.9f);
    }

    private void BowDrawSound()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Bow Draw " + UnityEngine.Random.Range(1, 4), 0.9f);
    }

    protected override IEnumerator SpawnDissolve()
    {
        yield return null;
        ChangeState(MONSTER_STATE.STATE_IDLE);
    }

    protected override void KillStart()
    {
        _monster.myAnimator.SetTrigger("Laugh");
    }
    /// <summary>
    /// 죽었을때 소리
    /// </summary>
    public override void DeathSound()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Monster Medium Die " + UnityEngine.Random.Range(1, 3), 0.8f);
    }
}
