using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    public int outfitGrade;
    public int masteryLevel;

    public bool skillBRelease = false;
    public bool skillCRelease = false;

    public float hitStun;
    public float hitRigid;
    public float hitFail;

    public int skillALevel;
    public int skillBLevel;
    public int skillCLevel;

    public float skillACoef;
    public float skillBCoef;
    public float skillCCoef;

    public float skillACool;
    public float skillBCool;
    public float skillCCool;

    public float expMax= 100;
    public float exp =0;

    public int attackLevel = 1;

    public float attackDamage;
    public float magicDamage;
    public float skillSpeed;

    public GameObject SkillAEffect;
    public GameObject SkillBEffect;
    public GameObject SkillCEffect;
    public GameObject SkillAEffect2;
    public GameObject SkillBEffect2;
    public GameObject SkillCEffect2;
    public GameObject AttackEffect;
    public GameObject AttackEffect2;
    public GameObject AttackEffect3;

    public Vector3 dir;
    
    public RuntimeAnimatorController WeaponAnimation;

    public virtual void Update()
    {
        OutfitGradeCheck();
        MasteryLevelUp();
    }

    public virtual GameObject SkillA()
    {
        return SkillAEffect;
    }
    public virtual GameObject SkillB()
    {
        return SkillBEffect;

    }
    public virtual GameObject SkillC()
    {
        return SkillCEffect;
    }

    public virtual GameObject SkillA2()
    {
        return SkillAEffect2;
    }

    public virtual GameObject SkillB2()
    {
        return SkillBEffect2;
    }

    public virtual GameObject SkillC2()
    {
        return SkillCEffect2;
    }

    public virtual GameObject Attack3()
    {
        return AttackEffect3;
    }

    public virtual GameObject Attack2()
    {
        return AttackEffect2;
    }

    public virtual GameObject Attack()
    {
        return AttackEffect;
    }

    public void OutfitGradeCheck()
    {
        if (outfitGrade <= 2)
        {
            if (masteryLevel > Player.Instance.weaponManager.GradeCriteria[outfitGrade + 1])
            {
                outfitGrade++;
            }
        }
    }

    public void MasteryLevelUp()
    {
        if(exp >= expMax)
        {
            exp = exp - expMax;
            masteryLevel++;
            SkillRelease();
        }
    }

    public void SkillRelease()
    {
        if (masteryLevel >= 19 && skillCRelease == false)
        {
            skillCRelease = true;
            skillCLevel = 1;
        }
        else if (masteryLevel >= 10 && skillBRelease == false)
        {
            skillBRelease = true;
            skillBLevel = 1;
        }
    }

    public bool CheckSkillB()
    {
        return skillBRelease;
    }

    public bool CheckSkillC()
    {
        return skillCRelease;
    }

 
}
