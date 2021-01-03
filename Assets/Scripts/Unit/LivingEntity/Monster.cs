using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] protected float _speed; public float speed { get { return _speed; } set { _speed = value; } }
    [SerializeField] protected float _MAXspeed; public float MAXspeed { get { return _MAXspeed; } set { _MAXspeed = value; } }
    [SerializeField] protected float _attackTime; public float attackTime { get { return _attackTime; } }
    [SerializeField] protected int _attackDamage; public int attackDamage { get { return _attackDamage; } }

    protected DebuffManager _DebuffManager; public DebuffManager DebuffManager { get { return _DebuffManager; } }
    protected MonsterAction _monsterAction;

    protected override void Start()
    {
        base.Start();
        MyAnimator = GetComponent<Animator>();
        _MAXspeed = speed;
        _DebuffManager = new DebuffManager();

    }

    protected override void Update()
    {
        base.Update();
        _DebuffManager.Update();
    }
    
    protected override void InitObject()
    {
        base.InitObject();

        _hp = _initHp;
        _monsterAction = GetComponent<MonsterAction>();
        _monsterAction.InitObject();
    }

   
}
