using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]

// 파티클(이펙트) 시간이 다되면 자동으로 오브젝트풀에 반환해주는 클래스 이펙트 프리팹에 넣으면됨.

public class ParticleAutoDelete : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(CheckAilve());
    }

    private IEnumerator CheckAilve()
    {
        yield return new WaitForSeconds(gameObject.GetComponent<ParticleSystem>().main.duration);
        ObjectPoolManager.Instance.ReturnObject(gameObject);
    }
}
