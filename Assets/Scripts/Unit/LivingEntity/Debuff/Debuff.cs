////////////////////////////////////////////////////
/*
    File Debuff.cs
    class Debuff
    
    담당자 : 안영훈
    부 담당자 : 

    디버프 추상클래스 - 새로운 디버프를 만들고 싶다면 이 클래스를 상속받으면 됨.
*/
////////////////////////////////////////////////////
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Debuff { 

    protected LivingEntity target; //타겟
    private float duration; //지속시간
    protected float elapsed; //시간
    protected GameObject effect;

    public Debuff(LivingEntity target, float duration) //디버프
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
        ObjectPoolManager.Instance.ReturnObject(effect);

        if (target != null)
        {
            target.speed = target.MAXspeed;     
            target.DebuffManager.RemoveDebuff(this);
        }
    }
        
}
