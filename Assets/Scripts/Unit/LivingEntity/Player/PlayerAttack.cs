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
        thisSkillsDamage = 0;
    }

    public virtual void OnLoad()
    {
        GameObject Effect = ObjectPoolManager.Instance.GetObject(_particleEffectPrefab);

        Effect.transform.position = transform.position;
        Effect.transform.rotation = Quaternion.identity;
        Effect.transform.Rotate(Quaternion.LookRotation(Player.Instance.transform.forward).eulerAngles);

        SetLocalRotation(Effect);

        if (_attackedTarget != null)
        {
            _attackedTarget.Clear();
        }
    }

    protected virtual void SetLocalRotation(GameObject Effect)
    {
        
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

                StartCoroutine(DoMultiDamage(other.GetComponent<MonsterAction>()));
            }
        }
    }

    public virtual IEnumerator DoMultiDamage(MonsterAction monster)
    {
        for(int i=0; i<_damageCount; i++)
        {
            thisSkillsDamage += monster.DamageCheck(_useFixedDmg ? _damage : _damage * StatusManager.Instance.finalStatus.attackDamage);
            if (MasteryManager.Instance.currentMastery.currentMasteryChoices[7] == -1)
            {
                thisSkillsDamage *= 1.1f;
                Player.Instance.RestoreHP(thisSkillsDamage * 0.02f);
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    private bool CanCollision(Collider other)
    {
        if (tag.Equals("PlayerAttack") && (other.CompareTag("Boss") || other.CompareTag("Monster"))) // 부모가 플레이어 && 충돌체가 적
        {
            if (!_attackedTarget.Contains(other.gameObject)) return true;
        }

        return false;
    }

    public void SetEnableCollider(bool active)
    {
        _collider.enabled = active;
    }

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