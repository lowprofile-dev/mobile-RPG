using UnityEngine;
using System.Collections;

public class SkillWand03 : PlayerAttack
{
    // 데미지 받는 빈도를 늘리기 위함.
    public override IEnumerator DoMultiDamage(MonsterAction monster)
    {
        for (int i = 0; i < _damageCount; i++)
        {
            thisSkillsDamage += monster.DamageCheck(_useFixedDmg ? _damage : _damage * StatusManager.Instance.finalStatus.attackDamage);
            yield return new WaitForSeconds(0.2f);
        }

        GetComponent<CCAttack>().ApplyCC(monster.gameObject, 0, 1, 0);
    }

    // 사라지면 바로 코루틴을 끄기위함
    protected override IEnumerator SetColliderTimer(float time)
    {
        _collider.enabled = true;
        yield return new WaitForSeconds(time);
        _collider.enabled = false;

        // 여유를 주고 삭제한다.
        //StopAllCoroutines();
        ObjectPoolManager.Instance.ReturnObject(gameObject);
    }
}
