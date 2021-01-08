using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UpGradeContents : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI beforeText;
    [SerializeField] TextMeshProUGUI afterText;
    [SerializeField] TextMeshProUGUI moneyText;

    public void SetText(int level, int money)
    {
        beforeText.text = "Lv." + level++;
        afterText.text = "Lv." + level;
        moneyText.text = money.ToString();
    }

}
