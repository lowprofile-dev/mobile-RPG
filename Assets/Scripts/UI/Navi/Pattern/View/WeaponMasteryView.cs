using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

////////////////////////////////////////////////////
/*
    File WeaponMasteryView.cs
    class WeaponMasteryView

    담당자 : 김의겸
    부 담당자 : 
*/
////////////////////////////////////////////////////

public class WeaponMasteryView : View
{
    [SerializeField] private Image _basePanel;

    WeaponManager weaponManager;
    MasteryManager masteryManager;
    WeaponSkillLevel weaponSkillLevel;

    Sword sword;
    Dagger dagger;
    Blunt blunt;
    Wand wand;
    Staff staff;

    private int swordLevel;
    private int daggerLevel;
    private int bluntLevel;
    private int wandLevel;
    private int staffLevel;

    [SerializeField] Button exitButton;

    [SerializeField] TextMeshProUGUI swordLevelText;
    [SerializeField] TextMeshProUGUI daggerLevelText;
    [SerializeField] TextMeshProUGUI bluntLevelText;
    [SerializeField] TextMeshProUGUI wandLevelText;
    [SerializeField] TextMeshProUGUI staffLevelText;

    [SerializeField] GameObject[] swordSkill;
    [SerializeField] GameObject[] daggerSkill;
    [SerializeField] GameObject[] bluntSkill;
    [SerializeField] GameObject[] wandSkill;
    [SerializeField] GameObject[] staffSkill;

    // 튜토리얼 관련
    [SerializeField] private GameObject _tutorialPage;
    [SerializeField] private Button _tutorialBtn;

    SkillScript[] _swordScript;
    SkillScript[] _daggerScript;
    SkillScript[] _bluntScript;
    SkillScript[] _wandScript;
    SkillScript[] _staffScript;

    private bool isInit = false;

    static int IsWeaponMasteryTutorial;


    private void Start()
    {
        _swordScript = new SkillScript[4];
        _daggerScript = new SkillScript[4];
        _bluntScript = new SkillScript[4];
        _wandScript = new SkillScript[4];
        _staffScript = new SkillScript[4];
        exitButton.onClick.AddListener(delegate { ExitButtonClick(); });
        ScriptInit();

        if (tutorialButton != null) tutorialButton.onClick.AddListener(delegate { TutorialClick(); });
        tutorialExitButton.onClick.AddListener(delegate { TutorialExit(); });

        IsWeaponMasteryTutorial = PlayerPrefs.GetInt("WeaponMasteryTutorial");

        if (IsWeaponMasteryTutorial == 0)
        {
            tutorialPage.SetActive(true);
            IsWeaponMasteryTutorial++;
            PlayerPrefs.SetInt("WeaponMasteryTutorial", IsWeaponMasteryTutorial);
        }
    }

    /// <summary>
    /// 무기 숙련도창 닫는 함수
    /// </summary>
    private void ExitButtonClick()
    {
        UINaviationManager.Instance.PopToNav("SubUI_WeaponMasteryView");
        SoundManager.Instance.PlayEffect(SoundType.UI, "UI/ClickLightBase2", 1.0f);
    }

    /// <summary>
    /// 각 무기별 스킬 스크립트 초기화
    /// </summary>

    private void ScriptInit()
    {
        for(int i =0; i <4; i++)
        {
            _swordScript[i] = swordSkill[i].GetComponent<SkillScript>();
            _daggerScript[i] = daggerSkill[i].GetComponent<SkillScript>();
            _bluntScript[i] = bluntSkill[i].GetComponent<SkillScript>();
            _wandScript[i] = wandSkill[i].GetComponent<SkillScript>();
            _staffScript[i] = staffSkill[i].GetComponent<SkillScript>();
        }
    }

