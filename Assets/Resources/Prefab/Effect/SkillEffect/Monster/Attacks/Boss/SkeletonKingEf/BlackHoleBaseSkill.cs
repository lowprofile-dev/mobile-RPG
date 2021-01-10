using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleBaseSkill : BossAttack
{
    public override IEnumerator DoMultiDamage(Player player)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);

            for (int i = 0; i < _damageCount; i++)
            {
             
                thisSkillsDamage += Mathf.Round(_baseParent.GetComponent<Monster>().attackDamage * _damage);
                player.Damaged(Mathf.Round(_baseParent.GetComponent<Monster>().attackDamage * _damage));
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        StopAllCoroutines();
    }

    protected override IEnumerator SetColliderTimer(float time)
    {
        _collider.enabled = true;
        yield return new WaitForSeconds(time);
        _collider.enabled = false;

        StopAllCoroutines();

        yield return new WaitForSeconds(10);
        ObjectPoolManager.Instance.ReturnObject(gameObject);

    }
}
