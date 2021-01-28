////////////////////////////////////////////////////
/*
    File CardManager.cs
    class CardManager

    담당자 : 이신홍
    부 담당자 : 

    카드의 동작, 배치 등을 관리한다.
*/
////////////////////////////////////////////////////

using CSVReader;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardManager : SingletonBase<CardManager>
{
    // CSV 데이터
    private Dictionary<int, CardData> _csvCardData;             // CSV 카드 데이터 목록
    private Dictionary<int, CardEffectData> _csvEffectData;     // CSV 이펙트 데이터 목록

    // 실질적 사용 데이터
    private Dictionary<int, Card> _cardData;                    // 카드 데이터 목록
    private Dictionary<int, CardEffect> _effectData;            // 이펙트 데이터 목록


    public Card[,] dungeonCardData;             // 던전에 배치된 카드 데이터 목록 [Floor, Room Area]

    public DungeonManager cntDungeon;           // 현재 던전 정보 
    public int currentFloor;                    // 현재 Floor
    List<Card> currentCards;                    // 현재 Active된 카드 목록
    public HashSet<CardEffect> activeEffects;   // 현재 Active된 이펙트 목록

    // 예외처리
    public bool isAcceptCardData;               // 카드 UI 반복 입장 시 스테이지의 중복 ++를 막기위함.

    // PROPERTY
    public Dictionary<int, CardData> csvCardData { get { return _csvCardData; } }
    public Dictionary<int, Card> cardData { get { return _cardData; } }
    public Dictionary<int, CardEffectData> csvEffectData { get { return _csvEffectData; } }
    public Dictionary<int, CardEffect> effectData { get { return _effectData; } }



    ////////// 베이스 //////////

    /// <summary>
    /// 카드 매니저 초기화 (데이터를 받고, 정제하며, 초기화한다)
    /// </summary>
    public void InitCardManager()
    {
        isAcceptCardData = false;
        cntDungeon = null;

        currentCards = new List<Card>();
        activeEffects = new HashSet<CardEffect>();
        dungeonCardData = new Card[4, 9];
        _cardData = new Dictionary<int, Card>();
        _effectData = new Dictionary<int, CardEffect>();

        GetEffectData();
        GetCardData();
    }



    ////////// 데이터 //////////

    /// <summary>
    /// CSV로부터 카드 데이터를 받아 정제한다.
    /// </summary>
    private void GetCardData()
    {
        Table table = CSVReader.Reader.ReadCSVToTable("CSVData/CardData");
        _csvCardData = table.TableToDictionary<int, CardData>();

        CardData data = null;
        for (int i=0; i<_csvCardData.Count; i++)
        {
            data = _csvCardData.Values.ElementAt(i);
            _cardData[data.id] = new Card(data);
            _cardData[data.id].ParsingCardData();
        }
    }

    /// <summary>
    /// CSV로부터 효과 데이터를 받아 정제한다.
    /// </summary>
    private void GetEffectData()
    {
        Table table = CSVReader.Reader.ReadCSVToTable("CSVData/CardEffectData");
        _csvEffectData = table.TableToDictionary<int, CardEffectData>();

        CardEffectData data = null;
        for (int i=0; i<_csvEffectData.Count; i++)
        {
            data = _csvEffectData.Values.ElementAt(i);
            _effectData[data.id] = new CardEffect(data);
            _effectData[data.id].ParsingEffectData();
        }
    }

    /// <summary>
    /// 현재 스테이지, 위치의 카드를 반환한다.
    /// </summary>
    public Card GetCardCntStage(int pos)
    {
        return dungeonCardData[currentFloor, pos];
    }



    ////////// 카드 활성화 //////////

    /// <summary>
    /// area의 카드 이펙트들을 실행한다.
    /// </summary>
    public void EnterEffectCards(int area)
    {
        for (int i = 0; i < 4; i++)
        {
            if (dungeonCardData[i, area] != null) dungeonCardData[i, area].CardStart();
        }
    }

    /// <summary>
    /// area의 카드 이펙트들을 업데이트한다.
    /// </summary>
    public void UpdateEffectCards()
    {
        for (int i = 0; i < 4; i++)
        {
            if (dungeonCardData[i, Player.Instance.currentDungeonArea] != null) dungeonCardData[i, Player.Instance.currentDungeonArea].CardUpdate();
        }
    }

    /// <summary>
    /// area의 카드 이펙트들을 종료한다.
    /// </summary>
    public void ExitEffectCards(int area)
    {
        for (int i = 0; i < 4; i++)
        {
            if (dungeonCardData[i, area] != null) dungeonCardData[i, area].CardEnd();
        }
    }



    ////////// 카드 생성 //////////

    /// <summary>
    /// 중복을 방지하여 새로운 카드들을 던전에 배치한다.
    /// </summary>
    public void SetNewCard()
    {
        for (int i = 0; i < 9; i++) dungeonCardData[currentFloor, i] = null;
        for (int i = 0; i < 9; i++) SetNewCardAtPosition(i);
    }

    /// <summary>
    /// 해당 위치에 새로운 카드를 중복을 방지하여 배치한다.
    /// </summary>
    public void SetNewCardAtPosition(int pos)
    {
        while (true) // 카드가 중복되어 뽑히지 않도록 하기 위함
        {
            int cardNum = UnityEngine.Random.Range(0, _cardData.Count);

            Card randomCard = _cardData.Values.ElementAt(cardNum);
            Card card = new Card(randomCard);
            RandomLevelToCard(card);        // 랜덤 레벨

            if (!FindEqualCardInStage(card)) // 중복이 아니면
            {
                dungeonCardData[currentFloor, pos] = card;

                RandomSetToCard(card);          // 랜덤 세트 효과
                break;
            }
        }
    }

    /// <summary>
    /// 카드가 중복되는지 여부를 검사한다.
    /// </summary>
    public bool FindEqualCardInStage(Card card)
    {
        bool isHere = false; // flag

        for (int i = 0; i < 9; i++)
        {
            if (dungeonCardData[currentFloor, i] == null) continue;

            if (dungeonCardData[currentFloor, i].cardData.id == card.cardData.id)
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
        bool useMastery = (MasteryManager.Instance.currentMastery.currentMasteryChoices[0] == -1); // 마스터리 사용 여부
        int levelRandom = Random.Range(0, 100);

        if (levelRandom >= (useMastery ? 75 : 80)) card.level = 2;
        else if (levelRandom >= (useMastery ? 40 : 50)) card.level = 1;
        else card.level = 0;
    }

    /// <summary>
    /// 랜덤으로 카드의 세트효과 여부를 정한다.
    /// </summary>
    public void RandomSetToCard(Card card)
    {
        if (Random.Range(0, 100) < 30) AddSetToCard(card);
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
