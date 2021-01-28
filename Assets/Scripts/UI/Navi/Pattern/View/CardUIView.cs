using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 카드 UI
/// </summary>
public class CardUIView : View
{
    // 튜토리얼 관련
    [SerializeField] private GameObject[] _tutorials;
    private int tutorialIdx = 0;

    [SerializeField] private Image _basePanel;
    [SerializeField] private Image _paperImg;
    [SerializeField] private CardUIRoomArea[] _roomAreaImg;
    [SerializeField] private Image _cardTooltipImg;
    [SerializeField] private Image _tooltipImageFrameImg;
    [SerializeField] private Image _tooltipCardImg;
    [SerializeField] private TextMeshProUGUI _tooltipCardNameText;
    [SerializeField] private Image[] _tooltipGradeStarImg;
    [SerializeField] private TextMeshProUGUI _tooltipDescriptionText;
    [SerializeField] private TextMeshProUGUI _tooltipEffectText;
    [SerializeField] private Button _rerollBtn;
    [SerializeField] private Button _watchBtn;
    [SerializeField] private Button _playBtn;
    [SerializeField] private TextMeshProUGUI _cntCoinText;
    [SerializeField] private TextMeshProUGUI _rerollCoinText;

    [SerializeField] private Sprite _level1Sprite;
    [SerializeField] private Sprite _level2Sprite;
    [SerializeField] private Sprite _level3Sprite;

    [SerializeField] private Sprite _starDark;
    [SerializeField] private Sprite _starGold;
    [SerializeField] private Sprite _rerollDark;
    [SerializeField] private Sprite _rerollGold;

    [SerializeField] private GameObject[] _setBar;


    private bool _isRerolling; public bool isRerolling { get { return _isRerolling; } }
    [HideInInspector] public CardUIRoomArea cntPointerArea;
    [HideInInspector] public bool isRerollingAnimationPlaying;

    string iconPath = "Image/TonityEden/Skill Icons Megapack/";

    static int IsCardTutorial;

    private void Start()
    {
        _watchBtn.onClick.AddListener(delegate { ToogleCardViews(); });
        _rerollBtn.onClick.AddListener(delegate { ToogleRerollBtn(); });
        _playBtn.onClick.AddListener(delegate { FinishCardSelect(); });

        _isRerolling = false;
        isRerollingAnimationPlaying = false;

        for (int i = 0; i < _roomAreaImg.Length; i++) _roomAreaImg[i].roomNumber = i;

        if (tutorialButton != null) tutorialButton.onClick.AddListener(delegate { TutorialClick(); });
        tutorialExitButton.onClick.AddListener(delegate { TutorialExit(); });

        IsCardTutorial = PlayerPrefs.GetInt("CardTutorial");
        if (IsCardTutorial == 0)
        {
            tutorialPage.SetActive(true);
            IsCardTutorial++;
            PlayerPrefs.SetInt("CardTutorial", IsCardTutorial);
        }
    }



    ///////////////////// 기본 동작 관련 ///////////////////////

    public override void UIStart()
    {
        SoundManager.Instance.PlayEffect(SoundType.UI, "UI/CardUIStart", 0.9f);

        base.UIStart();

        CardManager cardManager = CardManager.Instance;
        cardManager.SetNewCard(); // 카드 데이터 배치

        for (int i = 0; i < _roomAreaImg.Length; i++)
        {
            Card cntCard = cardManager.dungeonCardData[cardManager.currentFloor, i];
            _roomAreaImg[i].roomNumber = i;
            _roomAreaImg[i].InitCardRoomData(cntCard);
        } // 카드 갱신

        UpdateRerollCoin();
        CalculateRerollValue(); // 리롤 비용 초기화
        BingoCheck(); // 빙고 체크
    }

    public override void UIUpdate()
    {
        base.UIUpdate();
    }

    public override void UIExit()
    {
        base.UIExit();
    }



    ///////////////////// 완료 관련 ///////////////////////

    /// <summary>
    /// 카드 배치 & 선택 완료
    /// </summary>
    public void FinishCardSelect()
    {
        if (!isRerollingAnimationPlaying) // 리롤 애니메이션 재생 중이 아니면
        {
            UINaviationManager.Instance.PopToNav("SubUI_CardUIView"); // Navigation에서 UI를 지운다.
            CardManager.Instance.isAcceptCardData = false;
            SoundManager.Instance.PlayEffect(SoundType.UI, "UI/CardEnd", 0.9f);

            if(CardManager.Instance.cntDungeon == null)
            {
                UILoaderManager.Instance.LoadDungeon();
            } // 던전으로 가는것이면 던전으로 가도록

            else
            {
                CardManager.Instance.cntDungeon.ToNextStage();
            } // 다음 층으로 가는거면 다음 층으로 가도록
        }
    }



