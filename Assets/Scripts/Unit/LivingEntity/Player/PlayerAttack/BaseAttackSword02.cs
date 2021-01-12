﻿using UnityEngine;
using System.Collections;

public class BaseAttackSword02 : PlayerAttack
{
    protected override void SetLocalRotation(GameObject Effect)
    {
    }

    public override IEnumerator DoMultiDamage(MonsterAction monster)
    {
        GetComponent<CCAttack>().ApplyCC(monster.gameObject, 0, 0, 0.15f);
        yield return null;
    }
}
