using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class FaceCam : MonoBehaviour
{
    // Start is called before the first frame update
    CinemachineFreeLook cinemachine;
    public GameObject followAvatar;
    public void InitFaceCam(GameObject avatar)
    {
        //OnDisable();
        SetTargetWithGenerate(avatar);
    }

    private void OnDisable()
    {
        if (followAvatar != null)
            ObjectPoolManager.Instance.ReturnObject(followAvatar);
    }

    public void SetTargetWithGenerate(GameObject target)
    {
        cinemachine = GetComponent<CinemachineFreeLook>();
        followAvatar = ObjectPoolManager.Instance.GetObject(target);
        followAvatar.transform.position = new Vector3(950, 1000, -15);
        followAvatar.transform.rotation = Quaternion.Euler(-10, 170, 0);
        cinemachine.m_Follow = followAvatar.transform;
        cinemachine.m_LookAt = followAvatar.transform;
    }

    public void SetTarget(GameObject target)
    {
        cinemachine = GetComponent<CinemachineFreeLook>();
        followAvatar = target;
        cinemachine.m_Follow = followAvatar.transform;
        cinemachine.m_LookAt = followAvatar.transform;
    }
}
