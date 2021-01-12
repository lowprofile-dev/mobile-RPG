using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKingAtkBase : BossAttack
{
    public override void OnLoad(GameObject start, GameObject target)
    {
        GameObject effect = ObjectPoolManager.Instance.GetObject(_particleEffectPrefab);

        effect.transform.position = new Vector3(target.transform.position.x, start.transform.position.y, target.transform.position.z);

    }
    public override void SetParent(GameObject parent, Transform target)
    {
        _baseParent = parent;
        transform.SetParent(target);
        transform.localPosition = Vector3.zero;
    }
}
