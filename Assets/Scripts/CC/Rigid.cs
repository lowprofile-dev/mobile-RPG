////////////////////////////////////////////////////
/*
    File Rigid.cs
    class Rigid
    
    담당자 : 안영훈
    부 담당자 :


    상태 이상 경직 
*/
////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rigid : CwordControl
{
    public Rigid(float duration, LivingEntity target, string type) : base(target, duration, type) { }

    // Update is called once per frame
    public override void Updata()
    {
        base.Updata();
    }
}
