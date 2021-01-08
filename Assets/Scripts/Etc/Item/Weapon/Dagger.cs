using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : Weapon
{
    // Start is called before the first frame update
    public Dagger()
    {
        hitStun = 0.5f;
        hitRigid = 0.5f;
        hitFail = 0.5f;
        outfitGrade = 0;
        masteryLevel = 1;

        skillBRelease = false;
        skillCRelease = false;

        skillLevel[0] = 1;
        skillLevel[1] = 1;
        skillLevel[2] = 0;
        skillLevel[3] = 0;

        attackDamage = 1;
        magicDamage = 0;
        skillSpeed = 0;

        skillACoef = 0;
        skillBCoef = 0;
        skillCCoef = 0;

        skillACool = 0;
        skillBCool = 0;
        skillCCool = 0;

        WeaponAnimation = Resources.Load<RuntimeAnimatorController>("Animation/Animator/Player/Dagger Animator");
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
