using UnityEngine;
using UnityEditor;

public class CardEffect
{
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
        gradeNum[1] = effectData.gradeTwoValue;
        gradeNum[2] = effectData.gradeThreeValue;
    }

    public virtual void StartEffect()
    {
   //     Debug.Log("시작 : " + effectData.effectName);
        CardManager.Instance.activeEffects.Add(this);
    }

    public virtual void UpdateEffect()
    {

    }

    public virtual void EndEffect()
    {
     //   Debug.Log("종료 : " + effectData.effectName);
        CardManager.Instance.activeEffects.Remove(this);
    }

    public string GetDescription(Card card)
    {
        string description = effectData.description;
        description = description.Replace(";", effectData.effectName);
        description = description.Replace(":", gradeNum[card.level - 1].ToString());
        return description;
    }
}