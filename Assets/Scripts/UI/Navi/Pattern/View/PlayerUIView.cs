using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIView : View
{
    string iconPath = "Image/TonityEden/Skill Icons Megapack/";

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
    [SerializeField] private GameObject _buffFrame;
    [SerializeField] private GameObject _buffImgPrefab;

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

    public void SetEffectList()
    {
        int count = 0;
        while (_buffFrame.transform.childCount > 0)
        {
            ObjectPoolManager.Instance.ReturnObject(_buffFrame.transform.GetChild(0).gameObject);
            if (count > 100) break;
        }

        for (int i = 0; i < 4; i++)
        {
            Card cardData = CardManager.Instance.dungeonCardData[i, Player.Instance.currentDungeonArea];

            if (cardData != null)
            {
                GameObject buffImgObj = ObjectPoolManager.Instance.GetObject(_buffImgPrefab);
                buffImgObj.transform.SetParent(_buffFrame.transform);
                buffImgObj.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(iconPath + cardData.cardData.iconImg);
                buffImgObj.GetComponent<RectTransform>().localScale = Vector3.one;
            }
        }
    }
}
