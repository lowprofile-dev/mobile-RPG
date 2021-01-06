﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : Weapon
{
    // Start is called before the first frame update
    public Staff()
    {

        WeaponAnimation = Resources.Load<RuntimeAnimatorController>("Animation/Animator/Player/Staff Animator");
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