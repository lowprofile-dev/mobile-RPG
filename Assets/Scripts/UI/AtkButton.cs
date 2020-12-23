using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AtkButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isBtnDown = false;

    void Update()
    {
        AttackBtnClick();
    }

    private void AttackBtnClick()
    {
        if (isBtnDown)
        {
            if (Player.Instance != null) Player.Instance.SetAttackButton(isBtnDown);
        }
    }

    public void OnPointerDown(PointerEventData eventData) // 쭉누르기
    {
        isBtnDown = true;
    }

    public void OnPointerUp(PointerEventData eventData) // 눌렀다 뗏을 때
    {       
        isBtnDown = false;
    }
}
