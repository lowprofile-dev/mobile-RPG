////////////////////////////////////////////////////
/*
    File Attack.cs
    class Attack
    
    담당자 : 이신홍
    부 담당자 : 

    이펙트와 함께 콜라이더를 생성하여 공격을 표현하는 클래스
*/
////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] protected Collider _collider;                  // 충돌체
    [SerializeField] protected GameObject _particleEffectPrefab;    // 파티클 이펙트

    protected HashSet<GameObject> _attackedTarget;  // 공격된 대상 리스트 (중복 피격 막음)
    protected GameObject _baseParent;               // 공격을 한 개체

    protected bool _isParentPlayer;                 // 공격한 것이 플레이어인지
    [SerializeField] protected bool _useFixedDmg;   // 고정 데미지 여부
    [SerializeField] protected bool _useMeleeDmg;   // 물리 데미지 여부

    [SerializeField] protected float _damage;       // 데미지 (고뎀 : 그대로 적용, 물뎀 : 비율로 적용)

    protected virtual void Start()
    {
        _collider = GetComponent<Collider>();
        _attackedTarget = new HashSet<GameObject>();
    }

    protected virtual void OnEnable()
    {
        if(_attackedTarget != null) _attackedTarget.Clear(); // 활성화될때마다 공격된 타깃을 
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
            if(Player.Instance.GetState() == PLAYERSTATE.PS_EVADE && MasteryManager.Instance.currentMastery.currentMasteryChoices[6] == -1)
            {
                return;
            }
            _attackedTarget.Add(other.gameObject);
            if (_useFixedDmg) other.GetComponent<LivingEntity>().Damaged(_damage, false);
            else
            { 
                 other.GetComponent<LivingEntity>().Damaged(_baseParent.GetComponent<Monster>().attackDamage * _damage, false);
            }
        }
    }

    /// <summary>
    /// 충돌할 수 있는지 여부를 검사
    /// </summary>
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

    /// <summary>
    /// 일정 시간 충돌체를 켜고 없어진 후 소멸시킨다.
    /// </summary>
    private IEnumerator SetColliderTimer(float time)
    {
        _collider.enabled = true;
        yield return new WaitForSeconds(time);
        _collider.enabled = false;

        ObjectPoolManager.Instance.ReturnObject(gameObject);
    }
}