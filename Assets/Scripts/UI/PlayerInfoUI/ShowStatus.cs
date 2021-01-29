using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ShowStatus : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI StatusText;

    // Update is called once per frame
    void Update()
    {
        StatusText.text = " 마스터리 레벨 : " + MasteryManager.Instance.currentMastery.currentMasteryLevel + "\n";
        StatusText.text += " 공격력 : " + StatusManager.Instance.finalStatus.attackDamage + "\n";
        StatusText.text += " 방어력 : " + StatusManager.Instance.finalStatus.armor + "\n";
        StatusText.text += " 이동 속도 : " + StatusManager.Instance.finalStatus.moveSpeed + "\n";
        StatusText.text += " 최대 체력 : " + StatusManager.Instance.finalStatus.maxHp + "\n";
        StatusText.text += " 최대 스테미나 : " + StatusManager.Instance.finalStatus.maxStamina + "\n";
        StatusText.text += " 크리티컬 확률 : " + StatusManager.Instance.finalStatus.criticalPercent + "%\n";
        StatusText.text += " 크리티컬 데미지 : " + StatusManager.Instance.finalStatus.criticalDamage * 100 + "%\n";
    }
}
