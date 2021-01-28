////////////////////////////////////////////////////
/*
    File SoundManager.cs
    class SoundManager

    담당자 : 이신홍
    부 담당자 :
    
    퀘스트 및 대화를 담당하는 매니저
*/
////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using CSVReader;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

[System.Serializable]
public class TalkManager : SingletonBase<TalkManager>
{
    private Dictionary<string, TalkData> _csvTalkData;      // CSV로 받아온 대화 데이터
    private Dictionary<string, QuestData> _csvQuestData;    // CSV로 받아온 퀘스트 데이터
    public Dictionary<int, NpcData> _csvNpcData;            // CSV로 받아온 NPC 데이터

    public Dictionary<string, Talk> talkDatas;              // 대화 클래스 목록
    public Dictionary<string, Quest> questDatas;            // 퀘스트 클래스 목록
    public Dictionary<int, TalkChecker> talkCheckers;       // Object ID별 토크체커 목록

    public Dictionary<string, Quest> currentQuests;         // 진행중인 퀘스트 목록
    public Dictionary<string, Quest> endedQuests;           // 종료된 퀘스트 목록

    public TalkUIView talkUI;                               // 대화창 UI



    ////////// 베이스 //////////

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




    ////////// 데이터 //////////

    /// <summary>
    /// CSV를 통해 데이터를 받아오고, 이를 정제하여 Dictionary에 넣는다.
    /// </summary>
    public void GetData()
    {
        Table table = CSVReader.Reader.ReadCSVToTable("CSVData/ScriptData");
        _csvTalkData = table.TableToDictionary<string, TalkData>(); // 대화 데이터

        table = CSVReader.Reader.ReadCSVToTable("CSVData/QuestData");
        _csvQuestData = table.TableToDictionary<string, QuestData>(); // 퀘스트 데이터

        table = CSVReader.Reader.ReadCSVToTable("CSVData/NPCData");
        _csvNpcData = table.TableToDictionary<int, NpcData>(); // NPC 데이터

        for(int i=0; i<_csvTalkData.Count; i++) // TalkData -> Talk
        {
            Talk talk = new Talk();
            talk.ParsingConvData(_csvTalkData.Values.ElementAt(i));
            talkDatas[talk.talkData.convId] = talk;
        }

        for(int i=0; i<_csvQuestData.Count; i++) // QuestData -> Quest
        {
            Quest quest = new Quest();
            quest.ParsingQuestData(_csvQuestData.Values.ElementAt(i));
            questDatas[quest.id] = quest;
        }
    }



    ////////// 초기화 //////////
    
    /// <summary>
    /// 처음부터 시작이 가능한 퀘스트 리스트들을 받아온다.
    /// </summary>
    public void FindInitStartQuest()
    {
        Quest quest = null;
        for (int i = 0; i < questDatas.Count; i++)
        {
            quest = questDatas.Values.ElementAt(i);
            if(quest.neededQuestList[0].Equals("-")) quest.canStart = true;
        }
    }


    /// <summary>
    /// Object별 토크체커를 설정한다.
    /// </summary>
    public void SetTalkCheckers()
    {
        NpcData npcData = null;
        for (int i=0; i<_csvNpcData.Count; i++)
        {
            npcData = _csvNpcData.Values.ElementAt(i);
            talkCheckers[npcData.id] = new TalkChecker(npcData.id);
        }
    }




    ////////// 동기화 //////////

    /// <summary>
    /// 전체 퀘스트의 진행 중 상황을 업데이트한다.
    /// </summary>
    public void CheckQuestIsOn()
    {
        for(int i=0; i<talkCheckers.Count; i++)
        {
            talkCheckers.Values.ElementAt(i).CheckQuestIsOn();
        }
    }

    /// <summary>
    /// 현재 퀘스트의 조건 완료 여부를 검사한다.
    /// </summary>
    public void SetQuestCondition(int condition, int conditionId, int conditionNumber)
    {
        for (int i=0; i<currentQuests.Count; i++)
        {
            currentQuests.Values.ElementAt(i).UpdateQuestCondition(condition, conditionId, conditionNumber);
        }

        CheckQuestIsOn();
    }





    ////////// 세이브 & 로드 //////////

    /// <summary>
    /// 퀘스트 데이터 저장 (Newton JSON 사용)
    /// </summary>
    public void SaveCurrentQuests()
    {
        if (questDatas != null)
        {
            string jsonData = JsonConvert.SerializeObject(questDatas, Formatting.Indented);
            string path = Path.Combine(Application.persistentDataPath, "playerQuest.json");
            File.WriteAllText(path, jsonData);
        }
    }

    /// <summary>
    /// 퀘스트 데이터 로드 (Newton JSON 사용)
    /// </summary>
    public void LoadCurrentQuests()
    {
        if (PlayerPrefs.GetInt("LoadCurrentQuestKeys") == 0) // 처음 불러오는 케이스, 저장소 만들어놓기
        {
            PlayerPrefs.SetInt("LoadCurrentQuestKeys", 1);
            SaveCurrentQuests(); 
            PlayerPrefs.Save();
        }

        LoadQuestAllData();
    }

    /// <summary>
    /// 퀘스트 데이터를 파싱 및 로드한다.
    /// </summary>
    private void LoadQuestAllData()
    {
        string path = Path.Combine(Application.persistentDataPath, "playerQuest.json");
        string jsonData = File.ReadAllText(path);
        if (jsonData == null) return;
        questDatas = JsonConvert.DeserializeObject<Dictionary<string, Quest>>(jsonData);

        InputSaveQuestDataToCurrentEndQuests();
    }

    /// <summary>
    /// 로드한 퀘스트 데이터들을 분류한다.
    /// </summary>
    private void InputSaveQuestDataToCurrentEndQuests()
    {
        currentQuests.Clear();
        endedQuests.Clear();
        Quest quest = null;
        for (int i=0; i<questDatas.Count; i++)
        {
            quest = questDatas.Values.ElementAt(i);
            if (quest.isEnd) endedQuests.Add(quest.id, quest);
            else if (quest.isOn) currentQuests.Add(quest.id, quest);
        }
    }

    /// <summary>
    /// 종료시 현재 퀘스트 데이터를 저장해놓음.
    /// </summary>
    private void OnApplicationQuit()
    {
        SaveCurrentQuests();
    }
}
