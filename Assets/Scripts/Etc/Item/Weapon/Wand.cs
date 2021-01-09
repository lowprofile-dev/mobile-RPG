using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Wand : Weapon
{
    // Start is called before the first frame update
    public Wand()
    {
        name = "wand";
        hitStun = 0.5f;
        hitRigid = 0.5f;
        hitFail = 0.5f;
        outfitGrade = 0;
        masteryLevel = MasteryManager.Instance.currentMastery.currentWandMasteryLevel;

        skillBRelease = MasteryManager.Instance.currentMastery.currentWandSkillBReleased;
        skillCRelease = MasteryManager.Instance.currentMastery.currentWandSkillCReleased;

        skillLevel[0] = MasteryManager.Instance.weaponSkillLevel[3].autoAttackLevel;
        skillLevel[1] = MasteryManager.Instance.weaponSkillLevel[3].skillALevel;
        skillLevel[2] = MasteryManager.Instance.weaponSkillLevel[3].skillBLevel;
        skillLevel[3] = MasteryManager.Instance.weaponSkillLevel[3].skillCLevel;

        exp = MasteryManager.Instance.currentMastery.currentWandMasteryExp;


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
        base.Update();

        //if(Input.GetKeyDown(KeyCode.P))
        //{
        //    masteryLevel++;
        //}
    }
}
