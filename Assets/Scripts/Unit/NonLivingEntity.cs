////////////////////////////////////////////////////
/*
    File NonLivingEntity.cs
    class NonLivingEntity
    
    Object 및 NPC와 같은 개체들.
    
    담당자 : 이신홍
    부 담당자 : 
*/
////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.UI;

public class NonLivingEntity : Unit
{
    [SerializeField] private bool _isNpc;   // 대상 오브젝트가 NPC인지 여부

    private TalkManager _talkManager;       // 토크매니저 캐싱
    private TalkChecker _myTalkChecker;     // 해당 NPC의 토크체커 캐싱

    public Image  minimapIconSprite;        // 미니맵에 표시도리 퀘스트 관련 아이콘
    public GameObject canEndSquare;         // 퀘스트 진행가능 여부를 표시해주는 캐릭터 위의 사각형
    
    protected void OnDisable()
    {
        if(_myTalkChecker != null) _myTalkChecker._npcList.Remove(this);
    }

    protected virtual void Start()
    {
        _talkManager = TalkManager.Instance;
        _myTalkChecker = _talkManager.talkCheckers[_id];
        _myTalkChecker._npcList.Add(this);
        CheckQuestVisibleInfo();
        minimapIconSprite.transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    /// <summary>
    /// 가까운 오브젝트에 대해 상호작용을 실시한다.
    /// </summary>
    public void Interaction()
    {
        _myTalkChecker.Talk();
    }
    
    /// <summary>
    /// 현재 퀘스트 정보에 따라 미니맵 아이콘과 캐릭터 위 사각형 여부를 바꿔준다.
    /// </summary>
    public void CheckQuestVisibleInfo()
    {
        if(_myTalkChecker != null && minimapIconSprite != null)
        {
            if (_myTalkChecker.canFinishQuest) minimapIconSprite.sprite = GlobalDefine.Instance.questExitMinimapIcon;
            else if (_myTalkChecker.canStartQuest) minimapIconSprite.sprite = GlobalDefine.Instance.questStartMinimapIcon;
            else minimapIconSprite.sprite = GlobalDefine.Instance.npcBaseMinimapIcon;
        }

        if (_myTalkChecker != null)
        {
            if (_myTalkChecker.canFinishQuest || _myTalkChecker.canStartQuest) canEndSquare.SetActive(true);
            else canEndSquare.SetActive(false);
        }
    }
}
