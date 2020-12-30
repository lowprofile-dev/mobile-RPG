using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    // Start is called before the first frame update
    public Sword()
    {        
        damage = 10f;
        speed = 1.5f;
        masteryLevel = 1;
        outfitGrade = 0;
        AttackEffect = Resources.Load<GameObject>("Prefab/PlayerEffect/Sword Attack");
        SkillAEffect = Resources.Load<GameObject>("Prefab/PlayerEffect/Sword Skill A");
        SkillBEffect = Resources.Load<GameObject>("Prefab/PlayerEffect/Sword Skill B");
        SkillCEffect = Resources.Load<GameObject>("Prefab/PlayerEffect/Sword Skill C");

        WeaponAnimation = Resources.Load<RuntimeAnimatorController>("Animation/Animator/Player/Sword Animator");

    }

    // Update is called once per frame
    public override void Update()
    {
        OutfitGradeCheck();
        if(Input.GetKeyDown(KeyCode.P))
        {
            masteryLevel++;
        }
    }

    public override void SkillA()
    {

    }
    public override void SkillB()
    {

    }
    public override void SkillC()
    {

    }
    public override void Attack()
    {

    }
}
