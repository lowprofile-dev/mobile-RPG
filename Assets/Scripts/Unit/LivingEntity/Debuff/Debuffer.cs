using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Debuffer : MonoBehaviour
{
    [SerializeField]
    private float debuffDuration;   //디버프 지속시간
    [SerializeField]
    private float proc;             //디버프가 걸릴 확률

    public float DebuffDuration { get { return debuffDuration; } set { debuffDuration = value; } }
    public float Proc { get { return proc; } set { proc = value; } }

    public abstract void ApplyDebuff(GameObject monster); // 디버프 적용시키는 코드
    public abstract Debuff GetDebuff(LivingEntity unit); 
}
