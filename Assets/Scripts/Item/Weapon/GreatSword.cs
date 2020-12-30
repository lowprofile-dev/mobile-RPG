using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatSword : Weapon
{
    // Start is called before the first frame update
    public GreatSword()
    {
        damage = 10f;
        speed = 1.5f;
        masteryLevel = 1;
        outfitGrade = 0;
        WeaponAnimation = Resources.Load<RuntimeAnimatorController>("Animation/Animator/Player/GreatSword Animator");
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
