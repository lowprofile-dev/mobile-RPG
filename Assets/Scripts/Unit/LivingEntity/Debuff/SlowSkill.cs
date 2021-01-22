////////////////////////////////////////////////////
/*
    File SlowSkill.cs
    class SlowSkill
    
    담당자 : 안영훈
    부 담당자 : 

    슬로우 효과를 주는 디버퍼 (미완성)
    Player 스텟 관련 사항이 너무 복잡하여 아직 적용 시키지 못하였음 
*/
////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowSkill : Debuffer
{
    [SerializeField] float slowingFactor;
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
        //웨폰마스터리에서 디버프 능력치 구하기?
        return new SlowDebuff(slowingFactor, DebuffDuration, unit);
    }

    private void OnTriggerEnter(Collider other)
    {
       
            if (other.CompareTag("Player"))
            {
                ApplyDebuff(other.gameObject);
            }
        

    }
}
