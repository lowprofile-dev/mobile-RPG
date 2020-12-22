using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;
public class CameraZoom : MonoBehaviour
{
    CinemachineVirtualCamera cinemachineCamera;
    CinemachineFreeLook look;

    public float TouchSensitivity_x = 10f;
    public float TouchSensitivity_y = 10f;

    float m_fOldToucDis = 0f;       // 터치 이전 거리를 저장
    float m_fFieldOfView = 60f;     // 카메라의 FieldOfView의 기본값

    private void OnEnable()
    {
        cinemachineCamera = GetComponent<CinemachineVirtualCamera>();
        look = GetComponent<CinemachineFreeLook>();
    }

    private void Update()
    {
       ZoomInOut();
    }

    private void ZoomInOut()
    {
        float m_fToucDis = 0f;
        float fDis = 0f;
        if (Input.touchCount == 2 && (Input.touches[0].phase == TouchPhase.Moved || Input.touches[1].phase == TouchPhase.Moved))
        {
            m_fToucDis = (Input.touches[0].position - Input.touches[1].position).sqrMagnitude;

            fDis = (m_fToucDis - m_fOldToucDis) * 0.01f;

            // 이전 두 터치의 거리와 지금 두 터치의 거리의 차이를 FleldOfView를 차감
            m_fFieldOfView -= fDis;

            // 카메라 줌 최대 범위 지정 30 ~ 100;
            m_fFieldOfView = Mathf.Clamp(m_fFieldOfView, 30.0f, 100.0f);

            // 확대 / 축소가 갑자기 되지않도록 보간
            cinemachineCamera.m_Lens.FieldOfView = Mathf.Lerp(cinemachineCamera.m_Lens.FieldOfView, m_fFieldOfView, Time.deltaTime * 5);

            m_fOldToucDis = m_fToucDis;
        }
        
    }

}
