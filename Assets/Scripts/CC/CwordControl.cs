////////////////////////////////////////////////////
/*
    File CwordControl.cs
    class CwordControl
    
    담당자 : 안영훈
    부 담당자 :
*/
////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CwordControl
{
    protected LivingEntity target; // CC가 걸릴 타겟
    private float duration; // 지속시간
    protected float elapsed; // 지속시간 계산용 시간 변수
    protected string type; // CC 타입 ex) stun , fall ,rigid
    GameObject effect; // CC 이펙트

    public CwordControl(LivingEntity target, float duration, string type) // cc 생성자
    {
        this.target = target;
        this.duration = duration;
        this.type = type;
    }
    
    public virtual void Updata() // CC 지속시간 계산후 CC 삭제
    {
        elapsed += Time.deltaTime;
        if(elapsed >= duration)
        {
            Remove();
        }
    }
    public virtual void Remove() // CC 삭제
    {
        if (target != null)
        {

            target.CCManager.RemoveCC(type);
            target.CCManager.CurrentCC = null;

        }
    }
}
