////////////////////////////////////////////////////
/*
    File BossSkillRangeFill.cs
    class BossSkillRangeFill
    
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
using UnityEngine.UI;
public class BossSkillRangeFill : MonoBehaviour
{
    [SerializeField] GameObject backGround;
    [SerializeField] GameObject fillArea; // 경고 범위 색칠 범위
    [SerializeField] GameObject target; 
    [SerializeField] GameObject parent; //경고 범위가 따라다닐 타겟 (각도)

    private float angle;
    private float velocity;
    private float speed;

    private void OnEnable()
    {
        fillArea.GetComponent<Image>().fillAmount = 0f;
    }

    public void RemovedRange(GameObject parent ,GameObject target ,float speed)
    {
        this.parent = parent;
        this.target = target;
        this.speed = speed;
        StartCoroutine(Remove());
    }
    private IEnumerator Remove()
    {
        yield return new WaitForSeconds(speed);
        ObjectPoolManager.Instance.ReturnObject(gameObject);
    }
    void Update()
    {
        // 경고 범위를 속도에 맞게 Lerp 시킴
        fillArea.GetComponent<Image>().fillAmount = Mathf.Lerp(fillArea.GetComponent<Image>().fillAmount, 1f, Time.deltaTime * speed);       
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
                transform.position = parent.transform.position;

                Vector3 dir = (parent.transform.forward + parent.transform.right);

                dir.y = 0f;

                float pos = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
                angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, pos, ref velocity, 0.001f);

                transform.rotation = Quaternion.Euler(90f, angle - 45f, 0f);          
        }
    }
}
