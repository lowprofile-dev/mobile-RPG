using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MONSTERGRADE
{
    GRADE_NORMAL, GRADE_RARE, GRADE_EPIC, GRADE_LEGENDARY
}

public enum MONSTERRACE
{
    SLIME, SKELETON, ZOMBIE, DRAGON
}

public enum MONSTERSIZE
{
    BIG, MIDDLE, SMALL
}

public enum SPAWNAREA
{
    CAVE, BEACH, WINTER
}

public enum ATTACKTYPE
{
    MELEE, MAGIC
}


public class Monster : LivingEntity
{
    [SerializeField] protected string _monsterName; public string monsterName { get { return _monsterName; } }
    [SerializeField] protected MONSTERGRADE _grade; public MONSTERGRADE grade { get { return _grade; } }
    [SerializeField] protected MONSTERRACE _race; public MONSTERRACE race { get { return _race; } }
    [SerializeField] protected MONSTERSIZE _size; public MONSTERSIZE size { get { return _size; } }
    [SerializeField] protected SPAWNAREA _spawnArea; public SPAWNAREA spawnArea { get { return _spawnArea; } }
    [SerializeField] protected ATTACKTYPE _attackType; public ATTACKTYPE attackType { get { return _attackType; } }
    [SerializeField] protected int _attackPattern; public int attackPattern { get { return _attackPattern; } }
    [SerializeField] protected string _description; public string description { get { return _description; } }
    [SerializeField] protected float _speed;
    [SerializeField] protected int _castPercent; public int castPercent { get { return _castPercent; } }
    [SerializeField] protected float _attackTime; public float attackTime { get { return _attackTime; } }
    [SerializeField] protected float _attackDamage; public float attackDamage { get { return _attackDamage; } }

    protected MonsterAction _monsterAction;

    protected override void Start()
    {
        base.Start();
        MyAnimator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
    }
    
    protected override void InitObject()
    {
        base.InitObject();

        _hp = _initHp;
        _monsterAction = GetComponent<MonsterAction>();
        _monsterAction.InitObject();
    }


}
