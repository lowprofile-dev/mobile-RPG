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
    [SerializeField] Button tutorialButton;
    [SerializeField] GameObject tutorialPage;

    // Start is called before the first frame update
    void Start()
    {
        exitButton.onClick.AddListener(delegate { ExitButtonClicked(); });
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
