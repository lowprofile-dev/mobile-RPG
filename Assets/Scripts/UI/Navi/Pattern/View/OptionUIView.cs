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
    [SerializeField] private Image _resetView;
    [SerializeField] private Button _resetViewBtn;
    [SerializeField] private Button _cancelBtn;
    [SerializeField] private Button _resetBtn;

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

        _resetViewBtn.onClick.AddListener(delegate { ViewDataClearPanel(); });
        _cancelBtn.onClick.AddListener(delegate { CloseDataClearPanel(); });
        _resetBtn.onClick.AddListener(delegate { DataClear(); });
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
    /// 뷰 팝업 제거
    /// </summary>
    private void QuitView()
    {
        UINaviationManager.Instance.PopToNav("SubUI_OptionView");
        SoundManager.Instance.PlayEffect(SoundType.UI, "UI/ClickLightBase2", 1.0f);
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
            SoundManager.Instance.PlayEffect(SoundType.UI, "UI/ClickLightBase2", 1.0f);
            UILoaderManager.Instance.LoadVillage();
        }
    }

    private void ViewDataClearPanel()
    {
        _resetView.gameObject.SetActive(true);
    }

    private void CloseDataClearPanel()
    {
        _resetView.gameObject.SetActive(false);
    }

    private void DataClear()
    {
        PlayerPrefs.DeleteAll();
        QuitGame();
        PlayerPrefs.DeleteAll();
    }

    public override void UIExit()
    {
        base.UIExit();
    }

    public override void UIStart()
    {
        base.UIStart();
        SoundManager.Instance.PlayEffect(SoundType.UI, "UI/ClickMedium02", 0.9f);
    }

    public override void UIUpdate()
    {
        base.UIUpdate();
    }
}
