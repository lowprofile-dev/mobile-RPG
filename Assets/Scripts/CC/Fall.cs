////////////////////////////////////////////////////
/*
    File Fall.cs
    class Fall
    
    담당자 : 안영훈
    부 담당자 :
*/
////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : CwordControl
{
    //넘어짐 상태이상
    public Fall(float duration, LivingEntity target, string type) : base(target, duration, type) { }

    public override void Updata()
    {
        base.Updata();
    }
   
}
