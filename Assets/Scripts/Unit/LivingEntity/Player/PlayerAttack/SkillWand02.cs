using UnityEngine;
using System.Collections;

public class SkillWand02 : PlayerAttack
{

    public override void OnLoad()
    {
        StartCoroutine(PlaySound());

        GameObject Effect = ObjectPoolManager.Instance.GetObject(_particleEffectPrefab);

        Effect.transform.position = transform.position;
        Effect.transform.rotation = Quaternion.identity;
        Effect.transform.Rotate(Quaternion.LookRotation(Player.Instance.transform.forward).eulerAngles);

        SetLocalRotation(Effect);

        if (_attackedTarget != null)
        {
            _attackedTarget.Clear();
        }
    }


    private IEnumerator PlaySound()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "SkillEffect/Sword Attack 3", 0.6f);

        yield return new WaitForSeconds(0.2f);

        for(int i=0; i<5; i++)
        {
            SoundManager.Instance.PlayEffect(SoundType.EFFECT, "SkillEffect/Wand Skill 2 Attacks", 0.7f);
            yield return new WaitForSeconds(0.05f);
        }
    }

    // 데미지 받는 빈도를 늘리기 위함.
    public override IEnumerator DoMultiDamage(MonsterAction monster)
    {
        for (int i = 0; i < _damageCount; i++)
        {
            thisSkillsDamage += monster.DamageCheck(_useFixedDmg ? _damage : _damage * StatusManager.Instance.finalStatus.attackDamage);
            yield return new WaitForSeconds(0.2f);
        }
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
