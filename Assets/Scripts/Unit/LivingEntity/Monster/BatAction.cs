////////////////////////////////////////////////////
/*
    File BatAction.cs
    class BatAction
    
    담당자 : 이신홍
    부 담당자 : 안영훈

    박쥐의 행동을 정의한다.
*/
////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using System;

public class BatAction : MonsterAction
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

    /// <summary>
    /// 패닉이 생겼으므로 오버라이딩
    /// </summary>
    protected override void FindStart()
    {
        base.FindStart();

        if (canPanic)
        {
            _monster.myAnimator.SetTrigger("Panic"); // 패닉이 생겼으므로 오버라이딩
        }
    }

    /// <summary>
    /// 패닉 사운드를 재생한다. (애니메이션 이벤트로 호출)
    /// </summary>
    private void DoPanicSound()
    {
        AudioSource source = SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Small Monster Panic " + UnityEngine.Random.Range(1, 4), 0.5f);
        SoundManager.Instance.SetPitch(source, 0.8f);
    }

    protected override bool CheckFindAnimationOver()
    {
        if (canPanic) return CheckAnimationOver("Panic", 1.0f); // 패닉이 생겼으므로 오버라이딩
        else return true;
    }

    protected override void FindExit()
    {
        base.FindExit();
        canPanic = false; // 패닉이 생겼으므로 오버라이딩
    }

    /////////// 추적 관련 /////////////

    protected override void DoReturn()
    {
        base.DoReturn();
        canPanic = true; // 패닉이 생겼으므로 오버라이딩
    }

    /////////// 공격 관련 /////////////
    /// <summary>
    /// 몬스터 공격 소리 재생
    /// </summary>
    protected override void AttackSound()
    {
        AudioSource source = SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Monster Bite 2", 0.3f);
        SoundManager.Instance.SetPitch(source, 1.6f);
    }

    /////////// 캐스팅 관련 /////////////
}
