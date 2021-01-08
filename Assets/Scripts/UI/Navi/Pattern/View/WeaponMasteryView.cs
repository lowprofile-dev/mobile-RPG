using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 카드 UI
/// </summary>
public class WeaponMasteryView : View
{
    [SerializeField] private Image _basePanel;

    WeaponManager weaponManager;
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

    SkillScript[] _swordScript;
    SkillScript[] _daggerScript;
    SkillScript[] _bluntScript;
    SkillScript[] _wandScript;
    SkillScript[] _staffScript;

    private bool isInit = false;

    private void Start()
    {
        _swordScript = new SkillScript[4];
        _daggerScript = new SkillScript[4];
        _bluntScript = new SkillScript[4];
        _wandScript = new SkillScript[4];
        _staffScript = new SkillScript[4];

        ScriptInit();
    }

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

    public void UpgradePop()
    {

        Debug.Log("HELLO");

    }

    private void Update()
    {
        if (WeaponManager.Instance != null && !isInit)
        {
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

            isInit = true;
        }
        else if (WeaponManager.Instance != null && isInit)
        {
            LevelPrint();
            SkillLevelPrint();
            SkillUnLock();
        }
    }

    private void SkillLevelPrint()
    {   
        for(int i =0; i <4; i++)
        {
            if(_swordScript[i].skillLevel != sword.skillLevel[i])
            {
                if (_swordScript[i].skillLevel > sword.skillLevel[i]) sword.skillLevel[i] = _swordScript[i].skillLevel;
                else  _swordScript[i].skillLevel = sword.skillLevel[i];
                _swordScript[i].SkillLevelPrint();
            }
            if (_daggerScript[i].skillLevel != dagger.skillLevel[i])
            {
                if (_daggerScript[i].skillLevel > dagger.skillLevel[i]) dagger.skillLevel[i] = _daggerScript[i].skillLevel;
                else _daggerScript[i].skillLevel = dagger.skillLevel[i];
                _daggerScript[i].SkillLevelPrint();
            }
            if (_bluntScript[i].skillLevel != blunt.skillLevel[i])
            {
                if (_bluntScript[i].skillLevel > blunt.skillLevel[i]) blunt.skillLevel[i] = _bluntScript[i].skillLevel;
                else _bluntScript[i].skillLevel = blunt.skillLevel[i];
                _bluntScript[i].SkillLevelPrint();
            }
            if (_wandScript[i].skillLevel != wand.skillLevel[i])
            {
                if (_wandScript[i].skillLevel > wand.skillLevel[i]) wand.skillLevel[i] = _wandScript[i].skillLevel;
                else _wandScript[i].skillLevel = wand.skillLevel[i];
                _wandScript[i].SkillLevelPrint();
            }
            if (_staffScript[i].skillLevel != staff.skillLevel[i])
            {
                if (_staffScript[i].skillLevel > staff.skillLevel[i]) staff.skillLevel[i] = _staffScript[i].skillLevel;
                else _staffScript[i].skillLevel = staff.skillLevel[i];
                _staffScript[i].SkillLevelPrint();
            }
        }
    }

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

}
