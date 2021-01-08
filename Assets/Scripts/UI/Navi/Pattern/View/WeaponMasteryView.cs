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

    private bool isInit = false;

    private int swordLevel;
    private int daggerLevel;
    private int bluntLevel;
    private int wandLevel;
    private int staffLevel;

    [SerializeField] TextMeshProUGUI _swordLevel;
    [SerializeField] private Sprite _swordAttack;
    [SerializeField] private Sprite _swordSkillA;
    [SerializeField] private Sprite _swordSkillB;
    [SerializeField] private Sprite _swordSkillC;

    [SerializeField] TextMeshProUGUI _daggerLevel;
    [SerializeField] private Sprite _daggerAttack;
    [SerializeField] private Sprite _daggerSkillA;
    [SerializeField] private Sprite _daggerSkillB;
    [SerializeField] private Sprite _daggerSkillC;

    [SerializeField] TextMeshProUGUI _bluntLevel;
    [SerializeField] private Sprite _bluntAttack;
    [SerializeField] private Sprite _bluntSkillA;
    [SerializeField] private Sprite _bluntSkillB;
    [SerializeField] private Sprite _bluntSkillC;

    [SerializeField] TextMeshProUGUI _wandLevel;
    [SerializeField] private Sprite _wandAttack;
    [SerializeField] private Sprite _wandSkillA;
    [SerializeField] private Sprite _wandSkillB;
    [SerializeField] private Sprite _wandSkillC;

    [SerializeField] TextMeshProUGUI _staffLevel;
    [SerializeField] private Sprite _staffAttack;
    [SerializeField] private Sprite _staffSkillA;
    [SerializeField] private Sprite _staffSkillB;
    [SerializeField] private Sprite _staffSkillC;

    [SerializeField] private GameObject[] _setBar;
    private bool _isRerolling; public bool isRerolling { get { return _isRerolling; } }
    [HideInInspector] public CardUIRoomArea cntPointerArea;
    [HideInInspector] public bool isRerollingAnimationPlaying;

    string iconPath = "Image/Icons/150 Fantasy Skill Icons/";

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
        if(WeaponManager.Instance != null && !isInit)
        {
            swordLevel = WeaponManager.Instance._weaponDic["SWORD"].masteryLevel;
            daggerLevel = WeaponManager.Instance._weaponDic["DAGGER"].masteryLevel;
            bluntLevel = WeaponManager.Instance._weaponDic["BLUNT"].masteryLevel;
            wandLevel = WeaponManager.Instance._weaponDic["WAND"].masteryLevel;
            staffLevel = WeaponManager.Instance._weaponDic["STAFF"].masteryLevel;

            _swordLevel.text = "Lv." + swordLevel;
            _daggerLevel.text = "Lv." + daggerLevel;
            _bluntLevel.text = "Lv." + bluntLevel;
            _wandLevel.text = "Lv." + wandLevel;
            _staffLevel.text = "Lv." + staffLevel;

            isInit = true;
        }
        else if(WeaponManager.Instance != null && isInit) LevelPrint();
    }

    public void LevelPrint()
    {
        if (swordLevel != WeaponManager.Instance._weaponDic["SWORD"].masteryLevel)
        {
            swordLevel = WeaponManager.Instance._weaponDic["SWORD"].masteryLevel;
            _swordLevel.text = "Lv." + swordLevel;
        }
        if (daggerLevel != WeaponManager.Instance._weaponDic["DAGGER"].masteryLevel)
        {
            daggerLevel = WeaponManager.Instance._weaponDic["DAGGER"].masteryLevel;
            _daggerLevel.text = "Lv." + daggerLevel;
        }
        if (bluntLevel != WeaponManager.Instance._weaponDic["BLUNT"].masteryLevel)
        {
            bluntLevel = WeaponManager.Instance._weaponDic["BLUNT"].masteryLevel;
            _bluntLevel.text = "Lv." + bluntLevel;
        }
        if (wandLevel != WeaponManager.Instance._weaponDic["WAND"].masteryLevel)
        {
            wandLevel = WeaponManager.Instance._weaponDic["WAND"].masteryLevel;
            _wandLevel.text = "Lv." + wandLevel;
        }
        if (staffLevel != WeaponManager.Instance._weaponDic["STAFF"].masteryLevel)
        {
            staffLevel = WeaponManager.Instance._weaponDic["STAFF"].masteryLevel;
            _staffLevel.text = "Lv." + staffLevel;
        }
    }

}
