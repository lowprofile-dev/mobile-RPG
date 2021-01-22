////////////////////////////////////////////////////
/*
    File BleedingSkill.cs
    class BleedingSkill
    
    담당자 : 안영훈
    부 담당자 : 

    출혈 (지속 데미지) 효과가 있는 디버프를 적용시키려면 Collider를 가지고 있는 공격 오브젝트에 넣으면 됨.

*/
////////////////////////////////////////////////////
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedingSkill : Debuffer
{
   
    // 확률 계산후 맞은 적에게 디버프를 거는 함수
    public override void ApplyDebuff(GameObject unit)
    {
        float roll = Random.Range(0, 100);
        if (roll <= Proc)
        {
            unit.GetComponent<LivingEntity>().DebuffManager.AddDebuff(GetDebuff(unit.GetComponent<LivingEntity>()));      
        }
    }

    public override Debuff GetDebuff(LivingEntity unit)
    {
        //디버프 옵션 - 도트데미지 , 데미지 간격 , 지속시간 , 타겟
        return new BleedingDebuff(3, 1, 5, unit);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(transform.tag == "BossAttack")
        {
            if (other.CompareTag("Player"))
            {
                ApplyDebuff(other.gameObject);
            }
        }
        
    }
}
