using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rigid : CwordControl
{
    public Rigid(float duration, LivingEntity target, string type) : base(target, duration, type) { }
    void Start()
    {
        if (target != null)
        {
            target.Rigid = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        base.Updata();
    }
}
