////////////////////////////////////////////////////
/*
    File GruntAction.cs
    class GruntAction
    
    담당자 : 안영훈

*/
////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntAction : MonsterAction
{
    bool canPanic;
    enum GRUNTATTACKTYPE {DEFALUT_ATTACK , SHOULDER_BASH , SPIN , SLAM }; // 4가지 공격패턴
    GRUNTATTACKTYPE atktype;

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
    private void DoPanicSound(string monsterName)
    {
        if(monsterName.Equals("GruntBig"))
        {
            SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Monster Big Panic " + UnityEngine.Random.Range(1, 3), 0.6f);
        }

        else if (monsterName.Equals("GruntMedium"))
        {
            SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Monster Medium Panic " + UnityEngine.Random.Range(1, 4), 0.6f);
        }
    }

    public override void DeathSound()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Monster Medium Die " + UnityEngine.Random.Range(1, 3), 0.8f);
    }

    protected override bool CheckFindAnimationOver()
    {
        if (canPanic) return CheckAnimationOver("Panic", 1.0f);
        else return true;
    }

    protected override void FindPlayer()
    {
        base.FindPlayer();
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

        int proc = Random.Range(0, 100);

        if (proc <= 25)
        {
            _castTime = 1.5f;
            atktype = GRUNTATTACKTYPE.DEFALUT_ATTACK;
        }
        else if (proc <= 50)
        {
            _castTime = 1.5f;
            atktype = GRUNTATTACKTYPE.SHOULDER_BASH;
        }
        else if (proc <= 75)
        {
            _castTime = 2.5f;
            atktype = GRUNTATTACKTYPE.SLAM;
        }
        else
        {
            _castTime = 2.5f;
            atktype = GRUNTATTACKTYPE.SPIN;
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

    /// <summary>
    /// 휘두르기 시작할 때 소리
    /// </summary>
    private void AttackSoundWhooshStart(string monsterName)
    {
        if (monsterName.Equals("GruntBig"))
        {
            SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Monster Big Whoosh Start " + UnityEngine.Random.Range(1, 4), 0.9f);
        }

        else if(monsterName.Equals("GruntMedium"))
        {
            SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Monster Medium Whoosh " + UnityEngine.Random.Range(1, 6), 0.9f);
        }
    }

    /// <summary>
    /// 휘두른 후의 소리
    /// </summary>
    private void AttackSoundWhooshAfter(string monsterName)
    {
        if (monsterName.Equals("GruntBig"))
        {
            SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Monster Big Whoosh " + UnityEngine.Random.Range(1, 6), 1);
        }
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

    protected override void LookTarget() { }

    protected override void KillStart()
    {
        _monster.myAnimator.SetTrigger("Laugh");
    }
}
