using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonBase<SoundManager>
{
    private const int    audioCount  = 30; // 최대 동시재생 오디오 개수.
    private const string bgmPath     = "Sound/BGM/";
    private const string effectPath  = "Sound/Effect/";

    private GameObject   bgmContainer;
    private GameObject[] effectContainer;


    /// <summary>
    /// 초기 사운드들을 배치하도록 한다.
    /// </summary>
    public void InitSound()
    {
        effectContainer = new GameObject[audioCount];

        for (int i = 0; i < audioCount; i++)
        {
            effectContainer[i] = ResourceManager.Instance.Instantiate("EffectContainer" + i, "Prefab/Etc/Container/AudioSource", transform);
        }

        bgmContainer = ResourceManager.Instance.Instantiate("Prefab/Etc/Container/AudioSource", transform);
        bgmContainer.name = "bgmPlayer";
        bgmContainer.GetComponent<SoundInfor>().SetInfor(SoundType.BGM, 0);
        Debug.Log(effectContainer.Length);
    }

    /// <summary>
    /// 해당 사운드를 적절한 SoundContainer에 배치, 재생시킨다.
    /// </summary>
    public void SetSoundToAudio(SoundType type, string soundPath, bool isLoop = false, float vol = 1.0f)
    {

        if (type == SoundType.BGM) // BGM Case
        {
            AudioSource bgmSource = bgmContainer.GetComponent<AudioSource>();
            bgmSource.clip = Resources.Load(bgmPath + soundPath) as AudioClip;
            bgmSource.loop = isLoop;
            bgmSource.volume = vol;
            bgmSource.Play();
            bgmContainer.GetComponent<SoundInfor>().SetInfor(SoundType.BGM, Time.realtimeSinceStartup);
        }

        else // Effect & Ui Case
        {
            bool findPos = false;
            int targetPos = -1;

            for (int i = 0; i < audioCount; i++) // 전체를 돌며 정지된 사운드나 빈 clip을 찾는다.
            {
                AudioSource currentSource = effectContainer[i].GetComponent<AudioSource>();
                if (currentSource.clip == null || !currentSource.isPlaying)
                {
                    findPos = true;
                    targetPos = i;
                }
            }

            if (!findPos) // 모두 재생중인 상태 -> 가장 플레이한지 오래된 clip을 찾는다.
            {
                float minStartTime = float.MaxValue;

                for (int i = 0; i < audioCount; i++)
                {
                    AudioSource currentSource = effectContainer[i].GetComponent<AudioSource>();
                    SoundInfor currentInfor = currentSource.GetComponent<SoundInfor>();

                    if (currentSource.GetComponent<SoundInfor>().StartTime < minStartTime)
                    {
                        minStartTime = currentInfor.StartTime;
                        targetPos = i;
                    }
                }

                if (targetPos != -1)
                {
                    findPos = true;
                }
            }

            if (findPos) // 위치를 찾았다면 해당 container에 infor 대입 및 초기화
            {
                AudioSource targetSource = effectContainer[targetPos].GetComponent<AudioSource>();

                switch (type)
                {
                    case SoundType.EFFECT:
                        targetSource.clip = Resources.Load(effectPath + soundPath) as AudioClip;
                        bgmContainer.GetComponent<SoundInfor>().SetInfor(SoundType.EFFECT, Time.realtimeSinceStartup);
                        break;
                    case SoundType.UI:
                        targetSource.clip = Resources.Load(effectPath + soundPath) as AudioClip;
                        bgmContainer.GetComponent<SoundInfor>().SetInfor(SoundType.UI, Time.realtimeSinceStartup);
                        break;
                }

                targetSource.Play();
                targetSource.loop = isLoop;
                targetSource.volume = vol;
            }

            else
            {
                Debug.LogError("Cannot find old playing sound, cannot play audioSource");
            }
        }
    }
}
