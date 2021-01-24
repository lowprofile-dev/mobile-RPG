////////////////////////////////////////////////////
/*
    File DragonAction.cs
    class DragonAction
    
    담당자 : 이신홍
    부 담당자 : 

    드래곤의 행동을 정의한다.
*/
////////////////////////////////////////////////////

using System.Collections;
using UnityEngine;

public class DragonAction : MonsterAction
{
    bool canPanic;

    [SerializeField] private Transform _baseMeleeAttackPos;
    [SerializeField] private GameObject _baseMeleeAttackPrefab;

    Collider _baseAtkCollision;


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
        if(_attackType == 0 || _attackType == 1) // 공격 타입 2개
        {
            GameObject obj = ObjectPoolManager.Instance.GetObject(_baseMeleeAttackPrefab);
            obj.transform.SetParent(this.transform);
            obj.transform.position = _baseMeleeAttackPos.position;

            Attack atk = obj.GetComponent<Attack>();
            atk.SetParent(gameObject);
            atk.PlayAttackTimer(0.3f);
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
