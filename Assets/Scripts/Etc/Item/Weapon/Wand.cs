﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Wand : Weapon
{
    // Start is called before the first frame update
    public Wand()
    {
        hitStun = 0.5f;
        hitRigid = 0.5f;
        hitFail = 0.5f;
        outfitGrade = 0;
        masteryLevel = 1;

        skillBRelease = false;
        skillCRelease = false;

        skillALevel = 0;
        skillBLevel = 0;
        skillCLevel = 0;
        attackLevel = 1;

        attackDamage = 0;
        magicDamage = 1;
        skillSpeed = 0;

        skillACoef = 0;
        skillBCoef = 0;
        skillCCoef = 0;

        skillACool = 0;
        skillBCool = 0;
        skillCCool = 0;

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
