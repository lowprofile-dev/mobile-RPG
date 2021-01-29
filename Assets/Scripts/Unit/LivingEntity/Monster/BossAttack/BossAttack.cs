////////////////////////////////////////////////////
/*
    File BossAttack.cs
    class BossAttack
    
    담당자 : 안영훈
    부 담당자 : 

    보스 공격 콜라이더 스크립트
*/
////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [SerializeField] protected Collider _collider;
    [SerializeField] protected GameObject _particleEffectPrefab = null;
    [SerializeField] private int targetNumber;
    protected HashSet<GameObject> _attackedTarget;
    protected GameObject _baseParent;

    [SerializeField] protected bool _useFixedDmg;
    [SerializeField] protected bool _useMeleeDmg;

    [SerializeField] protected float _damage;
    [SerializeField] protected float _damageCount;
    protected float thisSkillsDamage;

    protected float angle;
    protected float velocity;

    protected virtual void Start()
    {
        _collider = GetComponent<Collider>();
        _attackedTarget = new HashSet<GameObject>();
        thisSkillsDamage = 0;
    }

    public virtual void OnLoad(GameObject start , GameObject target)
    {


        SetLocalRotation(start, target);

        if (_particleEffectPrefab != null)
        {
            GameObject Effect = ObjectPoolManager.Instance.GetObject(_particleEffectPrefab);
            Effect.transform.SetParent(gameObject.transform);
            Effect.transform.localPosition = Vector3.zero;
            Effect.transform.rotation = Quaternion.identity;
        }

        if (_attackedTarget != null)
        {
            _attackedTarget.Clear();
        }
    }
   

    protected virtual void SetLocalRotation(GameObject Effect , GameObject target)
    {

    }
    protected virtual void SetLocalRotation(GameObject start, GameObject Effect, GameObject target) { }

    public virtual void SetParent(GameObject parent)
    {
        _baseParent = parent;
        transform.SetParent(parent.transform);
        transform.localPosition = Vector3.zero;
    }

    public virtual void SetParent(GameObject parent, Transform target) {
        _baseParent = parent;
        transform.position = target.position;
    }


    protected virtual void OnTriggerEnter(Collider other)
    {
        if (CanCollision(other))
        {
            if (_attackedTarget.Count <= targetNumber)
            {
                _attackedTarget.Add(other.gameObject);

                StartCoroutine(DoMultiDamage(other.GetComponent<Player>()));
            }
        }
    }

    public virtual IEnumerator DoMultiDamage(Player player)
    {
        for (int i = 0; i < _damageCount; i++)
        {
            //마스터리 스킬 회피 무적 적용
            if (Player.Instance.GetState() == PLAYERSTATE.PS_EVADE && MasteryManager.Instance.currentMastery.currentMasteryChoices[6] == -1)
            {
                yield return new WaitForSeconds(0.05f);
            }
            thisSkillsDamage += _baseParent.GetComponent<Monster>().attackDamage * _damage;
            player.Damaged(_baseParent.GetComponent<Monster>().attackDamage * _damage, false);
            yield return new WaitForSeconds(0.05f);
        }
    }

    private bool CanCollision(Collider other)
    {
        if (tag.Equals("BossAttack") && other.CompareTag("Player")) // 부모가 보스 && 충돌체가 플레이어
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
        //Destroy(gameObject);
    }
}
