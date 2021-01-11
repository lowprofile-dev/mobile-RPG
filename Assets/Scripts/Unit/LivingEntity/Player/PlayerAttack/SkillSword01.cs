using UnityEngine;
using System.Collections;

public class SkillSword01 : PlayerAttack
{
    private float _restoreHp = 0;

    public override void OnLoad()
    {
        base.OnLoad();

        StartCoroutine(SkillASound());    
    }

    private IEnumerator SkillASound()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "SkillEffect/Sword Skill 1 Swing", 0.6f);
        yield return new WaitForSeconds(0.5f);
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "SkillEffect/Sword Skill 1 Heal", 0.6f);
    }

    protected override void SetLocalRotation(GameObject Effect)
    {
        Effect.transform.Rotate(new Vector3(0, 0, 0));
    }

    protected override IEnumerator SetColliderTimer(float time)
    {
        _collider.enabled = true;
        yield return new WaitForSeconds(time);
        _collider.enabled = false;

        yield return new WaitForSeconds(1.5f);
        _restoreHp = thisSkillsDamage * 0.15f;
        Player.Instance.RestoreHP(_restoreHp);

        // 여유를 주고 삭제한다.
        yield return new WaitForSeconds(8.5f);
        ObjectPoolManager.Instance.ReturnObject(gameObject);
    }
}
