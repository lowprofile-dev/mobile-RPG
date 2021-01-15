using UnityEngine;
using System.Collections;

public class SkillSword01 : PlayerAttack
{
    private float _restoreHp = 0;

    protected override IEnumerator PlaySound()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "SkillEffect/Sword Skill 1 Swing", 0.6f);
        yield return new WaitForSeconds(0.5f);
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "SkillEffect/Sword Skill 1 Heal", 0.6f);
    }
    
    protected override IEnumerator SetColliderTimer(float time)
    {
        _collider.enabled = true;
        yield return new WaitForSeconds(time);
        _collider.enabled = false;

        // 일정 시간 뒤에 흡혈 진행
        yield return new WaitForSeconds(0.5f);
        _restoreHp = thisSkillsDamage * 0.15f;
        Player.Instance.RestoreHP(_restoreHp);

        // 여유를 주고 삭제한다.
        yield return new WaitForSeconds(9.5f);
        ObjectPoolManager.Instance.ReturnObject(gameObject);
    }
}
