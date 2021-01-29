////////////////////////////////////////////////////
/*
    File GoblinAction.cs
    class GoblinAction
    
    담당자 : 이신홍
    부 담당자 : 

    고블린의 행동을 정의한다.
*/
////////////////////////////////////////////////////

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GoblinAction : MonsterAction
{
    bool _isDamaged = false;
    bool _canPlayPanicSound = true;

    /////////// 기본 /////////////

    /////////// 상태 관련 /////////////

    /////////// 스폰 관련 /////////////

    /////////// 대기 관련 /////////////

    protected override void UpdateMonster()
    {
        base.UpdateMonster();
    }
    protected override void IdleUpdate()
    {
        base.IdleUpdate();
        ToStirr(); // 자는 행동이 추가됨.
    }

    protected override void DoSomethingIdleSearchFind()
    {
        if (_isDamaged) base.DoSomethingIdleSearchFind();
        // 데미지를 안 받았다면 아무것도 하지 않는다.
    }

    /////////// 두리번거리기 관련 /////////////

    protected override void StirrStart()
    {
        _navMeshAgent.isStopped = true;
        _monster.myAnimator.SetTrigger("Sleep");
    }

    protected override void StirrUpdate()
    {
        if (CheckAnimationOver("Sleep", 1.0f))
        {
            ChangeState(MONSTER_STATE.STATE_IDLE); // 일정 시간 뒤 IDLE로 돌아옴.
        }
    }

    protected override void StirrExit()
    {
        _navMeshAgent.isStopped = false;
        _monster.myAnimator.ResetTrigger("Sleep");
    }

    /////////// 탐색 관련 /////////////

    protected override void FindStart()
    {
        ChangeState(MONSTER_STATE.STATE_TRACE); // 공격을 받으면 바로 추적하므로

        if(_canPlayPanicSound)
        {
            AudioSource source = SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Small Monster Find Type 1-" + UnityEngine.Random.Range(1, 4), 0.8f);
            SoundManager.Instance.SetPitch(source, 0.6f);
            _canPlayPanicSound = false;
        }
    }

    protected override void FindExit()
    {
        // 아무것도 하지 않는다.
    }

    protected override void DoReturn()
    {
        base.DoReturn();
        _isDamaged = false;
        _canPlayPanicSound = true;
    }

    /////////// 추적 관련 /////////////

    /////////// 공격 관련 /////////////
    /// <summary>
    /// 몬스터 공격 소리 재생
    /// </summary>
    protected override void AttackSound()
    {
        AudioSource source = SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Monster Punch " + UnityEngine.Random.Range(1, 4), 0.6f);
        SoundManager.Instance.SetPitch(source, 1.5f);

        int randomSound = UnityEngine.Random.Range(0, 10);
        if (randomSound < 2) SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Small Monster Growl " + 1, 0.5f);
        else if (randomSound < 4) SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Small Monster Growl " + 2, 0.5f);
    }
    /// <summary>
    /// 몬스터 공격 애니메이션 선택
    /// </summary>
    protected override void SetAttackAnimation()
    {
        if (_attackType == 0) _monster.myAnimator.SetTrigger("Attack");
        else if (_attackType == 1) _monster.myAnimator.SetTrigger("AttackSpecial");
    }
    /// <summary>
    /// 몬스터 공격 애니메이션 리셋
    /// </summary>
    protected override void ResetAttackAnimation()
    {
        if (_attackType == 0) _monster.myAnimator.ResetTrigger("Attack");
        else if (_attackType == 1) _monster.myAnimator.ResetTrigger("AttackSpecial");
    }

    /////////// 캐스팅 관련 /////////////
    /// <summary>
    /// 캐스팅이 시작되면 일정확률로 어떤 공격을 할지 정해짐
    /// </summary>
    protected override void CastStart()
    {
        int proc = Random.Range(0, 100);
        if (proc <= 30)
        {
            _castTime = 2f;
            _attackType = 1;
        }
        else
        {
            _castTime = 1f;
            _attackType = 0;
        }
    }

    /////////// 피격 관련 /////////////

    /// <summary>
    /// 데미지를 받았을때 진행할 알고리즘 목록
    /// </summary>
    protected override void DamagedProcess(float dmg, bool SetAnimation = true)
    {
        _monster.Damaged(dmg, false); // 몬스터에게 데미지를 입힌다.
        _bar.HpUpdate(); // 몬스터의 HP바를 갱신한다.

        DamagedChangeState();  // 고블린은 첫피격이 시작했을 때 부터 플레이어를 공격한다.
        bool isDeath = DeathCheck(); // hp가 0이하인지 판별

        if (isDeath) CheckDeathAndChange(); // 죽었다고 판단될경우 사망상태로 변경하는 함수
    }

    /// <summary>
    /// 직접적인 데미지를 받으면 플레이어를 인식한다.
    /// </summary>
    protected void DamagedChangeState()
    {
        _isDamaged = true;
        if (_currentState == MONSTER_STATE.STATE_IDLE || _currentState == MONSTER_STATE.STATE_STIRR) ChangeState(MONSTER_STATE.STATE_TRACE);
    }
}
