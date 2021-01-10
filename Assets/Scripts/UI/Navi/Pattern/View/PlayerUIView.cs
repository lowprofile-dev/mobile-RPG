using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIView : View
{
    [SerializeField] private Button _cardTestBtn;
    [SerializeField] private Button _shopTestBtn;
    [SerializeField] private Button _atkBtn;
    [SerializeField] private Button _invincibleBtn;
    [SerializeField] private Button _skillAButton;
    [SerializeField] private Button _skillBButton;
    [SerializeField] private Button _skillCButton;
    [SerializeField] private TextMeshProUGUI _masteryText;
    [SerializeField] private TextMeshProUGUI _weaponText;
    [SerializeField] private Image _hpSlider;
    [SerializeField] private Image _steminaSlider;

    private void Start()
    {
        _cardTestBtn.onClick.AddListener(delegate { UINaviationManager.Instance.ToggleCardUIView(); });
        _shopTestBtn.onClick.AddListener(delegate { UINaviationManager.Instance.ToggleShopView(); });
        _atkBtn.onClick.AddListener(delegate { Player.Instance.CheckInteractObject(); });
        _invincibleBtn.onClick.AddListener(delegate { Player.Instance.EvadeBtnClicked(); });

        _skillAButton.onClick.AddListener(delegate { Player.Instance.SkillABtnClicked(); });
        _skillAButton.onClick.AddListener(delegate { _skillAButton.GetComponent<CoolTimeScript>().StartCoolTime(); });
        _skillBButton.onClick.AddListener(delegate { Player.Instance.SkillBBtnClicked(); });
        _skillBButton.onClick.AddListener(delegate { _skillBButton.GetComponent<CoolTimeScript>().StartCoolTime(); });
        _skillCButton.onClick.AddListener(delegate { Player.Instance.SkillCBtnClicked(); });
        _skillCButton.onClick.AddListener(delegate { _skillCButton.GetComponent<CoolTimeScript>().StartCoolTime(); });
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

        SetMyStatusText();
        SetHpStemina();
    }

    /// <summary>
    /// 마스터리와 숙련도 레벨을 설정한다.
    /// </summary>
    public void SetMyStatusText()
    {
        _masteryText.text = MasteryManager.Instance.currentMastery.currentMasteryLevel.ToString();
        _weaponText.text = WeaponManager.Instance.GetWeapon().masteryLevel.ToString();
    }

    public void SetHpStemina()
    {
        _hpSlider.fillAmount = StatusManager.Instance.GetCurrentHpPercent();
        _steminaSlider.fillAmount = StatusManager.Instance.GetCurrentSteminaPercent();
    }
}
