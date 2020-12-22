using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CSVReader;

public class CardManager : SingletonBase<CardManager>
{
    private Dictionary<int, CardData> _csvCardData; public Dictionary<int, CardData> csvCardData { get { return _csvCardData; } }
    private Dictionary<int, Card> _cardData; public Dictionary<int, Card> cardData { get { return _cardData; } }

    List<Card> currentCards;

    private void Start()
    {
        currentCards = new List<Card>();
    }

    private void GetCardData()
    {
        Table table = CSVReader.Reader.ReadCSVToTable("CSVData/CardData");
        _csvCardData = table.TableToDictionary<int, CardData>();

        foreach(CardData data in _csvCardData.Values)
        {
            _cardData[data.id] = new Card(data);
            _cardData[data.id].RefineCardData();
        }
    }
}
