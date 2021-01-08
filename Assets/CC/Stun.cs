using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : CwordControl
{
    //Stun , Fall

    public Stun(float duration , LivingEntity target , string type) : base(target , duration , type)
    {

    }

    public void start()
    {
        if(target != null)
        {
            target.Stun = true;
        }
    }

    public override void Updata()
    {
        base.Updata();
    }
    
}
