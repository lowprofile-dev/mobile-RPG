using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    //[SerializeField] private Button AttackBtn;
    [SerializeField] private Button InvincibleBtn;
    [SerializeField] private Button SkillAbtn;
    [SerializeField] private Button SkillBbtn;
    [SerializeField] private Button SkillCbtn;
    [SerializeField] private Button OptionBtn;

    // Start is called before the first frame update

    private void OnEnable()
    {
        //AttackBtn.onClick.AddListener(AttackClick);
        InvincibleBtn.onClick.AddListener(InvincibleClick);
        SkillAbtn.onClick.AddListener(SkillAClick);
        SkillBbtn.onClick.AddListener(SkillBClick);
        SkillCbtn.onClick.AddListener(SkillCClick);
        OptionBtn.onClick.AddListener(OptionClick);
    }
    private void Start()
    {
        UILoaderManager.Instance.AddScene("DungeonScene");
    }

    private void OptionClick()
    {
        Debug.Log("옵션");
    }
    void SkillAClick() //A버튼 스킬
    {
        Debug.Log("A스킬");
    }
    void SkillBClick() //B버튼 스킬
    {
        Debug.Log("B스킬");
    }
    void SkillCClick() //C버튼 스킬
    {
        Debug.Log("C스킬");
    }
    void InvincibleClick() //회피
    {
        Debug.Log("회피");
        
    }

}
