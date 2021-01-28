//////////////////////////////////////////////
/*
    File Quest.cs
    class Quest
    
    퀘스트의 데이터, 작동 등을 모아둔 클래스

    담당자 : 이신홍
    부담당자 :
*/
////////////////////////////////////////////////////

using System.Collections.Generic;

[System.Serializable]
public class Quest
{
    // 퀘스트 데이터 (QuestData를 쓰지 않는 이유는 Save Load 라이브러리와의 호환성 때문)
    public string id;                   // 퀘스트 ID
    public string questName;            // 퀘스트 이름
    public string description;          // 퀘스트 설명
    public string clearDescription;     // 퀘스트 성공 조건 설명

    // 퀘스트 진행여부 관련
    public int currentIndex;                    // 현재 퀘스트의 몇번째 진행상황인지
    public bool canStart;                       // 퀘스트를 시작할 수 있는지.
    public bool canEnd;                         // 퀘스트 종료 조건이 채워졌는지.
    public bool isOn;                           // 퀘스트가 진행 중인지.
    public bool isEnd;                          // 퀘스트가 종료되었는지.
    private List<int> _curConditionAmountList;  // 조건 현황

    // 정제한 데이터
    private List<int> _npcList;                 // 상호작용 NPC가 순서대로 되어있다.
    private List<string> _convList;             // 대화 번호가 순서대로 되어있다.
    private List<string> _afterQuestList;       // 클리어시 새롭게 열리는 퀘스트 목록
    private List<string> _quitQuestList;        // 클리어시 닫는 퀘스트 목록
    private List<string> _neededQuestList;      // 열리기 위해 필요한 선행 퀘스트 목록
    private List<int> _rewardList;              // 보상 리스트
    private List<int> _rewardIdList;            // 보상 아이디 리스트
    private List<int> _rewardAmountList;        // 보상 개수 리스트    
    private List<int> _conditionList;           // 조건 리스트
    private List<int> _conditionIdList;         // 조건 아이디 리스트
    private List<int> _conditionAmountList;     // 조건 개수 리스트

    // property
    public List<int> curConditionAmountList { get { return _curConditionAmountList; } }
    public List<int> npcList { get { return _npcList; } }
    public List<string> convList { get { return _convList; } }
    public List<string> afterQuestList { get { return _afterQuestList; } }
    public List<string> quitQuestList { get { return _quitQuestList; } }
    public List<string> neededQuestList { get { return _neededQuestList; } }
    public List<int> rewardList { get { return _rewardList; } }
    public List<int> rewardIdList { get { return _rewardIdList; } }
    public List<int> rewardAmountList { get { return _rewardAmountList; } }
    public List<int> conditionList { get { return _conditionList; } }
    public List<int> conditionIdList { get { return _conditionIdList; } }
    public List<int> conditionAmountList { get { return _conditionAmountList; } }


    //////////// 베이스 /////////////

    public Quest()
    {
        _npcList = new List<int>();
        _convList = new List<string>();
        _afterQuestList = new List<string>();
        _quitQuestList = new List<string>();
        _neededQuestList = new List<string>();
        _rewardList = new List<int>();
        _rewardIdList = new List<int>();
        _rewardAmountList = new List<int>();
        _conditionList = new List<int>();
        _conditionIdList = new List<int>();
        _conditionAmountList = new List<int>();
        _curConditionAmountList = new List<int>();

        currentIndex = 0;

        isOn = false;
        isEnd = false;
        canEnd = false;
        canStart = false;
    }



    //////////// 데이터 /////////////

    /// <summary>
    /// 받은 퀘스트 데이터를 정제한다.
    /// </summary>
    public void ParsingQuestData(QuestData targetQuestData)
    {
        id = targetQuestData.id;
        questName = targetQuestData.questName;
        description = targetQuestData.description;
        clearDescription = targetQuestData.questDescription;

        string[] split = targetQuestData.npcId.Split(' ');
        for (int j = 0; j < split.Length; j++) _npcList.Add(int.Parse(split[j]));

        split = targetQuestData.convId.Split(' ');
        for (int j = 0; j < split.Length; j++) _convList.Add(split[j]);

        split = targetQuestData.afterQuestsId.Split(' ');
        for (int j = 0; j < split.Length; j++) _afterQuestList.Add(split[j]);

        split = targetQuestData.quitQuestId.Split(' ');
        for (int j = 0; j < split.Length; j++) _quitQuestList.Add(split[j]);

        split = targetQuestData.neededQuestsId.Split(' ');
        for (int j = 0; j < split.Length; j++) _neededQuestList.Add(split[j]);

        split = targetQuestData.reward.Split(' ');
        for (int j = 0; j < split.Length; j++) _rewardList.Add(int.Parse(split[j]));

        split = targetQuestData.rewardId.Split(' ');
        for (int j = 0; j < split.Length; j++) _rewardIdList.Add(int.Parse(split[j]));

        split = targetQuestData.rewardAmount.Split(' ');
        for (int j = 0; j < split.Length; j++) _rewardAmountList.Add(int.Parse(split[j]));

        split = targetQuestData.condition.Split(' ');
        for (int j = 0; j < split.Length; j++) _conditionList.Add(int.Parse(split[j]));

        split = targetQuestData.conditionId.Split(' ');
        for (int j = 0; j < split.Length; j++) _conditionIdList.Add(int.Parse(split[j]));

        split = targetQuestData.conditionAmount.Split(' ');
        for (int j = 0; j < split.Length; j++)
        {
            _conditionAmountList.Add(int.Parse(split[j]));
            _curConditionAmountList.Add(0);
        }
    }



