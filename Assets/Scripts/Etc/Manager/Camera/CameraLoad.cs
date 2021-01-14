using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraLoad : MonoBehaviour
{
    void Start()
    {
        CameraManager.Instance.InitCameraManager(gameObject);
    }

    
}
