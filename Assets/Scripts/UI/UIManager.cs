using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : SingletonBase<UIManager>
{

    //[SerializeField] private Button AttackBtn;
    [SerializeField] private Button InvincibleBtn;
    [SerializeField] private Button SkillAbtn;
    [SerializeField] private Button SkillBbtn;
    [SerializeField] private Button SkillCbtn;
    [SerializeField] private Button InventoryBtn;
    [SerializeField] private Button OptionBtn;

    // Start is called before the first frame update

    private void OnEnable()
    {
        //AttackBtn.onClick.AddListener(AttackClick);
        InvincibleBtn.onClick.AddListener(InvincibleClick);
        SkillAbtn.onClick.AddListener(SkillAClick);
        SkillBbtn.onClick.AddListener(SkillBClick);
        SkillCbtn.onClick.AddListener(SkillCClick);
        InventoryBtn.onClick.AddListener(InventoryClick);
        OptionBtn.onClick.AddListener(OptionClick);
    }

    void InventoryClick()
    {
        Debug.Log("인벤토리");
        UINavationManager.Instance.PushToNav("SubUI_Bag");
    }

    void Start()
    {
        UILoaderManager.Instance.AddScene("DungeonScene");
    }

    public void OptionClick()
    {
        Debug.Log("옵션");
        UINavationManager.Instance.PushToNav("SubUI_PlayerInfo");
    }
    public void SkillAClick() //A버튼 스킬
    {
        Player.Instance.PlayerSkillA();
        Debug.Log("A스킬");
    }
    public void SkillBClick() //B버튼 스킬
    {
        Player.Instance.PlayerSkillB();
        Debug.Log("B스킬");
    }
    public void SkillCClick() //C버튼 스킬
    {
        Player.Instance.PlayerSkillC();
        Debug.Log("C스킬");
    }
    public void InvincibleClick() //회피
    {
        Debug.Log("회피");
        Player.Instance.SetAvoidButton(true);
    }
    public void PlayerInfoClick()
    {
        Debug.Log("플레이어 정보창");
        UINavationManager.Instance.PushToNav("SubUI_Info");
    }

}
