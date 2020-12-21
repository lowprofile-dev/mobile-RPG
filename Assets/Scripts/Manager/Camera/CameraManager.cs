using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.EventSystems;

public class CameraManager : MonoBehaviour
{

    CinemachineFreeLook CinemachineCamera;

    void OnEnable()
    {
        CinemachineCore.GetInputAxis = this.HandleAxisInputDelegate;
        CinemachineCamera = GetComponent<CinemachineFreeLook>();
    }
    private void Update()
    {
        ZoomInOut();
    }

    public float TouchSensitivity_x = 10f;
    public float TouchSensitivity_y = 10f;

    float m_fOldToucDis = 0f;       // 터치 이전 거리를 저장
    float m_fFieldOfView = 60f;     // 카메라의 FieldOfView의 기본값

    private float HandleAxisInputDelegate(string axisName)
    {
        switch (axisName)
        {
            case "rotationX":
                if (Input.touchCount > 0)
                {
                    //Is mobile touch
                    // if 조이스틱 터치가 아닐경우 추가하기.
                    if(EventSystem.current.IsPointerOverGameObject() == false)
                    return Input.touches[0].deltaPosition.x / TouchSensitivity_x;
                }
                else if (Input.GetMouseButton(0))
                {
                    // is mouse click
                    if (EventSystem.current.IsPointerOverGameObject() == false)
                        return Input.GetAxis("Mouse X");
                }
                break;
            //case "rotationY":
            //    if (Input.touchCount > 0)
            //    {
            //        //Is mobile touch
            //        return Input.touches[0].deltaPosition.y / TouchSensitivity_y;
            //    }
            //    else if (Input.GetMouseButton(0))
            //    {
            //        // is mouse click
            //        return Input.GetAxis("Mouse Y");
            //    }
            //    break;
            default:
#if UNITYEDITOR
                Debug.LogError("Input <" + axisName + "> not recognized.", this);
#endif
                break;
        }

        return 0f;
    }

    private void ZoomInOut()
    {
        float m_fToucDis = 0f;
        float fDis = 0f;
        if (Input.touchCount == 2 && (Input.touches[0].phase == TouchPhase.Moved || Input.touches[1].phase == TouchPhase.Moved))
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                m_fToucDis = (Input.touches[0].position - Input.touches[1].position).sqrMagnitude;

                fDis = (m_fToucDis - m_fOldToucDis) * 0.01f;

                // 이전 두 터치의 거리와 지금 두 터치의 거리의 차이를 FleldOfView를 차감
                m_fFieldOfView -= fDis;

                // 카메라 줌 최대 범위 지정 30 ~ 100;
                m_fFieldOfView = Mathf.Clamp(m_fFieldOfView, 30.0f, 100.0f);

                // 확대 / 축소가 갑자기 되지않도록 보간
                //cinemachineCamera.m_Lens.FieldOfView = Mathf.Lerp(cinemachineCamera.m_Lens.FieldOfView, m_fFieldOfView, Time.deltaTime * 5);
                CinemachineCamera.GetRig(0).m_Lens.FieldOfView = Mathf.Lerp(CinemachineCamera.GetRig(0).m_Lens.FieldOfView, m_fFieldOfView, Time.deltaTime * 5);
                m_fOldToucDis = m_fToucDis;
            }
        }

    }
}
