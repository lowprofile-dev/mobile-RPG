using UnityEngine;
using UnityEditor;

public class CardEffect
{
    public Card parentCard;
    public CardEffectData effectData;
    public float[] gradeNum;
    
    public CardEffect(CardEffectData data)
    {
        effectData = data;
    }

    public void RefineEffectData()
    {
        gradeNum = new float[3];
        gradeNum[0] = effectData.gradeOneValue;
        gradeNum[0] = effectData.gradeTwoValue;
        gradeNum[0] = effectData.gradeThreeValue;
    }

    public void SetEffectParent(Card card)
    {
        parentCard = card;
    }

    public virtual void StartEffect()
    {

    }

    public virtual void UpdateEffect()
    {

    }

    public virtual void EndEffect()
    {

    }
}