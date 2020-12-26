using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Coffee.UIEffects;
using TMPro;

public class CardUIRoomArea : MonoBehaviour
{
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

    string iconPath = "Image/Icons/150 Fantasy Skill Icons/";

    public void InitCardRoomData(Card card)
    {
        _isSetImg.gameObject.SetActive(card.isSet);
        _rerollICONImg.gameObject.SetActive(false);
        _roomAreaImg.GetComponent<UIShadow>().enabled = false;
        _roomAreaImg.GetComponent<Animator>().enabled = false;
        _cardBtn.GetComponent<UIDissolve>().Stop(true);

        SetCurrentCardData(card);
    }

    public void SetCurrentCardData(Card card)
    {
        _cardIconImg.sprite = Resources.Load<Sprite>(iconPath + card.cardData.iconImg);
        switch(card.level)
        {
            case 1: _cardFrameImg.sprite = _level1Sprite; break;
            case 2: _cardFrameImg.sprite = _level2Sprite; break;
            case 3: _cardFrameImg.sprite = _level3Sprite; break;
        }
        
        _cardNameText.text = card.cardData.cardName;
    }

    
}
