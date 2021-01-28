using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TalkUIView : View
{
    [SerializeField] private TextMeshProUGUI _speakerNameText;
    [SerializeField] private TextMeshProUGUI _talkText;

    [SerializeField] private Button _acceptBtn;
    [SerializeField] private Button _declineBtn;
    [SerializeField] private Image _nextTalkImg;
    [SerializeField] private Image _rewardImg;
    [SerializeField] private TextMeshProUGUI _questTitle;
    [SerializeField] private TextMeshProUGUI _questDescript;

    [SerializeField] private RewardList _rewardList;

    [SerializeField] private GameObject _npcCamGameObject;
    [SerializeField] private GameObject _npcFaceCam;

    [SerializeField] private Button _acceptRewardButton;

    private TalkChecker _cntTalkChecker;
    private bool _isAcceptInput; public bool isAcceptInput { get { return _isAcceptInput; } }

    public override void UIStart()
    {
        base.UIStart();
        TalkManager.Instance.talkUI = this;
    }

    public override void UIUpdate()
    {
        base.UIUpdate();
    }

    public override void UIExit()
    {
        base.UIExit();
    }

    private void Start()
    {
        _acceptBtn.onClick.AddListener(delegate { AcceptQuest(); });
        _declineBtn.onClick.AddListener(delegate { DeclineQuest(); });
        _acceptRewardButton.onClick.AddListener(delegate { AcceptRewardButton(); });
    }
    
    public void AcceptRewardButton()
    {
        _cntTalkChecker.FinishQuest();
    }

    public void AcceptQuest()
    {
        _cntTalkChecker.AcceptQuest();
    }

    public void DeclineQuest()
    {
        _cntTalkChecker.DeclineQuest();
    }

    public void Talk(Talk target, TalkChecker tc)
    {
        _talkText.DOKill();
        _cntTalkChecker = tc;

        if (target.talkData.convType == "QUEST" && target.HasNext() && tc.GetTargetQuest().canStart)
        {
            _acceptBtn.gameObject.SetActive(true);
            _declineBtn.gameObject.SetActive(true);
            _nextTalkImg.gameObject.SetActive(false);
            _rewardImg.gameObject.SetActive(false);
            _isAcceptInput = true;
        }

        else if (target.talkData.convType == "QUEST" && target.HasNext() && tc.GetTargetQuest().canEnd)
        {
            Quest quest = tc.GetTargetQuest();
            _acceptBtn.gameObject.SetActive(false);
            _declineBtn.gameObject.SetActive(false);
            _nextTalkImg.gameObject.SetActive(true);
            _rewardImg.gameObject.SetActive(true);
            _rewardList.SetRewards(quest);
            _questTitle.text = quest.questName;
            _questDescript.text = quest.description;
            _isAcceptInput = true;
        }

        else
        {
            _acceptBtn.gameObject.SetActive(false);
            _declineBtn.gameObject.SetActive(false);
            _nextTalkImg.gameObject.SetActive(true);
            _rewardImg.gameObject.SetActive(false);
            _isAcceptInput = false;
        }

        NpcData npcData = TalkManager.Instance._csvNpcData[int.Parse(target.speaker[target.convIndex])];

        _talkText.text = target.texts[target.convIndex];
        DOTween.To(x => _talkText.maxVisibleCharacters = (int)x, 0f, _talkText.text.Length, _talkText.text.Length * 0.015f).SetEase(Ease.Linear);

        _speakerNameText.text = npcData.name;

        FindNpcCams();
        
        if (_npcCamGameObject.GetComponent<Unit>() == null || _npcCamGameObject.GetComponent<Unit>().id != npcData.id)
        {
            DestroyImmediate(_npcCamGameObject);
            _npcCamGameObject = npcData.id == 998 ? Instantiate(Player.Instance.playerAvater, new Vector3(1050, 1050, 1050), Quaternion.Euler(-10, 170, 0)) : ResourceManager.Instance.Instantiate("Prefab/NPC/" + npcData.prefabPath, new Vector3(1050, 1050, 1050), Quaternion.Euler(-10, 170, 0));
            _npcCamGameObject.tag = "NpcCamGameObject";
            _npcFaceCam.GetComponent<FaceCam>().SetTarget(_npcCamGameObject);
        }

        SoundManager.Instance.PlayEffect(SoundType.UI, "UI/ClickLightHigh" + UnityEngine.Random.Range(1, 3), 1.0f);
    }

    public void FindNpcCams()
    {
        if (_npcCamGameObject == null)
        {
            _npcCamGameObject = GameObject.FindGameObjectWithTag("NpcCamGameObject");
            if (_npcCamGameObject == null)
            {
                _npcCamGameObject = new GameObject("npcCamGameObject");
                _npcCamGameObject.tag = "NpcCamGameObject";
            }
        }
        if (_npcFaceCam == null) _npcFaceCam = GameObject.FindGameObjectWithTag("NpcFaceCam");
    }
}
