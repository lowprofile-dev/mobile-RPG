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
        StatusText.text += " 물리 공격력 : " + StatusManager.Instance.finalStatus.attackDamage + "\n";
        StatusText.text += " 마법 공격력 : " + StatusManager.Instance.finalStatus.magicDamage + "\n";
        StatusText.text += " 물리 방어력 : " + StatusManager.Instance.finalStatus.armor + "\n";
        StatusText.text += " 마법 방어력 : " + StatusManager.Instance.finalStatus.magicResistance + "\n";
        StatusText.text += " 이동 속도 : " + StatusManager.Instance.finalStatus.moveSpeed + "\n";
        StatusText.text += " 최대 체력 : " + StatusManager.Instance.finalStatus.maxHp + "\n";
        StatusText.text += " 최대 스테미나 : " + StatusManager.Instance.finalStatus.maxStamina + "\n";
    }
}
