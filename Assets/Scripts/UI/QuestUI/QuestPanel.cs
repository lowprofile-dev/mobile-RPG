////////////////////////////////////////////////////
/*
    File QuestPanel.cs
    class QuestPanel : 활성화된 퀘스트의 정보를 서술한 패널

    담당자 : 이신홍
    부 담당자 : 
*/
////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using TMPro;

public class QuestPanel : MonoBehaviour
{
    public Quest parentQuest = null;

    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI descript;    
    
    /// <summary>
    /// 퀘스트 패널의 데이터를 설정한다.
    /// </summary>
    public void SetData(Quest quest)
    {
        parentQuest = quest;
        
        title.text = quest.questName;
        descript.richText = true;
        descript.text = quest.clearDescription;

        for(int i=0; i<quest.conditionList.Count; i++)
        {
            AddConditionData(quest, i);
        }
    }

    /// <summary>
    /// 조건 데이터를 넣어준다.
    /// </summary>
    private void AddConditionData(Quest quest, int index)
    {
        string typeName = "";

        int curCondition = quest.conditionList[index];
        int curConditionId = quest.conditionIdList[index];
        int curConditionAmount = quest.curConditionAmountList[index];
        int conditionAmount = quest.conditionAmountList[index];

        switch (curCondition)
        {
            case 0:
                break;
            case 1:
                typeName += "몬스터 처치 (" + curConditionAmount + " / " + conditionAmount + ")"; break;
            case 2:
                // 아이템은 구현되지 않음.
                break;
            case 3:
                switch (curConditionId)
                {
                    case 0:
                        typeName += "던전 탐사 (" + curConditionAmount + " / " + conditionAmount + ")"; break;
                    case 1:
                        typeName += "던전 클리어 (" + curConditionAmount + " / " + conditionAmount + ")"; break;
                }
                break;
        }

        if (curConditionAmount < conditionAmount) descript.text += '\n' + UIManager.Instance.AddFontData(typeName, "red", 13);
        else descript.text += '\n' + UIManager.Instance.AddFontData(typeName, "lightblue", 13);
    }

    public void DestroyPanel()
    {
        DestroyImmediate(gameObject);
    }
}
