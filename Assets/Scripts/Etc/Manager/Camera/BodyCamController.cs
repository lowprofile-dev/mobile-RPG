////////////////////////////////////////////////////
/*
    File BodyCamController.cs
    class BodyCamController
    
    담당자 : 안영훈
    부 담당자 : 

    내 정보 UI에 있는 캐릭터 거울 UI에 관한 스크립트
*/
////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class BodyCamController : MonoBehaviour , IPointerDownHandler , IPointerUpHandler
{
    bool isButtonDown;
    [SerializeField] GameObject bodyAvata;

    [SerializeField] float rotateSpeed = 10f;

    private void OnEnable()
    {
        Invoke("LoadAvata", 1f);
    }
    private void LoadAvata()
    {
        if(Player.Instance != null)
        bodyAvata = Player.Instance.FaceCam.BodyAvata;
    }

    private IEnumerator RotateMirror() // 캐릭터쪽 거울을 터치하면 회전시키는 함수
    {
        while (true)
        {
            yield return null;

            if (isButtonDown && bodyAvata != null)
            {

                Vector3 rot = bodyAvata.transform.rotation.eulerAngles;
#if UNITY_EDITOR
                rot.y -= Input.GetAxis("Mouse X") * rotateSpeed;
#endif

#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
            rot.y += Input.touches[0].deltaPosition.x * rotateSpeed;
#endif
                Quaternion q = Quaternion.Euler(rot);
                bodyAvata.transform.rotation = Quaternion.Slerp(bodyAvata.transform.rotation, q, 2f);
            }
            else
            {
                break;
            }

        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isButtonDown = true;
        StartCoroutine(RotateMirror());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isButtonDown = false;
        StopCoroutine(RotateMirror());
    }
}
