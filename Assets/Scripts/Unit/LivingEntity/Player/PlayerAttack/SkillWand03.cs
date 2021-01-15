using UnityEngine;
using System.Collections;

public class SkillWand03 : PlayerAttack
{
    // 타격 빈도의 변경
    public override void CallMultiDamageCoroutine(Collider other)
    {
        StartCoroutine(DoMultiDamage(other.GetComponent<MonsterAction>(), 0.2f));
    }

    // 사라지면 바로 코루틴을 끄기위함
    protected override IEnumerator SetColliderTimer(float time)
    {
        _collider.enabled = true;
        yield return new WaitForSeconds(time);
        _collider.enabled = false;

        // 여유를 주고 삭제한다.
        ObjectPoolManager.Instance.ReturnObject(gameObject);
    }
}
