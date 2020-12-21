﻿using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TalkManager : SingletonBase<TalkManager>
{
    private Dictionary<int, TalkData> _csvTalkData;         // CSV로 받아온 대화 데이터
    private Dictionary<int, QuestData> _csvQuestData;       // CSV로 받아온 퀘스트 데이터

    public Dictionary<int, Talk> talkDatas;                 // 대화 클래스 목록
    public Dictionary<int, Quest> questDatas;               // 퀘스트 클래스 목록
    public Dictionary<int, TalkChecker> talkCheckers;       // Object ID별 토크체커 목록

    public HashSet<Quest> startAbleQuests = new HashSet<Quest>();   // 시작 가능한 퀘스트 목록
    public HashSet<Quest> currentQuests = new HashSet<Quest>();     // 진행중인 퀘스트 목록
    public HashSet<Quest> endedQuests = new HashSet<Quest>();       // 종료된 퀘스트 목록

    private void Start()
    {
        talkDatas = new Dictionary<int, Talk>();
        questDatas = new Dictionary<int, Quest>();
        talkCheckers = new Dictionary<int, TalkChecker>();

        GetData();
        SetTalkCheckers();
        questDatas[0].canStart = true;
        questDatas[4].canStart = true;
        CheckQuestIsOn();
    }

    /// <summary>
    /// CSV를 통해 데이터를 받아오고, 이를 정제하여 Dictionary에 넣는다.
    /// </summary>
    public void GetData()
    {
        Table table = CSVReader.Reader.ReadCSVToTable("CSVData/ScriptData");
        _csvTalkData = table.TableToDictionary<int, TalkData>();

        table = CSVReader.Reader.ReadCSVToTable("CSVData/QuestData");
        _csvQuestData = table.TableToDictionary<int, QuestData>();

        foreach (TalkData data in _csvTalkData.Values)
        {
            Talk talk = new Talk();
            talk.RefineConvData(data);
            talkDatas[talk.talkData.convId] = talk;
        }

        foreach (QuestData data in _csvQuestData.Values)
        {
            Quest quest = new Quest();
            quest.RefineQuestData(data);
            questDatas[quest.questData.id] = quest;
        }
    }

    /// <summary>
    /// [임시] Object별 토크체커를 설정한다.
    /// </summary>
    public void SetTalkCheckers()
    {
        // Object CSV의 완성이 이뤄지면 자동화시킨다.
        talkCheckers[100] = new TalkChecker(100);
        talkCheckers[101] = new TalkChecker(101);
        talkCheckers[102] = new TalkChecker(102);
        talkCheckers[103] = new TalkChecker(103);
    }

    /// <summary>
    /// 현재 대화의 현재 진행중인 대사를 가져온다.
    /// </summary>
    /// <param name="convId">해당 대화의 ID</param>
    public string GetCurrentConv(int convId)
    {
        Talk data = talkDatas[convId];
        string text = data.texts[data.convIndex];
        return text;
    }

    /// <summary>
    /// 전체 퀘스트의 진행 중 상황을 업데이트한다.
    /// </summary>
    public void CheckQuestIsOn()
    {
        foreach (TalkChecker talkChecker in talkCheckers.Values)
        {
            talkChecker.CheckQuestIsOn();
        }
    }
}