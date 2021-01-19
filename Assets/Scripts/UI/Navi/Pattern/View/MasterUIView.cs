using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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

    private void ExitButtonClicked()
    {
        UIManager.Instance.playerUIView.SetEffectList();
        UINaviationManager.Instance.PopToNav("SubUI_MasteryView");
    }

    // Update is called once per frame
    void Update()
    {
        LevelPrint();
    }

    public void LevelPrint()
    {
        masteryLevelText.text = "Lv." + MasteryManager.Instance.currentMastery.currentMasteryLevel;
    }
}
