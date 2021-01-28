using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
////////////////////////////////////////////////////
/*
    File MasterUIView.cs
    class MasterUIView

    담당자 : 김의겸
    부 담당자 : 
*/
////////////////////////////////////////////////////
public class MasterUIView : View
{
    [SerializeField] TextMeshProUGUI masteryLevelText;
    [SerializeField] Button exitButton;

    
    static int IsMasterTutorial;
    // Start is called before the first frame update
    void Start()
    {
        exitButton.onClick.AddListener(delegate { ExitButtonClicked(); });

        if (tutorialButton != null) tutorialButton.onClick.AddListener(delegate { TutorialClick(); });
        tutorialExitButton.onClick.AddListener(delegate { TutorialExit(); });

        IsMasterTutorial = PlayerPrefs.GetInt("MasterUITutorial");
        if (IsMasterTutorial == 0)
        {
            tutorialPage.SetActive(true);
            IsMasterTutorial++;
            PlayerPrefs.SetInt("MasterUITutorial", IsMasterTutorial);
        }

    }

    /// <summary>
    /// 닫기 버튼을 눌렀을 경우 팝업을 꺼주는 함수
    /// </summary>
    private void ExitButtonClicked()
    {
        UIManager.Instance.playerUIView.SetEffectList();
        UINaviationManager.Instance.PopToNav("SubUI_MasteryView");
        SoundManager.Instance.PlayEffect(SoundType.UI, "UI/ClickLightBase2", 1.0f);
    }

    void Update()
    {
        LevelPrint();
    }

    /// <summary>
    /// 마스터리의 현재 레벨을 계속 출력해주는 함수
    /// </summary>
    public void LevelPrint()
    {
        masteryLevelText.text = "Lv." + MasteryManager.Instance.currentMastery.currentMasteryLevel;
    }

    public override void UIStart()
    {
        base.UIStart();
        SoundManager.Instance.PlayEffect(SoundType.UI, "UI/OpenMastery", 0.9f);
    }

    public override void UIUpdate()
    {
        base.UIUpdate();
    }

    public override void UIExit()
    {
        base.UIExit();
    }
}
