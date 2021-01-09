using UnityEngine;
using System.Collections;

public class SkillSword03 : PlayerAttack
{
    public override void OnLoad()
    {
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

    protected override IEnumerator SetColliderTimer(float time)
    {
        yield return new WaitForSeconds(0.2f);
        _collider.enabled = true;
        yield return new WaitForSeconds(0.3f);
        CameraManager.Instance.ShakeCamera(5, 2, 0.25f); // 카메라 흔들기 연출
        yield return new WaitForSeconds(0.3f);
        _collider.enabled = false;

        // 여유를 주고 삭제한다.
        yield return new WaitForSeconds(10f);
        ObjectPoolManager.Instance.ReturnObject(gameObject);
    }
}
