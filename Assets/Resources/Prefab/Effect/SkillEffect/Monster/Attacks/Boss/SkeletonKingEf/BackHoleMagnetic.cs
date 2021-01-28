////////////////////////////////////////////////////
/*
    File BackHoleMagnetic.cs
    class BackHoleMagnetic
    
    담당자 : 안영훈

    스켈레톤 킹이 사용하는 블랙홀 스킬의 효과 (플레이어를 블랙홀로 잡아당김)
*/
////////////////////////////////////////////////////
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackHoleMagnetic : MonoBehaviour
{
    [SerializeField] private float forceFactor = 10f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<CharacterController>().SimpleMove((transform.position - other.gameObject.transform.position) * forceFactor);
        }
    }
}
