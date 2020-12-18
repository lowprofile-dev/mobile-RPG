using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : LivingEntity
{
    protected MonsterAction _monsterAction;

    protected override void Start()
    {
        base.Start();
        
    }

    protected override void Update()
    {
        base.Update();
    }
    
    protected override void InitObject()
    {
        base.InitObject();

        _monsterAction = GetComponent<MonsterAction>();
        _monsterAction.InitObject();
    }
}
