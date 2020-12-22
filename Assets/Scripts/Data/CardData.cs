using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CSVReader.Data("id")]
public class CardData : ScriptableObject
{
    public int id;
    public string cardName;   
    public string grade;
    public string floatInitValue;
    public string floatAddValue;
    public string effectname;
}

public enum CARDGRADE
{
    NORMAL, RARE, EPIC, LEGENDARY
}