    private void Update()
    {
        //출력되는 부분들을 초기화 해주는 부분
        if (WeaponManager.Instance != null && !isInit)
        {
            masteryManager = MasteryManager.Instance;
            weaponManager = WeaponManager.Instance;

            sword = weaponManager._weaponDic["SWORD"] as Sword;
            dagger = weaponManager._weaponDic["DAGGER"] as Dagger;
            blunt = weaponManager._weaponDic["BLUNT"] as Blunt;
            wand = weaponManager._weaponDic["WAND"] as Wand;
            staff = weaponManager._weaponDic["STAFF"] as Staff;

            swordLevel = sword.masteryLevel; 
            daggerLevel = dagger.masteryLevel;
            bluntLevel = blunt.masteryLevel;
            wandLevel = wand.masteryLevel; 
            staffLevel = staff.masteryLevel;

            LevelPrint();

            swordLevelText.text = "Lv." + swordLevel;
            daggerLevelText.text = "Lv." + daggerLevel;
            bluntLevelText.text = "Lv." + bluntLevel;
            wandLevelText.text = "Lv." + wandLevel;
            staffLevelText.text = "Lv." + staffLevel;

            SkillLevelInit();

            isInit = true;
        }
        // 초기화가 끝난 후 스킬 레벨과 무기 숙련도 레벨등을 업데이트 해주는 부분
        else if (WeaponManager.Instance != null && isInit)
        {
            LevelPrint();
            SkillLevelPrint();
            SkillUnLock();
        }
    }

    /// <summary>
    /// 각 무기별 스킬들의 레벨을 초기화 하고 출력
    /// </summary>
    private void SkillLevelInit()
    {
        for (int i = 0; i < 4; i++)
        {
            _swordScript[i].skillLevel = sword.skillLevel[i];
            _swordScript[i].SkillLevelPrint();

            _daggerScript[i].skillLevel = dagger.skillLevel[i];
            _daggerScript[i].SkillLevelPrint();

            _bluntScript[i].skillLevel = blunt.skillLevel[i];
            _bluntScript[i].SkillLevelPrint();

            _wandScript[i].skillLevel = wand.skillLevel[i];
            _wandScript[i].SkillLevelPrint();

            _staffScript[i].skillLevel = staff.skillLevel[i];
            _staffScript[i].SkillLevelPrint();
        }
    }

    /// <summary>
    /// 각 무기별 스킬들의 레벨을 최신화 하여 출력
    /// 각 무기가 가지고 있는 스킬 레벨과 스킬 스크립트 상의 레벨이 다를 경우
    /// 무기의 레벨을 최신화하고 출력
    /// </summary>
    private void SkillLevelPrint()
    {   
        for(int i =0; i <4; i++)
        {
            if(_swordScript[i].skillLevel != sword.skillLevel[i])
            {
                if (_swordScript[i].skillLevel > sword.skillLevel[i])
                {
                    sword.skillLevel[i] = _swordScript[i].skillLevel;
                    switch (i)
                    {
                        case 0:
                            masteryManager.incrementSkillLevel("sword", "autoAttack");
                            break;
                        case 1:
                            masteryManager.incrementSkillLevel("sword", "skillA");
                            break;
                        case 2:
                            masteryManager.incrementSkillLevel("sword", "skillB");
                            break;
                        case 3:
                            masteryManager.incrementSkillLevel("sword", "skillC");
                            break;
                    }
                }
                else _swordScript[i].skillLevel = sword.skillLevel[i];
                _swordScript[i].SkillLevelPrint();
            }
            if (_daggerScript[i].skillLevel != dagger.skillLevel[i])
            {
                if (_daggerScript[i].skillLevel > dagger.skillLevel[i])
                {
                    dagger.skillLevel[i] = _daggerScript[i].skillLevel;
                    switch (i)
                    {
                        case 0:
                            masteryManager.incrementSkillLevel("dagger", "autoAttack");
                            break;
                        case 1:
                            masteryManager.incrementSkillLevel("dagger", "skillA");
                            break;
                        case 2:
                            masteryManager.incrementSkillLevel("dagger", "skillB");
                            break;
                        case 3:
                            masteryManager.incrementSkillLevel("dagger", "skillC");
                            break;
                    }
                }
                else _daggerScript[i].skillLevel = dagger.skillLevel[i];
                _daggerScript[i].SkillLevelPrint();
            }
            if (_bluntScript[i].skillLevel != blunt.skillLevel[i])
            {
                if (_bluntScript[i].skillLevel > blunt.skillLevel[i])
                {
                    blunt.skillLevel[i] = _bluntScript[i].skillLevel;
                    switch (i)
                    {
                        case 0:
                            masteryManager.incrementSkillLevel("blunt", "autoAttack");
                            break;
                        case 1:
                            masteryManager.incrementSkillLevel("blunt", "skillA");
                            break;
                        case 2:
                            masteryManager.incrementSkillLevel("blunt", "skillB");
                            break;
                        case 3:
                            masteryManager.incrementSkillLevel("blunt", "skillC");
                            break;
                    }
                }
                else _bluntScript[i].skillLevel = blunt.skillLevel[i];

                _bluntScript[i].SkillLevelPrint();
            }
            if (_wandScript[i].skillLevel != wand.skillLevel[i])
            {
                if (_wandScript[i].skillLevel > wand.skillLevel[i])
                {
                    wand.skillLevel[i] = _wandScript[i].skillLevel;
                    switch (i)
                    {
                        case 0:
                            masteryManager.incrementSkillLevel("wand", "autoAttack");
                            break;
                        case 1:
                            masteryManager.incrementSkillLevel("wand", "skillA");
                            break;
                        case 2:
                            masteryManager.incrementSkillLevel("wand", "skillB");
                            break;
                        case 3:
                            masteryManager.incrementSkillLevel("wand", "skillC");
                            break;
                    }
                }
                else _wandScript[i].skillLevel = wand.skillLevel[i];
                _wandScript[i].SkillLevelPrint();
            }
            if (_staffScript[i].skillLevel != staff.skillLevel[i])
            {
                if (_staffScript[i].skillLevel > staff.skillLevel[i])
                {
                    staff.skillLevel[i] = _staffScript[i].skillLevel;
                    switch (i)
                    {
                        case 0:
                            masteryManager.incrementSkillLevel("staff", "autoAttack");
                            break;
                        case 1:
                            masteryManager.incrementSkillLevel("staff", "skillA");
                            break;
                        case 2:
                            masteryManager.incrementSkillLevel("staff", "skillB");
                            break;
                        case 3:
                            masteryManager.incrementSkillLevel("staff", "skillC");
                            break;
                    }
                }
                else _staffScript[i].skillLevel = staff.skillLevel[i];
                _staffScript[i].SkillLevelPrint();
            }
        }
    }

