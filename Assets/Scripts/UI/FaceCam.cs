﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class FaceCam : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject avatar;
    CinemachineFreeLook cinemachine;
    GameObject followAvatar;
    private void OnEnable()
    {
        cinemachine = GetComponent<CinemachineFreeLook>();
        followAvatar = Instantiate(avatar);
        followAvatar.transform.position = new Vector3(1000, 1000, 0);
        followAvatar.transform.rotation = Quaternion.Euler(0, 180, 0);
        cinemachine.m_Follow = followAvatar.transform;
        cinemachine.m_LookAt = followAvatar.transform;

    }
    private void OnDisable()
    {
        if (followAvatar != null)
            Destroy(followAvatar);
    }

}