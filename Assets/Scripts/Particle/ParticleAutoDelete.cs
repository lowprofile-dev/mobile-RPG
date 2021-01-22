////////////////////////////////////////////////////
/*
    File ParticleAutoDelete.cs
    class ParticleAutoDelete
    
    담당자 : 안영훈
    부 담당자 : 

    // 파티클(이펙트) 시간이 다되면 자동으로 오브젝트풀에 반환해주는 클래스
    이펙트 프리팹에 넣으면 자동 적용

*/
////////////////////////////////////////////////////
///
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]

public class ParticleAutoDelete : MonoBehaviour
{
    [SerializeField] private bool deleteParent = false;

    private void OnEnable()
    {
        StartCoroutine(CheckAilve());
    }

    private IEnumerator CheckAilve()
    {
        // 파티클 시스템 메인의 지속시간 만큼 기다리고 삭제
        yield return new WaitForSeconds(gameObject.GetComponent<ParticleSystem>().main.duration);

        if (deleteParent) // 부모 등록시 부모와 같이 사라짐
        {
            ObjectPoolManager.Instance.ReturnObject(transform.parent.gameObject);
        }
        else
        {
            ObjectPoolManager.Instance.ReturnObject(gameObject);
        }
    }
}
