////////////////////////////////////////////////////
/*
    File LivingEntity.cs
    class LivingEntity
    
    몬스터 및 플레이어와 같은 계열의 개체들. 전투와 연관됨.
    
    담당자 : 이신홍
    부 담당자 : 안영훈
*/
////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : Unit
{
    [SerializeField] protected float _hp;               // 현재 체력
    [SerializeField] protected float _stemina;          // 현재 스테미너

    [SerializeField] protected GameObject _DamageText;  // 해당 개체에서 생성될 데미지 텍스트 개체
    [SerializeField] protected float _speed;            // 속도
    protected float _maxSpeed;                          // 최대 속도

    // CC 관련
    protected DebuffManager _DebuffManager = new DebuffManager();
    protected CCManager _CCManager = null;

    // 캐싱
    public Animator myAnimator;

    // property
    public float Hp { get { return _hp; } set { _hp = value; } }
    public float Stemina { get { return _stemina; } set { _stemina = value; } }
    public GameObject DamageText { get { return _DamageText; } }
    public DebuffManager DebuffManager { get { return _DebuffManager; } }
    public float speed { get { return _speed; } set { _speed = value; } }
    public float MAXspeed { get { return _maxSpeed; } set { _maxSpeed = value; } }
    public CCManager CCManager { get { return _CCManager; } set { _CCManager = value; } }

    protected virtual void OnEnable() { }
    protected virtual void Start()
    {
        InitObject();
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

    /// <summary>
    /// 피해 처리, 데미지 UI도 생성해준다.
    /// </summary>
    public virtual void Damaged(float damage, bool noArmorDmg)
    {
        bool isCritical = false;
        if (IsCritical())
        {
            damage = StatusManager.Instance.GetCriticalDamageRandomly(); // 플레이어의 공격에서만 적용이 된다. (Monster.cs)
            isCritical = true;
        }

        int resultDmg = (int)(noArmorDmg ? damage : GetArmorFromDamaged(damage));
        _hp -= resultDmg;

        if (isCritical) ObjectPoolManager.Instance.GetObject(DamageText, transform.position, Quaternion.identity).GetComponent<DamageText>().PlayCriticalDamage(resultDmg, true);
        else ObjectPoolManager.Instance.GetObject(DamageText, transform.position, Quaternion.identity).GetComponent<DamageText>().PlayDamage(resultDmg, true);
    }

    public virtual bool IsCritical()
    {
        return false;
    }

    public virtual float GetArmorFromDamaged(float damage)
    {
        return damage;
    }

    public virtual void UseStemina(float skillmp)
    {
        _stemina -= skillmp;
    }

    public virtual void setSpeed(float speed) { }
}
