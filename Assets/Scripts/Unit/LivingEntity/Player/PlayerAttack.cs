////////////////////////////////////////////////////
/*
    File PlayerAttack.cs
    class PlayerAttack
    
    플레이어의 공격의 기본이 되는 클래스. 파티클을 생성하고 일정 시간 뒤 사라진다.
    
    담당자 : 이신홍
    부 담당자 : 
*/
////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("기본")]
    [SerializeField] protected Collider _collider;                  // 충돌체
    [SerializeField] protected GameObject _particleEffectPrefab;    // 생성할 파티클 오브젝트
    protected int _skillLevel;                                      // 스킬의 레벨

    [Header("공격 속성")]
    [SerializeField] private int _targetNumber;                      // 타격한 수
    [SerializeField] protected bool _useFixedDmg;                   // 고정 데미지인지 
    [SerializeField] protected bool _useMeleeDmg;                   // 물리 데미지인지
    [SerializeField] protected float _damage;                       // 데미지 수치
    [SerializeField] protected float _damageCount;                  // 데미지 입히는 횟수
    [SerializeField] protected float _levelAtkCount;                // 레벨 별 추가되는 데미지 수치                   
    [SerializeField] protected float _levelMultiCount;              // 레벨 별 추가되는 다중 공격 수치

    protected HashSet<GameObject> _attackedTarget;                  // 타격한 몬스터 목록 (중복 방지)
    protected GameObject _baseParent;                               // 공격을 생성한 대상

    protected float thisSkillsDamage;                               // 이 스킬이 입힌 데미지

    protected virtual void Start()
    {
        _collider = GetComponent<Collider>();
        _attackedTarget = new HashSet<GameObject>();
    }

    /// <summary>
    /// 불러오면서 이펙트의 설정 및 초기화를 진행한다.
    /// </summary>
    public virtual void OnLoad()
    {
        thisSkillsDamage = 0;

        StartCoroutine(PlaySound());
        SpawnEffect();
        if (_attackedTarget != null) _attackedTarget.Clear();
    }

    /// <summary>
    /// 이펙트 소환시 재생되는 사운드. 없으면 구현하지 않음.
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator PlaySound()
    {
        // 사운드 재생
        yield return null;
    }

    /// <summary>
    /// 이펙트를 소환한다.
    /// </summary>
    public virtual void SpawnEffect()
    {
        GameObject Effect = ObjectPoolManager.Instance.GetObject(_particleEffectPrefab);

        Effect.transform.position = transform.position;
        Effect.transform.rotation = Quaternion.identity;
        Effect.transform.Rotate(Quaternion.LookRotation(Player.Instance.transform.forward).eulerAngles);
    }

    /// <summary>
    /// 부모를 설정하고 이에 따라 위치를 설정한다.
    /// </summary>
    public void SetParent(GameObject parent, int level)
    {
        _baseParent = parent;
        transform.SetParent(parent.transform);
        transform.localPosition = Vector3.zero;

        _skillLevel = level;
        if (GetComponent<CCAttack>()) GetComponent<CCAttack>()._level = level;
    }
    
    /// <summary>
    /// 충돌할때의 판정을 내린다.
    /// </summary>
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (CanCollision(other))
        {
            if (_attackedTarget.Count <= _targetNumber)
            {
                _attackedTarget.Add(other.gameObject);
                CallMultiDamageCoroutine(other);
            }
        }
    }

    /// <summary>
    /// ~초마다 1회 타격하도록 하는 코루틴을 부른다.
    /// </summary>
    public virtual void CallMultiDamageCoroutine(Collider other)
    {
        StartCoroutine(DoMultiDamage(other.GetComponent<MonsterAction>(), 0.05f));
    }

    /// <summary>
    /// 멀티 타격을 지원하는 공격 프로세스
    /// </summary>
    public virtual IEnumerator DoMultiDamage(MonsterAction monster, float waitTime)
    {
        int tempDamageCount = (int)(_damageCount + (_levelMultiCount * _skillLevel));

        for (int i=0; i<tempDamageCount; i++)
        {
            float damageNum = DoDamage(monster);
            DoRestoreFromDamage(damageNum);
            thisSkillsDamage += damageNum;

            yield return new WaitForSeconds(waitTime);
        }
    }

    /// <summary>
    /// 데미지 연산을 적용하고 피해 데미지를 반환한다.
    /// </summary>
    public virtual float DoDamage(MonsterAction monster)
    {
        float tempDamage = _damage + (_skillLevel * _levelAtkCount);
        float finalDamage = _useFixedDmg ? tempDamage : tempDamage * StatusManager.Instance.GetFinalDamageRandomly();

        if (Player.Instance.canDealFullHealth)
        {
            finalDamage += tempDamage * Player.Instance.fullHealthDamage;
        }
        if (Player.Instance.canDealProportional)
        {
            finalDamage += monster.monster.Hp * Player.Instance.proportionalDamage;
        }
        if (Player.Instance.canDealGiant && monster.monster.Hp / monster.monster.initHp >= 0.8f)
        {
            finalDamage += tempDamage * Player.Instance.giantDamage;
        }
        if (Player.Instance.canDealAnnoyed && Player.Instance.isAnnoyed && Player.Instance.annoyedTime <= 2.0f)
        {
            finalDamage += tempDamage * Player.Instance.annoyedDamage;
        }
        if (Player.Instance.canDealSpellStrike && Player.Instance.hasSpellStrike)
        {
            finalDamage += tempDamage * Player.Instance.spellStrikeDamage;
        }
        return monster.DamageCheck(finalDamage);
    }

    /// <summary>
    /// 데미지로부터 흡혈을 실시한다.
    /// </summary>
    public void DoRestoreFromDamage(float damage)
    {
        if (MasteryManager.Instance.masterySet[7,0] == true)
        {
            Player.Instance.RestoreHP(damage * 0.02f);
        }
    }

    /// <summary>
    /// 공격의 충돌 결과가 데미지를 입힐 수 있는 상태인지를 체크한다.
    /// </summary>
    private bool CanCollision(Collider other)
    {
        if (tag.Equals("PlayerAttack") && (other.CompareTag("Boss") || other.CompareTag("Monster"))) // 부모가 플레이어 && 충돌체가 적
            if (!_attackedTarget.Contains(other.gameObject)) return true;

        return false;
    }

    /// <summary>
    /// 충돌체의 on/off
    /// </summary>
    public void SetEnableCollider(bool active)
    {
        _collider.enabled = active;
    }

    /// <summary>
    /// time 시간동안 콜라이더를 On/Off시킨다.
    /// </summary>
    /// <param name="time"></param>
    public void PlayAttackTimer(float time)
    {
        StartCoroutine(SetColliderTimer(time));
    }

    /// <summary>
    /// 일정 시간 뒤에 충돌체를 off하며, 객체를 삭제한다.
    /// </summary>
    protected virtual IEnumerator SetColliderTimer(float time)
    {
        _collider.enabled = true;
        yield return new WaitForSeconds(time);
        _collider.enabled = false;

        // 여유를 주고 삭제한다.
        yield return new WaitForSeconds(10);
        ObjectPoolManager.Instance.ReturnObject(gameObject);
    }
}