using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : CwordControl
{

    public Fall(float duration, LivingEntity target, string type) : base(target, duration, type) { }
    void Start()
    {
        if (target != null)
        {
            target.Fall = true;
        }
    }

    
    void Update()
    {
        base.Updata();
    }
}
