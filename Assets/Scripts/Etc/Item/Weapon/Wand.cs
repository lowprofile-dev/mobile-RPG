using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Wand : Weapon
{
    // Start is called before the first frame update
    public Wand(WeaponData wandData)
    {
        id = wandData.id;
        weaponModelIndex = wandData.weaponModelIndex;
        weaponType = wandData.weaponType;

        attackDamage = wandData.attackDamage;
        attackSpeed = wandData.attackSpeed;
        magicDamage = wandData.magicDamage;

        skillACoef = wandData.skillACoef;
        skillBCoef = wandData.skillBCoef;
        skillCCoef = wandData.skillCCoef;

        skillACool = wandData.skillACool;
        skillBCool = wandData.skillBCool;
        skillCCool = wandData.skillCCool;

        masteryLevel = 1;
        skillBRelease = false;
        skillCRelease = false;

        AttackEffect = Resources.Load<GameObject>("Prefab/PlayerEffect/Wand Attack");
        SkillAEffect = Resources.Load<GameObject>("Prefab/PlayerEffect/Wand Skill A");
        SkillBEffect = Resources.Load<GameObject>("Prefab/PlayerEffect/Wand Skill B");
        SkillCEffect = Resources.Load<GameObject>("Prefab/PlayerEffect/Wand Skill C");
        WeaponAnimation = Resources.Load<RuntimeAnimatorController>("Animation/Animator/Player/Wand Animator");

    }


    // Update is called once per frame
    public override void Update()
    {
        OutfitGradeCheck();
        if (Input.GetKeyDown(KeyCode.P))
        {
            masteryLevel++;
        }
    }
}
