using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using CSVReader;
using Newtonsoft.Json;
using System.IO;

[CSVReader.Data("id")]
[System.Serializable]
public class NpcData
{
    public int id;
    public string name;
    public string prefabPath;
}

[System.Serializable]
public class TalkManager : SingletonBase<TalkManager>
{
    private Dictionary<string, TalkData> _csvTalkData;         // CSV로 받아온 대화 데이터
    private Dictionary<string, QuestData> _csvQuestData;       // CSV로 받아온 퀘스트 데이터
    public Dictionary<int, NpcData> _csvNpcData;            // CSV로 받아온 NPC 데이터

    public Dictionary<string, Talk> talkDatas;                 // 대화 클래스 목록
    public Dictionary<string, Quest> questDatas;               // 퀘스트 클래스 목록
    public Dictionary<int, TalkChecker> talkCheckers;       // Object ID별 토크체커 목록

    public Dictionary<string, Quest> currentQuests;     // 진행중인 퀘스트 목록
    public Dictionary<string, Quest> endedQuests;       // 종료된 퀘스트 목록

    public TalkUIView talkUI;

    public void InitTalkManager()
    {
        talkDatas = new Dictionary<string, Talk>();
        questDatas = new Dictionary<string, Quest>();
        talkCheckers = new Dictionary<int, TalkChecker>();

        currentQuests = new Dictionary<string, Quest>();
        endedQuests = new Dictionary<string, Quest>();

        GetData();
        FindInitStartQuest();
        LoadCurrentQuests();
        SetTalkCheckers();
        CheckQuestIsOn();
    }

    
    public void SaveCurrentQuests()
    {
        if(questDatas != null)
        {
            string jsonData = JsonConvert.SerializeObject(questDatas, Formatting.Indented);
            string path = Path.Combine(Application.persistentDataPath, "playerQuest.json");
            File.WriteAllText(path, jsonData);
        }
    }
    
    public void LoadCurrentQuests()
    {
        if (PlayerPrefs.GetInt("LoadCurrentQuestKeys") == 0)
        {
            PlayerPrefs.SetInt("LoadCurrentQuestKeys", 1);
            SaveCurrentQuests();
            PlayerPrefs.Save();
        }

        LoadQuestAllData();
    }

    private void LoadQuestAllData()
    {
        string path = Path.Combine(Application.persistentDataPath, "playerQuest.json");
        string jsonData = File.ReadAllText(path);
        if (jsonData == null) return;
        questDatas = JsonConvert.DeserializeObject<Dictionary<string, Quest>>(jsonData);
        
        currentQuests.Clear();
        endedQuests.Clear();
        foreach(Quest quest in questDatas.Values)
        {
            if (quest.isEnd) endedQuests.Add(quest.id, quest);
            else if (quest.isOn) currentQuests.Add(quest.id, quest);
        }
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
            questDatas[quest.id] = quest;
        }
    }

    /// <summary>
    /// Object별 토크체커를 설정한다.
    /// </summary>
    public void SetTalkCheckers()
    {
        foreach (NpcData npcData in _csvNpcData.Values)
        {
            talkCheckers[npcData.id] = new TalkChecker(npcData.id);
        }
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

    /// <summary>
    /// 현재 퀘스트의 조건 완료 여부를 검사한다.
    /// </summary>
    public void SetQuestCondition(int condition, int conditionId, int conditionNumber)
    {
        foreach (Quest quest in currentQuests.Values)
        {
            quest.UpdateQuestCondition(condition, conditionId, conditionNumber);
        }

        CheckQuestIsOn();
    }

    private void OnApplicationQuit()
    {
        SaveCurrentQuests();
    }
}
