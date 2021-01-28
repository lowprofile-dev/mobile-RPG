///////////////////////////////////////
/*
    File UIManager.cs
    class UIManager : UI의 기타 기능들을 모아둔 매니저

    담당자 : 안영훈
    부 담당자 : 이신홍 
*/
////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : SingletonBase<UIManager>
{
    public PlayerUIView playerUIView;

    [SerializeField] private Button InvincibleBtn;
    [SerializeField] private Button SkillAbtn;
    [SerializeField] private Button SkillBbtn;
    [SerializeField] private Button SkillCbtn;
    [SerializeField] private Button InventoryBtn;

    public void InitUIManager()
    {
        playerUIView = GameObject.FindGameObjectWithTag("View").transform.Find("PlayerUI_View").GetComponent<PlayerUIView>();

        InvincibleBtn = playerUIView.invincibleBtn;
        SkillAbtn = playerUIView.skillAButton;
        SkillBbtn = playerUIView.skillBButton;
        SkillCbtn = playerUIView.skillCButton;
        InventoryBtn = playerUIView.inventoryButton;

        AddListenerToUI();
    }

    public void AddListenerToUI()
    {
        InvincibleBtn.onClick.AddListener(InvincibleClick);
        InventoryBtn.onClick.AddListener(InventoryClick);
    }

    void InventoryClick()
    {
        UINaviationManager.Instance.PushToNav("SubUI_Inventory");
    }
    
    public void InvincibleClick() //회피
    {
    }

    public void PlayerInfoClick()
    {
        UINaviationManager.Instance.PushToNav("SubUI_Info");
    }

    /// <summary>
    /// RichText의 다양한 표현을 지원하기위한 함수
    /// </summary>
    /// <param name="text">string에 들어갈 내용</param>
    /// <param name="colorName">적용할 색</param>
    /// <param name="fontSize">폰트 크기</param>
    public string AddFontData(string text, string colorName, float fontSize = 20)
    {
        string rtnText = "";

        rtnText += "<color=";
        switch (colorName)
        {
            case "aqua": rtnText += "#00ffffff"; break;
            case "black": rtnText += "#000000ff"; break;
            case "blue": rtnText += "#0000ffff"; break;
            case "brown": rtnText += "#a52a2aff"; break;
            case "cyan": rtnText += "#00ffffff"; break;
            case "darkblue": rtnText += "#0000a0ff"; break;
            case "fuchsia": rtnText += "#ff00ffff"; break;
            case "green": rtnText += "#008000ff"; break;
            case "grey": rtnText += "#808080ff"; break;
            case "lightblue": rtnText += "#add8e6ff"; break;
            case "lime": rtnText += "#00ff00ff"; break;
            case "magenta": rtnText += "#ff00ffff"; break;
            case "maroon": rtnText += "#800000ff"; break;
            case "navy": rtnText += "#000080ff"; break;
            case "olive": rtnText += "#808000ff"; break;
            case "purple": rtnText += "#800080ff"; break;
            case "red": rtnText += "#ff0000ff"; break;
            case "silver": rtnText += "#c0c0c0ff"; break;
            case "teal": rtnText += "#008080ff"; break;
            case "white": rtnText += "#ffffffff"; break;
            case "yellow": rtnText += "#ffff00ff"; break;
            default: rtnText += colorName; break;
        }
        rtnText += ">";

        rtnText += "<size=" + fontSize + ">";
        rtnText += text;

        rtnText += "</size>";
        rtnText += "</color>";

        return rtnText;
    }
}
