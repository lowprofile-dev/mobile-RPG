using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AtkButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isBtnDown = false;
    private float counter = 0f;
    private float atkTime = 0.8f;
    private bool atkcheck = true;
    void Update()
    {
        if (!Player.Instance.isdead) AttackBtnClick();

    }

    private void AttackBtnClick()
    {
        if (atkcheck)
        {
            if (Player.Instance != null && isBtnDown)
            {
                Player.Instance.SetAttackButton(isBtnDown);
                atkcheck = false;
            }
        }
        else
        {
            counter += Time.deltaTime;
            if(counter >= atkTime)
            {
                counter = 0f;
                atkcheck = true;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData) // 쭉누르기
    {
        isBtnDown = true;
    }

    public void OnPointerUp(PointerEventData eventData) // 눌렀다 뗏을 때
    {       
        isBtnDown = false;
        Player.Instance.SetAttackButton(isBtnDown);
    }

}
