using System.Collections.Generic;
using System.Collections;
using UnityEngine;

/// <summary>
/// 각 Object별 퀘스트 및 대화를 관리하는 클래스
/// </summary>
[System.Serializable]
public class TalkChecker
{
    private int _objectId;

    private List<Quest> _relevantQuests; public List<Quest> relevantQuests { get { return _relevantQuests; } }  // 관계된 퀘스트 목록
    private List<Talk> _relevantTalks; public List<Talk> relevantTalks { get { return _relevantTalks; } }       // 관계된 대사 목록
    private bool _canStartQuest; public bool canStartQuest { get { return _canStartQuest; } }                   // 현재 퀘스트 시작이 가능한지
    private bool _canFinishQuest; public bool canFinishQuest { get { return _canFinishQuest; } }                // 현재 퀘스트 종료가 가능한지
    private bool _canProgressQuest; public bool canProcressQuest { get { return _canProgressQuest; } }          // 현재 퀘스트 진행중인지
    private int _talkIndex; public int talkIndex { get { return _talkIndex; } }                                 // relevantTalk에서 몇번째 대화인지          

    public HashSet<NonLivingEntity> _npcList;

    public TalkChecker(int id)
    {
        _objectId = id;

        _relevantQuests = new List<Quest>();
        _relevantTalks = new List<Talk>();
        _npcList = new HashSet<NonLivingEntity>();
        AddRelevantQuests();
        AddRelevantTalks();
        _talkIndex = 0;
    }

    /// <summary>
    /// 대사 전체를 돌며 이 NPC와 관련된 대사들을 등록시킨다.
    /// </summary>
    private void AddRelevantTalks()
    {
        TalkManager manager = TalkManager.Instance;
        foreach (Talk cntTalk in manager.talkDatas.Values)
        {
            // 일반 대사이며 현 npc라면
            if (cntTalk.talkData.npcId == _objectId && cntTalk.talkData.convType.Equals("NORMAL"))
            {
                _relevantTalks.Add(cntTalk);
            }
        }
    }

