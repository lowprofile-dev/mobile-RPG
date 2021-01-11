using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 퀘스트의 정보 및 기능이 담겨있다.
/// </summary>
public class Quest
{
    private TalkManager _talkManager; // 토크매니저 캐싱

    private QuestData _questData; public QuestData questData { get { return _questData; } }     // CSV를 통해 읽어온 퀘스트 데이터

    // 퀘스트 진행여부 관련
    private int _currentIndex; public int currentIndex { get { return _currentIndex; } }        // 현재 퀘스트의 몇번째 진행상황인지
    public bool canStart;   // 퀘스트를 시작할 수 있는지.
    public bool canEnd;     // 퀘스트 종료 조건이 채워졌는지.
    public bool isOn;       // 퀘스트가 진행 중인지.
    public bool isEnd;      // 퀘스트가 종료되었는지.

    // 정제한 데이터
    private List<int> _npcList; public List<int> npcList { get { return _npcList; } }                                       // 상호작용 NPC가 순서대로 되어있다.
    private List<string> _convList; public List<string> convList { get { return _convList; } }                              // 대화 번호가 순서대로 되어있다.
    private List<string> _afterQuestList; public List<string> afterQuestList { get { return _afterQuestList; } }            // 클리어시 새롭게 열리는 퀘스트 목록
    private List<string> _quitQuestList; public List<string> quitQuestList { get { return _quitQuestList; } }               // 클리어시 닫는 퀘스트 목록
    private List<string> _neededQuestList; public List<string> neededQuestList { get { return _neededQuestList; } }         // 열리기 위해 필요한 선행 퀘스트 목록
    private List<int> _rewardList; public List<int> rewardList { get { return _rewardList; } }                              // 보상 리스트
    private List<int> _rewardIdList; public List<int> rewardIdList { get { return _rewardIdList; } }                        // 보상 아이디 리스트
    private List<int> _rewardAmountList; public List<int> rewardAmountList { get { return _rewardAmountList; } }            // 보상 개수 리스트    
    private List<int> _conditionIdList; public List<int> conditionIdList { get { return _conditionIdList; } }               // 조건 아이디 리스트
    private List<int> _conditionAmountList; public List<int> conditionAmountList { get { return _conditionAmountList; } }   // 조건 개수 리스트


    public Quest()
    {
        _talkManager = TalkManager.Instance;

        _npcList = new List<int>();
        _convList = new List<string>();
        _afterQuestList = new List<string>();
        _quitQuestList = new List<string>();
        _neededQuestList = new List<string>();
        _rewardList = new List<int>();
        _rewardIdList = new List<int>();
        _rewardAmountList = new List<int>();
        _conditionIdList = new List<int>();
        _conditionAmountList = new List<int>();

        _currentIndex = 0;

        isOn = false;
        isEnd = false;
        canStart = false;
    }

    /// <summary>
    /// 퀘스트를 시작한다.
    /// </summary>
    public void StartQuest()
    {
        // 시작 연출

        Debug.Log("퀘스트 [" + questData.questName + "] 을 시작합니다.");
        canStart = false;
        isOn = true;

        _talkManager.currentQuests.Add(this);
        TalkManager.Instance.CheckQuestIsOn();
        QuestDropdown.Instance.ViewDropdown();
    }

    /// <summary>
    /// 퀘스트의 완료를 처리한다.
    /// </summary>
    public void SuccessQuest()
    {
        Debug.Log("퀘스트 [" + questData.questName + "] 을 클리어하였습니다.");

        // 완료 연출
        // 보상하는 알고리즘.
        // TO DO // 

        // 결과물 관리
        _talkManager.endedQuests.Add(this);
        _talkManager.currentQuests.Remove(this);
        isOn = false;
        isEnd = true;

        // - = 이어지는 퀘스트가 없다.
        if (_afterQuestList[0] != "-")
        {
            // 이어지는 퀘스트 리스트의 추가 가능 여부를 보고 퀘스트의 추가를 결정한다.
            for (int i = 0; i < _afterQuestList.Count; i++)
            {
                _talkManager.questDatas[_afterQuestList[i]].CheckQuestCanStart();
            }
        }

        TalkManager.Instance.CheckQuestIsOn();
        QuestDropdown.Instance.ViewDropdown();
    }

    /// <summary>
    /// 다음 퀘스트 인덱스로 넘어간다.
    /// </summary>
    public void NextIndex()
    {
        _currentIndex++;

        // 퀘스트 완료
        if (_currentIndex == convList.Count)
        {
            SuccessQuest();
        }
    }

    /// <summary>
    /// 받은 퀘스트 데이터를 정제한다.
    /// </summary>
    public void RefineQuestData(QuestData targetQuestData)
    {
        _questData = targetQuestData;

        string[] split = _questData.npcId.Split(' ');
        for (int j = 0; j < split.Length; j++) _npcList.Add(int.Parse(split[j]));

        split = _questData.convId.Split(' ');
        for (int j = 0; j < split.Length; j++) _convList.Add(split[j]);

        split = _questData.afterQuestsId.Split(' ');
        for (int j = 0; j < split.Length; j++) _afterQuestList.Add(split[j]);

        split = _questData.quitQuestId.Split(' ');
        for (int j = 0; j < split.Length; j++) _quitQuestList.Add(split[j]);

        split = _questData.neededQuestsId.Split(' ');
        for (int j = 0; j < split.Length; j++) _neededQuestList.Add(split[j]);

        try
        {
            split = _questData.reward.Split(' ');
            for (int j = 0; j < split.Length; j++) _rewardList.Add(int.Parse(split[j]));

        }
        catch
        {
            Debug.Log("WQKJEKL");
        }

        split = _questData.rewardId.Split(' ');
        for (int j = 0; j < split.Length; j++) _rewardIdList.Add(int.Parse(split[j]));

        split = _questData.rewardAmount.Split(' ');
        for (int j = 0; j < split.Length; j++) _rewardAmountList.Add(int.Parse(split[j]));

        split = _questData.conditionId.Split(' ');
        for (int j = 0; j < split.Length; j++) _conditionIdList.Add(int.Parse(split[j]));

        split = _questData.conditionAmount.Split(' ');
        for (int j = 0; j < split.Length; j++) _conditionAmountList.Add(int.Parse(split[j]));
    }

    /// <summary>
    /// 퀘스트의 시작 가능 여부를 판단
    /// </summary>
    public void CheckQuestCanStart()
    {
        // 모든 필요 퀘스트가 종료 퀘스트에 들어갔는지 확인
        bool startFlag = true;
        for (int i = 0; i < _neededQuestList.Count; i++)
        {
            if (!_talkManager.endedQuests.Contains(_talkManager.questDatas[_neededQuestList[i]]))
            {
                startFlag = false;
                break;
            }
        }

        // 조건이 맞는다면 시작 가능 퀘스트로 추가
        if (startFlag)
        {
            _talkManager.startAbleQuests.Add(this);
            canStart = true;
        }
    }

    public override string ToString()
    {
        string data = "";

        data += "이름 : " + questData.questName + '\n';
        for (int i = 0; i < _npcList.Count; i++)
        {
            data += _npcList[i] + " " + _convList[i] + "\n";
        }
        data += "설명 : " + questData.description + '\n';

        return data;
    }
}