    //////////// 퀘스트 진행 /////////////

    /// <summary>
    /// 퀘스트의 시작 가능 여부를 판단
    /// </summary>
    public void CheckQuestCanStart()
    {
        // 모든 필요 퀘스트가 종료 퀘스트에 들어갔는지 확인
        bool startFlag = true;
        for (int i = 0; i < _neededQuestList.Count; i++)
        {
            if (!TalkManager.Instance.endedQuests.ContainsValue(TalkManager.Instance.questDatas[_neededQuestList[i]]))
            {
                startFlag = false;
                break;
            }
        }

        // 조건이 맞는다면 시작 가능 퀘스트로 추가
        if (startFlag) canStart = true;
    }

    /// <summary>
    /// 퀘스트를 시작한다.
    /// </summary>
    public void StartQuest()
    {
        canStart = false;
        isOn = true;
        canEnd = false;
        isEnd = false;

        SoundManager.Instance.PlayEffect(SoundType.UI, "UI/QuestAccepted", 1f);

        TalkManager.Instance.currentQuests.Add(id, this);
        TalkManager.Instance.CheckQuestIsOn();
        QuestDropdown.Instance.ViewDropdown();
        TalkManager.Instance.SaveCurrentQuests();
        SystemPanel.instance.SetText(questName + " 퀘스트를 시작합니다.");
        SystemPanel.instance.FadeOutStart();
    }

    /// <summary>
    /// 퀘스트의 완료를 처리한다.
    /// </summary>
    public void SuccessQuest()
    {
        GetQuestReward();

        SoundManager.Instance.PlayEffect(SoundType.UI, "UI/QuestRewardBag", 1f);
        SoundManager.Instance.PlayEffect(SoundType.UI, "UI/QuestRewardJewel", 1f);

        // 결과물 관리
        TalkManager.Instance.endedQuests.Add(id, this);
        TalkManager.Instance.currentQuests.Remove(id);
        canStart = false;
        canEnd = false;
        isOn = false;
        isEnd = true;

        // - = 이어지는 퀘스트가 없다.
        if (_afterQuestList[0] != "-")
        {
            // 이어지는 퀘스트 리스트의 추가 가능 여부를 보고 퀘스트의 추가를 결정한다.
            for (int i = 0; i < _afterQuestList.Count; i++)
            {
                TalkManager.Instance.questDatas[_afterQuestList[i]].CheckQuestCanStart();
            }
        }

        TalkManager.Instance.CheckQuestIsOn();
        QuestDropdown.Instance.ViewDropdown();
        TalkManager.Instance.SaveCurrentQuests();
    }

    /// <summary>
    /// 다음 퀘스트 인덱스로 넘어간다.
    /// </summary>
    public void NextIndex()
    {
        currentIndex++;
    }



    //////////// 보상 /////////////

    /// <summary>
    /// 퀘스트 보상을 받는다.
    /// </summary>
    public void GetQuestReward()
    {
        for (int i = 0; i < _rewardList.Count; i++)
        {
            int rewardId = _rewardIdList[i];
            int rewardAmount = _rewardAmountList[i];

            switch (_rewardList[i])
            {
                case 0: // 재화 
                    GetGoldReward(rewardId, rewardAmount);
                    break;
                case 1: // 숙련도
                    GetExpReward(rewardId, rewardAmount);
                    break;
                case 2: // 아이템
                    for (int j = 0; j < rewardAmount; j++) ItemManager.Instance.AddItem(ItemManager.Instance.itemDictionary[rewardId]); // 해당 아이템을 n개만큼 추가한다.
                    break;
            }
        }
    }