    /// <summary>
    /// 퀘스트 전체를 돌며 이 NPC와 관련된 퀘스트들을 등록시킨다.
    /// </summary>
    private void AddRelevantQuests()
    {
        foreach (Quest cntQuest in TalkManager.Instance.questDatas.Values)
        {
            for (int j = 0; j < cntQuest.npcList.Count; j++)
            {
                if (cntQuest.npcList[j] == _objectId)
                {
                    _relevantQuests.Add(cntQuest);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 해당 TalkIndex로 대사뭉치를 설정하거나, 다음 대사뭉치로 넘어가도록 한다.
    /// </summary>
    public void SetTalkIndex(int index)
    {
        if (index == -1)
        {
            _talkIndex++;
        }

        else
        {
            _talkIndex = index;
        }

        if (_talkIndex < 0 || _talkIndex >= _relevantTalks.Count)
        {
            _talkIndex = 0;
        }
    }

    /// <summary>
    /// 현재 NPC가 퀘스트를 시작하거나 완료할 수 있는 상황인지 반환한다.
    /// </summary>
    public void CheckQuestIsOn()
    {
        _canStartQuest = false;
        _canFinishQuest = false;
        _canProgressQuest = false;

        for (int i = 0; i < _relevantQuests.Count; i++)
        {
            if (!_relevantQuests[i].isEnd && _relevantQuests[i].npcList[_relevantQuests[i].currentIndex] == _objectId) // 퀘스트가 끝나지 않았으며, 현재 진행도가 해당 NPC와의 상호작용이라면
            {
                if (_relevantQuests[i].canStart) // 퀘스트가 시작 가능함을 알림
                {
                    _canStartQuest = true;
                }

                if (_relevantQuests[i].canEnd) // 퀘스트가 종료 가능함을 알림
                {
                    _canFinishQuest = true;
                }
            }
        }

        foreach(NonLivingEntity entity in _npcList)
        {
            entity.CheckQuestVisibleInfo();
        }
    }
    
    public void RewardQuest()
    {
        Quest quest = GetTargetQuest();

        Player.Instance.ChangeState(PLAYERSTATE.PS_IDLE);
        quest.NextIndex();
        quest.SuccessQuest();
        TalkManager.Instance.CheckQuestIsOn();
        UINaviationManager.Instance.PopToNav("PlayerUI_TalkUIView");
    }

    public void AcceptQuest()
    {
        Quest quest = GetTargetQuest();

        Player.Instance.ChangeState(PLAYERSTATE.PS_IDLE);
        quest.NextIndex(); // 다음 퀘스트 Index로 넘어간다.

        if (quest.canStart)
        {
            quest.StartQuest();
        }

        quest.UpdateQuestCondition(0, 0, 0);
        TalkManager.Instance.CheckQuestIsOn(); // 퀘스트 진행도 Refresh
        UINaviationManager.Instance.PopToNav("PlayerUI_TalkUIView");
    }

    public void DeclineQuest()
    {
        Quest quest = GetTargetQuest();

        Talk talk = TalkManager.Instance.talkDatas[quest.convList[quest.currentIndex]];
        talk.InitConvIndex();

        Player.Instance.ChangeState(PLAYERSTATE.PS_IDLE);

        TalkManager.Instance.CheckQuestIsOn(); // 퀘스트 진행도 Refresh

        UINaviationManager.Instance.PopToNav("PlayerUI_TalkUIView");
    }

    /// <summary>
    /// 대화를 실시한다.
    /// </summary>
    public void Talk()
    {
        bool isQuest = false;
        Talk talk = null;
        Quest quest = null;

        // 퀘스트가 없다면
        if (!canStartQuest && !canFinishQuest)
        {
            talk = relevantTalks[_talkIndex];
            isQuest = false;
        }

        // 퀘스트가 있다면
        else
        {
            quest = GetTargetQuest();
            talk = TalkManager.Instance.talkDatas[quest.convList[quest.currentIndex]];

            isQuest = true;
        }

        // 현재 진행중인게 일반 대화라면
        if (!isQuest)
        {
            if (talk.IsFinished()) // 대화가 끝났으면
            {
                talk.InitConvIndex();
                Player.Instance.ChangeState(PLAYERSTATE.PS_IDLE);

                UINaviationManager.Instance.PopToNav("PlayerUI_TalkUIView");

                SetTalkIndex(-1); // 다음 대화로 넘어가도록 (임시)
            }

            else
            {
                if (talk.convIndex == 0)
                {
                    Player.Instance.ChangeState(PLAYERSTATE.PS_INTERACTING);
                    UINaviationManager.Instance.PushToNav("PlayerUI_TalkUIView"); // 첫 대화창이면 대화창 생성
                }

                TalkManager.Instance.talkUI.Talk(talk, this);
                talk.ToNextConv();
            }
        }

        // 현재 진행중인게 퀘스트 대화라면
        else
        {
            if (talk.IsFinished())
            {
                if(!TalkManager.Instance.talkUI.isAcceptInput) // 퀘스트 수락 / 거절이 아닐 시에는 입력을 통해 자동으로 진행된다.
                {
                    AcceptQuest();
                }
            }

            else
            {
                if (talk.convIndex == 0)
                {
                    Player.Instance.ChangeState(PLAYERSTATE.PS_INTERACTING);
                    UINaviationManager.Instance.PushToNav("PlayerUI_TalkUIView");
                }

                TalkManager.Instance.talkUI.Talk(talk, this);
                talk.ToNextConv();
            }
        }
    }

    /// <summary>
    /// 가장 최근의 진행 가능한 퀘스트를 가져온다. (UI 구현 시 선택하는 것으로 바뀜)
    /// </summary>
    public Quest GetTargetQuest()
    {
        for (int i = 0; i < relevantQuests.Count; i++)
        {
            Quest curQuest = relevantQuests[i];
            if (curQuest.canEnd && !curQuest.isEnd && curQuest.npcList[curQuest.currentIndex] == _objectId)
            {
                return curQuest;
            }
        }

        for (int i=0; i < relevantQuests.Count; i++)
        {
            Quest curQuest = relevantQuests[i];
            if(curQuest.canStart && curQuest.npcList[curQuest.currentIndex] == _objectId)
            {
                return curQuest;
            }
        }

        return null;
    }
}


