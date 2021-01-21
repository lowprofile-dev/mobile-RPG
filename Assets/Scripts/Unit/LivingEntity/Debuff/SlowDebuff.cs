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
        private bool applied;
        string tag;

        public SlowDebuff(float slowingFactor, float duration, LivingEntity target) : base(target, duration)
        {
            tag = target.tag;
            this.slowingFactor = slowingFactor;
        }
        public override void Update()
        {
            if (target != null)
            {
                if (!applied)
                {
                    applied = true;
                   if (target.speed > minSpeed)
                    {
                        if (tag == "Player")
                            StatusManager.Instance.finalStatus.moveSpeed -= (StatusManager.Instance.finalStatus.moveSpeed * slowingFactor) / 100f;
                        else
                        {
                            target.setSpeed(target.speed -= (target.MAXspeed * slowingFactor) / 100f);
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
                if (tag == "Player") StatusManager.Instance.finalStatus.moveSpeed = StatusManager.Instance.finalStatus.maxHp;
                else
                {
                    target.setSpeed(target.MAXspeed);
                }

                base.Remove();
            }
        }


    
}
