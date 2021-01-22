using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

////////////////////////////////////////////////////
/*
    File UpGradeContents.cs
    class UpGradeContents

    담당자 : 김의겸
    부 담당자 : 
*/
////////////////////////////////////////////////////
///
public class UpGradeContents : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI beforeText;
    [SerializeField] TextMeshProUGUI afterText;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI playerMoneyText;

    /// <summary>
    /// 현재 가지고있는 돈과 레벨, 강화된 레벨을 UI로 표시해주기 위한 함수이다.
    /// </summary>
    /// <param name="level"> 현재 레벨을 입력 받는다</param>
    /// <param name="money"> 현재 가지고 있는 돈을 입력받는다</param>
    public void SetUpgradeMoneyText(int level, int money)
    {
        beforeText.text = "Lv." + level++;
        afterText.text = "Lv." + level;
        moneyText.text = money.ToString();
    }

    public void SetPlayerMoneyText(int money)
    {
        playerMoneyText.text = money.ToString();
    }
}
