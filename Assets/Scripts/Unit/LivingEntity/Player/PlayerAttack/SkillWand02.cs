////////////////////////////////////////////////////
/*
    File SkillWand02.cs
    class SkillWand02
    
    모션 이후 여러번의 공격을 빠르게 실행하는 스킬

    담당자 : 이신홍
    부 담당자 : 
*/
////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class SkillWand02 : PlayerAttack
{
    protected override IEnumerator PlaySound()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "SkillEffect/Sword Attack 3", 0.6f);

        yield return new WaitForSeconds(0.2f);

        for(int i=0; i<5; i++)
        { // 빠르게 5회 사운드를 냄
            SoundManager.Instance.PlayEffect(SoundType.EFFECT, "SkillEffect/Wand Skill 2 Attacks", 0.7f);
            yield return new WaitForSeconds(0.05f);
        }
    }

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
        //StopAllCoroutines();
        ObjectPoolManager.Instance.ReturnObject(gameObject);
    }
}