    /// <summary>
    /// 숙련도(Exp) 보상을 받는다.
    /// </summary>
    private void GetExpReward(int rewardId, int rewardAmount)
    {
        switch (rewardId)
        {
            case 0: WeaponManager.Instance.AddExpToSpecificWeapon("SWORD", rewardAmount); break;
            case 1: WeaponManager.Instance.AddExpToSpecificWeapon("WAND", rewardAmount); break;
            case 2: WeaponManager.Instance.AddExpToSpecificWeapon("DAGGER", rewardAmount); break;
            case 3: WeaponManager.Instance.AddExpToSpecificWeapon("BLUNT", rewardAmount); break;
            case 4: WeaponManager.Instance.AddExpToSpecificWeapon("STAFF", rewardAmount); break;
            case 5: // 전체 숙련도 적용
                WeaponManager.Instance.AddExpToSpecificWeapon("SWORD", rewardAmount);
                WeaponManager.Instance.AddExpToSpecificWeapon("WAND", rewardAmount);
                WeaponManager.Instance.AddExpToSpecificWeapon("DAGGER", rewardAmount);
                WeaponManager.Instance.AddExpToSpecificWeapon("BLUNT", rewardAmount);
                WeaponManager.Instance.AddExpToSpecificWeapon("STAFF", rewardAmount);
                break;
        }
    }

    /// <summary>
    /// 재화 보상을 받는다.
    /// </summary>
    private void GetGoldReward(int rewardId, int rewardAmount)
    {
        switch (rewardId)
        {
            case 0: ItemManager.Instance.AddGold(rewardAmount); break; // 돈
            case 1: ItemManager.Instance.AddCoin(rewardAmount); break; // 코인
            case 2: ItemManager.Instance.AddGem(rewardAmount); break; // 젬
        }
    }


    
    //////////// 클리어 조건 /////////////
    
    /// <summary>
    /// 조건을 받아 완료 조건을 체크한다.
    /// </summary>
    /// <param name="condition">조건 종류</param>
    /// <param name="conditionId">조건 세부 종류</param>
    /// <param name="conditionNumber">채우기 개수</param>
    public void UpdateQuestCondition(int condition, int conditionId, int conditionNumber)
    {
        switch (condition)
        {
            case 0: break; // 바로 클리어 가능
            case 1: MonsterHuntCheck(conditionId, conditionNumber); break; // 몬스터를 잡아야 클리어 가능
            case 2: break; // 아이템을 모아야 클리어 가능
            case 3: dungeonCheck(conditionId, conditionNumber); break; // 던전을 방문 혹은 클리어해야 클리어 가능
        }

        CheckQuestCanEnd();
        UIManager.Instance.playerUIView.questDropdown.UpdatePanel(this);
    }

    /// <summary>
    /// 조건들을 모두 완료했는지 체크
    /// </summary>
    public void CheckQuestCanEnd()
    {
        bool endFlag = true; // flag

        for (int i = 0; i < _conditionList.Count; i++)
        {
            if (_curConditionAmountList[i] < _conditionAmountList[i]) // 하나라도 완료가 덜 되었다면
            {
                endFlag = false; // flag flase
                break;
            }
        }

        if (endFlag) canEnd = true;
        else canEnd = false;
    }

    /// <summary>
    /// 던전 관련 퀘스트 조건 체크
    /// </summary>
    private void dungeonCheck(int conditionId, int conditionNumber)
    {
        for (int i = 0; i < _conditionList.Count; i++)
        {
            if (_conditionList[i] == 3) // 던전 관련 조건이고
            {
                if (conditionId == _conditionIdList[i]) // 그 조건이 현재 들어온 조건과 같다면
                {
                    _curConditionAmountList[i] += conditionNumber;
                }
            }
        }
    }

    /// <summary>
    /// 몬스터 관련 퀘스트 조건 체크
    /// </summary>
    private void MonsterHuntCheck(int conditionId, int conditionNumber)
    {
        for (int i = 0; i < _conditionList.Count; i++)
        {
            if (_conditionList[i] == 1) // 몬스터 관련 조건이고
            {
                if (conditionId == _conditionIdList[i]) // 조건의 몬스터가 현재 잡은 몬스터라면
                {
                    _curConditionAmountList[i] += conditionNumber;
                }
            }
        }
    }
    
    
    
    //////////// 기타 /////////////

    public override string ToString()
    {
        string data = "";

        data += "이름 : " + questName + '\n';
        for (int i = 0; i < _npcList.Count; i++)
        {
            data += _npcList[i] + " " + _convList[i] + "\n";
        }
        data += "설명 : " + description + '\n';

        return data;
    }
}