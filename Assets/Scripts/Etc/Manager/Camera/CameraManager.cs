using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.EventSystems;

public class CameraManager : SingletonBase<CameraManager>
{
   
    [SerializeField] private float Max_Speed = 200f;
    [SerializeField] private float TouchSensitivity_x = 10f;
    [SerializeField] CinemachineFreeLook _playerFollowCamera; public CinemachineFreeLook PlayerFollowCamera { get { return _playerFollowCamera; } }
    SimpleInputNamespace.Joystick joystick;
    float m_fOldToucDis = 0f;       // 터치 이전 거리를 저장
    float m_fFieldOfView = 7;     // 카메라의 FieldOfView의 기본값
    private float shakeTimer;

    public void InitCameraManager(GameObject obj)
    {
        _playerFollowCamera = obj.GetComponent<CinemachineFreeLook>();
        CinemachineCore.GetInputAxis = this.HandleAxisInputDelegate;
        _playerFollowCamera.m_CommonLens = true;
    }

    private void Update()
    {
        if(joystick == null) joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<SimpleInputNamespace.Joystick>();
        ZoomInOut();
        CameraShake();
    }

    public void CameraSetTarget(GameObject target)
    {
        StartCoroutine(CameraReturnToPlayer(target.transform));
    }

    private IEnumerator CameraReturnToPlayer(Transform target)
    {
        UILoaderManager.Instance.PlayerUI.SetActive(false);
        _playerFollowCamera.m_Follow = target;
        _playerFollowCamera.m_LookAt = target;

        yield return new WaitForSeconds(0.5f);

        ShakeCamera(1, 1, 3);

        yield return new WaitForSeconds(3f);
        UILoaderManager.Instance.NameText.text = "";
        _playerFollowCamera.m_Follow = Player.Instance.gameObject.transform;
        _playerFollowCamera.m_LookAt = Player.Instance.gameObject.transform;
        UILoaderManager.Instance.PlayerUI.SetActive(true);
    }

    private void CameraShake()
    {
        if(shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                _playerFollowCamera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
                _playerFollowCamera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0f;
            }
        }
    }

    private float HandleAxisInputDelegate(string axisName)
    {
        switch (axisName)
        {
            case "rotationX":
                if(Input.touchCount == 1)
                {
                    if (!joystick.getHold() && (IsPointerOverGameObject(0) == false && Input.touches[0].phase == TouchPhase.Moved))//(!joystick.getHold())
                    {              
                         return Input.touches[0].deltaPosition.x / TouchSensitivity_x;
                    }
                    
                }
                else if (Input.touchCount > 0)
                {              
                    Touch[] touch = Input.touches;

                    for (int i = 0; i < touch.Length; i++)
                    {
                       
                        if ((touch[i].phase == TouchPhase.Moved || touch[i].phase == TouchPhase.Stationary) && IsPointerOverGameObject(i) == false)
                            return -Input.touches[i].deltaPosition.x / TouchSensitivity_x;
                    }           
                }
                break;
            default:
#if UNITY_EDITOR
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
        if (!joystick.getHold() && Input.touchCount == 2 && (Input.touches[0].phase == TouchPhase.Moved || Input.touches[1].phase == TouchPhase.Moved))
        {
            
            if (!IsPointerOverGameObject(0) && !IsPointerOverGameObject(1))
            {
                m_fToucDis = (Input.touches[0].position - Input.touches[1].position).sqrMagnitude;

                fDis = (m_fToucDis - m_fOldToucDis) * 0.01f;

                // 이전 두 터치의 거리와 지금 두 터치의 거리의 차이를 FleldOfView를 차감
                m_fFieldOfView -= fDis;

                // 카메라 줌 최대 범위 지정 2 ~ 7;
                m_fFieldOfView = Mathf.Clamp(m_fFieldOfView, 2, 8);

                // 확대 / 축소가 갑자기 되지않도록 보간
                //_playerFollowCamera.m_Lens.FieldOfView = Mathf.Lerp(_playerFollowCamera.m_Lens.FieldOfView, m_fFieldOfView, Time.deltaTime * 5);
                //_playerFollowCamera.GetRig(0).m_Lens.OrthographicSize = Mathf.Lerp(_playerFollowCamera.GetRig(0).m_Lens.OrthographicSize, m_fFieldOfView, Time.deltaTime * 5);
                _playerFollowCamera.m_Lens.OrthographicSize = Mathf.Lerp(_playerFollowCamera.m_Lens.OrthographicSize, m_fFieldOfView, Time.deltaTime * 5);
                m_fOldToucDis = m_fToucDis;
            }
        }

    }

    //강도 , 빈도 , 시간
    public void ShakeCamera(float intensity , float frequency, float time)
    {     
        _playerFollowCamera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequency;
        _playerFollowCamera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;

        shakeTimer = time;
    }

    public static bool IsPointerOverGameObject(int idx)
    {
      if (EventSystem.current.IsPointerOverGameObject(Input.touches[idx].fingerId)) return true;
        return false;
    }
}
