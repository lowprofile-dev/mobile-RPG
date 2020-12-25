using System.Collections;
using UnityEngine;

public class SoundManager : SingletonBase<SoundManager>
{
    // 오디오 경로
    private const string    _bgmPath            = "Sound/BGM/";
    private const string    _effectPath         = "Sound/Effect/";

    private const int       _audioCount         = 30; // 최대 동시재생 오디오 개수.

    // 사운드 컨테이너
    private GameObject      _bgmContainer1;
    private GameObject      _bgmContainer2;
    private GameObject[]    _effectContainer;

    // 각 사운드의 음량
    public float _masterVolume  = 1.0f;
    public float _bgmVolume     = 1.0f;
    public float _effectVolume  = 1.0f;
    public float _uiVolume      = 1.0f;

    // distance 비례 음량
    private const float     _minDistanceVolume  = 15; // 거리가 이만큼 될때부터 소리가 작아지기 시작
    
    public void InitSoundManager()
    {
        InitSound();
    }

    public void UpdateSoundManager()
    {
        SoundTester();
    }

    // 사운드 테스트용
    void SoundTester()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0)) SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Coin/coin_01", 1, 0);
        else if (Input.GetKeyDown(KeyCode.Alpha1)) SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Coin/coin_02", 1, 10);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Coin/coin_03", 1, 20);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Coin/coin_04", 1, 30);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Coin/coin_05", 1, 40);
        else if (Input.GetKeyDown(KeyCode.Alpha5)) SoundManager.Instance.PlayBGM("BGM_02", 1, true);
        else if (Input.GetKeyDown(KeyCode.Alpha6)) SoundManager.Instance.PlayBGM("BGM_01", 1, false);
        else if (Input.GetKeyDown(KeyCode.Alpha7)) SoundManager.Instance.PlayEffect(SoundType.UI, "Coin/coin_06", 1, 40);
    }

    /// 초기 사운드들을 배치하도록 한다.
    public void InitSound()
    {
        _effectContainer = new GameObject[_audioCount];

        for (int i = 0; i < _audioCount; i++)
        {
            _effectContainer[i] = ResourceManager.Instance.Instantiate("EffectContainer" + i, "Prefab/Etc/Container/AudioSource", transform);
        }

        _bgmContainer1 = ResourceManager.Instance.Instantiate("bgmPlayer1", "Prefab/Etc/Container/AudioSource", transform);
        _bgmContainer2 = ResourceManager.Instance.Instantiate("bgmPlayer2", "Prefab/Etc/Container/AudioSource", transform);
        _bgmContainer1.GetComponent<SoundInfor>().SetInfor(SoundType.BGM, 0);
        _bgmContainer2.GetComponent<SoundInfor>().SetInfor(SoundType.BGM, 0);
    }

    // BGM을 재생시킨다. 기본적으로 FadeIn / Out이 적용된다.
    public void PlayBGM(string soundPath, float vol = 1.0f, bool startImmediatly = false)
    {
        AudioSource startSource;
        AudioSource endSource;

        if (_bgmContainer1.GetComponent<AudioSource>().isPlaying)
        {
            startSource = _bgmContainer2.GetComponent<AudioSource>();
            endSource = _bgmContainer1.GetComponent<AudioSource>();
            _bgmContainer2.GetComponent<SoundInfor>().SetInfor(SoundType.BGM, Time.realtimeSinceStartup);
        }

        else
        {
            startSource = _bgmContainer1.GetComponent<AudioSource>();
            endSource = _bgmContainer2.GetComponent<AudioSource>();
            _bgmContainer1.GetComponent<SoundInfor>().SetInfor(SoundType.BGM, Time.realtimeSinceStartup);
        }

        startSource.clip = Resources.Load(_bgmPath + soundPath) as AudioClip;
        startSource.loop = true;

        if(startImmediatly)
        {
            // 현재 재생중인 BGM 즉시 중지시킴
            StopCoroutine("FadeOutBGM");
            endSource.Stop();

            // 재생할 BGM 즉시 FadeIn
            StopCoroutine("FadeInBGM");
            StartCoroutine(FadeInBGM(startSource, 2));
        }

        else // 현재 재생중인 음악 FadeOut으로 정지 후, 재생할 음악 FadeIn으로 재생
        {
            StopCoroutine("FadeOutBGM");
            StartCoroutine(FadeOutBGM(endSource, 2, startSource, true));
        }
    }

    // BGM을 정지시킨다. 별도의 입력이 없으면 fadeOut으로 정지시킨다.
    public void PauseBGM(bool stopImmediatly = false)
    {
        AudioSource sourceA = _bgmContainer1.GetComponent<AudioSource>();
        AudioSource sourceB = _bgmContainer2.GetComponent<AudioSource>();

        if (sourceA.isPlaying)
        {
            if (stopImmediatly) sourceA.Stop();
            else
            {
                StopCoroutine("FadeOutBGM");
                StopCoroutine(FadeOutBGM(sourceA, 2));
            }
        }

        if (sourceB.isPlaying)
        {
            if (stopImmediatly) sourceB.Stop();
            else
            {
                StopCoroutine("FadeOutBGM");
                StopCoroutine(FadeOutBGM(sourceB, 2));
            }
        }
    }

    // BGM을 서서히 재생시킨다.
    private IEnumerator FadeInBGM(AudioSource fadeInTarget, float period)
    {
        fadeInTarget.Play();
        fadeInTarget.volume = 0;

        float curTime = 0;
        while (true)
        {
            curTime += Time.deltaTime;

            fadeInTarget.volume = _masterVolume * _bgmVolume * Mathf.Lerp(0, 1, curTime / period);

            if (curTime >= period)
            {
                break;
            }

            yield return null;
        }
    }

    // BGM을 서서히 정지시킨다. isChange가 true일 경우, 정지 후에 타겟 BGM을 서서히 재생시킨다.
    private IEnumerator FadeOutBGM(AudioSource fadeOutTarget, float period, AudioSource fadeInTarget = null, bool isChange = false)
    {
        float curTime = 0;

        while (true)
        {
            curTime += Time.deltaTime;

            fadeOutTarget.volume = _masterVolume * _bgmVolume * Mathf.Lerp(1, 0, curTime / period);

            if (curTime >= period)
            {
                break;
            }

            yield return null;
        }

        fadeOutTarget.volume = 0;
        fadeOutTarget.Stop();

        if(isChange && fadeInTarget != null)
        {
            StopCoroutine("FadeInBGM");
            StartCoroutine(FadeInBGM(fadeInTarget, period));
        }
    }

    // SoundContainer 중 적합한 사운드 컨테이너 Index를 찾아 반환한다.
    private int FindEffectContainerPosition()
    {
        bool findPos = false;
        int targetPos = -1;

        for (int i = 0; i < _audioCount; i++) // 전체를 돌며 정지된 사운드나 빈 clip을 찾는다.
        {
            AudioSource currentSource = _effectContainer[i].GetComponent<AudioSource>();
            if (currentSource.clip == null || !currentSource.isPlaying)
            {
                findPos = true;
                targetPos = i;
            }
        }

        if (!findPos) // 모두 재생중인 상태 -> 가장 플레이한지 오래된 clip을 찾는다.
        {
            float minStartTime = float.MaxValue;

            for (int i = 0; i < _audioCount; i++)
            {
                AudioSource currentSource = _effectContainer[i].GetComponent<AudioSource>();
                SoundInfor currentInfor = currentSource.GetComponent<SoundInfor>();

                if (currentSource.GetComponent<SoundInfor>().StartTime < minStartTime)
                {
                    minStartTime = currentInfor.StartTime;
                    targetPos = i;
                }
            }
        }

        return targetPos;
    }

    // Effect / UI 사운드를 재생하도록 한다.
    public void PlayEffect(SoundType type, string soundPath, float volume = 1.0f, float distance = 0f, bool isLoop = false)
    {
        int targetPos = FindEffectContainerPosition();

        AudioSource targetSource = _effectContainer[targetPos].GetComponent<AudioSource>();

        switch (type)
        {
            case SoundType.EFFECT:
                targetSource.clip = Resources.Load(_effectPath + soundPath) as AudioClip;
                targetSource.volume = _masterVolume * _effectVolume * volume * ((distance > _minDistanceVolume) ? (_minDistanceVolume / distance) : 1); // distance가 최소 distanceVolume보다 크면 거리에 비례한 적은 소리를 배출하도록 한다.
                _effectContainer[targetPos].GetComponent<SoundInfor>().SetInfor(SoundType.EFFECT, Time.realtimeSinceStartup);
                break;
            case SoundType.UI:
                targetSource.clip = Resources.Load(_effectPath + soundPath) as AudioClip;
                targetSource.volume = _masterVolume * _uiVolume * volume; // ui는 어디서든 volume만큼의 소리를 낸다.
                _effectContainer[targetPos].GetComponent<SoundInfor>().SetInfor(SoundType.UI, Time.realtimeSinceStartup);
                break;
        }

        targetSource.Play();
        targetSource.loop = isLoop;
    }

    // BGM의 볼륨을 조절
    public void ChangeBGMVolume(float volume)
    {
        if (volume <= 0) volume = 0.0001f;

        if (_bgmContainer1.GetComponent<AudioSource>().isPlaying) _bgmContainer1.GetComponent<AudioSource>().volume *= (volume / _bgmVolume);
        if(_bgmContainer2.GetComponent<AudioSource>().isPlaying) _bgmContainer2.GetComponent<AudioSource>().volume *= (volume / _bgmVolume);
        _bgmVolume = volume;
    }

    // Effect의 볼륨을 조절
    public void ChangeEffectVolume(float volume)
    {
        if (volume <= 0) volume = 0.0001f;

        for (int i=0; i<_audioCount; i++)
        {
            if(_effectContainer[i].GetComponent<SoundInfor>().SoundType == SoundType.EFFECT && _effectContainer[i].GetComponent<AudioSource>().isPlaying)
                _effectContainer[i].GetComponent<AudioSource>().volume *= (volume / _effectVolume);
        }

        _effectVolume = volume;
    }

    // UI의 볼륨을 조절
    public void ChangeUIVolume(float volume)
    {
        if (volume <= 0) volume = 0.0001f;

        for (int i = 0; i < _audioCount; i++)
        {
            if (_effectContainer[i].GetComponent<SoundInfor>().SoundType == SoundType.UI && _effectContainer[i].GetComponent<AudioSource>().isPlaying)
                _effectContainer[i].GetComponent<AudioSource>().volume *= (volume / _uiVolume);
        }

        _uiVolume = volume;
    }

    // 마스터 볼륨을 조절
    public void ChangeMasterVolume(float volume)
    {
        if (volume <= 0) volume = 0.0001f;
        _bgmContainer1.GetComponent<AudioSource>().volume *= (volume / _masterVolume);
        _bgmContainer2.GetComponent<AudioSource>().volume *= (volume / _masterVolume);

        for (int i = 0; i < _audioCount; i++)
        {
            if (_effectContainer[i].GetComponent<AudioSource>().isPlaying)
                _effectContainer[i].GetComponent<AudioSource>().volume *= (volume / _masterVolume);
        }

        _masterVolume = volume;
    }
}