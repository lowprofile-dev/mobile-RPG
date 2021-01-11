using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TalkUIView : View
{
    [SerializeField] private TextMeshProUGUI _speakerNameText;
    [SerializeField] private TextMeshProUGUI _talkText;

    [SerializeField] private Button _acceptBtn;
    [SerializeField] private Button _declineBtn;
    [SerializeField] private Image _nextTalkImg;

    [SerializeField] private GameObject _npcCamGameObject;
    [SerializeField] private GameObject _npcFaceCam;

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

    void Start()
    {
        _acceptBtn.onClick.AddListener(delegate { AcceptQuest(); });
        _declineBtn.onClick.AddListener(delegate { DeclineQuest(); });
    }

    void Update()
    {

    }

    public void DoNext()
    {

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
        _cntTalkChecker = tc;

        if (target.talkData.convType == "QUEST" && target.HasNext() && tc.GetTargetQuest().canStart)
        {
            _acceptBtn.gameObject.SetActive(true);
            _declineBtn.gameObject.SetActive(true);
            _nextTalkImg.gameObject.SetActive(false);
            _isAcceptInput = true;
        }

        else
        {
            _acceptBtn.gameObject.SetActive(false);
            _declineBtn.gameObject.SetActive(false);
            _nextTalkImg.gameObject.SetActive(true);
            _isAcceptInput = false;
        }

        NpcData npcData = TalkManager.Instance._csvNpcData[int.Parse(target.speaker[target.convIndex])];

        _talkText.text = target.texts[target.convIndex];
        _speakerNameText.text = npcData.name;

        if (_npcCamGameObject.GetComponent<Unit>() == null || _npcCamGameObject.GetComponent<Unit>().id != npcData.id)
        {
            DestroyImmediate(_npcCamGameObject);
            _npcCamGameObject = npcData.id == 998 ? Instantiate(Player.Instance.playerAvater, new Vector3(1050, 1050, 1050), Quaternion.Euler(-10, 170, 0)) : ResourceManager.Instance.Instantiate("Prefab/NPC/" + npcData.prefabPath, new Vector3(1050, 1050, 1050), Quaternion.Euler(-10, 170, 0));
            _npcFaceCam.GetComponent<FaceCam>().SetTarget(_npcCamGameObject);
        }
    }
}