    ///////////////////// 카드 시각화 관련 ///////////////////////

    /// <summary>
    /// 각 방의 카드 뷰를 Toggle한다.
    /// </summary>
    public void ToogleCardViews()
    {
        SoundManager.Instance.PlayEffect(SoundType.UI, "UI/ClickMedium01", 1.0f);
        for(int i=0; i<_roomAreaImg.Length; i++) _roomAreaImg[i].ToggleCardView();
    }



    ///////////////////// 툴팁 관련 ///////////////////////

    /// <summary>
    /// 툴팁 정보(UI)를 card로 갱신
    /// </summary>
    public void SetTooltip(Card card)
    {
        switch (card.level) // 테두리
        {
            case 0: _tooltipImageFrameImg.sprite = _level1Sprite; break;
            case 1: _tooltipImageFrameImg.sprite = _level2Sprite; break;
            case 2: _tooltipImageFrameImg.sprite = _level3Sprite; break;
        }

        _tooltipCardImg.sprite = Resources.Load<Sprite>(iconPath + card.cardData.iconImg); // 카드 이미지
        _tooltipCardNameText.text = card.cardData.cardName; // 이름 갱신

        for (int i = 0; i < 3; i++) // 레벨에 따른 별 이미지 변화
        {
            if (card.level >= i)
            {
                _tooltipGradeStarImg[i].sprite = _starGold;
            }
            else
            {
                _tooltipGradeStarImg[i].sprite = _starDark;
            }
        }

        _tooltipDescriptionText.text = card.cardData.description; // 카드 설명
        _tooltipEffectText.text = card.EffectToString(); // 카드 효과 설명
    }



    ///////////////////// 리롤 관련 ///////////////////////

    private IEnumerator PlayRerollSound()
    {
        SoundManager.Instance.PlayEffect(SoundType.UI, "UI/CardSwapStart", 0.9f);
        yield return new WaitForSeconds(2.5f);
        SoundManager.Instance.PlayEffect(SoundType.UI, "UI/CardSwapEnd", 0.9f);
    }

    /// <summary>
    /// 리롤 사용 여부를 Toggle한다.
    /// </summary>
    public void ToogleRerollBtn()
    {
        SoundManager.Instance.PlayEffect(SoundType.UI, "UI/ClickMedium03", 0.9f);

        if (!isRerollingAnimationPlaying) // 리롤 애니메이션 재생중이 아니면
        {
            if (_isRerolling) // 리롤 중이면
            {
                if (StatusManager.Instance.needToCardRerollCoin <= ItemManager.Instance.currentItems.coin) // 재화가 충분하면
                {
                    if(StatusManager.Instance.rerollCount != 0) StartCoroutine(PlayRerollSound());

                    // 리롤을 하고, 버튼을 원래대로 돌린다.
                    ItemManager.Instance.currentItems.coin -= StatusManager.Instance.needToCardRerollCoin;
                    _rerollBtn.GetComponent<Image>().sprite = _rerollDark;

                    for (int i = 0; i < _roomAreaImg.Length; i++)
                    {
                        _roomAreaImg[i].ClearCardData();
                    } // 각 카드의 중복 방지, 재배치를 위한 리롤 카드 배치 정보 초기화 

                    for (int i = 0; i < _roomAreaImg.Length; i++)
                    {
                        _roomAreaImg[i].RerollCardData();
                    } // 카드 리롤

                    CalculateRerollValue();
                    UpdateRerollCoin();
                    // 리롤 정보 갱신

                    _isRerolling = !_isRerolling;
                }
            }

            else // 리롤 중이 아니라면
            {
                _rerollBtn.GetComponent<Image>().sprite = _rerollGold;
                _isRerolling = !_isRerolling;
            } // 리롤 상태로 만듦

        }
    }

    /// <summary>
    /// 리롤 코인 정보(UI) 갱신
    /// </summary>
    public void UpdateRerollCoin()
    {
        _cntCoinText.text = ItemManager.Instance.currentItems.coin.ToString();
    }

    /// <summary>
    /// 리롤 비용을 계산한다.
    /// </summary>
    public void CalculateRerollValue()
    {
        StatusManager.Instance.needToCardRerollCoin = (int)Mathf.Pow(2, StatusManager.Instance.rerollCount - 1) * 10;
        _rerollCoinText.text = StatusManager.Instance.needToCardRerollCoin.ToString();
    }



    ///////////////////// 빙고 관련 ///////////////////////

