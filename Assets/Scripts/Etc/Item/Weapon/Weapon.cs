using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : WeaponData
{
    public int outfitGrade;
    public int masteryLevel;

    public bool skillBRelease = false;
    public bool skillCRelease = false;

    public float hitStun;
    public float hitRigid;
    public float hitFail;

    public GameObject SkillAEffect;
    public GameObject SkillBEffect;
    public GameObject SkillCEffect;
    public GameObject AttackEffect;

    public Vector3 dir;
    
    public RuntimeAnimatorController WeaponAnimation;

    public virtual void Update()
    {
     
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

    public void SkillRelease()
    {
        if (masteryLevel >= 19 && skillCRelease == false) skillCRelease = true;
        else if (masteryLevel >= 10 && skillBRelease == false) skillBRelease = true;
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
