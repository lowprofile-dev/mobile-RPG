////////////////////////////////////////////////////
/*
    File CCAttack.cs
    class CCAttack
    
    담당자 : 안영훈
    부 담당자 : 이신홍
*/
////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCAttack : MonoBehaviour
{
    [Tooltip("넘어짐 확률 0 ~ 1")]
    [SerializeField] float FallProc;
    [Tooltip("스턴 확률 0 ~ 1")]
    [SerializeField] float StunProc;
    [Tooltip("경직 확률 0 ~ 1")]
    [SerializeField] float FirgProc;

    [HideInInspector] public int _level;
    [SerializeField] private float _levelFallProc; // 레벨 별 추가되는 수치 : fall
    [SerializeField] private float _levelStunProc; // 레벨 별 추가되는 수치 : stun
    [SerializeField] private float _levelFrigProc; // 레벨 별 추가되는 수치 : frig

    

    /// <summary>
    /// 대상이 CC를 받을 수 있는 상태인지 체크한다.
    /// </summary>
    public bool CheckCanApplyCC(GameObject unit)
    {
        MonsterAction monsterAction = unit.GetComponent<MonsterAction>();
        Player playerAction = unit.GetComponent<Player>();

        if (unit.GetComponent<LivingEntity>().Hp <= 0) return false;
        if (monsterAction != null && (monsterAction.currentState == MONSTER_STATE.STATE_DIE || monsterAction.currentState == MONSTER_STATE.STATE_SPAWN)) return false;
        else if (playerAction != null && playerAction.cntState == PLAYERSTATE.PS_DIE) return false;
        else return true;
    }

    public void ApplyCC(GameObject unit , float fstun , float ffall , float frig) // CC 적용시키는 코드
    {
        if(CheckCanApplyCC(unit) == false) return;

        /*
         *  확률 계산 우선순위
         *  넘어짐 > 스턴 > 경직 순으로 계산됨
         */

        fstun += _level * _levelStunProc;
        ffall += _level * _levelFallProc;
        frig += _level * _levelFrigProc;

        if (fstun > 1) fstun = 1;
        if (ffall > 1) ffall = 1;
        if (frig > 1) frig = 1;

        float roll = Random.Range(0, 100);

        float fallRate = ffall * 100f;
        float stunRate = ((100 - fallRate) * fstun);
        float frigRate = ((1 - ffall) * (1 - fstun) * frig) * 100f;
        float none = ((1 - fstun) * (1 - ffall) * (1 - frig)) * 100f;


        if(roll <= fallRate)
        {
            //넘어짐
            unit.GetComponent<LivingEntity>().CCManager.AddCC("fall", new Fall(2, unit.GetComponent<LivingEntity>(), "fall") , unit);
        }
        else if (roll <= stunRate + fallRate)
        {
            //스턴
            unit.GetComponent<LivingEntity>().CCManager.AddCC("stun", new Stun(2, unit.GetComponent<LivingEntity>(), "stun") , unit);
        }
        else if (roll <= frigRate + fallRate + frigRate)
        {
            //경직
            unit.GetComponent<LivingEntity>().CCManager.AddCC("rigid", new Rigid(1, unit.GetComponent<LivingEntity>(), "rigid") , unit);
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            ApplyCC(other.gameObject, StunProc, FallProc, FirgProc);
        }
    }
}
