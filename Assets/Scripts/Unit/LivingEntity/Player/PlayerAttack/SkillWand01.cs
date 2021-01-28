////////////////////////////////////////////////////
/*
    File SkillWand01.cs
    class SkillWand01
    
    검은 구체를 생성해 공격하는 스킬.

    담당자 : 이신홍
    부 담당자 : 
*/
////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class SkillWand01 : PlayerAttack
{
    public override void CallMultiDamageCoroutine(Collider other)
    {
        StartCoroutine(DoMultiDamage(other.GetComponent<MonsterAction>(), 0.2f)); // 0.2초에 한번 데미지를 주도록
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
