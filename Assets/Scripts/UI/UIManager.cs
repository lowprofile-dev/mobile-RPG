using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : SingletonBase<UIManager>
{
    public PlayerUIView playerUIView;

    //[SerializeField] private Button AttackBtn;
    [SerializeField] private Button InvincibleBtn;
    [SerializeField] private Button SkillAbtn;
    [SerializeField] private Button SkillBbtn;
    [SerializeField] private Button SkillCbtn;
    [SerializeField] private Button InventoryBtn;

    public void InitUIManager()
    {
        playerUIView = GameObject.FindGameObjectWithTag("View").transform.Find("PlayerUI_View").GetComponent<PlayerUIView>();

        Transform playerView = GameObject.Find("PlayerUI_View").transform;
        InvincibleBtn = playerView.Find("InvincibleFrame").GetComponent<Button>();
        SkillAbtn = playerView.Find("SkillA").GetComponent<Button>();
        SkillBbtn = playerView.Find("SkillB").GetComponent<Button>();
        SkillCbtn = playerView.Find("SkillC").GetComponent<Button>();
        InventoryBtn = playerView.Find("Inventory").GetComponent<Button>();

        AddListenerToUI();
    }

    public void AddListenerToUI()
    {
        InvincibleBtn.onClick.AddListener(InvincibleClick);
        InventoryBtn.onClick.AddListener(InventoryClick);
    }

    void InventoryClick()
    {
        //Debug.log("인벤토리");
        UINaviationManager.Instance.PushToNav("SubUI_Inventory");
    }

    /*
    public void OptionClick()
    {
        //Debug.log("옵션");

        if (Time.timeScale != 0)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        //옵션 구현
        //UINaviationManager.Instance.PushToNav("SubUI_Info");
    }
    */

    public void InvincibleClick() //회피
    {
        //Debug.log("회피");
        if (!Player.Instance.isdead) Player.Instance.SetAvoidButton(true);
    }
    public void PlayerInfoClick()
    {
        //Debug.log("플레이어 정보창");
        UINaviationManager.Instance.PushToNav("SubUI_Info");
    }

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
