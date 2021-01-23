////////////////////////////////////////////////////
/*
    File CardData.cs
    class CardData
    
    담당자 : 이신홍
    부 담당자 :

    카드 데이터를 CSV로 받아오는 데이터 전용 클래스.
*/
////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CSVReader.Data("id")]
public class CardData : ScriptableObject
{
    public int id;                  // 카드의 ID
    public string cardName;         // 카드의 이름
    public string iconImg;          // 카드 이미지 경로 
    public string effectId;         // 이펙트 ID 목록 (여러개가 들어간다)
    public string description;      // 이 카드에 대한 설명
    public string joke;             // 카드에 관한 농담 (사용되지 않음)
}

