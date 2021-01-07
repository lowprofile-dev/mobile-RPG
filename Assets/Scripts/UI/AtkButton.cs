using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class AtkButton : MonoBehaviour //, IPointerDownHandler, IPointerUpHandler
{
    private bool isBtnDown = false;
    private float counter = 0f;
    private float atkTime = 0.8f;
    private bool atkcheck = true;
    [SerializeField] GameObject attackobj;
    Image attackImg;
    private void Start()
    {
        //attackImg = attackobj.GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(delegate { Player.Instance.AttackBtnClicked(); });
    }

    void Update()
    {
        //if (Player.Instance != null && !Player.Instance.isdead ) AttackBtnClick();
    }

    /*
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
       // attackobj.GetComponent<Image>().color = new Color(attackImg.color.r, attackImg.color.g, attackImg.color.b, 80f);
        isBtnDown = true;
    }

    public void OnPointerUp(PointerEventData eventData) // 눌렀다 뗏을 때
    {
        //attackobj.GetComponent<Image>().color = new Color(attackImg.color.r, attackImg.color.g, attackImg.color.b, 100f);
        isBtnDown = false;
        Player.Instance.SetAttackButton(isBtnDown);
    }
    */
}
