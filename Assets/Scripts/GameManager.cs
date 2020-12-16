using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBase<GameManager>
{
    void Start()
    {
        SoundManager.Instance.InitSound();
        SoundManager.Instance.SetSoundToAudio(SoundType.BGM, "BGM_01", true);
    }

    void Update()
    {
        SoundTester();
    }

    void SoundTester()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0)) SoundManager.Instance.SetSoundToAudio(SoundType.EFFECT, "Coin/coin_01");
        else if(Input.GetKeyDown(KeyCode.Alpha1)) SoundManager.Instance.SetSoundToAudio(SoundType.EFFECT, "Coin/coin_02");
        else if(Input.GetKeyDown(KeyCode.Alpha2)) SoundManager.Instance.SetSoundToAudio(SoundType.EFFECT, "Coin/coin_03");
        else if(Input.GetKeyDown(KeyCode.Alpha3)) SoundManager.Instance.SetSoundToAudio(SoundType.EFFECT, "Coin/coin_04");
        else if(Input.GetKeyDown(KeyCode.Alpha4)) SoundManager.Instance.SetSoundToAudio(SoundType.EFFECT, "Coin/coin_05");
        else if(Input.GetKeyDown(KeyCode.Alpha5)) SoundManager.Instance.SetSoundToAudio(SoundType.BGM, "BGM_02", true);
        else if(Input.GetKeyDown(KeyCode.Alpha6)) SoundManager.Instance.SetSoundToAudio(SoundType.BGM, "BGM_01", true);
    }
}
