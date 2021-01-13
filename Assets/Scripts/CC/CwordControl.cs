using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CwordControl
{
    protected LivingEntity target;
    private float duration;
    protected float elapsed;
    protected string type;
    GameObject effect;

    public CwordControl(LivingEntity target, float duration, string type)
    {
        this.target = target;
        this.duration = duration;
        this.type = type;
    }
    
    public virtual void Updata()
    {
        elapsed += Time.deltaTime;
        if(elapsed >= duration)
        {
      //      Debug.Log("CC 지속시간 끝");
            Remove();
        }
    }
    public virtual void Remove()
    {
        if (target != null)
        {

            target.CCManager.RemoveCC(type);
            target.CCManager.CurrentCC = null;

        }
    }
}
