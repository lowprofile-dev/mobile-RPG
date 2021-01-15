using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPS_Checker : MonoBehaviour
{
    float deltaTime = 0f;
    [SerializeField] private TextMeshProUGUI text;

    private void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1f / deltaTime;
        text.text = fps.ToString();
    }
}
