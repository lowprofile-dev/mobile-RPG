using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Card
{
    public CardData cardData;

    public int level;
    public List<CardEffect> effectList;
    public List<CardEffect> addedSetEffectList;
    public CardEffect setEffect;

    public bool isSet;
    public bool isSetOn;

    public Card copyedCard;

    public Card(Card copyCard)
    {
        copyedCard = copyCard;
        cardData = copyCard.cardData;
        level = 1;
        isSet = false;
        isSetOn = false;

        effectList = new List<CardEffect>(copyCard.effectList);

        addedSetEffectList = new List<CardEffect>();
    }

    public Card(CardData data)
    {
        level = 1;
        cardData = data;
        isSet = false;
        isSetOn = false;

        effectList = new List<CardEffect>();
        addedSetEffectList = new List<CardEffect>();
    }

    public string EffectToString()
    {
        string effectString = "";

        for(int i=0; i<effectList.Count; i++)
        {
            if(effectList[i].effectData.isSet)
            {
                effectString += "세트효과 : ";
            }

            effectString += effectList[i].GetDescription(this);

            if (effectList.Count - 1 != i) effectString += "\n";
        }

        return effectString;
    }

    public void AddNewEffect(CardEffect effect)
    {
        effect.level = level;
        effectList.Add(effect);
    }

    public void AddNewSetEffect(CardEffect effect)
    {
        effect.level = level;
        addedSetEffectList.Add(effect);
    }

    public void RefineCardData()
    {
        string[] split = cardData.effectId.Split(' ');
        for (int i = 0; i < split.Length; i++)
        {
            effectList.Add(CardManager.Instance.effectData[int.Parse(split[i])]);
        }
    }

    public virtual void CardStart()
    {
        for(int i=0; i<effectList.Count; i++)
        {
            effectList[i].StartEffect();
        }

        for(int i=0; i<addedSetEffectList.Count; i++)
        {
            addedSetEffectList[i].StartEffect();
        }
    }

    public virtual void CardUpdate()
    {
        for (int i = 0; i < effectList.Count; i++)
        {
            effectList[i].UpdateEffect();
        }

        for(int i=0; i<addedSetEffectList.Count; i++)
        {
            addedSetEffectList[i].UpdateEffect();
        }
    }

    public virtual void CardEnd()
    {
        for (int i = 0; i < effectList.Count; i++)
        {
            effectList[i].EndEffect();
        }

        for(int i=0; i<addedSetEffectList.Count; i++)
        {
            addedSetEffectList[i].EndEffect();
        }
    }
}
