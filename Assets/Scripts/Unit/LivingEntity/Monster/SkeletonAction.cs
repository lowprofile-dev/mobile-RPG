////////////////////////////////////////////////////
/*
    File SkeletonAction.cs
    class SkeletonAction
    
    담당자 : 안영훈

    모히칸 스켈레톤 몬스터

*/
////////////////////////////////////////////////////
using UnityEngine;

public class SkeletonAction : MonsterAction
{
    bool canPanic;
    enum MOHICANATTACKTYPE { LEFT_ATTACK, RIGHT_ATTACK, SLAM } // 3가지 공격패턴
    MOHICANATTACKTYPE atktype;

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
            DoPanicSound();
        }
    }

    /// <summary>
    /// 패닉일때 소리
    /// </summary>
    private void DoPanicSound()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Monster Big Panic " + UnityEngine.Random.Range(1, 3), 0.6f);
    }

    protected override bool CheckFindAnimationOver()
    {
        if (canPanic) return CheckAnimationOver("Panic", 1.0f);
        else return true;
    }

    protected override void FindPlayer()
    {
        //base.FindPlayer();
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

    protected override void CastStart()
    {
        //_monster.myAnimator.SetTrigger("Idle");

        int proc = Random.Range(0, 100);

        if (proc <= 35)
        {
            _castTime = 1.5f;
            atktype = MOHICANATTACKTYPE.LEFT_ATTACK;
        }
        else if (proc <= 70)
        {
            _castTime = 1.5f;
            atktype = MOHICANATTACKTYPE.RIGHT_ATTACK;
        }
        else
        {
            _castTime = 2f;
            atktype = MOHICANATTACKTYPE.SLAM;
        }
    }

    protected override void SetAttackAnimation()
    {
        switch (atktype)
        {
            case MOHICANATTACKTYPE.RIGHT_ATTACK:
                _monster.myAnimator.SetTrigger("Attack0");
                break;
            case MOHICANATTACKTYPE.LEFT_ATTACK:
                _monster.myAnimator.SetTrigger("Attack1");
                break;
            case MOHICANATTACKTYPE.SLAM:
                _monster.myAnimator.SetTrigger("Attack2");
                break;
            default:
                break;
        }
    }


    /// <summary>
    /// 휘두르기 시작할 때 소리
    /// </summary>
    private void AttackSoundWhooshStart()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Monster Big Whoosh Start " + UnityEngine.Random.Range(1, 4), 0.9f);
    }

    /// <summary>
    /// 휘두른 후의 소리
    /// </summary>
    private void AttackSoundWhooshAfter()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Monster Big Whoosh " + UnityEngine.Random.Range(1, 6), 1);
    }

    protected override void LookTarget() { }

    protected override void KillStart()
    {
        _monster.myAnimator.SetTrigger("Laugh");
    }

    public override void DeathSound()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Monster Big Death " + UnityEngine.Random.Range(1, 3), 0.8f);
    }
}
