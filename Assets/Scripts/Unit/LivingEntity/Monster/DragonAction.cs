////////////////////////////////////////////////////
/*
    File DragonAction.cs
    class DragonAction
    
    담당자 : 이신홍
    부 담당자 : 안영훈

    드래곤의 행동을 정의한다.
*/
////////////////////////////////////////////////////

using System.Collections;
using UnityEngine;

public class DragonAction : MonsterAction
{
    bool canPanic;

    /////////// 기본 /////////////
    
    public override void InitObject()
    {
        base.InitObject();
        canPanic = true; // 패닉이 추가되었으므로 오버라이딩
    }

    /////////// 상태 관련 /////////////

    /////////// 스폰 관련 /////////////

    /////////// 대기 관련 /////////////

    protected override void IdleUpdate()
    {
        base.IdleUpdate();
        ToStirr(); // 주변을 둘러보는 애니메이션이 추가되었으므로 오버라이딩
    }
    
    /////////// 두리번거리기 관련 /////////////

    /////////// 탐색 관련 /////////////

    protected override void FindStart()
    {
        base.FindStart();

        if (canPanic)
        {
            _monster.myAnimator.SetTrigger("Panic"); // 패닉이 추가되었으므로 오버라이딩
        }
    }
    protected override void CastStart()
    {

        int proc = Random.Range(0, 100);
        if(proc <= 30)
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

    /// <summary>
    /// 패닉 사운드를 재생한다. (애니메이션 이벤트로 호출)
    /// </summary>
    private void DoPanicSound()
    {
        AudioSource source = SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Small Monster Panic " + UnityEngine.Random.Range(1, 4), 0.5f);
        SoundManager.Instance.SetPitch(source, 0.7f);
    }

    protected override bool CheckFindAnimationOver()
    {
        if (canPanic) return CheckAnimationOver("Panic", 1.0f); // 패닉이 추가되었으므로 오버라이딩
        else return true;
    }

    protected override void FindExit()
    {
        base.FindExit();
        canPanic = false; // 패닉이 추가되었으므로 오버라이딩
    }

    /////////// 추적 관련 /////////////

    protected override void DoReturn()
    {
        base.DoReturn();
        canPanic = true; // 패닉이 추가되었으므로 오버라이딩
    }

    /////////// 공격 관련 /////////////

    protected override void DoAttack()
    {
        
         GameObject obj = ObjectPoolManager.Instance.GetObject(_baseMeleeAttackPrefab);
         obj.transform.SetParent(this.transform);
         obj.transform.position = _baseMeleeAttackPos.position;

         Attack atk = obj.GetComponent<Attack>();
         atk.SetParent(gameObject);
         atk.PlayAttackTimer(0.3f);
         AttackSound();
        
        _navMeshAgent.isStopped = false;
        ChangeState(MONSTER_STATE.STATE_TRACE);
    }

    /// <summary>
    /// 몬스터 공격 소리 재생
    /// </summary>
    protected override void AttackSound()
    {
        AudioSource source = null;

        switch(_attackType)
        {
            case 0:
                source = SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Monster Bite 2", 0.3f);
                SoundManager.Instance.SetPitch(source, 1.6f);
                break;
            case 1:
                source = SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Monster/Monster Bite 1", 0.3f);
                SoundManager.Instance.SetPitch(source, 1.2f);
                break;
        }
    }

    protected override void SetAttackAnimation()
    {
        // 애니메이션에 따라 공격 알고리즘이 달라진다..

        if (_attackType == 0) _monster.myAnimator.SetTrigger("Attack");
        else if(_attackType == 1) _monster.myAnimator.SetTrigger("AttackSpecial");
    }

    protected override void ResetAttackAnimation()
    {
        if(_attackType == 0) _monster.myAnimator.ResetTrigger("Attack");
        else if(_attackType == 1) _monster.myAnimator.ResetTrigger("AttackSpecial");
    }

    /////////// 캐스팅 관련 /////////////
}
