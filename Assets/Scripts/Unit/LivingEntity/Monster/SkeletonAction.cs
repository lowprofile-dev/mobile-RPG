////////////////////////////////////////////////////
/*
    File SkeletonAction.cs
    class SkeletonAction
    
    담당자 : 안영훈

    모히칸 스켈레톤 몬스터

*/
////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAction : MonsterAction
{
    bool canPanic;
    enum MOHICANATTACKTYPE { LEFT_ATTACK , RIGHT_ATTACK , SLAM} // 3가지 공격패턴
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
        }
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
    protected override void LookTarget() { }

    protected override void KillStart()
    {
        _monster.myAnimator.SetTrigger("Laugh");
    }

}
