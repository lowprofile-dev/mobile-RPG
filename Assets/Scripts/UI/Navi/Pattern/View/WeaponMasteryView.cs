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

    private bool isInit = false;

    private int _swordLevel;
    private int _daggerLevel;
    private int _bluntLevel;
    private int _wandLevel;
    private int _staffLevel;
    [SerializeField] TextMeshProUGUI _swordLevelText;
    [SerializeField] private Sprite _swordAttack;
    [SerializeField] private Sprite _swordSkillA;
    [SerializeField] private Sprite _swordSkillB;
    [SerializeField] private Sprite _swordSkillC;
    [SerializeField] private RawImage _swordSkillBLockImage;
    [SerializeField] private RawImage _swordSkillCLockImage;

    [SerializeField] TextMeshProUGUI _swordAttackLevelText;
    [SerializeField] TextMeshProUGUI _swordSkillALevelText;
    [SerializeField] TextMeshProUGUI _swordSkillBLevelText;
    [SerializeField] TextMeshProUGUI _swordSkillCLevelText;
    private int _swordAttackLevel;
    private int _swordSkillALevel;
    private int _swordSkillBLevel;
    private int _swordSkillCLevel;
    private bool _swordSkillBReleased = false;
    private bool _swordSkillCReleased = false;

    [SerializeField] TextMeshProUGUI _daggerLevelText;
    [SerializeField] private Sprite _daggerAttack;
    [SerializeField] private Sprite _daggerSkillA;
    [SerializeField] private Sprite _daggerSkillB;
    [SerializeField] private Sprite _daggerSkillC;
    [SerializeField] private RawImage _daggerSkillBLockImage;
    [SerializeField] private RawImage _daggerSkillCLockImage;

    [SerializeField] TextMeshProUGUI _daggerAttackLevelText;
    [SerializeField] TextMeshProUGUI _daggerSkillALevelText;
    [SerializeField] TextMeshProUGUI _daggerSkillBLevelText;
    [SerializeField] TextMeshProUGUI _daggerSkillCLevelText;

    private int _daggerAttackLevel;
    private int _daggerSkillALevel;
    private int _daggerSkillBLevel;
    private int _daggerSkillCLevel;
    private bool _daggerSkillBReleased = false;
    private bool _daggerSkillCReleased = false;

    [SerializeField] TextMeshProUGUI _bluntLevelText;
    [SerializeField] private Sprite _bluntAttack;
    [SerializeField] private Sprite _bluntSkillA;
    [SerializeField] private Sprite _bluntSkillB;
    [SerializeField] private Sprite _bluntSkillC;
    [SerializeField] private RawImage _bluntSkillBLockImage;
    [SerializeField] private RawImage _bluntSkillCLockImage;

    [SerializeField] TextMeshProUGUI _bluntAttackLevelText;
    [SerializeField] TextMeshProUGUI _bluntSkillALevelText;
    [SerializeField] TextMeshProUGUI _bluntSkillBLevelText;
    [SerializeField] TextMeshProUGUI _bluntSkillCLevelText;

    private int _bluntAttackLevel;
    private int _bluntSkillALevel;
    private int _bluntSkillBLevel;
    private int _bluntSkillCLevel;
    private bool _bluntSkillBReleased = false;
    private bool _bluntSkillCReleased = false;

    [SerializeField] TextMeshProUGUI _wandLevelText;
    [SerializeField] private Sprite _wandAttack;
    [SerializeField] private Sprite _wandSkillA;
    [SerializeField] private Sprite _wandSkillB;
    [SerializeField] private Sprite _wandSkillC;
    [SerializeField] private RawImage _wandSkillBLockImage;
    [SerializeField] private RawImage _wandSkillCLockImage;

    [SerializeField] TextMeshProUGUI _wandAttackLevelText;
    [SerializeField] TextMeshProUGUI _wandSkillALevelText;
    [SerializeField] TextMeshProUGUI _wandSkillBLevelText;
    [SerializeField] TextMeshProUGUI _wandSkillCLevelText;

    private int _wandAttackLevel;
    private int _wandSkillALevel;
    private int _wandSkillBLevel;
    private int _wandSkillCLevel;
    private bool _wandSkillBReleased = false;
    private bool _wandSkillCReleased = false;

    [SerializeField] TextMeshProUGUI _staffLevelText;
    [SerializeField] private Sprite _staffAttack;
    [SerializeField] private Sprite _staffSkillA;
    [SerializeField] private Sprite _staffSkillB;
    [SerializeField] private Sprite _staffSkillC;
    [SerializeField] private RawImage _staffSkillBLockImage;
    [SerializeField] private RawImage _staffSkillCLockImage;
    
    [SerializeField] TextMeshProUGUI _staffAttackLevelText;
    [SerializeField] TextMeshProUGUI _staffSkillALevelText;
    [SerializeField] TextMeshProUGUI _staffSkillBLevelText;
    [SerializeField] TextMeshProUGUI _staffSkillCLevelText;

    private int _staffAttackLevel;
    private int _staffSkillALevel;
    private int _staffSkillBLevel;
    private int _staffSkillCLevel;
    private bool _staffSkillBReleased = false;
    private bool _staffSkillCReleased = false;

    private void Start()
    {
        Transform[] objList = gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform child in objList)
        {
            
            if (child.name == "Sword")
            {
                Image attack;
                Image skillA;
                Image skillB;
                Image skillC;

               attack = child.Find("Skills").Find("Auto Attack").Find("Skill Image").GetComponent<Image>();
               attack.sprite = _swordAttack;
               skillA = child.Find("Skills").Find("Skill A").Find("Skill Image").GetComponent<Image>();
               skillA.sprite = _swordSkillA;
               skillB = child.Find("Skills").Find("Skill B").Find("Skill Image").GetComponent<Image>();
               skillB.sprite = _swordSkillB;
               skillC = child.Find("Skills").Find("Skill C").Find("Skill Image").GetComponent<Image>();
               skillC.sprite = _swordSkillC;
            }

            if (child.name == "Dagger")
            {
                Image attack;
                Image skillA;
                Image skillB;
                Image skillC;

                attack = child.Find("Skills").Find("Auto Attack").Find("Skill Image").GetComponent<Image>();
                attack.sprite = _daggerAttack;
                skillA = child.Find("Skills").Find("Skill A").Find("Skill Image").GetComponent<Image>();
                skillA.sprite = _daggerSkillA;
                skillB = child.Find("Skills").Find("Skill B").Find("Skill Image").GetComponent<Image>();
                skillB.sprite = _daggerSkillB;
                skillC = child.Find("Skills").Find("Skill C").Find("Skill Image").GetComponent<Image>();
                skillC.sprite = _daggerSkillC;
            }
            
            if (child.name == "Blunt")
            {
                Image attack;
                Image skillA;
                Image skillB;
                Image skillC;

                attack = child.Find("Skills").Find("Auto Attack").Find("Skill Image").GetComponent<Image>();
                attack.sprite = _bluntAttack;
                skillA = child.Find("Skills").Find("Skill A").Find("Skill Image").GetComponent<Image>();
                skillA.sprite = _bluntSkillA;
                skillB = child.Find("Skills").Find("Skill B").Find("Skill Image").GetComponent<Image>();
                skillB.sprite = _bluntSkillB;
                skillC = child.Find("Skills").Find("Skill C").Find("Skill Image").GetComponent<Image>();
                skillC.sprite = _bluntSkillC;
            }
            
            if (child.name == "Wand")
            {
                Image attack;
                Image skillA;
                Image skillB;
                Image skillC;

                attack = child.Find("Skills").Find("Auto Attack").Find("Skill Image").GetComponent<Image>();
                attack.sprite = _wandAttack;
                skillA = child.Find("Skills").Find("Skill A").Find("Skill Image").GetComponent<Image>();
                skillA.sprite = _wandSkillA;
                skillB = child.Find("Skills").Find("Skill B").Find("Skill Image").GetComponent<Image>();
                skillB.sprite = _wandSkillB;
                skillC = child.Find("Skills").Find("Skill C").Find("Skill Image").GetComponent<Image>();
                skillC.sprite = _wandSkillC;
            }
            
            if (child.name == "Staff")
            {
                Image attack;
                Image skillA;
                Image skillB;
                Image skillC;

                attack = child.Find("Skills").Find("Auto Attack").Find("Skill Image").GetComponent<Image>();
                attack.sprite = _staffAttack;
                skillA = child.Find("Skills").Find("Skill A").Find("Skill Image").GetComponent<Image>();
                skillA.sprite = _staffSkillA;
                skillB = child.Find("Skills").Find("Skill B").Find("Skill Image").GetComponent<Image>();
                skillB.sprite = _staffSkillB;
                skillC = child.Find("Skills").Find("Skill C").Find("Skill Image").GetComponent<Image>();
                skillC.sprite = _staffSkillC;
            }
        }

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

            _swordLevel = sword.masteryLevel;
            _swordAttackLevel = sword.attackLevel;
            _swordSkillALevel = sword.skillALevel;
            _swordSkillBLevel = sword.skillBLevel;
            _swordSkillCLevel = sword.skillCLevel;

            _daggerLevel = dagger.masteryLevel;
            _daggerAttackLevel = dagger.attackLevel;
            _daggerSkillALevel = dagger.skillALevel;
            _daggerSkillBLevel = dagger.skillBLevel;
            _daggerSkillCLevel = dagger.skillCLevel;

            _bluntLevel = blunt.masteryLevel;
            _bluntAttackLevel = blunt.attackLevel;
            _bluntSkillALevel = blunt.skillALevel;
            _bluntSkillBLevel = blunt.skillBLevel;
            _bluntSkillCLevel = blunt.skillCLevel;

            _wandLevel = wand.masteryLevel;
            _wandAttackLevel = wand.attackLevel;
            _wandSkillALevel = wand.skillALevel;
            _wandSkillBLevel = wand.skillBLevel;
            _wandSkillCLevel = wand.skillCLevel;

            _staffLevel = staff.masteryLevel;
            _staffAttackLevel = staff.attackLevel;
            _staffSkillALevel = staff.skillALevel;
            _staffSkillBLevel = staff.skillBLevel;
            _staffSkillCLevel = staff.skillCLevel;

            _swordLevelText.text = "Lv." + _swordLevel;
            _daggerLevelText.text = "Lv." + _daggerLevel;
            _bluntLevelText.text = "Lv." + _bluntLevel;
            _wandLevelText.text = "Lv." + _wandLevel;
            _staffLevelText.text = "Lv." + _staffLevel;

            _swordAttackLevelText.text = "Lv." + _swordAttackLevel;
            _swordSkillALevelText.text = "Lv." + _swordSkillALevel;
            _swordSkillBLevelText.text = "Lv." + _swordSkillBLevel;
            _swordSkillCLevelText.text = "Lv." + _swordSkillCLevel;

            _daggerAttackLevelText.text = "Lv." + _daggerAttackLevel;
            _daggerSkillALevelText.text = "Lv." + _daggerSkillALevel;
            _daggerSkillBLevelText.text = "Lv." + _daggerSkillBLevel;
            _daggerSkillCLevelText.text = "Lv." + _daggerSkillCLevel;

            _bluntAttackLevelText.text = "Lv." + _bluntAttackLevel;
            _bluntSkillALevelText.text = "Lv." + _bluntSkillALevel;
            _bluntSkillBLevelText.text = "Lv." + _bluntSkillBLevel;
            _bluntSkillCLevelText.text = "Lv." + _bluntSkillCLevel;

            _wandAttackLevelText.text = "Lv." + _wandAttackLevel;
            _wandSkillALevelText.text = "Lv." + _wandSkillALevel;
            _wandSkillBLevelText.text = "Lv." + _wandSkillBLevel;
            _wandSkillCLevelText.text = "Lv." + _wandSkillCLevel;

            _staffAttackLevelText.text = "Lv." + _staffAttackLevel;
            _staffSkillALevelText.text = "Lv." + _staffSkillALevel;
            _staffSkillBLevelText.text = "Lv." + _staffSkillBLevel;
            _staffSkillCLevelText.text = "Lv." + _staffSkillCLevel;

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
        SwordSkillPrint();
        DaggerSkillPrint();
        BluntSkillPrint();
        WandSkillPrint();
        StaffSkillPrint();
    }

    private void StaffSkillPrint()
    {
        if (_staffAttackLevel != staff.attackLevel)
        {
            _staffAttackLevel = staff.attackLevel;
            _staffAttackLevelText.text = "Lv." + _staffAttackLevel;
        }
        if (_staffSkillALevel != staff.skillALevel)
        {
            _staffSkillALevel = staff.skillALevel;
            _staffSkillALevelText.text = "Lv." + _staffSkillALevel;
        }
        if (_staffSkillBLevel != staff.skillBLevel)
        {
            _staffSkillBLevel = staff.skillBLevel;
            _staffSkillBLevelText.text = "Lv." + _staffSkillBLevel;
        }
        if (_staffSkillCLevel != staff.skillCLevel)
        {
            _staffSkillCLevel = staff.skillCLevel;
            _staffSkillCLevelText.text = "Lv." + _staffSkillCLevel;
        }
    }
    private void WandSkillPrint()
    {
        if (_wandAttackLevel != wand.attackLevel)
        {
            _wandAttackLevel = wand.attackLevel;
            _wandAttackLevelText.text = "Lv." + _wandAttackLevel;
        }
        if (_wandSkillALevel != wand.skillALevel)
        {
            _wandSkillALevel = wand.skillALevel;
            _wandSkillALevelText.text = "Lv." + _wandSkillALevel;
        }
        if (_wandSkillBLevel != wand.skillBLevel)
        {
            _wandSkillBLevel = wand.skillBLevel;
            _wandSkillBLevelText.text = "Lv." + _wandSkillBLevel;
        }
        if (_wandSkillCLevel != wand.skillCLevel)
        {
            _wandSkillCLevel = wand.skillCLevel;
            _wandSkillCLevelText.text = "Lv." + _wandSkillCLevel;
        }
    }

    private void BluntSkillPrint()
    {
        if (_bluntAttackLevel != blunt.attackLevel)
        {
            _bluntAttackLevel = blunt.attackLevel;
            _bluntAttackLevelText.text = "Lv." + _bluntAttackLevel;
        }
        if (_bluntSkillALevel != blunt.skillALevel)
        {
            _bluntSkillALevel = blunt.skillALevel;
            _bluntSkillALevelText.text = "Lv." + _bluntSkillALevel;
        }
        if (_bluntSkillBLevel != blunt.skillBLevel)
        {
            _bluntSkillBLevel = blunt.skillBLevel;
            _bluntSkillBLevelText.text = "Lv." + _bluntSkillBLevel;
        }
        if (_bluntSkillCLevel != blunt.skillCLevel)
        {
            _bluntSkillCLevel = blunt.skillCLevel;
            _bluntSkillCLevelText.text = "Lv." + _bluntSkillCLevel;
        }
    }

    private void DaggerSkillPrint()
    {
        if (_daggerAttackLevel != dagger.attackLevel)
        {
            _daggerAttackLevel = dagger.attackLevel;
            _daggerAttackLevelText.text = "Lv." + _daggerAttackLevel;
        }
        if (_daggerSkillALevel != dagger.skillALevel)
        {
            _daggerSkillALevel = dagger.skillALevel;
            _daggerSkillALevelText.text = "Lv." + _daggerSkillALevel;
        }
        if (_daggerSkillBLevel != dagger.skillBLevel)
        {
            _daggerSkillBLevel = dagger.skillBLevel;
            _daggerSkillBLevelText.text = "Lv." + _daggerSkillBLevel;
        }
        if (_daggerSkillCLevel != dagger.skillCLevel)
        {
            _daggerSkillCLevel = dagger.skillCLevel;
            _daggerSkillCLevelText.text = "Lv." + _daggerSkillCLevel;
        }
    }

    private void SwordSkillPrint()
    {
        if (_swordAttackLevel != sword.attackLevel)
        {
            _swordAttackLevel = sword.attackLevel;
            _swordAttackLevelText.text = "Lv." + _swordAttackLevel;
        }
        if (_swordSkillALevel != sword.skillALevel)
        {
            _swordSkillALevel = sword.skillALevel;
            _swordSkillALevelText.text = "Lv." + _swordSkillALevel;
        }
        if (_swordSkillBLevel != sword.skillBLevel)
        {
            _swordSkillBLevel = sword.skillBLevel;
            _swordSkillBLevelText.text = "Lv." + _swordSkillBLevel;
        }
        if (_swordSkillCLevel != sword.skillCLevel)
        {
            _swordSkillCLevel = sword.skillCLevel;
            _swordSkillCLevelText.text = "Lv." + _swordSkillCLevel;
        }
    }

    private void SkillUnLock()
    {
        if(sword.skillBRelease && _swordSkillBReleased == false)
        {
            _swordSkillBLockImage.gameObject.SetActive(false);
            _swordSkillBLevelText.gameObject.SetActive(true);
            _swordSkillBReleased = true;
        }
        if (sword.skillCRelease && _swordSkillCReleased == false)
        {
            _swordSkillCLockImage.gameObject.SetActive(false);
            _swordSkillCLevelText.gameObject.SetActive(true);
            _swordSkillCReleased = true;
        }

        if (dagger.skillBRelease && _daggerSkillBReleased == false)
        {
            _daggerSkillBLockImage.gameObject.SetActive(false);
            _daggerSkillBLevelText.gameObject.SetActive(true);
            _daggerSkillBReleased = true;
        }
        if (dagger.skillCRelease && _daggerSkillCReleased == false)
        {
            _daggerSkillCLockImage.gameObject.SetActive(false);
            _daggerSkillCLevelText.gameObject.SetActive(true);
            _daggerSkillCReleased = true;
        }

        if (blunt.skillBRelease && _bluntSkillBReleased == false)
        {
            _bluntSkillBLockImage.gameObject.SetActive(false);
            _bluntSkillBLevelText.gameObject.SetActive(true);
            _bluntSkillBReleased = true;
        }
        if (blunt.skillCRelease && _bluntSkillCReleased == false)
        {
            _bluntSkillCLockImage.gameObject.SetActive(false);
            _bluntSkillCLevelText.gameObject.SetActive(true);
            _bluntSkillCReleased = true;
        }

        if (wand.skillBRelease && _wandSkillBReleased == false)
        {
            _wandSkillBLockImage.gameObject.SetActive(false);
            _wandSkillBLevelText.gameObject.SetActive(true);
            _wandSkillBReleased = true;
        }
        if (wand.skillCRelease && _wandSkillCReleased == false)
        {
            _wandSkillCLockImage.gameObject.SetActive(false);
            _wandSkillCLevelText.gameObject.SetActive(true);
            _wandSkillCReleased = true;
        }

        if (staff.skillBRelease && _staffSkillBReleased == false)
        {
            _staffSkillBLockImage.gameObject.SetActive(false);
            _staffSkillBLevelText.gameObject.SetActive(true);
            _staffSkillBReleased = true;
        }
        if (staff.skillCRelease && _staffSkillCReleased == false)
        {
            _staffSkillCLockImage.gameObject.SetActive(false);
            _staffSkillCLevelText.gameObject.SetActive(true);
            _staffSkillCReleased = true;
        }
    }

    public void LevelPrint()
    {
        if (_swordLevel != sword.masteryLevel)
        {
            _swordLevel = sword.masteryLevel;
            _swordLevelText.text = "Lv." + _swordLevel;
        }
        if (_daggerLevel != dagger.masteryLevel)
        {
            _daggerLevel = dagger.masteryLevel;
            _daggerLevelText.text = "Lv." + _daggerLevel;
        }
        if (_bluntLevel != blunt.masteryLevel)
        {
            _bluntLevel = blunt.masteryLevel;
            _bluntLevelText.text = "Lv." + _bluntLevel;
        }
        if (_wandLevel != wand.masteryLevel)
        {
            _wandLevel = wand.masteryLevel;
            _wandLevelText.text = "Lv." + _wandLevel;
        }
        if (_staffLevel != staff.masteryLevel)
        {
            _staffLevel = staff.masteryLevel;
            _staffLevelText.text = "Lv." + _staffLevel;
        }
    }

}
