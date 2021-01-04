using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 테스트용으로 만들어본 출혈 디버프 

public class BleedingDebuff : Debuff
{
    private float tickTime; //지속데미지 간격

    private float timeSinceTick; //지속시간

    private float tickDamage; //지속데미지

    public BleedingDebuff(float tickDamage, float tickTime, float duration, MonsterAction target) : base(target, duration)
    { //디버프
        this.tickDamage = tickDamage;
        this.tickTime = tickTime;
    }

    public override void Update()
    {
        if (target != null)
        {
            timeSinceTick += Time.deltaTime;

            if (timeSinceTick >= tickTime) //지속시간이 지속간격보다 크면
            {
                timeSinceTick = 0f;
                target.Damaged(tickDamage, false);
            }
        }

        base.Update();
    }
}
