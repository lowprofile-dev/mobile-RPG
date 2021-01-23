////////////////////////////////////////////////////
/*
    File CardEffectData.cs
    class CardEffectData
    
    담당자 : 이신홍
    부 담당자 :

    카드 이펙트 데이터를 CSV로 받아오는 데이터 전용 클래스.
*/
////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

[CSVReader.Data("id")]
public class CardEffectData : ScriptableObject
{
    public int id;                  // 이펙트 ID
    public bool isSet;              // 이펙트가 세트효과인지 여부
    public float gradeOneValue;     // 레벨 1일때의 효과 수치
    public float gradeTwoValue;     // 레벨 2일때의 효과 수치
    public float gradeThreeValue;   // 레벨 3일때의 효과 수치
    public string description;      // 해당 이펙트에 관한 설명
    public string effectName;       // 이 이펙트 수치의 이름
}
