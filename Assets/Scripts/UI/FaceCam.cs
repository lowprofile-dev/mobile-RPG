using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class FaceCam : MonoBehaviour
{
    // Start is called before the first frame update
    CinemachineFreeLook faceCam;
    [SerializeField] CinemachineFreeLook bodycam;
    GameObject followAvatar = null;
    GameObject bodyAvatar = null;


    public void InitFaceCam(GameObject avatar)
    {
        //OnDisable();
        SetTargetWithGenerate(avatar);
    }

    private void OnDisable()
    {
        if (followAvatar != null)
            ObjectPoolManager.Instance.ReturnObject(followAvatar);
        if (bodyAvatar != null)
            ObjectPoolManager.Instance.ReturnObject(bodyAvatar);
    }

    public void SetTargetWithGenerate(GameObject target)
    {
        faceCam = GetComponent<CinemachineFreeLook>();
        followAvatar = ObjectPoolManager.Instance.GetObject(target);
        followAvatar.transform.position = new Vector3(950, 1000, -15);
        followAvatar.transform.rotation = Quaternion.Euler(-10, 170, 0);
        followAvatar.tag = "FaceCamAvata";
        followAvatar.GetComponent<Animator>().enabled = true;
        faceCam.m_Follow = followAvatar.transform;
        faceCam.m_LookAt = followAvatar.transform;

        bodyAvatar = ObjectPoolManager.Instance.GetObject(target);
        bodyAvatar.transform.position = new Vector3(2000, 2000, -15);
        bodyAvatar.transform.rotation = Quaternion.Euler(-10, 170, 0);
        bodyAvatar.tag = "BodyCamAvata";
        bodyAvatar.GetComponent<Animator>().enabled = true;
        bodycam.m_Follow = bodyAvatar.transform;
        bodycam.m_LookAt = bodyAvatar.transform;
        
    }

    public void SetTarget(GameObject target)
    {
        faceCam = GetComponent<CinemachineFreeLook>();
        followAvatar = target;
        faceCam.m_Follow = followAvatar.transform;
        faceCam.m_LookAt = followAvatar.transform;
    }
}
