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
    public string quitQuestId; // 선택 시 잠금되는 퀘스트
    public string neededQuestsId; // 개방에 필요한 퀘스트
    public string description; // 퀘스트 설명
    public string reward; // 보상 번호 0 : 골드, 1 : 현재 낀 장비의 경험치
    public string rewardId; // 아이템의 경우 id로 해당 아이템을 받을 수 있도록 함.
    public string rewardAmount; // 보상 개수를 스페이스바로 나눠서 나옴
    public string questDescription; // 퀘스트에 관한 설명
    public string condition; // 조건 0 : 조건 없음, 1 : 몬스터 잡기, 2 : 아이템 운반, 3 : 던전 플레이
    public string conditionId; // 조건 0 : 조건 없음, 조건 1 : 대상 몬스터, 조건 2 : 대상 아이템, 3 : 0 - 클리어, 1 - 방문
    public string conditionAmount; // 조건 0 : 조건 없음, 조건 1 : 몬스터 잡는 개수, 조건 2 : 아이템 가지고 오는 개수, 3 : 던전 플레이 횟수
}