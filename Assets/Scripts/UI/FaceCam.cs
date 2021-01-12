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

    private void OnEnable()
    {
        faceCam = gameObject.GetComponent<CinemachineFreeLook>();
    }
    public void InitFaceCam(GameObject avatar)
    {
        ResetAvata();
        StartCoroutine(SetTargetWithGenerate(avatar));
        //SetTargetWithGenerate(avatar);
    }

    private void ResetAvata()
    {
        if (followAvatar != null)
            Destroy(followAvatar);
        //ObjectPoolManager.Instance.ReturnObject(followAvatar);
        if (bodyAvatar != null)
            Destroy(bodyAvatar);
            //ObjectPoolManager.Instance.ReturnObject(bodyAvatar);
    }

    private void OnDisable()
    {
        ResetAvata();
    }

    public IEnumerator SetTargetWithGenerate(GameObject target)
    {

        yield return new WaitForSeconds(1f);
        followAvatar = Instantiate(target);
        bodyAvatar = ObjectPoolManager.Instance.GetObject(target);
        
        followAvatar.transform.position = new Vector3(0, 1000, -15);
        followAvatar.transform.rotation = Quaternion.Euler(-10, 170, 0);
        bodyAvatar.transform.position = new Vector3(0, 2000, -15);
        bodyAvatar.transform.rotation = Quaternion.Euler(-10, 170, 0);
        followAvatar.tag = "FaceCamAvata";
        bodyAvatar.tag = "BodyCamAvata";

        followAvatar.GetComponent<Animator>().enabled = true;
        bodyAvatar.GetComponent<Animator>().enabled = true;

        yield return new WaitForSeconds(1f);
        faceCam.m_Follow = followAvatar.transform;
        faceCam.m_LookAt = followAvatar.transform;

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
