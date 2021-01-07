using UnityEngine;
using System.Collections;
using TMPro;

public class QuestPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI descript;    
    
    public void SetData(Quest quest)
    {
        title.text = quest.questData.questName;
        descript.richText = true;
        descript.text = quest.questData.description;
    }

    public void DestroyPanel()
    {
        DestroyImmediate(gameObject);
    }
}
