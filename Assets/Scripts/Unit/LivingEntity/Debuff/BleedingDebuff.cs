////////////////////////////////////////////////////
/*
    File BleedingDebuff.cs
    class BleedingDebuff
    
    담당자 : 안영훈
    부 담당자 : 

    출혈 (지속 데미지) 효과가 있는 디버프
*/
////////////////////////////////////////////////////
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedingDebuff : Debuff
{
    private float tickTime; //지속데미지 간격

    private float timeSinceTick; //지속시간

    private float tickDamage; //지속데미지

    public BleedingDebuff(float tickDamage, float tickTime, float duration, LivingEntity target) : base(target, duration)
    { 
        this.tickDamage = tickDamage;
        this.tickTime = tickTime;

        //출혈 디버프 텍스트 출력
        GameObject txt = ObjectPoolManager.Instance.GetObject("UI/DamageTEXT");
        txt.transform.SetParent(target.gameObject.transform);
        txt.transform.localPosition = Vector3.zero;
        txt.transform.rotation = Quaternion.identity;
        txt.GetComponent<DamageText>().PlayText("출혈!", "player");

        effect = ObjectPoolManager.Instance.GetObject("Effect/DebuffEffect/BleedingDebuffEffet");
        effect.transform.SetParent(target.gameObject.transform);
        effect.transform.localPosition = Vector3.zero;
        effect.transform.rotation = Quaternion.identity;
    }

    public override void Update() // 디버프 지속시간 계산
    {
        if (target != null)
        {
            timeSinceTick += Time.deltaTime;

            if (timeSinceTick >= tickTime) //지속시간이 지속간격보다 크면
            {
                timeSinceTick = 0f;
                target.Damaged(tickDamage, true);
            }
        }

        base.Update();
    }
}
