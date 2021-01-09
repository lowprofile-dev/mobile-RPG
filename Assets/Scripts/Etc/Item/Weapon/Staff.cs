using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : Weapon
{
    // Start is called before the first frame update
    public Staff()
    {
        name = "staff";
        hitStun = 0.5f;
        hitRigid = 0.5f;
        hitFail = 0.5f;
        outfitGrade = 0;
        masteryLevel = MasteryManager.Instance.currentMastery.currentStaffMasteryLevel;

        skillBRelease = MasteryManager.Instance.currentMastery.currentStaffSkillBReleased;
        skillCRelease = MasteryManager.Instance.currentMastery.currentStaffSkillCReleased;

        skillLevel[0] = MasteryManager.Instance.weaponSkillLevel[4].autoAttackLevel;
        skillLevel[1] = MasteryManager.Instance.weaponSkillLevel[4].skillALevel;
        skillLevel[2] = MasteryManager.Instance.weaponSkillLevel[4].skillBLevel;
        skillLevel[3] = MasteryManager.Instance.weaponSkillLevel[4].skillCLevel;

        exp = MasteryManager.Instance.currentMastery.currentStaffMasteryExp;

        attackDamage = 1;
        magicDamage = 0;
        skillSpeed = 0;

        skillACoef = 0;
        skillBCoef = 0;
        skillCCoef = 0;

        skillACool = 0;
        skillBCool = 0;
        skillCCool = 0;

        WeaponAnimation = Resources.Load<RuntimeAnimatorController>("Animation/Animator/Player/Staff Animator");
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
