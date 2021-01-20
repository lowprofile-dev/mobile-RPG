using System;
using System.Collections;
using System.Collections.Generic;
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
        dropdownButton.onClick.AddListener(delegate { ToggleDropdown(); });
        panels = new List<QuestPanel>();
    }

    private void ToggleDropdown()
    {
        questContainer.gameObject.SetActive(!questContainer.gameObject.activeSelf);
        SetQuestUIData();
    }
    
    public void ViewDropdown()
    {
        questContainer.gameObject.SetActive(true);
        SetQuestUIData();
    }


    public void SetQuestUIData()
    {
        foreach(QuestPanel panel in panels) panel.DestroyPanel();
        panels.Clear();

        Dictionary<string, Quest> dics = TalkManager.Instance.currentQuests;

        foreach(Quest quest in dics.Values)
        {
            AddPanel(quest);
        }
    }

    private void AddPanel(Quest quest)
    {
        QuestPanel panel = Instantiate(questObject, contents.transform).GetComponent<QuestPanel>();
        panel.SetData(quest);
        panels.Add(panel);
    }

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
