using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MinimapCameraRender : MonoBehaviour
{
    float storedShadowDistance;

    private void OnPreRender()
    {
        storedShadowDistance = QualitySettings.shadowDistance;
        QualitySettings.shadowDistance = 0;
    }
    private void OnPostRender()
    {
        QualitySettings.shadowDistance = storedShadowDistance;
    }
}
