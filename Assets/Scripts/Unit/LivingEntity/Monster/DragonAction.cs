using System.Collections;
using UnityEngine;

public class DragonAction : MonsterAction
{
    bool canPanic;

    [SerializeField] private Transform _baseMeleeAttackPos;
    [SerializeField] private GameObject _baseMeleeAttackPrefab;

    Collider _baseAtkCollision;


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

    protected override void IdleUpdate()
    {
        base.IdleUpdate();
        ToStirr();
    }


    /////////// 두리번거리기 관련 /////////////

    /////////// 탐색 관련 /////////////

    public override void FindStart()
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
        if(_attackType == 0 || _attackType == 1)
        {
            GameObject obj = ObjectPoolManager.Instance.GetObject(_baseMeleeAttackPrefab);
            obj.transform.SetParent(this.transform);
            obj.transform.position = _baseMeleeAttackPos.position;

            Attack atk = obj.GetComponent<Attack>();
            atk.SetParent(gameObject);
            atk.PlayAttackTimer(1);
        }
    }

    protected override void SetAttackAnimation()
    {
        if(_attackType == 0) _monster.MyAnimator.SetTrigger("Attack");
        else if(_attackType == 1) _monster.MyAnimator.SetTrigger("AttackSpecial");
    }

    protected override void ResetAttackAnimation()
    {
        if(_attackType == 0) _monster.MyAnimator.ResetTrigger("Attack");
        else if(_attackType == 1) _monster.MyAnimator.ResetTrigger("AttackSpecial");
    }

    /////////// 캐스팅 관련 /////////////
}
