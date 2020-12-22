using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Card
{
    public CardData cardData;

    public int level;
    public List<float> floatInitValue;
    public List<float> floatAddValue;
    public List<string> effectName;

    public Card(CardData data)
    {
        level = 1;
        floatInitValue = new List<float>();
        floatAddValue = new List<float>();
        effectName = new List<string>();
        cardData = data;
    }

    public void RefineCardData()
    {
        string[] split = cardData.floatInitValue.Split(' ');
        for (int i = 0; i < split.Length; i++)
        {
            floatInitValue.Add(float.Parse(split[i]));
        }

        split = cardData.floatAddValue.Split(' ');
        for (int i = 0; i < split.Length; i++)
        {
            floatAddValue.Add(float.Parse(split[i]));
        }

        split = cardData.effectname.Split(' ');
        for (int i = 0; i < split.Length; i++)
        {
            effectName.Add(split[i]);
        }
    }

    public virtual void CardStart()
    {

    }

    public virtual void CardUpdate()
    {

    }

    public virtual void CardEnd()
    {

    }
}
