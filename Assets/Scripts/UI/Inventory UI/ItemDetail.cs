/*
    File ItemDetail.cs
    class ItemDetail
    
    담당자 : 김기정
    부 담당자 : 이신홍
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemDetail : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI itemText;
    [SerializeField] GameObject itemStatusPrefab;

    /// <summary>
    /// 아이템 데이터를 받아 이를 TextMeshProUGUI에 표기한다.
    /// </summary>
    public void LoadItemDetail(ItemData itemData)
    {
        string allData = "";
        
        itemText.richText = true;
        itemText.verticalAlignment = VerticalAlignmentOptions.Top;

        allData += UIManager.Instance.AddFontData(itemData.itemName, "white", 24) + "\n";
        allData += UIManager.Instance.AddFontData(itemData.itemType, "white", 16) + "\n";
        allData += UIManager.Instance.AddFontData("\n", "white", 8);

        // 데이터의 존재 유무에 따라 UI 표시가 달라진다.
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
        itemText.text = allData;
    }
}
