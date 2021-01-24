////////////////////////////////////////////////////
/*
    File Monster.cs
    class Monster
    enum MONSTER_STATE

    담당자 : 이신홍
    부 담당자 : 

    몬스터의 베이스가 되는 클래스
*/
////////////////////////////////////////////////////


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MONSTER_STATE
{
    STATE_NULL, STATE_IDLE, STATE_FIND, STATE_TRACE, STATE_ATTACK, STATE_CAST, STATE_DEBUFF, STATE_KILL, STATE_DIE, STATE_STIRR, STATE_SPAWN, STATE_HIT, STATE_FALL, STATE_STUN, STATE_RIGID
}

public class Monster : LivingEntity
{
    [SerializeField] protected float _initHp; 
    [SerializeField] protected float _initStemina; 

    [SerializeField] protected string _monsterName; 
    [SerializeField] protected int _attackPattern; 
    [SerializeField] protected string _description; 
    [SerializeField] protected float _attackTime; 
    [SerializeField] protected int _attackDamage; 

    [SerializeField] protected Material _dissolveMaterial; 
    [SerializeField] protected Material _nonDissolveMaterial; 
    [SerializeField] protected GameObject _avatarObject; 
    [SerializeField] protected EnemySliderBar _hpbarObject;

    protected MonsterAction _monsterAction;

    // property
    public float initHp { get { return _initHp; } set { _initHp = value; } }
    public float initStemina { get { return _initStemina; } set { _initStemina = value; } }
    public string monsterName { get { return _monsterName; } }
    public int attackPattern { get { return _attackPattern; } }
    public string description { get { return _description; } }
    public float attackTime { get { return _attackTime; } }
    public int attackDamage { get { return _attackDamage; } set { _attackDamage = value; } }
    public Material dissolveMaterial { get { return _dissolveMaterial; } }
    public Material nonDissolveMaterial { get { return _nonDissolveMaterial; } }
    public GameObject avatarObject { get { return _avatarObject; } }
    public EnemySliderBar hpbarObject { get { return _hpbarObject; } }


    protected override void Start()
    {
        base.Start();
        myAnimator = GetComponent<Animator>();
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
}