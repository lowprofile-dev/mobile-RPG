using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack: MonoBehaviour
{
    [SerializeField] protected Collider _collider;
    [SerializeField] protected GameObject _particleEffectPrefab;
    [SerializeField] private int targetNumber;
    protected HashSet<GameObject> _attackedTarget;
    protected GameObject _baseParent;

    protected bool _isParentPlayer;
    [SerializeField] protected bool _useFixedDmg;
    [SerializeField] protected bool _useMeleeDmg;

    [SerializeField] protected float _damage;
    [SerializeField] protected float _damageCount;
    protected float thisSkillsDamage;

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

    public void SetParent(GameObject parent)
    {
        _baseParent = parent;
        transform.SetParent(parent.transform);
        transform.localPosition = Vector3.zero;

        if (parent.GetComponent<Player>() != null) _isParentPlayer = true;
        else _isParentPlayer = false;
    }
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (CanCollision(other))
        {
            if (_attackedTarget.Count <= targetNumber)
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
        for (int i=0; i<_damageCount; i++)
        {
            thisSkillsDamage += DoDamage(monster);
            DoRestoreFromDamage();

            yield return new WaitForSeconds(waitTime);
        }
    }

    /// <summary>
    /// 데미지 연산을 적용하고 피해 데미지를 반환한다.
    /// </summary>
    public virtual float DoDamage(MonsterAction monster)
    {
        return monster.DamageCheck(_useFixedDmg ? _damage : _damage * StatusManager.Instance.GetFinalDamageRandomly());
    }

    /// <summary>
    /// 데미지로부터 흡혈을 실시한다.
    /// </summary>
    public void DoRestoreFromDamage()
    {
        if (MasteryManager.Instance.currentMastery.currentMasteryChoices[7] == -1)
        {
            thisSkillsDamage *= 1.1f;
            Player.Instance.RestoreHP(thisSkillsDamage * 0.02f);
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