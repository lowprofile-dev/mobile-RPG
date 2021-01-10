using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MONSTER_STATE
{
    STATE_NULL, STATE_IDLE, STATE_FIND, STATE_TRACE, STATE_ATTACK, STATE_CAST, STATE_DEBUFF, STATE_KILL, STATE_DIE , STATE_STIRR , STATE_SPAWN , STATE_HIT , STATE_FALL , STATE_STUN ,STATE_RIGID
}

public class LivingEntity : Unit
{
    
    [SerializeField] protected float _hp; public float Hp { get { return _hp; } set { _hp = value; } }
    [SerializeField] protected float _stemina; public float Stemina { get { return _stemina; } set { _stemina = value; } }

    [SerializeField] protected GameObject _DamageText; public GameObject DamageText { get { return _DamageText; } }
    protected DebuffManager _DebuffManager; public DebuffManager DebuffManager { get { return _DebuffManager; } }
    [SerializeField] protected float _speed; public float speed { get { return _speed; } set { _speed = value; } }
    [SerializeField] protected float _MAXspeed; public float MAXspeed { get { return _MAXspeed; } set { _MAXspeed = value; } }
    private StateMachine _myStateMachine; public StateMachine MyStateMachine { get { return _myStateMachine; } }
    protected CCManager _CCManager; public CCManager CCManager { get { return _CCManager; } set { _CCManager = value; } }
    public Animator myAnimator;

    [Header("상태이상")]
    protected bool isStun = false; public bool Stun { get { return isStun; } set { isStun = value; } }
    protected bool isFall = false; public bool Fall { get {return isFall; } set { isFall = value; } }
    protected bool isRigid = false; public bool Rigid { get {return isRigid; } set { isRigid = value; } }

    protected virtual void Start()
    {
        InitObject();
        _DebuffManager = new DebuffManager();
        
    }

    protected virtual void Update()
    {
        _DebuffManager.Update();
        
    }
    
    // 오브젝트에서 필요한 초기화들을 실시한다.
    protected virtual void InitObject()
    {
        gameObject.GetComponent<Collider>().enabled = true;
    }

    public virtual void Damaged(float damage)
    {
        _hp -= damage;
        ObjectPoolManager.Instance.GetObject(DamageText, transform.position, Quaternion.identity).GetComponent<DamageText>().PlayDamage(damage, true);
    }

    public virtual void UseStemina(float skillmp)
    {
        _stemina -= skillmp;
    }
}
