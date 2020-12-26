using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUIView : View
{
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

    private void Start()
    {
    }

    private void OnEnable()
    {
    }

    public override void UIExit()
    {
        base.UIExit();
    }

    public override void UIStart()
    {
        base.UIStart();
        CardManager cardManager = CardManager.Instance;
        cardManager.SetNewCard();

        for (int i=0; i<_roomAreaImg.Length; i++)
        {
            Card cntCard = cardManager.dungeonCardData[cardManager.currentStage, i];
            _roomAreaImg[i].InitCardRoomData(cntCard);
        }
    }

    public override void UIUpdate()
    {
        base.UIUpdate();
    }

    public void SetToolTipOn(Card card)
    {

    }
}
