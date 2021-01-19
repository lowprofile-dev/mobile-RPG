using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUIView : View
{
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _bgmVolumeSlider;
    [SerializeField] private Slider _effectVolumeSlider;
    [SerializeField] private Slider _uiVolumeSlider;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _returnVillageButton;
    [SerializeField] private Button _exitGameButton;

    private void Start()
    {
        _masterVolumeSlider.value = SoundManager.Instance._masterVolume;
        _bgmVolumeSlider.value = SoundManager.Instance._bgmVolume;
        _effectVolumeSlider.value = SoundManager.Instance._effectVolume;
        _uiVolumeSlider.value = SoundManager.Instance._uiVolume;


        _masterVolumeSlider.onValueChanged.AddListener(delegate { SoundManager.Instance.ChangeMasterVolume(_masterVolumeSlider.value); });
        _bgmVolumeSlider.onValueChanged.AddListener(delegate { SoundManager.Instance.ChangeBGMVolume(_bgmVolumeSlider.value); });
        _effectVolumeSlider.onValueChanged.AddListener(delegate { SoundManager.Instance.ChangeEffectVolume(_effectVolumeSlider.value); });
        _uiVolumeSlider.onValueChanged.AddListener(delegate { SoundManager.Instance.ChangeUIVolume(_uiVolumeSlider.value); });

        _exitButton.onClick.AddListener(delegate { QuitView(); });
        _exitGameButton.onClick.AddListener(delegate { QuitGame(); });
        _returnVillageButton.onClick.AddListener(delegate { ReturnToVillage(); });
    }

    /// <summary>
    /// 강제 게임 종료
    /// </summary>
    private void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    /// <summary>
    /// 게임 확인
    /// </summary>
    private void QuitView()
    {
        UINaviationManager.Instance.PopToNav("SubUI_OptionView");
    }

    /// <summary>
    /// 마을로 돌아감
    /// </summary>
    private void ReturnToVillage()
    {
        if(UILoaderManager.Instance.IsSceneDungeon())
        {
            Player.Instance.ChangeState(PLAYERSTATE.PS_IDLE);
            UINaviationManager.Instance.PopToNav("SubUI_OptionView");
            UILoaderManager.Instance.LoadVillage();
        }
    }

    public override void UIExit()
    {
        base.UIExit();
    }

    public override void UIStart()
    {
        base.UIStart();
    }

    public override void UIUpdate()
    {
        base.UIUpdate();
    }
}
