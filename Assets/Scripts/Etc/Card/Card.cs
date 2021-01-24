////////////////////////////////////////////////////
/*
    File Card.cs
    class Card
    
    담당자 : 이신홍
    부 담당자 :

    데이터를 파싱하고, 실질적인 카드의 작동을 관리한다.
*/
////////////////////////////////////////////////////

using System.Collections.Generic;
using UnityEngine;

public class Card
{
    // 기본 데이터
    public CardData cardData;                       // 해당 카드의 CSV 데이터

    // 파싱 데이터
    public List<CardEffect> effectList;             // 이펙트 리스트

    // 세트 관련
    public CardEffect setEffect;                    // 이 카드의 세트이펙트
    public List<CardEffect> addedSetEffectList;     // 세트 효과로 추가된 다른 카드의 세트 이펙트 리스트 
    public bool isSet;                              // 세트 카드인지 여부
    public bool isSetOn;                            // 세트 효과가 발동되었는지 여부

    // 기타 데이터
    public int level;                               // 현재 카드의 레벨



    ////////// 베이스 //////////

    /// <summary>
    /// 카드 데이터 기반 생성자
    /// </summary>
    public Card(CardData data)
    {
        cardData = data;

        level = 0;
        isSet = false;
        isSetOn = false;
        effectList = new List<CardEffect>();
        addedSetEffectList = new List<CardEffect>();
    }

    /// <summary>
    /// 복사 생성자
    /// </summary>
    public Card(Card copyCard)
    {
        cardData = copyCard.cardData;

        level = 0;
        isSet = false;
        isSetOn = false;
        effectList = new List<CardEffect>(copyCard.effectList);
        addedSetEffectList = new List<CardEffect>();
    }



    ////////// 데이터 //////////

    /// <summary>
    /// CardData Class 파싱 작업
    /// </summary>
    public void ParsingCardData()
    {
        string[] split = cardData.effectId.Split(' ');
        for (int i = 0; i < split.Length; i++)
        {
            effectList.Add(CardManager.Instance.effectData[int.Parse(split[i])]);
        }
    }




    ////////// 카드 관리 //////////

    /// <summary>
    /// 카드에 새 이펙트 추가
    /// </summary>
    public void AddNewEffect(CardEffect effect)
    {
        effect.level = level;
        effectList.Add(effect);
    }

    /// <summary>
    /// 카드에 세트 이펙트 추가 (빙고가 이뤄졌을때 다른 카드의 세트 이펙트들을 추가해줌)
    /// </summary>
    public void AddNewSetEffect(CardEffect effect)
    {
        effect.level = level;
        addedSetEffectList.Add(effect);
    }

    public virtual void CardStart()
    {
        for (int i = 0; i < effectList.Count; i++) effectList[i].StartEffect();

        // 세트효과
        if (isSetOn)
        {
            setEffect.StartEffect();
            for (int i = 0; i < addedSetEffectList.Count; i++) addedSetEffectList[i].StartEffect();
        }
    }

    public virtual void CardUpdate()
    {
        for (int i = 0; i < effectList.Count; i++) effectList[i].UpdateEffect();

        // 세트효과
        if (isSetOn)
        {
            setEffect.UpdateEffect();
            for (int i = 0; i < addedSetEffectList.Count; i++) addedSetEffectList[i].UpdateEffect();
        }
    }

    public virtual void CardEnd()
    {
        for (int i = 0; i < effectList.Count; i++) effectList[i].EndEffect();

        // 세트효과
        if(isSetOn)
        {
            setEffect.EndEffect();
            for (int i = 0; i < addedSetEffectList.Count; i++) addedSetEffectList[i].EndEffect();
        }
    }



    ////////// UI //////////

    /// <summary>
    /// 이펙트 목록 설명 리스트 반환
    /// </summary>
    public string EffectToString()
    {
        string effectString = "";

        for (int i = 0; i < effectList.Count; i++)
        {
            if (effectList[i].effectData.isSet) effectString += "세트효과 : "; // 세트 효과 존재하면 세트 효과 추가
            effectString += effectList[i].GetDescription();
            if (effectList.Count - 1 != i) effectString += "\n";
        }

        return effectString;
    }
}
