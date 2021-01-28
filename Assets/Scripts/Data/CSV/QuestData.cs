////////////////////////////////////////////////////
/*
    File QuestData.cs
    class QuestData
    
    담당자 : 이신홍
    부 담당자 :

    퀘스트의 CSV 데이터를 받는 데이터 전용 클래스.
*/
////////////////////////////////////////////////////

[CSVReader.Data("id")]
public class QuestData
{
    public string id;               // 퀘스트 ID
    public string questName;        // 퀘스트 이름
    public string npcId;            // NPC 순번
    public string convId;           // 대화 순번
    public string afterQuestsId;    // 이후 개방되는 퀘스트
    public string quitQuestId;      // 선택 시 잠금되는 퀘스트
    public string neededQuestsId;   // 개방에 필요한 퀘스트
    public string description;      // 퀘스트 설명. (보상 화면에서 등장)
    public string reward;           // 보상 번호 0 : 재화, 1 : 숙련도 경험치, 2 : 아이템
    public string rewardId;         // 보상 0 : 코인 / 지폐 / 보석, 보상 1 : 검 / 완드 / 대거 / 스태프 / 둔기, 보상 2 : 해당 아이템의 ID
    public string rewardAmount;     // 보상 개수를 스페이스바로 나눠서 나옴
    public string questDescription; // 퀘스트 클리어 조건을 설명한다. (퀘스트 상태 화면에서 등장)
    public string condition;        // 조건 0 : 조건 없음, 1 : 몬스터 잡기, 2 : 아이템 운반, 3 : 던전 플레이
    public string conditionId;      // 조건 0 : 조건 없음, 조건 1 : 대상 몬스터, 조건 2 : 대상 아이템, 3 : 0 - 클리어, 1 - 방문
    public string conditionAmount;  // 조건 0 : 조건 없음, 조건 1 : 몬스터 잡는 개수, 조건 2 : 아이템 가지고 오는 개수, 3 : 던전 플레이 횟수
}