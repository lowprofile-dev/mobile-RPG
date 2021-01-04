using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Debuff { //디버프 추상클래스

    protected MonsterAction target; //몬스터
    private float duration; //지속시간
    protected float elapsed; //시간
    
    public Debuff(MonsterAction target, float duration) //디버프
    {
        this.target = target;
        this.duration = duration;
    }
    public virtual void Update() //디버프 업데이트
    {
      
        elapsed += Time.deltaTime;
        if (elapsed >= duration)
        {
            Remove();
        }

    }
    public virtual void Remove() //디버프 삭제
    {
        if (target != null)
        {
            target.monster.speed = target.monster.MAXspeed;     
            target.monster.DebuffManager.RemoveDebuff(this);
        }
    }
        
}
