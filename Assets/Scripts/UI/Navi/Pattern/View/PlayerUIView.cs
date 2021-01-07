using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIView : View
{
    [SerializeField] private Button _cardTestBtn;
    [SerializeField] private Button _talkTestBtn;
    [SerializeField] private Button _atkBtn;
    [SerializeField] private Button _invincibleBtn;
    [SerializeField] private Button _skillAButton;
    [SerializeField] private Button _skillBButton;
    [SerializeField] private Button _skillCButton;

    private void Start()
    {
        _cardTestBtn.onClick.AddListener(delegate { UINaviationManager.Instance.ToggleCardUIView(); });
        _talkTestBtn.onClick.AddListener(delegate { UINaviationManager.Instance.ToggleTalkView(); });
        _atkBtn.onClick.AddListener(delegate { Player.Instance.CheckInteractObject(); });
        _invincibleBtn.onClick.AddListener(delegate { Player.Instance.EvadeBtnClicked(); });

        _skillAButton.onClick.AddListener(delegate { Player.Instance.SkillABtnClicked(); });
        _skillAButton.onClick.AddListener(delegate { GetComponent<CoolTimeScript>().StartCoolTime(); });
        _skillBButton.onClick.AddListener(delegate { Player.Instance.SkillBBtnClicked(); });
        _skillBButton.onClick.AddListener(delegate { GetComponent<CoolTimeScript>().StartCoolTime(); });
        _skillCButton.onClick.AddListener(delegate { Player.Instance.SkillCBtnClicked(); });
        _skillCButton.onClick.AddListener(delegate { GetComponent<CoolTimeScript>().StartCoolTime(); });
    }

    public override void UIExit()
    {
        base.UIExit();
    }

    public override void UIStart()
    {
        base.UIStart();
    }

    public override void UIUpdate()
    {
        base.UIUpdate();
    }
}
