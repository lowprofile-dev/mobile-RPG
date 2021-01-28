////////////////////////////////////////////////////
/*
    File QuestDropdown.cs
    class QuestDropdown : 현재 활성화된 퀘스트 목록을 표시하는 UI

    담당자 : 이신홍
    부 담당자 : 
*/
////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestDropdown : SingletonBase<QuestDropdown>
{
    [SerializeField] private GameObject questObject;
    [SerializeField] private Button dropdownButton;
    [SerializeField] private Image questContainer;
    [SerializeField] private GameObject contents;

    List<QuestPanel> panels;

    private void Start()
    {
        dropdownButton.onClick.AddListener(delegate { ToggleDropdown(); }); // 드롭다운 버튼
        panels = new List<QuestPanel>();
    }

    /// <summary>
    /// 드롭다운을 켜거나 끈다.
    /// </summary>
    private void ToggleDropdown()
    {
        SoundManager.Instance.PlayEffect(SoundType.UI, "UI/ClickLightBase2", 0.9f);
        questContainer.gameObject.SetActive(!questContainer.gameObject.activeSelf);
        SetQuestUIData();
    }
    
    /// <summary>
    /// 드롭다운을 켠다.
    /// </summary>
    public void ViewDropdown()
    {
        questContainer.gameObject.SetActive(true);
        SetQuestUIData();
    }

    /// <summary>
    /// 각 퀘스트들의 정보를 설정하고 표출한다.
    /// </summary>
    public void SetQuestUIData()
    {

        for (int i = 0; i < panels.Count; i++) panels.ElementAt(i).DestroyPanel();
        panels.Clear();

        Dictionary<string, Quest> dics = TalkManager.Instance.currentQuests;

        for(int i=0; i<dics.Count; i++) AddPanel(dics.Values.ElementAt(i));
    }

    /// <summary>
    /// 퀘스트 패널을 추가한다.
    /// </summary>
    private void AddPanel(Quest quest)
    {
        QuestPanel panel = Instantiate(questObject, contents.transform).GetComponent<QuestPanel>();
        panel.SetData(quest);
        panels.Add(panel);
    }

    /// <summary>
    /// 퀘스트 패널의 데이터를 갱신한다.
    /// </summary>
    public void UpdatePanel(Quest quest)
    {
        for(int i=0; i<panels.Count; i++)
        {
            if(panels[i].parentQuest.id == quest.id)
            {
                panels[i].SetData(quest);
            }
        }
    }
}
