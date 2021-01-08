using UnityEngine;
using System.Collections;

public class BaseAttackSword03 : PlayerAttack
{
    protected override void SetLocalRotation(GameObject Effect)
    {
        Effect.transform.Rotate(new Vector3(0, 0, 90));
    }
}
