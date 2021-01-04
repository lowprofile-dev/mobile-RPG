using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedingSkill : Debuffer
{
   
    public override void ApplyDebuff(GameObject monster)
    {
        float roll = Random.Range(0, 100);
        if (roll <= Proc)
        {
            monster.GetComponent<MonsterAction>().monster.DebuffManager.AddDebuff(GetDebuff(monster.GetComponent<MonsterAction>()));      
        }
    }

    public override Debuff GetDebuff(MonsterAction monster)
    {
        //웨폰마스터리에서 디버프 능력치 구하기?
        return new BleedingDebuff(3, 1, 3, monster);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            ApplyDebuff(other.gameObject);
        }
    }
}
