using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : Weapon
{
    // Start is called before the first frame update
    public Dagger()
    {
        WeaponAnimation = Resources.Load<RuntimeAnimatorController>("Animation/Animator/Player/Dagger Animator");
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
