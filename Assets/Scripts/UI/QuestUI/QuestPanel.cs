using UnityEngine;
using System.Collections;
using TMPro;

public class QuestPanel : MonoBehaviour
{
    public Quest parentQuest = null;

    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI descript;    
    
    public void SetData(Quest quest)
    {
        parentQuest = quest;
        
        title.text = quest.questData.questName;
        descript.richText = true;
        descript.text = quest.questData.questDescription;

        for(int i=0; i<quest.conditionList.Count; i++)
        {
            string typeName = "";

            int curCondition = quest.conditionList[i];
            int curConditionId = quest.conditionIdList[i];
            int curConditionAmount = quest.curConditionAmountList[i];
            int conditionAmount = quest.conditionAmountList[i];

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
    }

    public void DestroyPanel()
    {
        DestroyImmediate(gameObject);
    }
}
