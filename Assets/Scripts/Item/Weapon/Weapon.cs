using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    public float damage;
    public float speed;

    public int masteryLevel ;
    public int outfitGrade ;

    public GameObject SkillAEffect;
    public GameObject SkillBEffect;
    public GameObject SkillCEffect;
    public GameObject AttackEffect;
    public Transform EffectPosition;

    
    public RuntimeAnimatorController WeaponAnimation;

    public virtual void Update()
    {
     
    }

    public virtual void SkillA()
    {

    }
    public virtual void SkillB()
    {

    }
    public virtual void SkillC()
    {

    }
    public virtual void Attack()
    {

    }

    public void OutfitGradeCheck()
    {
        if (outfitGrade <= 3)
        {
            if (masteryLevel > Player.Instance.weaponManager.GradeCriteria[outfitGrade + 1])
            {
                outfitGrade++;
            }
        }
    }

}
