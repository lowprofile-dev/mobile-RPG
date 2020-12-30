using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class FaceCam : MonoBehaviour
{
    // Start is called before the first frame update
    CinemachineFreeLook cinemachine;
    public GameObject followAvatar;
    public void Init(GameObject avatar)
    {
        SetTarget(avatar);
    }

    private void OnDisable()
    {
        if (followAvatar != null)
            Destroy(followAvatar);
    }

    private void SetTarget(GameObject target)
    {
        cinemachine = GetComponent<CinemachineFreeLook>();
        followAvatar = Instantiate(target);
        followAvatar.transform.position = new Vector3(1000, 1000, 0);
        followAvatar.transform.rotation = Quaternion.Euler(-10, 170, 0);
        cinemachine.m_Follow = followAvatar.transform;
        cinemachine.m_LookAt = followAvatar.transform;
    }
}
