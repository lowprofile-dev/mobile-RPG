using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using System;

public class GhostAction : MonsterAction
{
    bool canPanic;
    /////////// 기본 /////////////

    /// <summary>
    /// 패닉이 추가되었으므로 오버라이딩
    /// </summary>
    public override void InitObject()
    {
        base.InitObject();
        canPanic = true;
    }

    /////////// 상태 관련 /////////////

    /////////// 스폰 관련 /////////////

    /////////// 대기 관련 /////////////

    /////////// 두리번거리기 관련 /////////////

    /////////// 탐색 관련 /////////////

    protected override void FindStart()
    {
        base.FindStart();

        if (canPanic)
        {
            _monster.myAnimator.SetTrigger("Panic");
            AudioSource source = SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Small Monster Find Type 2-" + UnityEngine.Random.Range(1, 4), 0.7f);
            SoundManager.Instance.SetPitch(source, 1.2f);
        }
    }

    protected override bool CheckFindAnimationOver()
    {
        if (canPanic) return CheckAnimationOver("Panic", 1.0f);
        else return true;
    }

    protected override void FindExit()
    {
        base.FindExit();
        canPanic = false;
    }

    /////////// 추적 관련 /////////////

    protected override void DoReturn()
    {
        base.DoReturn();
        canPanic = true;
    }

    /////////// 공격 관련 /////////////

    /// <summary>
    /// 몬스터 공격 소리 재생
    /// </summary>
    protected override void AttackSound()
    {
        AudioSource source = SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Monster Punch " + UnityEngine.Random.Range(1,4), 0.5f);
        SoundManager.Instance.SetPitch(source, 1.5f);

        int randomSound = UnityEngine.Random.Range(0, 10);
        if (randomSound < 2) SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Small Monster Growl " + 1, 0.5f);
        else if (randomSound < 4) SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Small Monster Growl " + 2, 0.5f);
    }


    /////////// 캐스팅 관련 /////////////
    /// <summary>
    /// </summary>

    protected override void LookTarget() {  }
}
