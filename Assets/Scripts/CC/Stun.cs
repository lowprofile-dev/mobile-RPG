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
