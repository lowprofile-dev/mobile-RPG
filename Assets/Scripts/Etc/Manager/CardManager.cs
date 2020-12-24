using CSVReader;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CardManager : SingletonBase<CardManager>
{
    private Dictionary<int, CardData> _csvCardData; public Dictionary<int, CardData> csvCardData { get { return _csvCardData; } }
    private Dictionary<int, Card> _cardData; public Dictionary<int, Card> cardData { get { return _cardData; } }
    private Dictionary<int, CardEffectData> _csvEffectData; public Dictionary<int, CardEffectData> csvEffectData { get { return _csvEffectData; } }
    private Dictionary<int, CardEffect> _effectData; public Dictionary<int, CardEffect> effectData { get { return _effectData; } }

    List<Card> currentCards;

    public int[,] dungeonCardData;
    public int currentStage;

    private void Start()
    {
        currentCards = new List<Card>();
        dungeonCardData = new int[4, 9];

        _cardData = new Dictionary<int, Card>();
        _effectData = new Dictionary<int, CardEffect>();

        GetEffectData();
        GetCardData();

        Debug.Log(_cardData.Count);
    }

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

    public void SetNewCard()
    {
        Dictionary<int, Card>.ValueCollection.Enumerator cardEnum;
        for (int i=0; i<9; i++)
        {
            while(true)
            {
                int cardNum = UnityEngine.Random.Range(0, _cardData.Count);

                Card card;
                cardEnum = _cardData.Values.GetEnumerator();
                for (int j = 1; j < cardNum; j++) cardEnum.MoveNext();
                card = cardEnum.Current;

                if (!card.isPlaced)
                {
                    dungeonCardData[currentStage, i] = card.cardData.id;
                    card.isPlaced = true;
                    break;
                }
            }
        }
    }

    public void CheckBingo()
    {
        HashSet<int> bingoNums = new HashSet<int>();

        bool isSet = true;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (!_cardData[dungeonCardData[currentStage, i + (j * 3)]].isSetOn) // 종
                {
                    isSet = false;
                }
            }

            if (isSet)
            {
                bingoNums.Add(dungeonCardData[currentStage, i]);
                bingoNums.Add(dungeonCardData[currentStage, i + 3]);
                bingoNums.Add(dungeonCardData[currentStage, i + 6]);
            }

            isSet = true;

            for (int j = 0; j < 3; j++)
            {
                if (!_cardData[dungeonCardData[currentStage, j + (i * 3)]].isSetOn) // 횡
                {
                    isSet = false;
                }
            }

            if (isSet)
            {
                bingoNums.Add(dungeonCardData[currentStage, i * 3]);
                bingoNums.Add(dungeonCardData[currentStage, i * 3 + 1]);
                bingoNums.Add(dungeonCardData[currentStage, i * 3 + 2]);
            }
        }

        if (_cardData[dungeonCardData[currentStage, 0]].isSetOn || _cardData[dungeonCardData[currentStage, 4]].isSetOn || _cardData[dungeonCardData[currentStage, 8]].isSetOn)
        {
            bingoNums.Add(dungeonCardData[currentStage, 0]);
            bingoNums.Add(dungeonCardData[currentStage, 4]);
            bingoNums.Add(dungeonCardData[currentStage, 8]);
        }

        if (_cardData[dungeonCardData[currentStage, 2]].isSetOn || _cardData[dungeonCardData[currentStage, 4]].isSetOn || _cardData[dungeonCardData[currentStage, 7]].isSetOn)
        {
            bingoNums.Add(dungeonCardData[currentStage, 2]);
            bingoNums.Add(dungeonCardData[currentStage, 4]);
            bingoNums.Add(dungeonCardData[currentStage, 7]);
        }

        foreach(int nums in bingoNums)
        {
            //  Effect 맵에 추가
        }
    }
}
