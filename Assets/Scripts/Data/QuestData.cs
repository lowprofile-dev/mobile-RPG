using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CSV를 통해 읽어온 퀘스트 데이터베이스
/// </summary>
[CSVReader.Data("id")]
public class QuestData 
{
    public string id; // 퀘스트 ID
    public string questName; // 퀘스트 이름
    public string npcId; // NPC 순번
    public string convId; // 대화 순번
    public string afterQuestsId; // 이후 개방되는 퀘스트
    public string neededQuestsId; // 개방에 필요한 퀘스트
    public string description; // 퀘스트 설명
}