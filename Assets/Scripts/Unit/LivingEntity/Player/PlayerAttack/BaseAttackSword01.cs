using UnityEngine;
using System.Collections;

public class BaseAttackSword01 : PlayerAttack
{
    protected override void SetLocalRotation(GameObject Effect)
    {
        Effect.transform.Rotate(new Vector3(0f, 0f, 90f));
    }
}
