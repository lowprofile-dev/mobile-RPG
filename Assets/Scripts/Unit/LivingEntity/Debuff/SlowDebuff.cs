////////////////////////////////////////////////////
/*
    File SlowDebuff.cs
    class SlowDebuff
    
    담당자 : 안영훈
    부 담당자 : 

    슬로우 효과를 주는 디버프 (미완성)
    Player 스텟 관련 사항이 너무 복잡하여 아직 적용 시키지 못하였음
*/
////////////////////////////////////////////////////
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDebuff : Debuff
{
    private float slowingFactor;
    private float minSpeed = 0.5f;
    private float defalutSpeed;
    private bool applied;
    string tag;

    public SlowDebuff(float slowingFactor, float duration, LivingEntity target) : base(target, duration)
    {
        tag = target.tag;
        this.slowingFactor = slowingFactor;
        //슬로우 디버프 텍스트 출력
        GameObject txt = ObjectPoolManager.Instance.GetObject("UI/DamageTEXT");
        txt.transform.SetParent(target.gameObject.transform);
        txt.transform.localPosition = Vector3.zero;
        txt.transform.rotation = Quaternion.identity;
        txt.GetComponent<DamageText>().PlayText("슬로우!", "player");

        effect = ObjectPoolManager.Instance.GetObject("Effect/DebuffEffect/SlowDebuffEffet");
        effect.transform.SetParent(target.gameObject.transform);
        effect.transform.localPosition = Vector3.zero;
        effect.transform.rotation = Quaternion.identity;
    }
    public override void Update()
    {
        if (target != null)
        {
            if (!applied)
            {
                applied = true;
               if (Player.Instance.statusManager.finalStatus.moveSpeed > minSpeed)
                {
                    if (tag == "Player")
                    {
                        //Player.Instance.statusManager.finalStatus.moveSpeed -= (Player.Instance.statusManager.finalStatus.moveSpeed * slowingFactor) / 100f;
                        Player.Instance.SlowingFactor = slowingFactor;
                    }
                }
            }
        }

        base.Update();
    }
    public override void Remove()
    {
        if (target != null)
        {
            //if (tag == "Player") Player.Instance.statusManager.finalStatus.moveSpeed = defalutSpeed;
            Player.Instance.SlowingFactor = 0f;
            base.Remove();
        }
    }
   
}
