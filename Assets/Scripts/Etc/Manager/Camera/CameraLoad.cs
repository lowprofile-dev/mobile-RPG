using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLoad : MonoBehaviour
{
    void Start()
    {
        CameraManager.Instance.InitCameraManager(gameObject);
    }

    
}
