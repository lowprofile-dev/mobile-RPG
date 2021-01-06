using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] protected Collider _collider;
    [SerializeField] protected GameObject _particleEffectPrefab;

    protected HashSet<GameObject> _attackedTarget;
    protected GameObject _baseParent;

    protected bool _isParentPlayer;
    [SerializeField] protected bool _useFixedDmg;
    [SerializeField] protected bool _useMeleeDmg;

    [SerializeField] protected float _damage;

    protected virtual void Start()
    {
        _collider = GetComponent<Collider>();
        _attackedTarget = new HashSet<GameObject>();
    }

    protected virtual void OnEnable()
    {
        if(_attackedTarget != null) _attackedTarget.Clear();
    }

    public void SetParent(GameObject parent)
    {
        _baseParent = parent;
        if (parent.GetComponent<Player>() != null) _isParentPlayer = true;
        else _isParentPlayer = false;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (CanCollision(other))
        {
            _attackedTarget.Add(other.gameObject);
            if (_useFixedDmg) other.GetComponent<LivingEntity>().Damaged(_damage);
            else
            {
                if (_isParentPlayer) other.GetComponent<LivingEntity>().Damaged((_useMeleeDmg ? StatusManager.Instance.playerStatus.attackDamage : StatusManager.Instance.playerStatus.magicDamage) * _damage);
                else other.GetComponent<LivingEntity>().Damaged(_baseParent.GetComponent<Monster>().attackDamage * _damage);
            }
        }
    }

    private bool CanCollision(Collider other)
    {
        if ((_isParentPlayer && (other.CompareTag("Boss") || other.CompareTag("Monster"))) || // 부모가 플레이어 && 충돌체가 적
            (!_isParentPlayer && other.CompareTag("Player"))) // 부모가 적 && 충돌체가 플레이어
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

    private IEnumerator SetColliderTimer(float time)
    {
        _collider.enabled = true;
        yield return new WaitForSeconds(time);
        _collider.enabled = false;

        ObjectPoolManager.Instance.ReturnObject(gameObject);
    }
}