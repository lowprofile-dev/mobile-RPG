using UnityEngine;
using System.Collections;

public class SkillSword03 : PlayerAttack
{
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
