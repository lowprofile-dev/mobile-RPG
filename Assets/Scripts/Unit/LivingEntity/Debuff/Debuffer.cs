////////////////////////////////////////////////////
/*
    File Debuffer.cs
    class Debuffer
    
    담당자 : 안영훈
    부 담당자 : 

    디버퍼 추상클래스 - 디버프를 적용시킬 스킬에 상속을 받으면 사용 가능.
*/
////////////////////////////////////////////////////
///
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
    public abstract Debuff GetDebuff(LivingEntity unit);  // 디버프 타입을 가져오는 코드
}
