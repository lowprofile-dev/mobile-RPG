using CSVReader;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardManager : SingletonBase<CardManager>
{
    private Dictionary<int, CardData> _csvCardData; public Dictionary<int, CardData> csvCardData { get { return _csvCardData; } }
    private Dictionary<int, Card> _cardData; public Dictionary<int, Card> cardData { get { return _cardData; } }
    private Dictionary<int, CardEffectData> _csvEffectData; public Dictionary<int, CardEffectData> csvEffectData { get { return _csvEffectData; } }
    private Dictionary<int, CardEffect> _effectData; public Dictionary<int, CardEffect> effectData { get { return _effectData; } }

    public DungeonManager _cntDungeon; 

    List<Card> currentCards;

    public Card[,] dungeonCardData;
    public int currentStage;

    public HashSet<CardEffect> activeEffects;
    public bool isAcceptCardData; // 카드매니저 스테이지의 중복 ++를 막기위함.

    /// <summary>
    /// 카드 매니저 초기화 (데이터를 받고, 정제하며, 초기화한다)
    /// </summary>
    public void InitCardManager()
    {
        isAcceptCardData = false;
        _cntDungeon = null;
        currentCards = new List<Card>();
        dungeonCardData = new Card[4, 9];

        _cardData = new Dictionary<int, Card>();
        _effectData = new Dictionary<int, CardEffect>();
        activeEffects = new HashSet<CardEffect>();

        GetEffectData();
        GetCardData();
    }

    /// <summary>
    /// 현재 스테이지, 위치의 카드를 반환한다.
    /// </summary>
    public Card GetCardCntStage(int pos)
    {
        return dungeonCardData[currentStage, pos];
    }
    
    /// <summary>
    /// area의 카드 이펙트들을 실행한다.
    /// </summary>
    public void EnterEffectCards(int area)
    {
        for(int i=0; i<4; i++)
        {
            if (dungeonCardData[i, area] != null) dungeonCardData[i, area].CardStart();
        }
    }

    public void UpdateEffectCards()
    {
        for (int i = 0; i<4; i++)
        {
            if (dungeonCardData[i, Player.Instance.currentDungeonArea] != null) dungeonCardData[i, Player.Instance.currentDungeonArea].CardUpdate();
        }
    }

    /// <summary>
    /// area의 카드 이펙트들을 종료한다.
    /// </summary>
    public void ExitEffectCards(int area)
    {
        for(int i=0; i<4; i++)
        {
            if(dungeonCardData[i, area] != null) dungeonCardData[i, area].CardEnd();
        }
    }

    /// <summary>
    /// CSV로부터 카드 데이터를 받아 정제한다.
    /// </summary>
    private void GetCardData()
    {
        Table table = CSVReader.Reader.ReadCSVToTable("CSVData/CardData");
        _csvCardData = table.TableToDictionary<int, CardData>();

        foreach (CardData data in _csvCardData.Values)
        {
            _cardData[data.id] = new Card(data);
            _cardData[data.id].RefineCardData();
        }
    }

    /// <summary>
    /// CSV로부터 효과 데이터를 받아 정제한다.
    /// </summary>
    private void GetEffectData()
    {
        Table table = CSVReader.Reader.ReadCSVToTable("CSVData/CardEffectData");
        _csvEffectData = table.TableToDictionary<int, CardEffectData>();

        foreach (CardEffectData data in _csvEffectData.Values)
        {
            _effectData[data.id] = new CardEffect(data);
            _effectData[data.id].RefineEffectData();
        }
    }
    
    /// <summary>
    /// 중복을 방지하여 새로운 카드들을 던전에 배치한다.
    /// </summary>
    public void SetNewCard()
    {
        for (int i = 0; i < 9; i++) dungeonCardData[currentStage, i] = null;

        for (int i = 0; i < 9; i++)
        {
            while (true)
            {
                int cardNum = UnityEngine.Random.Range(0, _cardData.Count);

                Card randomCard = _cardData.Values.ElementAt(cardNum);
                Card card = new Card(randomCard);

                if (!FindEqualCardInStage(card))
                {
                    dungeonCardData[currentStage, i] = card;

                    RandomLevelToCard(card);
                    RandomSetToCard(card);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 해당 위치에 새로운 카드를 중복을 방지하여 배치한다.
    /// </summary>
    public void SetNewCard(int pos)
    {
        while (true)
        {
            int cardNum = UnityEngine.Random.Range(0, _cardData.Count);

            Card randomCard = _cardData.Values.ElementAt(cardNum);
            Card card = new Card(randomCard);

            if (!FindEqualCardInStage(card))
            {
                dungeonCardData[currentStage, pos] = card;

                RandomLevelToCard(card);
                RandomSetToCard(card);
                break;
            }
        }
    }

    /// <summary>
    /// 카드가 중복되는지 여부를 검사한다.
    /// </summary>
    public bool FindEqualCardInStage(Card card)
    {
        bool isHere = false;
        for (int i = 0; i < 9; i++)
        {
            if (dungeonCardData[currentStage, i] == null)
            {
                continue;
            }

            if (dungeonCardData[currentStage, i].cardData.id == card.cardData.id)
            {
                isHere = true;
                break;
            }
        }

        return isHere;
    }

    /// <summary>
    /// 랜덤으로 카드 레벨을 정한다.
    /// </summary>
    public void RandomLevelToCard(Card card)
    {
        int levelRandom = Random.Range(0, 100);
        //카드 리롤 확률 증가
        if(MasteryManager.Instance.currentMastery.currentMasteryChoices[0] == -1)
        {
            if (levelRandom >= 75)
            {
                card.level = 3;
            }
            else if (levelRandom >= 40)
            {
                card.level = 2;
            }
            else
            {
                card.level = 1;
            }
        }
        else
        {
            if (levelRandom >= 80)
            {
                card.level = 3;
            }
            else if (levelRandom >= 50)
            {
                card.level = 2;
            }
            else
            {
                card.level = 1;
            }
        }
    }

    /// <summary>
    /// 랜덤으로 카드의 세트효과 여부를 정한다.
    /// </summary>
    public void RandomSetToCard(Card card)
    {
        if (Random.Range(0, 100) < 30)
        {
            AddSetToCard(card);
        }
    }

    /// <summary>
    /// 랜덤으로 세트효과가 있는 카드에 세트효과를 부여한다.
    /// </summary>
    public void AddSetToCard(Card card)
    {
        while (true)
        {
            CardEffect effect = _effectData.Values.ElementAt(Random.Range(0, _effectData.Count));

            if (effect.effectData.isSet)
            {
                card.AddNewEffect(effect);
                card.isSetOn = false;
                card.isSet = true;
                card.setEffect = effect;
                break;
            }
        }
    }
}
