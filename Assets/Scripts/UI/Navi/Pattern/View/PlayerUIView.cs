using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerUIView : View
{
    string iconPath = "Image/TonityEden/Skill Icons Megapack/";
    string masteryIconPath = "Image/MasteryIcon/";

    [SerializeField] private Sprite _atkSprite;
    [SerializeField] private Sprite _talkSprite;

    [SerializeField] private Button _cardTestBtn;
    [SerializeField] private Button _shopTestBtn;
    [SerializeField] private Button _shopTestBtn1;
    [SerializeField] private Button _atkBtn;
    [SerializeField] private Image _atkImg;
    [SerializeField] private Button _invincibleBtn;
    [SerializeField] private Button _skillAButton; 
    [SerializeField] private Image _skillAImg;
    [SerializeField] private Button _skillBButton; 
    [SerializeField] private Image _skillBImg;
    [SerializeField] private Button _skillCButton; 
    [SerializeField] private Image _skillCImg;
    [SerializeField] private TextMeshProUGUI _masteryText;
    [SerializeField] private TextMeshProUGUI _weaponText;
    [SerializeField] private Button _masteryButton;
    [SerializeField] private Button _weaponButton;
    [SerializeField] private Image _hpSlider;
    [SerializeField] private Image _steminaSlider;
    [SerializeField] private GameObject _buffFrame;
    [SerializeField] private GameObject _buffImgPrefab;
    [SerializeField] private Button _optionButton;
    [SerializeField] private QuestDropdown _questDropdown; 
    [SerializeField] private Sprite[] _swordSkiilsImg;
    [SerializeField] private Sprite[] _staffSkillsImg;
    [SerializeField] private Sprite[] _daggerSkillImg;
    [SerializeField] private Sprite[] _wandSkillImg;
    [SerializeField] private Sprite[] _bluntSkillImg;
    [SerializeField] private Button _inventoryButton;
    [SerializeField] private UIBuffInfo _buffInfo;
    private WeaponMasteryView weaponView;

    // property
    public Button invincibleBtn { get { return _invincibleBtn; } }
    public Button skillAButton { get { return _skillAButton; } }
    public Button skillBButton { get { return _skillBButton; } }
    public Button skillCButton { get { return _skillCButton; } }
    public Button inventoryButton { get { return _inventoryButton; } }
    public QuestDropdown questDropdown { get { return _questDropdown; } }
    public UIBuffInfo buffInfo { get { return _buffInfo; } }

    static int IsPlayerTutorial;
    private void Start()
    {
        _cardTestBtn.onClick.AddListener(delegate { UINaviationManager.Instance.ToggleCardUIView(); });
        _shopTestBtn.onClick.AddListener(delegate { UINaviationManager.Instance.ToggleShopView(); });
        _shopTestBtn1.onClick.AddListener(delegate { UINaviationManager.Instance.TogglePurchaseShopView(); });
        _atkBtn.onClick.AddListener(delegate { Player.Instance.CheckInteractObject(); });
        _invincibleBtn.onClick.AddListener(delegate { Player.Instance.EvadeBtnClicked(); });
        _optionButton.onClick.AddListener(delegate { AddOptionView(); });

        _skillAButton.onClick.AddListener(delegate { Player.Instance.SkillABtnClicked(); });
        _skillAButton.onClick.AddListener(delegate { _skillAButton.GetComponent<CoolTimeScript>().StartCoolTime(); });
        _skillBButton.onClick.AddListener(delegate { Player.Instance.SkillBBtnClicked(); });
        _skillBButton.onClick.AddListener(delegate { _skillBButton.GetComponent<CoolTimeScript>().StartCoolTime(); });
        _skillCButton.onClick.AddListener(delegate { Player.Instance.SkillCBtnClicked(); });
        _skillCButton.onClick.AddListener(delegate { _skillCButton.GetComponent<CoolTimeScript>().StartCoolTime(); });
        _masteryButton.onClick.AddListener(delegate { UINaviationManager.Instance.ToggleMasteryView(); });
        _weaponButton.onClick.AddListener(delegate { UINaviationManager.Instance.ToggleWeaponView(); });

        if (tutorialButton != null) tutorialButton.onClick.AddListener(delegate { TutorialClick(); });
        tutorialExitButton.onClick.AddListener(delegate { TutorialExit(); });

        IsPlayerTutorial = PlayerPrefs.GetInt("PlayerTutorial");
        if (IsPlayerTutorial == 0)
        {
            tutorialPage.SetActive(true);
            IsPlayerTutorial++;
            PlayerPrefs.SetInt("PlayerTutorial", IsPlayerTutorial);
        }
    }

    public override void UIExit()
    {
        base.UIExit();
    }

    public override void UIStart()
    {
        base.UIStart();
        SetEffectList();
    }

    public override void UIUpdate()
    {
        base.UIUpdate();

        SetMyStatusText();
        SetHpStemina();
        SetTalkOrAttackSprite();
        ActivateShopBtn();
    }

    private void ActivateShopBtn()
    {
        if (SceneManager.GetSceneByName("VillageScene").isLoaded == false)
        {
            _shopTestBtn.gameObject.SetActive(false);
            _shopTestBtn1.gameObject.SetActive(false);
        }
        else if (SceneManager.GetSceneByName("VillageScene").isLoaded == true)
        {
            _shopTestBtn.gameObject.SetActive(true);
            _shopTestBtn1.gameObject.SetActive(true);
        }
    }

    public void AddOptionView()
    {
        UINaviationManager.Instance.PushToNav("SubUI_OptionView");
    }

    public void SetWeaponSkillIcon()
    {
        _skillAImg.type = Image.Type.Filled;
        _skillAImg.fillMethod = Image.FillMethod.Radial360;
        _skillAImg.fillClockwise = true;
        _skillBImg.type = Image.Type.Filled;
        _skillBImg.fillMethod = Image.FillMethod.Radial360;
        _skillBImg.fillClockwise = true;
        _skillCImg.type = Image.Type.Filled;
        _skillCImg.fillMethod = Image.FillMethod.Radial360;
        _skillCImg.fillClockwise = true;

        switch (WeaponManager.Instance.GetWeaponName())
        {
            case "SWORD":
                _skillAImg.sprite = _swordSkiilsImg[0];
                _skillBImg.sprite = _swordSkiilsImg[1];
                _skillCImg.sprite = _swordSkiilsImg[2];
                break;
            case "STAFF":
                _skillAImg.sprite = _staffSkillsImg[0];
                _skillBImg.sprite = _staffSkillsImg[1];
                _skillCImg.sprite = _staffSkillsImg[2];
                break;
            case "DAGGER":
                _skillAImg.sprite = _daggerSkillImg[0];
                _skillBImg.sprite = _daggerSkillImg[1];
                _skillCImg.sprite = _daggerSkillImg[2];
                break;

            case "BLUNT":
                _skillAImg.sprite = _bluntSkillImg[0];
                _skillBImg.sprite = _bluntSkillImg[1];
                _skillCImg.sprite = _bluntSkillImg[2];
                break;

            case "WAND":
                _skillAImg.sprite = _wandSkillImg[0];
                _skillBImg.sprite = _wandSkillImg[1];
                _skillCImg.sprite = _wandSkillImg[2];
                break;
        }
    }

    /// <summary>
    /// 대화 혹은 공격으로 스프라이트를 적용한다.
    /// </summary>
    public void SetTalkOrAttackSprite()
    {
        //Debug.Log((Player.Instance != null) + " " + Player.Instance.CheckThereisObject());
        if (Player.Instance != null && Player.Instance.CheckThereisObject()) _atkImg.sprite = _talkSprite;
        else _atkImg.sprite = _atkSprite;
    }

    /// <summary>
    /// 마스터리와 숙련도 레벨을 설정한다.
    /// </summary>
    public void SetMyStatusText()
    {
        if (WeaponManager.Instance.GetWeapon() != null)
        {
            _masteryText.text = MasteryManager.Instance.currentMastery.currentMasteryLevel.ToString();
            _weaponText.text = WeaponManager.Instance.GetWeapon().masteryLevel.ToString();
        }
        
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

        if (UILoaderManager.Instance.IsSceneDungeon())
        {
            for (int i = 0; i < 4; i++)
            {
                Card cardData = CardManager.Instance.dungeonCardData[i, Player.Instance.currentDungeonArea - 1];

                if (cardData != null)
                {
                    GameObject buffImgObj = ObjectPoolManager.Instance.GetObject(_buffImgPrefab);
                    buffImgObj.transform.SetParent(_buffFrame.transform);
                    Sprite sprite = Resources.Load<Sprite>(iconPath + cardData.cardData.iconImg);
                    buffImgObj.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
                    buffImgObj.GetComponent<RectTransform>().localScale = Vector3.one;
                    buffImgObj.transform.GetChild(0).GetComponent<BuffFrame>().SetData(sprite, cardData.EffectToString(), cardData.cardData.cardName);

                }
            }
        }

        for (int i = 0; i < 10; i++)
        {
            if (MasteryManager.Instance.currentMastery.currentMasteryChoices[i] != 0)
            {
                int currentChoice = MasteryManager.Instance.currentMastery.currentMasteryChoices[i];
                GameObject buffImgObj = ObjectPoolManager.Instance.GetObject(_buffImgPrefab);
                buffImgObj.transform.SetParent(_buffFrame.transform);
                string path = masteryIconPath + "MasteryIcon" + i.ToString() + (currentChoice + 1).ToString();
                Sprite sprite = Resources.Load<Sprite>(path);
                buffImgObj.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
                buffImgObj.GetComponent<RectTransform>().localScale = Vector3.one;

                PlayerMasteryData data = MasteryManager.Instance.masteryDictionary[(i * 2) + (currentChoice == -1 ? 0 : 1) + 1];
                buffImgObj.transform.GetChild(0).GetComponent<BuffFrame>().SetData(sprite, data.masteryDescription, data.masteryName);
            }
        }
    }

}
