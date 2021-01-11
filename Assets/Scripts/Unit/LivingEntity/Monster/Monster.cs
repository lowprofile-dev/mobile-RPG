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
    [SerializeField] protected float _initHp; public float initHp { get { return _initHp; } set { _initHp = value; } }
    [SerializeField] protected float _initStemina; public float initStemina { get { return _initStemina; } set { _initStemina = value; } }

    [SerializeField] protected string _monsterName; public string monsterName { get { return _monsterName; } }
    [SerializeField] protected MONSTERGRADE _grade; public MONSTERGRADE grade { get { return _grade; } }
    [SerializeField] protected MONSTERRACE _race; public MONSTERRACE race { get { return _race; } }
    [SerializeField] protected MONSTERSIZE _size; public MONSTERSIZE size { get { return _size; } }
    [SerializeField] protected SPAWNAREA _spawnArea; public SPAWNAREA spawnArea { get { return _spawnArea; } }
    [SerializeField] protected ATTACKTYPE _attackType; public ATTACKTYPE attackType { get { return _attackType; } }
    [SerializeField] protected int _attackPattern; public int attackPattern { get { return _attackPattern; } }
    [SerializeField] protected string _description; public string description { get { return _description; } }
    [SerializeField] protected float _attackTime; public float attackTime { get { return _attackTime; } }
    [SerializeField] protected int _attackDamage; public int attackDamage { get { return _attackDamage; } }

    [SerializeField] protected Material _dissolveMaterial; public Material dissolveMaterial { get { return _dissolveMaterial; } }
    [SerializeField] protected Material _nonDissolveMaterial; public Material nonDissolveMaterial { get { return _nonDissolveMaterial; } }
    [SerializeField] protected GameObject _avatarObject; public GameObject avatarObject { get { return _avatarObject; } }
    [SerializeField] protected EnemySliderBar _hpbarObject; public EnemySliderBar hpbarObject { get { return _hpbarObject; } }
    
    protected MonsterAction _monsterAction;

    protected override void Start()
    {
        base.Start();
        myAnimator = GetComponent<Animator>();
        _MAXspeed = speed;

        _hp = _initHp;
        _stemina = _initStemina;
    }

    protected override void Update()
    {
        base.Update();       
    }

    protected override void InitObject()
    {
        base.InitObject();
        myAnimator = GetComponent<Animator>();

        _hp = _initHp;
        _monsterAction = GetComponent<MonsterAction>();
        _monsterAction.InitObject();
    }

    //private void OnDestroy()
    //{
        
    //}
}