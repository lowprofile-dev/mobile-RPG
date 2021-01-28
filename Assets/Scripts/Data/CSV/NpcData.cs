////////////////////////////////////////////////////
/*
    File NpcData.cs
    class NpcData

    담당자 : 이신홍
    부 담당자 :
    
    CSV로 불러온 NPC Data 정보
*/
////////////////////////////////////////////////////

[CSVReader.Data("id")]
[System.Serializable]
public class NpcData
{
    public int id;              // NPC ID
    public string name;         // NPC 이름
    public string prefabPath;   // NPC 프리팹 경로
}