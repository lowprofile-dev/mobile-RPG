using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CSV를 통해 읽어온 대화 데이터베이스
/// </summary>
[CSVReader.Data("convId")]
public class TalkData : ScriptableObject
{
    public string convId; // 전체 대화 ID
    public int npcId; // 대화의 주체가 되는 NPC ID
    public int convNum; // 해당 NPC의 ~번째 대화 ID인지 표현
    public string convType; // 해당 대화가 퀘스트인지 노말인지
    public string convData; // 대화 데이터
    public string parentQuest; // 부모 퀘스트
}