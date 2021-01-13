using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBaseShokeWave2 : BossAttack
{

    public override void SetParent(GameObject parent, Transform target)
    {
        _baseParent = parent;
        transform.SetParent(target);
        transform.localPosition = Vector3.zero;

    }
    protected override void SetLocalRotation(GameObject eft, GameObject _target)
    {
        Vector3 dir = transform.forward + _target.transform.right;
        float pos = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        angle = Mathf.SmoothDampAngle(eft.transform.eulerAngles.y, pos, ref velocity, 1f);
        transform.rotation = Quaternion.Euler(90f, angle, 0f);
        transform.parent = null;
    }
}
