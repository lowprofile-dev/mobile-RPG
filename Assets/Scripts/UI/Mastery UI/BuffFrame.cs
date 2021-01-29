////////////////////////////////////////////////////
/*
    File BuffFrame.cs
    class BuffFrame
    
    담당자 : 이신홍
    부 담당자 : 김의겸

    버프 / 디버프의 정보에 접근하고, 이들을 이미지로 표현해놓은 클래스
*/
////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class BuffFrame : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    private UIBuffInfo buffInfo;    // UI 버프정보 툴팁
    private Sprite _mySprite;       // 이 버프의 이미지
    private string _myDescription;  // 이 버프의 설명
    private string _myName;         // 이 버프의 이름

    private void Start()
    {
        buffInfo = UIManager.Instance.playerUIView.buffInfo;
    }
    
    public void SetData(Sprite sprite, string des, string name)
    {
        _mySprite = sprite;
        _myDescription = des;
        _myName = name;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buffInfo.gameObject.SetActive(true);
        buffInfo.transform.position = eventData.position + new Vector2(5, 5);
        buffInfo.SetData(_mySprite, _myName, _myDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buffInfo.gameObject.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buffInfo.gameObject.SetActive(false);
    }
}
