using UnityEngine;

/// <summary>
/// Object 및 NPC에 사용되는 클래스
/// </summary>
public class NonLivingEntity : Unit
{
    [SerializeField] private bool _isNpc;

    private TalkManager _talkManager;   // 토크매니저 캐싱
    private TalkChecker _myTalkChecker; // 해당 NPC의 토크체커 캐싱

    public GameObject canQuestSquare;
    public GameObject canEndSquare;

    protected virtual void Start()
    {
        _talkManager = TalkManager.Instance;
        _myTalkChecker = _talkManager.talkCheckers[_id];
    }

    protected virtual void Update()
    {
        if (_myTalkChecker.canFinishQuest)
        {
            canEndSquare.SetActive(true);
            canQuestSquare.SetActive(false);
        }

        else
        {
            canEndSquare.SetActive(false);

            if (_myTalkChecker.canStartQuest || _myTalkChecker.canProcressQuest)
            {
                canQuestSquare.SetActive(true);
            }

            else
            {
                canQuestSquare.SetActive(false);
            }

        }
    }

    /// <summary>
    /// 가까운 오브젝트에 대해 상호작용을 실시한다.
    /// </summary>
    public void Interaction()
    {
        _myTalkChecker.Talk();
    }
}
