using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Card
{
    public CardData cardData;

    public int level;
    public List<CardEffect> effectList;
    public bool isSetOn;
    public bool isPlaced;

    public Card(CardData data)
    {
        level = 1;
        cardData = data;
        isSetOn = false;

        effectList = new List<CardEffect>();
    }

    public void RefineCardData()
    {
        string[] split = cardData.effectId.Split(' ');
        for (int i = 0; i < split.Length; i++)
        {
            effectList.Add(CardManager.Instance.effectData[int.Parse(split[i])]);
            effectList[effectList.Count - 1].SetEffectParent(this);
        }
    }

    public virtual void CardStart()
    {
        for(int i=0; i<effectList.Count; i++)
        {
            effectList[i].StartEffect();
        }
    }

    public virtual void CardUpdate()
    {
        for (int i = 0; i < effectList.Count; i++)
        {
            effectList[i].UpdateEffect();
        }
    }

    public virtual void CardEnd()
    {
        for (int i = 0; i < effectList.Count; i++)
        {
            effectList[i].EndEffect();
        }
    }
}
