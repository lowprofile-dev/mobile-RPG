using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EquipDetail : MonoBehaviour
{
   [SerializeField] TextMeshProUGUI descriptionText;
   [SerializeField] TextMeshProUGUI nameText;
   [SerializeField] TextMeshProUGUI typeText;

    public Image icon = null;

    
    public void LoadData(ItemData itemData, Sprite sprite)
    {
        string allData = "";

        descriptionText.richText = true;
        descriptionText.verticalAlignment = VerticalAlignmentOptions.Top;

        nameText.richText = true;
        nameText.verticalAlignment = VerticalAlignmentOptions.Top;

        typeText.richText = true;
        typeText.verticalAlignment = VerticalAlignmentOptions.Top;

        nameText.text = UIManager.Instance.AddFontData(itemData.itemName, "white", 24);
        typeText.text = UIManager.Instance.AddFontData(itemData.itemType, "white", 16);

        if (itemData.itemDescription != null) allData += UIManager.Instance.AddFontData(itemData.itemDescription, "white", 18) + "\n";
        if (itemData.hpIncreaseRate != 0) allData += UIManager.Instance.AddFontData("최대 HP \t\t +" + itemData.hpIncreaseRate + "%", "white", 18) + "\n";
        if (itemData.hp != 0) allData += UIManager.Instance.AddFontData("최대 HP \t\t +" + itemData.hp, "white", 18) + "\n";
        if (itemData.hpRecovery != 0) allData += UIManager.Instance.AddFontData("HP 회복 \t\t +" + itemData.hpRecovery, "white", 18) + "\n";
        if (itemData.stamina != 0) allData += UIManager.Instance.AddFontData("스태미너 \t\t +" + itemData.stamina, "white", 18) + "\n";
        if (itemData.staminaRecovery != 0) allData += UIManager.Instance.AddFontData("스태미너 회복 \t +" + itemData.staminaRecovery, "white", 18) + "\n";
        if (itemData.attackDamage != 0) allData += UIManager.Instance.AddFontData("공격력 \t\t +" + itemData.attackDamage + "%", "white", 18) + "\n";
        if (itemData.attackCooldown != 0) allData += UIManager.Instance.AddFontData("공격 쿨타임 \t\t -" + itemData.attackCooldown + "%", "white", 18) + "\n";
        if (itemData.criticalDamage != 0) allData += UIManager.Instance.AddFontData("크리티컬 데미지 \t +" + (itemData.criticalDamage * 100) + "%", "white", 18) + "\n";
        if (itemData.criticalPercent != 0) allData += UIManager.Instance.AddFontData("크리티컬 확률 \t +" + itemData.criticalPercent + "%", "white", 18) + "\n";
        if (itemData.armor != 0) allData += UIManager.Instance.AddFontData("방어력 \t\t +" + itemData.armor + "%", "white", 18) + "\n";
        if (itemData.moveSpeed != 0) allData += UIManager.Instance.AddFontData("이동 속도 \t\t +" + itemData.moveSpeed + "%", "white", 18) + "\n";
        if (itemData.dashStamina != 0) allData += UIManager.Instance.AddFontData("대쉬 스태미너 \t -" + itemData.dashStamina + "%", "white", 18) + "\n";

        allData = allData.Remove(allData.Length - 1, 1);
        descriptionText.text = allData;
        icon.sprite = sprite;
    }
}
