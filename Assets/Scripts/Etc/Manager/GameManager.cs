using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBase<GameManager>
{
    public bool isInteracting;

    void Start()
    {
        isInteracting = false;

        SoundManager.Instance.InitSound();
        SoundManager.Instance.PlayBGM("BGM_01");
    }
    
    void Update()
    {
        SoundTester();
    }

    void SoundTester()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0)) SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Coin/coin_01", 1, 0);
        else if(Input.GetKeyDown(KeyCode.Alpha1)) SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Coin/coin_02", 1, 10);
        else if(Input.GetKeyDown(KeyCode.Alpha2)) SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Coin/coin_03", 1, 20);
        else if(Input.GetKeyDown(KeyCode.Alpha3)) SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Coin/coin_04", 1, 30);
        else if(Input.GetKeyDown(KeyCode.Alpha4)) SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Coin/coin_05", 1, 40);
        else if(Input.GetKeyDown(KeyCode.Alpha5)) SoundManager.Instance.PlayBGM("BGM_02", 1, true);
        else if(Input.GetKeyDown(KeyCode.Alpha6)) SoundManager.Instance.PlayBGM("BGM_01", 1, false);
        else if(Input.GetKeyDown(KeyCode.Alpha7)) SoundManager.Instance.PlayEffect(SoundType.UI, "Coin/coin_06", 1, 40);
    }
}
