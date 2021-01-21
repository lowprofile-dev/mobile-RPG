////////////////////////////////////////////////////
/*
    File FPS_Checker.cs
    class FPS_Checker
    
    담당자 : 안영훈
    부 담당자 : 

    게임 내의 프레임레이트 FPS 확인을 위해 임시로 만든 스크립트
*/
////////////////////////////////////////////////////
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