    /// <summary>
    /// 빙고 정보 (세트 정보)를 계산하고, 관련 이벤트들을 발생시킨다.
    /// </summary>
    public void BingoCheck()
    {
        // 연출 및 세트 관련 데이터 초기화
        for (int i = 0; i < _roomAreaImg.Length; i++)
        {
            _roomAreaImg[i].OnOffBingoEffect(false);
            _roomAreaImg[i].cntCard.addedSetEffectList.Clear();
        }

        // 세트 바 비활성화
        for (int i = 0; i < _setBar.Length; i++)
        {
            _setBar[i].SetActive(false);
        }

        HashSet<int> bingoNums = new HashSet<int>();

        bool isSet = true;
        for (int i = 0; i < 3; i++)
        {
            isSet = true;
            for (int j = 0; j < 3; j++)
            {
                if (!_roomAreaImg[i + j * 3].cntCard.isSet) // 종
                {
                    isSet = false;
                }
            }

            if (isSet)
            {
                bingoNums.Add(i);
                bingoNums.Add(i + 3);
                bingoNums.Add(i + 6);

                AddSetEffect(_roomAreaImg[i].cntCard, _roomAreaImg[i + 3].cntCard, _roomAreaImg[i + 6].cntCard);
                _setBar[i].SetActive(true);
            }

            isSet = true;

            for (int j = 0; j < 3; j++)
            {
                if (!_roomAreaImg[j + i * 3].cntCard.isSet) // 횡
                {
                    isSet = false;
                }
            }

            if (isSet)
            {
                bingoNums.Add(i * 3);
                bingoNums.Add(i * 3 + 1);
                bingoNums.Add(i * 3 + 2);

                AddSetEffect(_roomAreaImg[i * 3].cntCard, _roomAreaImg[i * 3 + 1].cntCard, _roomAreaImg[i * 3 + 2].cntCard);
                _setBar[i + 3].SetActive(true);
            }
        }

        if (_roomAreaImg[0].cntCard.isSet && _roomAreaImg[4].cntCard.isSet && _roomAreaImg[8].cntCard.isSet) // 대각 1
        {
            bingoNums.Add(0);
            bingoNums.Add(4);
            bingoNums.Add(8);

            AddSetEffect(_roomAreaImg[0].cntCard, _roomAreaImg[4].cntCard, _roomAreaImg[8].cntCard);
            _setBar[6].SetActive(true);
        }

        if (_roomAreaImg[2].cntCard.isSet && _roomAreaImg[4].cntCard.isSet && _roomAreaImg[6].cntCard.isSet) // 대각 2
        {
            bingoNums.Add(2);
            bingoNums.Add(4);
            bingoNums.Add(6);

            AddSetEffect(_roomAreaImg[2].cntCard, _roomAreaImg[4].cntCard, _roomAreaImg[6].cntCard);
            _setBar[7].SetActive(true);
        }

        if(isSet)
        {
            SoundManager.Instance.PlayEffect(SoundType.UI, "UI/SpecialText", 1.0f);
        }

        for(int i=0; i<bingoNums.Count; i++) // 세트임을 알리고 효과를 켠다.
        {
            _roomAreaImg[bingoNums.ElementAt(i)].OnOffBingoEffect(true);
        }
    }

    /// <summary>
    /// 세트 효과를 위해 같은 세트끼리는 세트효과를 공유시킨다.
    /// </summary>
    public void AddSetEffect(Card card1, Card card2, Card card3)
    {
        card1.AddNewSetEffect(card2.setEffect);
        card1.AddNewSetEffect(card3.setEffect);
        card2.AddNewSetEffect(card1.setEffect);
        card2.AddNewSetEffect(card3.setEffect);
        card3.AddNewSetEffect(card1.setEffect);
        card3.AddNewSetEffect(card2.setEffect);
    }



    ///////////////////// 튜토리얼 관련 ///////////////////////

    protected override void TutorialClick()
    {
        base.TutorialClick();
        tutorialIdx = 0;
        _tutorials[tutorialIdx].SetActive(true);
    }

    protected override void TutorialExit()
    {
        SoundManager.Instance.PlayEffect(SoundType.UI, "UI/ClickPage", 0.9f);

        tutorialIdx++;
        if (tutorialIdx >= _tutorials.Length)
        {
            for (int i = 0; i < _tutorials.Length; i++)
            {
                _tutorials[i].SetActive(false);
            }
            tutorialPage.SetActive(false);
        }
        else
        {
            _tutorials[tutorialIdx - 1].SetActive(false);
            _tutorials[tutorialIdx].SetActive(true);
        }
    }
}
