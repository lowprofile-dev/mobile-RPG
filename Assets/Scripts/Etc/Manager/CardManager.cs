using CSVReader;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class CardManager : SingletonBase<CardManager>
{
    private Dictionary<int, CardData> _csvCardData; public Dictionary<int, CardData> csvCardData { get { return _csvCardData; } }
    private Dictionary<int, Card> _cardData; public Dictionary<int, Card> cardData { get { return _cardData; } }
    private Dictionary<int, CardEffectData> _csvEffectData; public Dictionary<int, CardEffectData> csvEffectData { get { return _csvEffectData; } }
    private Dictionary<int, CardEffect> _effectData; public Dictionary<int, CardEffect> effectData { get { return _effectData; } }

    List<Card> currentCards;

    public Card[,] dungeonCardData;
    public int currentStage;

    public void InitCardManager()
    {
        currentCards = new List<Card>();
        dungeonCardData = new Card[4, 9];

        _cardData = new Dictionary<int, Card>();
        _effectData = new Dictionary<int, CardEffect>();

        GetEffectData();
        GetCardData();
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
        for (int i = 0; i < 9; i++)
        {
            while (true)
            {
                int cardNum = UnityEngine.Random.Range(0, _cardData.Count);

                Card card = new Card(_cardData.Values.ElementAt(cardNum));

                if (!card.isPlaced)
                {
                    dungeonCardData[currentStage, i] = card;
                    card.isPlaced = true;

                    RandomLevelToCard(card);
                    RandomSetToCard(card);
                    break;
                }
            }
        }
    }

    public void RandomLevelToCard(Card card)
    {
        int levelRandom = Random.Range(0, 100);

        if (levelRandom >= 80) card.level = 3;
        else if (levelRandom >= 50) card.level = 2;
        else card.level = 1;
    }

    public void RandomSetToCard(Card card)
    {
        if (Random.Range(0, 100) < 30) AddSetToCard(card);
    }

    public void AddSetToCard(Card card)
    {
        while(true)
        {
            CardEffect effect = _effectData.Values.ElementAt(Random.Range(0, _effectData.Count));

            if(effect.effectData.isSet)
            {
                card.AddNewEffect(effect);
                card.isSetOn = false;
                card.isSet = true;
                break;
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
                if (!dungeonCardData[currentStage, i + (j * 3)].isSetOn) // 종
                {
                    isSet = false;
                }
            }

            if (isSet)
            {
                bingoNums.Add(i);
                bingoNums.Add(i+3);
                bingoNums.Add(i+6);
            }

            isSet = true;

            for (int j = 0; j < 3; j++)
            {
                if (!dungeonCardData[currentStage, j + (i * 3)].isSetOn) // 횡
                {
                    isSet = false;
                }
            }

            if (isSet)
            {
                bingoNums.Add(i*3);
                bingoNums.Add(i*3+1);
                bingoNums.Add(i*3+2);
            }
        }

        if (dungeonCardData[currentStage, 0].isSetOn || dungeonCardData[currentStage, 4].isSetOn || dungeonCardData[currentStage, 8].isSetOn)
        {
            bingoNums.Add(0);
            bingoNums.Add(4);
            bingoNums.Add(8);
        }

        if (dungeonCardData[currentStage, 2].isSetOn || dungeonCardData[currentStage, 4].isSetOn || dungeonCardData[currentStage, 7].isSetOn)
        {
            bingoNums.Add(2);
            bingoNums.Add(4);
            bingoNums.Add(7);
        }

        foreach (int nums in bingoNums)
        {
            //  Effect 맵에 추가
        }
    }
}
