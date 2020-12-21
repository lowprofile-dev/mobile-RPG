using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    BGM, EFFECT, UI
}

/// <summary>
/// 사운드 관련 정보를 모아두는 클래스이다.
/// </summary>
public class SoundInfor : MonoBehaviour
{
    private SoundType   soundType; public SoundType SoundType { get { return soundType; } }
    private float       startTime; public float     StartTime { get { return startTime; } }

    public void SetInfor(SoundType soundType, float startTime)
    {
        this.soundType = soundType;
        this.startTime = startTime;
    }
}
