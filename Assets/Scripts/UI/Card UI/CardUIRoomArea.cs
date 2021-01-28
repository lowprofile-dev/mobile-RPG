////////////////////////////////////////////////////
/*
    File CardUIRoomArea.cs
    class CardUIRoomArea
    
    담당자 : 이신홍
    부 담당자 : 

    방 구역에 대한 카드 UI
*/
////////////////////////////////////////////////////

using Coffee.UIEffects;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUIRoomArea : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IEndDragHandler
{
    [SerializeField] private CardUIView _parentView;        
    [SerializeField] private Image _roomAreaImg;            
    [SerializeField] private Image[] _roomFramesImg;
    [SerializeField] private Image[] _roomIconsImg;
    [SerializeField] private Button _cardBtn;
    [SerializeField] private TextMeshProUGUI _cardNameText;
    [SerializeField] private Image _cardFrameImg;
    [SerializeField] private Image _cardIconImg;
    [SerializeField] private Image _isSetImg;
    [SerializeField] private Image _rerollICONImg;

    [SerializeField] private Sprite _level1Sprite;
    [SerializeField] private Sprite _level2Sprite;
    [SerializeField] private Sprite _level3Sprite;

    private Vector3 _initCardPos;
    public int roomNumber;
    public Card cntCard;

    string iconPath = "Image/TonityEden/Skill Icons Megapack/";

    public void Start()
    {
        _initCardPos = _cardBtn.transform.position;
        _cardBtn.onClick.AddListener(delegate { _parentView.SetTooltip(cntCard); });
        _cardBtn.onClick.AddListener(delegate { RerollCheck(); });
    }



    ///////////////////// 데이터 관련 ///////////////////////

    /// <summary>
    /// 카드를 받아 해당 카드로 방 데이터를 초기화한다.
    /// </summary>
    public void InitCardRoomData(Card card)
    {
        cntCard = card;

        _isSetImg.gameObject.SetActive(card.isSet);
        _rerollICONImg.gameObject.SetActive(false);
        _roomAreaImg.GetComponent<UIShadow>().enabled = false;
        _roomAreaImg.GetComponent<Animator>().enabled = false;
        _cardBtn.GetComponent<UIDissolve>().Stop(true);
        _cardBtn.GetComponent<UIDissolve>().effectFactor = 0;

        AlphaAllRoomCardData();
        for (int i = 0; i < CardManager.Instance.currentFloor; i++) SetRoomCardData(CardManager.Instance.dungeonCardData[i, roomNumber], i);

        SetCurrentCardData(card);
    }

    /// <summary>
    /// 해당 방의 카드 데이터를 갱신한다.
    /// </summary>
    public void ReloadCardData()
    {
        _isSetImg.gameObject.SetActive(cntCard.isSet);
        SetCurrentCardData(cntCard);
    }

    private void AlphaAllRoomCardData()
    {
        for (int i = 0; i < 4; i++)
        {
            _roomIconsImg[i].color = new Color(_roomIconsImg[i].color.r, _roomIconsImg[i].color.g, _roomIconsImg[i].color.b, 0);
            _roomFramesImg[i].color = new Color(_roomFramesImg[i].color.r, _roomFramesImg[i].color.g, _roomFramesImg[i].color.b, 0);
        }
    }

    /// <summary>
    /// 카드 방 데이터를 업데이트한다.
    /// </summary>
    private void SetRoomCardData(Card card, int floor)
    {
        if(card != null)
        {
            _roomIconsImg[floor].color = new Color(_roomIconsImg[floor].color.r, _roomIconsImg[floor].color.g, _roomIconsImg[floor].color.b, 1);
            _roomIconsImg[floor].sprite = Resources.Load<Sprite>(iconPath + card.cardData.iconImg);

            _roomFramesImg[floor].color = new Color(_roomFramesImg[floor].color.r, _roomFramesImg[floor].color.g, _roomFramesImg[floor].color.b, 1);
            switch (card.level)
            {
                case 1: _roomFramesImg[floor].sprite = _level1Sprite; break;
                case 2: _roomFramesImg[floor].sprite = _level2Sprite; break;
                case 3: _roomFramesImg[floor].sprite = _level3Sprite; break;
            } // 카드 액자 (Level에 따라)
        }

        else
        {
            _roomIconsImg[floor].color = new Color(_roomIconsImg[floor].color.r, _roomIconsImg[floor].color.g, _roomIconsImg[floor].color.b, 0);
            _roomFramesImg[floor].color = new Color(_roomFramesImg[floor].color.r, _roomFramesImg[floor].color.g, _roomFramesImg[floor].color.b, 0);
        }
    }

    /// <summary>
    /// 현재 카드 데이터를 설정한다.
    /// </summary>
    private void SetCurrentCardData(Card card)
    {
        _cardIconImg.sprite = Resources.Load<Sprite>(iconPath + card.cardData.iconImg); // 카드 그림
        switch (card.level)
        {
            case 0: _cardFrameImg.sprite = _level1Sprite; break;
            case 1: _cardFrameImg.sprite = _level2Sprite; break;
            case 2: _cardFrameImg.sprite = _level3Sprite; break;
        } // 카드 액자 (Level에 따라)

        _cardNameText.text = card.cardData.cardName;
    }



    ///////////////////// 카드 시각화 관련 ///////////////////////

    /// <summary>
    /// 해당 방 카드 정보의 시각화를 OnOff한다.
    /// </summary>
    public void ToggleCardView()
    {
        _cardBtn.gameObject.SetActive(!_cardBtn.gameObject.activeSelf);
    }



    ///////////////////// 빙고 관련 ///////////////////////

    /// <summary>
    /// 빙고와 관련된 이펙트를 on/off한다.
    /// </summary>
    public void OnOffBingoEffect(bool isOn)
    {
        _roomAreaImg.GetComponent<UIShadow>().enabled = isOn;
        _roomAreaImg.GetComponent<Animator>().enabled = isOn;
    }




    ///////////////////// 리롤 관련 ///////////////////////

    /// <summary>
    /// 리롤을 위해 해당 위치 카드 데이터를 없앤다.
    /// </summary>
    public void ClearCardData()
    {
        if (_rerollICONImg.gameObject.activeSelf)
        {
            CardManager.Instance.dungeonCardData[CardManager.Instance.currentFloor, roomNumber] = null;
        }
    }

    /// <summary>
    /// 리롤 버튼을 눌렀을 시 해당 방의 리롤 정보를 갱신한다.
    /// </summary>
    public void RerollCheck()
    {
        if (_parentView.isRerolling) // 리롤중 버튼이면
        {
            if (_rerollICONImg.gameObject.activeSelf) // 리롤중이면
            {
                StatusManager.Instance.rerollCount--;
                _rerollICONImg.gameObject.SetActive(false);
            }

            else // 리롤중이 아니면
            {
                StatusManager.Instance.rerollCount++;
                _rerollICONImg.gameObject.SetActive(true);
            }

            _parentView.CalculateRerollValue(); // 카드 리롤 비용을 갱신한다.
        }
    }

    /// <summary>
    /// 리롤 버튼 누를 시에 일어나는 애니메이션
    /// </summary>
    public IEnumerator RerollAnimation()
    {
        _parentView.isRerollingAnimationPlaying = true;

        UIDissolve dissolveEffect = _cardBtn.GetComponent<UIDissolve>();
        dissolveEffect.effectPlayer.duration = 1.5f;
        dissolveEffect.effectPlayer.loop = false;
        dissolveEffect.Reverse = false;
        dissolveEffect.Play();
        _cardNameText.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.49f); // 사라지는 부분

        _cardBtn.gameObject.SetActive(false);

        yield return new WaitForSeconds(1.0f); // 대기

        _cardBtn.gameObject.SetActive(true);

        CardManager.Instance.SetNewCardAtPosition(roomNumber); // 해당 위치를 새로운 카드로 설정해준다.
        InitCardRoomData(CardManager.Instance.GetCardCntStage(roomNumber)); // 해당 방의 카드 데이터를 새롭게 초기화한다.
        ReloadCardData(); // 카드 데이터를 갱신한다.
        dissolveEffect.Reverse = true;
        dissolveEffect.effectFactor = 1;
        dissolveEffect.color = new Color(103, 127, 203);
        dissolveEffect.Play();

        yield return new WaitForSeconds(1.49f); // 드러나는 부분

        dissolveEffect.effectFactor = 0f;
        dissolveEffect.Stop(false);
        _cardNameText.gameObject.SetActive(true);
        _parentView.BingoCheck(); // 빙고 체크
        _parentView.isRerollingAnimationPlaying = false;
    }

    /// <summary>
    /// 해당 방의 카드를 리롤한다.
    /// </summary>
    public void RerollCardData()
    {
        if (_rerollICONImg.gameObject.activeSelf) // 리롤하는 대상이라면
        {
            StatusManager.Instance.rerollCount--; // 리롤 개수 설정
            _rerollICONImg.gameObject.SetActive(false);

            StartCoroutine(RerollAnimation());
        }
    }




    ///////////////////// 드래그 관련 ///////////////////////

    public void OnDrag(PointerEventData eventData)
    {
        _cardBtn.transform.position = eventData.position; // 마우스 포인터에 카드 데이터가 따라오도록 한다.
        _cardBtn.GetComponent<Image>().raycastTarget = false;
        _cardBtn.targetGraphic.raycastTarget = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _parentView.cntPointerArea = this; // 현재 방이 마우스 포인터에 올라가있다 알림.
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _parentView.cntPointerArea = null; // 마우스 포인터가 현재 방에서 빠져나갔다 알림.
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_parentView.cntPointerArea != null) // 마우스 포인터가 방에 존재하면
        {
            Card tempCard = null;

            if (_parentView.cntPointerArea != this)
            {
                tempCard = cntCard;
                cntCard = _parentView.cntPointerArea.cntCard;
                _parentView.cntPointerArea.cntCard = tempCard;
                // 카드들의 위치를 바꾼다.

                ReloadCardData();
                _parentView.cntPointerArea.ReloadCardData();
                // 카드들의 데이터를 갱신
            }

            _parentView.cntPointerArea._cardBtn.transform.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            _cardBtn.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            // 카드들의 위치를 0로

            _parentView.BingoCheck(); // 빙고 체크
            SoundManager.Instance.PlayEffect(SoundType.UI, "UI/CardReplace", 0.9f);
        }

        else _cardBtn.transform.GetComponent<RectTransform>().anchoredPosition = Vector3.zero; // 마우스 포인터가 방에 존재하지 않으면 (위치를 되돌린다.)
        _cardBtn.GetComponent<Image>().raycastTarget = true;
        _cardBtn.targetGraphic.raycastTarget = true;
    }
}
