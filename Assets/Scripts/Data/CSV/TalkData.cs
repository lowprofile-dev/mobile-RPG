////////////////////////////////////////////////////
/*
    File TalkData.cs
    class TalkData
    
    담당자 : 이신홍
    부 담당자 :

    대화 목록 CSV 데이터를 받는 데이터 전용 클래스.
*/
////////////////////////////////////////////////////
    
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CSV를 통해 읽어온 대화 데이터베이스
/// </summary>
[CSVReader.Data("convId")]
[System.Serializable]
public class TalkData : ScriptableObject
{
    public string convId;       // 전체 대화 ID
    public int npcId;           // 대화의 주체가 되는 NPC ID
    public int convNum;         // 해당 NPC의 ~번째 대화 ID인지 표현
    public string convType;     // 해당 대화가 퀘스트인지 노말인지
    public string convData;     // 대화 데이터
    public string parentQuest;  // 부모 퀘스트
}