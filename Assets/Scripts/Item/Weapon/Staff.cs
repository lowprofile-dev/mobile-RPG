using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class Staff : Weapon
{
    // Start is called before the first frame update
    public Staff()
    {
        damage = 10f;
        speed = 1.5f;
        masteryLevel = 1;
        outfitGrade = 0;
        WeaponAnimationController = Resources.Load<AnimatorController>("Animation/Animator/Player/Staff Animator");
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
