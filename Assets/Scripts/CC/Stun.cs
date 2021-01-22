////////////////////////////////////////////////////
/*
    File Stun.cs
    class Stun
    
    담당자 : 안영훈
    부 담당자 :

    상태이상 스턴 
*/
////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : CwordControl
{
    
    public Stun(float duration, LivingEntity target, string type) : base(target, duration, type) { }

    public override void Updata()
    {
        base.Updata();
    }
    
}
