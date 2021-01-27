////////////////////////////////////////////////////
/*
    File QuestData.cs
    class QuestData - 사운드 관련 정보를 모아두는 클래스이다.
    enum SoundType - Sound 타입을 서술한다.

    담당자 : 이신홍
    부 담당자 :
*/
////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    BGM, EFFECT, UI
}

public class SoundInfor : MonoBehaviour
{
    private SoundType   _soundType;      // 사운드의 형태
    private float       _startTime;      // 사운드 시작 시간
    private string      _path;           // 사운드 경로
    private float       _initVolume;     // 사운드 시작 볼륨

    // property
    public SoundType soundType { get { return _soundType; } }
    public float startTime { get { return _startTime; } }
    public string path { get { return _path; } }
    public float initVolume { get { return _initVolume; } }

    public void SetInfor(SoundType soundType, float startTime, string path, float initVolume) 
    {
        this._soundType = soundType;
        this._startTime = startTime;
        this._path = path;
        this._initVolume = initVolume;
    }
}
