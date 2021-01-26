////////////////////////////////////////////////////
/*
    File GruntAction.cs
    class GruntAction
    
    담당자 : 안영훈

*/
////////////////////////////////////////////////////
using System.Collections;
using UnityEngine;

public class GruntAction : MonsterAction
{
    bool canPanic;
    enum GRUNTATTACKTYPE { DEFALUT_ATTACK, SHOULDER_BASH, SPIN, SLAM }; // 4가지 공격패턴
    enum GRUNTSIZE { BIG, MEDIUM };

    GRUNTATTACKTYPE atktype;

    [Header("Attack")]
    [SerializeField] private Transform _baseMeleeAttackPos; //공격하는 pos
    [SerializeField] private GameObject _baseMeleeAttackPrefab; //공격 콜라이더 오브젝트

    [Header("Etc")]
    [SerializeField] private GRUNTSIZE _gruntSize;

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
        switch (_gruntSize)
        {
            case GRUNTSIZE.BIG:
                SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Monster Big Panic " + UnityEngine.Random.Range(1, 3), 0.6f);
                break;
            case GRUNTSIZE.MEDIUM:
                SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Monster Medium Panic " + UnityEngine.Random.Range(1, 4), 0.6f);
                break;
        }
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
        GameObject obj = ObjectPoolManager.Instance.GetObject(_baseMeleeAttackPrefab);
        obj.transform.SetParent(transform);
        obj.transform.position = _baseMeleeAttackPos.position;

        Attack atk = obj.GetComponent<Attack>();
        atk.SetParent(gameObject);
        atk.PlayAttackTimer(0.3f);

    }

    protected override void CastStart()
    {
        int proc = Random.Range(0, 100);

        if (proc <= 50)
        {
            atktype = GRUNTATTACKTYPE.SPIN;
        }
        else
        {
            atktype = GRUNTATTACKTYPE.SLAM;
        }

    }
    protected override void DoCastingAction()
    {
        _cntCastTime += Time.deltaTime;
        _bar.CastUpdate();

        if (_cntCastTime >= _castTime)
        {
            _cntCastTime = 0;
            _readyCast = true;
            ChangeState(MONSTER_STATE.STATE_ATTACK);
        }
    }
    protected override void CastExit()
    {
        base.CastExit();
    }

    protected override void AttackStart()
    {
        int proc = Random.Range(0, 100);

        if (proc <= 25)
        {
            atktype = GRUNTATTACKTYPE.DEFALUT_ATTACK;
        }
        else if (proc <= 50)
        {
            atktype = GRUNTATTACKTYPE.SHOULDER_BASH;
        }
        else if (proc <= 75)
        {
            atktype = GRUNTATTACKTYPE.SLAM;
        }
        else
        {
            atktype = GRUNTATTACKTYPE.SPIN;
        }
        base.AttackStart();
    }

    /// <summary>
    /// 휘두르기 시작할 때 소리
    /// </summary>
    private void AttackSoundWhooshStart()
    {
        switch (_gruntSize)
        {
            case GRUNTSIZE.BIG:
                SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Monster Big Whoosh Start " + UnityEngine.Random.Range(1, 4), 0.9f);
                break;
            case GRUNTSIZE.MEDIUM:
                SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Monster Medium Whoosh " + UnityEngine.Random.Range(1, 6), 0.9f);
                break;
        }
    }

    /// <summary>
    /// 휘두른 후의 소리
    /// </summary>
    private void AttackSoundWhooshAfter()
    {
        switch (_gruntSize)
        {
            case GRUNTSIZE.BIG:
                SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Monster Big Whoosh " + UnityEngine.Random.Range(1, 6), 1);
                break;
        }
    }


    protected override void AttackExit()
    {
        if (_attackCoroutine != null) StopCoroutine(_attackCoroutine);
    }
    protected override IEnumerator AttackTarget()
    {
        while (true)
        {
            yield return null;

            if (CanAttackState())
            {

                yield return new WaitForSeconds(_attackSpeed - _monster.myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);

                SetAttackAnimation();

                LookTarget();

                // 사운드 재생

                StartCoroutine(DoAttackAction());

                yield return new WaitForSeconds(_monster.myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime / 2);

                _readyCast = false;
                if (!_readyCast && ToCast()) break;
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
            case GRUNTATTACKTYPE.DEFALUT_ATTACK:
                _monster.myAnimator.SetTrigger("Attack0");
                break;
            case GRUNTATTACKTYPE.SHOULDER_BASH:
                _monster.myAnimator.SetTrigger("Attack1");
                break;
            case GRUNTATTACKTYPE.SPIN:
                _monster.myAnimator.SetTrigger("Attack2");
                break;
            case GRUNTATTACKTYPE.SLAM:
                _monster.myAnimator.SetTrigger("Attack3");
                break;
            default:
                break;
        }
    }



    protected override void IdleStart()
    {
        base.IdleStart();
    }
    protected override void IdleUpdate()
    {
        base.IdleUpdate();
    }
    protected override void IdleExit()
    {
        base.IdleExit();
    }

    protected override IEnumerator SpawnDissolve()
    {
        yield return null;
        ChangeState(MONSTER_STATE.STATE_IDLE);
    }

    protected override void TraceStart()
    {
        base.TraceStart();
    }
    protected override void TraceUpdate()
    {
        base.TraceUpdate();
    }

    protected override void LookTarget() { }

    protected override void KillStart()
    {
        _monster.myAnimator.SetTrigger("Laugh");
    }

    /// <summary>
    /// 죽었을때 소리
    /// </summary>
    public override void DeathSound()
    {
        switch (_gruntSize)
        {
            case GRUNTSIZE.BIG:
                SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Monster Big Death " + UnityEngine.Random.Range(1, 3), 0.8f);
                break;
            case GRUNTSIZE.MEDIUM:
                SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Monster Medium Die" + UnityEngine.Random.Range(1, 3), 0.8f);
                break;
        }
    }
}
