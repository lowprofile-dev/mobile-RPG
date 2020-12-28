﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : Weapon
{
    // Start is called before the first frame update
    public Dagger()
    {
        damage = 10f;
        speed = 1.5f;
        masteryLevel = 1;
        outfitGrade = 0;
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
