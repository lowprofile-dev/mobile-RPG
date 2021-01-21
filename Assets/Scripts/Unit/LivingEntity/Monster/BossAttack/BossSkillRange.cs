////////////////////////////////////////////////////
/*
    File BossSkillRange.cs
    class BossSkillRange
    
    담당자 : 안영훈
    부 담당자 : 

    보스가 스킬을 사용할 때 경고 범위 출력을 위한 코드

*/
////////////////////////////////////////////////////
///
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillRange : MonoBehaviour
{
    [SerializeField] GameObject backGround;
    [SerializeField] GameObject fillArea; // 경고 범위 색칠 범위
    [SerializeField] GameObject target; // 경고 범위가 따라다닐 타겟
    private float angle; // 회전용 각도
    private float velocity; // 회전용 속도
    private float speed; // 스킬이 발동되기 까지의 시간 (범위 색칠 속도)

    private void OnEnable()
    {
        // 경고선 범위 초기화
        fillArea.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    public void RemovedRange(GameObject target , float speed)
    {
        this.target = target;
        this.speed = speed;
        StartCoroutine(Remove());

    }
    private IEnumerator Remove() // 범위 삭제
    {
        yield return new WaitForSeconds(speed);
        ObjectPoolManager.Instance.ReturnObject(gameObject);
    }
    void Update()
    {
        // 경고 범위를 속도에 맞게 Lerp 시킴
        fillArea.transform.localScale = Vector3.Lerp(fillArea.transform.localScale, Vector3.one, Time.deltaTime * speed);
    }

    private void OnDisable()
    {
        StopCoroutine(RotateToTarget());
    }

    public void setFollow()
    {
        StartCoroutine(RotateToTarget());
    }

    private IEnumerator RotateToTarget()
    {
        while (true)
        {
            yield return null;

            // 경고 범위의 각도를 타겟에 맞춤
            transform.position = target.transform.position;

            Vector3 dir = (target.transform.forward + target.transform.right);

            dir.y = 0f;

            float pos = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, pos, ref velocity, 0.001f);

            transform.rotation = Quaternion.Euler(90f, angle + 90f, 0f);
        }
    }
}
