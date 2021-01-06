﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Blunt : Weapon
{
    // Start is called before the first frame update
    public Blunt()
    {
        WeaponAnimation = Resources.Load<RuntimeAnimatorController>("Animation/Animator/Player/Blunt Animator");
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