    /// <summary>
    /// 각 무기 별 스킬의 해금 조건이 만족할 경우 스킬 해금을 풀어주는 함수
    /// </summary>
    private void SkillUnLock()
    {
        if(sword.skillBRelease && _swordScript[2].skillReleased == false)
        {
            _swordScript[2].SkillUnLock();
        }
        if (sword.skillCRelease && _swordScript[3].skillReleased == false)
        {
            _swordScript[3].SkillUnLock();
        }
        if (dagger.skillBRelease && _daggerScript[2].skillReleased == false)
        {
            _daggerScript[2].SkillUnLock();
        }
        if (dagger.skillCRelease && _daggerScript[3].skillReleased == false)
        {
            _daggerScript[3].SkillUnLock();
        }
        if (blunt.skillBRelease && _bluntScript[2].skillReleased == false)
        {
            _bluntScript[2].SkillUnLock();
        }
        if (blunt.skillCRelease && _bluntScript[3].skillReleased == false)
        {
            _bluntScript[3].SkillUnLock();
        }
        if (wand.skillBRelease && _wandScript[2].skillReleased == false)
        {
            _wandScript[2].SkillUnLock();
        }
        if (wand.skillCRelease && _wandScript[3].skillReleased == false)
        {
            _wandScript[3].SkillUnLock();
        }
        if (staff.skillBRelease && _staffScript[2].skillReleased == false)
        {
            _staffScript[2].SkillUnLock();
        }
        if (staff.skillCRelease && _staffScript[3].skillReleased == false)
        {
            _staffScript[3].SkillUnLock();
        }
    }

    /// <summary>
    /// 무기별 숙련도 레벨을 출력해주는 함수
    /// </summary>
    public void LevelPrint()
    {
        
        if (swordLevel != sword.masteryLevel)
        {
            swordLevel = sword.masteryLevel;
            swordLevelText.text = "Lv." + swordLevel;
        }
        if (daggerLevel != dagger.masteryLevel)
        {
            daggerLevel = dagger.masteryLevel;
            daggerLevelText.text = "Lv." + daggerLevel;
        }
        if (bluntLevel != blunt.masteryLevel)
        {
            bluntLevel = blunt.masteryLevel;
            bluntLevelText.text = "Lv." + bluntLevel;
        }
        if (wandLevel != wand.masteryLevel)
        {
            wandLevel = wand.masteryLevel;
            wandLevelText.text = "Lv." + wandLevel;
        }
        if (staffLevel != staff.masteryLevel)
        {
            staffLevel = staff.masteryLevel;
            staffLevelText.text = "Lv." + staffLevel;
        }
        
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
