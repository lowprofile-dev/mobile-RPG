//////////////////////////////////////////////
/*
    File TalkChecker.cs
    class TalkChecker
    
    담당자 : 이신홍
    부담당자 :

    NPC ID별로 배치되어 NPC와 퀘스트 / 대화를 연동시키는 클래스
*/
////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class TalkChecker
{
    private int _objectId;                      // 이 토크체커가 담당하는 ID

    private List<Quest> _relevantQuests;        // 관계된 퀘스트 목록
    private List<Talk> _relevantTalks;          // 관계된 대사 목록
    private bool _canStartQuest;                // 현재 퀘스트 시작이 가능한지
    private bool _canFinishQuest;               // 현재 퀘스트 종료가 가능한지
    private bool _canProgressQuest;             // 현재 퀘스트 진행중인지
    private int _talkIndex;                     // relevantTalk에서 몇번째 대화인지          

    public HashSet<NonLivingEntity> _npcList;   // npc 목록을 중복 없이 보여준다.


    // property
    public List<Quest> relevantQuests { get { return _relevantQuests; } }
    public List<Talk> relevantTalks { get { return _relevantTalks; } }
    public bool canStartQuest { get { return _canStartQuest; } }
    public bool canFinishQuest { get { return _canFinishQuest; } }
    public bool canProcressQuest { get { return _canProgressQuest; } }
    public int talkIndex { get { return _talkIndex; } }



    //////////// 베이스 /////////////

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
        Talk cntTalk;
        TalkManager manager = TalkManager.Instance;

        for(int i=0; i<manager.talkDatas.Values.Count; i++)
        {
            cntTalk = manager.talkDatas.Values.ElementAt(i);
            if (cntTalk.talkData.npcId == _objectId && cntTalk.talkData.convType.Equals("NORMAL"))
            {
                _relevantTalks.Add(cntTalk); // 일반 대사이고 현 NPC이면
            }
        }
    }

    /// <summary>
    /// 퀘스트 전체를 돌며 이 NPC와 관련된 퀘스트들을 등록시킨다.
    /// </summary>
    private void AddRelevantQuests()
    {
        Quest cntQuest;
        for(int i=0; i<TalkManager.Instance.questDatas.Values.Count; i++)
        {
            cntQuest = TalkManager.Instance.questDatas.Values.ElementAt(i);
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



    //////////// 진행 /////////////
    
    /// <summary>
    /// 해당 TalkIndex로 대사뭉치를 설정하거나, 다음 대사뭉치로 넘어가도록 한다.
    /// </summary>
    public void SetTalkIndex(int index)
    {
        if (index == -1) _talkIndex++; // -1이면 다음 대사뭉치로 간다.
        else _talkIndex = index;

        if (_talkIndex < 0 || _talkIndex >= _relevantTalks.Count) _talkIndex = 0; // 끝까지 갔으면 처음으로 돌아감
    }
    
    /// <summary>
    /// 퀘스트의 완료
    /// </summary>
    public void FinishQuest()
    {
        Quest quest = GetTargetQuest();

        quest.NextIndex(); // 다음 퀘스트로 넘어감
        quest.SuccessQuest(); // 퀘스트 완료
        TalkManager.Instance.CheckQuestIsOn(); // 퀘스트 정보 갱신

        QuitTalkUIVIew();
    }

    /// <summary>
    /// 퀘스트의 수락
    /// </summary>
    public void AcceptQuest()
    {
        Quest quest = GetTargetQuest();

        quest.NextIndex(); // 다음 퀘스트 Index로 넘어간다.

        if (quest.canStart) quest.StartQuest();
        quest.UpdateQuestCondition(0, 0, 0);
        TalkManager.Instance.CheckQuestIsOn(); // 퀘스트 진행도 Refresh

        QuitTalkUIVIew();
    }

    /// <summary>
    /// 퀘스트의 거절
    /// </summary>
    public void DeclineQuest()
    {
        Quest quest = GetTargetQuest();

        Talk talk = TalkManager.Instance.talkDatas[quest.convList[quest.currentIndex]];
        talk.InitConvIndex();

        TalkManager.Instance.CheckQuestIsOn(); // 퀘스트 진행도 Refresh

        QuitTalkUIVIew();
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
                QuitTalkUIVIew();
                SetTalkIndex(-1); // 다음 대화로 넘어가도록 (임시)
            }

            else
            {
                if (talk.convIndex == 0) StartTalkUIView();
                TalkManager.Instance.talkUI.Talk(talk, this);
                talk.ToNextConv();
            }
        }

        // 현재 진행중인게 퀘스트 대화라면
        else
        {
            if (talk.IsFinished()) // 마지막 대화에서 Reward 혹은 퀘스트 수락 / 거절이 이루어짐
            {
                if(!TalkManager.Instance.talkUI.isAcceptInput) AcceptQuest(); 
            }

            else // 평소에는 누르면 다음으로 넘어감
            {
                if (talk.convIndex == 0) StartTalkUIView();
                TalkManager.Instance.talkUI.Talk(talk, this);
                talk.ToNextConv();
            }
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
                    _canStartQuest = true;

                if (_relevantQuests[i].canEnd) // 퀘스트가 종료 가능함을 알림
                    _canFinishQuest = true;
            }
        }

        for (int i = 0; i < _npcList.Count; i++) _npcList.ElementAt(i).CheckQuestVisibleInfo();
    }

    /// <summary>
    /// 가장 최근의 진행 가능한 퀘스트를 가져온다.
    /// </summary>
    public Quest GetTargetQuest()
    {
        for (int i = 0; i < relevantQuests.Count; i++) // 1. 클리어 가능한 것
        {
            Quest curQuest = relevantQuests[i];
            if (curQuest.canEnd && !curQuest.isEnd && curQuest.npcList[curQuest.currentIndex] == _objectId)
            {
                return curQuest;
            }
        }

        for (int i=0; i < relevantQuests.Count; i++) // 2. 시작 가능한 것
        {
            Quest curQuest = relevantQuests[i];
            if(curQuest.canStart && curQuest.npcList[curQuest.currentIndex] == _objectId)
            {
                return curQuest;
            }
        }

        return null;
    }


    /// <summary>
    /// UI창 끄기
    /// </summary>
    private void QuitTalkUIVIew()
    {
        Player.Instance.ChangeState(PLAYERSTATE.PS_IDLE);
        UINaviationManager.Instance.PopToNav("PlayerUI_TalkUIView");
    }

    /// <summary>
    /// UI창 켜기
    /// </summary>
    private void StartTalkUIView()
    {
        Player.Instance.ChangeState(PLAYERSTATE.PS_INTERACTING);
        UINaviationManager.Instance.PushToNav("PlayerUI_TalkUIView");
    }
}


