using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using System;

public class BatAction : MonsterAction
{
    bool canPanic;

    [SerializeField] private Transform _baseMeleeAttackPos;
    [SerializeField] private GameObject _baseMeleeAttackPrefab;

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
            _monster.MyAnimator.SetTrigger("Panic");
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

    protected override void DoAttack()
    {
        GameObject obj = ObjectPoolManager.Instance.GetObject(_baseMeleeAttackPrefab);
        obj.transform.SetParent(this.transform);
        obj.transform.position = _baseMeleeAttackPos.position;

        Attack atk = obj.GetComponent<Attack>();
        atk.SetParent(gameObject);
        atk.PlayAttackTimer(1);
    }

    /////////// 캐스팅 관련 /////////////
}
