using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using CSVReader;


[CSVReader.Data("id")]
public class NpcData
{
    public int id;
    public string name;
    public string prefabPath;
}

public class TalkManager : SingletonBase<TalkManager>
{
    private Dictionary<string, TalkData> _csvTalkData;         // CSV로 받아온 대화 데이터
    private Dictionary<string, QuestData> _csvQuestData;       // CSV로 받아온 퀘스트 데이터
    public Dictionary<int, NpcData> _csvNpcData;            // CSV로 받아온 NPC 데이터

    public Dictionary<string, Talk> talkDatas;                 // 대화 클래스 목록
    public Dictionary<string, Quest> questDatas;               // 퀘스트 클래스 목록
    public Dictionary<int, TalkChecker> talkCheckers;       // Object ID별 토크체커 목록

    public HashSet<Quest> startAbleQuests = new HashSet<Quest>();   // 시작 가능한 퀘스트 목록
    public HashSet<Quest> currentQuests = new HashSet<Quest>();     // 진행중인 퀘스트 목록
    public HashSet<Quest> endedQuests = new HashSet<Quest>();       // 종료된 퀘스트 목록

    public TalkUIView talkUI;

    public void InitTalkManager()
    {
        talkDatas = new Dictionary<string, Talk>();
        questDatas = new Dictionary<string, Quest>();
        talkCheckers = new Dictionary<int, TalkChecker>();

        GetData();
        SetTalkCheckers();
        FindInitStartQuest();
        CheckQuestIsOn();
    }

    /// <summary>
    /// 처음부터 시작이 가능한 퀘스트 리스트들을 받아온다.
    /// </summary>
    public void FindInitStartQuest()
    {
        foreach(Quest quest in questDatas.Values)
        {
            if(quest.neededQuestList[0].Equals("-")) quest.canStart = true;
        }
    }

    /// <summary>
    /// CSV를 통해 데이터를 받아오고, 이를 정제하여 Dictionary에 넣는다.
    /// </summary>
    public void GetData()
    {
        Table table = CSVReader.Reader.ReadCSVToTable("CSVData/ScriptData");
        _csvTalkData = table.TableToDictionary<string, TalkData>();

        table = CSVReader.Reader.ReadCSVToTable("CSVData/QuestData");
        _csvQuestData = table.TableToDictionary<string, QuestData>();

        table = CSVReader.Reader.ReadCSVToTable("CSVData/NPCData");
        _csvNpcData = table.TableToDictionary<int, NpcData>();

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
        talkCheckers[104] = new TalkChecker(104);
        talkCheckers[105] = new TalkChecker(105);
        talkCheckers[106] = new TalkChecker(106);
        talkCheckers[107] = new TalkChecker(107);
        talkCheckers[108] = new TalkChecker(108);
        talkCheckers[110] = new TalkChecker(110);
        talkCheckers[111] = new TalkChecker(111);
        talkCheckers[112] = new TalkChecker(112);
        talkCheckers[113] = new TalkChecker(113);
        talkCheckers[114] = new TalkChecker(114);
        talkCheckers[998] = new TalkChecker(998);
        talkCheckers[999] = new TalkChecker(999);
    }

    /// <summary>
    /// 현재 대화의 현재 진행중인 대사를 가져온다.
    /// </summary>
    /// <param name="convId">해당 대화의 ID</param>
    public string GetCurrentConv(string convId)
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
