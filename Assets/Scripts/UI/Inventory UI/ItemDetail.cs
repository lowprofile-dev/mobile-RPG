using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemDetail : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI itemText;
    [SerializeField] GameObject itemStatusPrefab;
    void Start()
    {
    }

    private string AddFontData(string text, string colorName, float fontSize = 20)
    {
        string rtnText = "";

        rtnText += "<color=";
        switch (colorName)
        {
            case "aqua": rtnText += "#00ffffff"; break;
            case "black": rtnText += "#000000ff"; break;
            case "blue": rtnText += "#0000ffff"; break;
            case "brown": rtnText += "#a52a2aff"; break;
            case "cyan": rtnText += "#00ffffff"; break;
            case "darkblue": rtnText += "#0000a0ff"; break;
            case "fuchsia": rtnText += "#ff00ffff"; break;
            case "green": rtnText += "#008000ff"; break;
            case "grey": rtnText += "#808080ff"; break;
            case "lightblue": rtnText += "#add8e6ff"; break;
            case "lime": rtnText += "#00ff00ff"; break;
            case "magenta": rtnText += "#ff00ffff"; break;
            case "maroon": rtnText += "#800000ff"; break;
            case "navy": rtnText += "#000080ff"; break;
            case "olive": rtnText += "#808000ff"; break;
            case "purple": rtnText += "#800080ff"; break;
            case "red": rtnText += "#ff0000ff"; break;
            case "silver": rtnText += "#c0c0c0ff"; break;
            case "teal": rtnText += "#008080ff"; break;
            case "white": rtnText += "#ffffffff"; break;
            case "yellow": rtnText += "#ffff00ff"; break;
            default: rtnText += colorName; break;
        }
        rtnText += ">";

        rtnText += "<size=" + fontSize + ">";
        rtnText += text;

        rtnText += "</size>";
        rtnText += "</color>";

        return rtnText;
    }

    public void LoadItemDetail(ItemData itemData)
    {
        string allData = "";
        
        itemText.richText = true;
        itemText.verticalAlignment = VerticalAlignmentOptions.Top;

        allData += AddFontData(itemData.itemName, "white", 24) + "\n";
        allData += AddFontData(itemData.itemType, "white", 16) + "\n";
        allData += AddFontData("\n", "white", 8);

        if (itemData.hpIncreaseRate != 0) allData += AddFontData("최대 HP \t\t +" + itemData.hpIncreaseRate + "%", "white", 18) + "\n";
        if (itemData.hp != 0) allData += AddFontData("최대 HP \t\t +" + itemData.hp, "white", 18) + "\n";
        if (itemData.hpRecovery != 0) allData += AddFontData("HP 회복 \t\t +" + itemData.hpRecovery, "white", 18) + "\n";
        if (itemData.stamina != 0) allData += AddFontData("스태미너 \t\t +" + itemData.stamina, "white", 18) + "\n";
        if (itemData.staminaRecovery != 0) allData += AddFontData("스태미너 회복 \t +" + itemData.staminaRecovery, "white", 18) + "\n";
        if (itemData.attackDamage != 0) allData += AddFontData("공격력 \t\t +" + itemData.attackDamage + "%", "white", 18) + "\n";
        if (itemData.attackSpeed != 0) allData += AddFontData("공격 속도 \t\t +" + itemData.attackSpeed + "%", "white", 18) + "\n";
        if (itemData.attackCooldown != 0) allData += AddFontData("공격 쿨타임 \t\t -" + itemData.attackCooldown + "%", "white", 18) + "\n";
        if (itemData.armor != 0) allData += AddFontData("물리 방어력 \t\t +" + itemData.armor + "%", "white", 18) + "\n";
        if (itemData.magicResistance != 0) allData += AddFontData("마법 방어력 \t\t +" + itemData.magicResistance + "%", "white", 18) + "\n";
        if (itemData.moveSpeed != 0) allData += AddFontData("이동 속도 \t\t +" + itemData.moveSpeed + "%", "white", 18) + "\n";
        if (itemData.dashCooldown != 0) allData += AddFontData("대쉬 쿨타임 \t\t -" + itemData.dashCooldown + "%", "white", 18) + "\n";
        if (itemData.dashStamina != 0) allData += AddFontData("대쉬 스태미너 \t -" + itemData.dashStamina + "%", "white", 18) + "\n";

        allData = allData.Remove(allData.Length - 1, 1);
        itemText.text = allData;

        /*
        itemBasicInfo.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(itemData.itemName);
        itemBasicInfo.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(itemData.itemType);
        
        switch (itemData.itemType)
        {
            case "Armor":
                GameObject hpStatus = Instantiate(itemStatusPrefab, transform);
                hpStatus.GetComponent<ItemStatus>().SetPanel("HP %");
                hpStatus.GetComponent<ItemStatus>().SetValue(itemData.hpIncreaseRate);
                GameObject armorStatus = Instantiate(itemStatusPrefab, transform);
                armorStatus.GetComponent<ItemStatus>().SetPanel("방어력 %");
                armorStatus.GetComponent<ItemStatus>().SetValue(itemData.armor);
                GameObject magicResistanceStatus = Instantiate(itemStatusPrefab, transform);
                magicResistanceStatus.GetComponent<ItemStatus>().SetPanel("마법 방어력 %");
                magicResistanceStatus.GetComponent<ItemStatus>().SetValue(itemData.magicResistance);
                break;
            case "Bottom":
                GameObject staminaStatus = Instantiate(itemStatusPrefab, transform);
                staminaStatus.GetComponent<ItemStatus>().SetPanel("스태미너");
                staminaStatus.GetComponent<ItemStatus>().SetValue(itemData.stamina);
                GameObject staminaRecoveryStatus = Instantiate(itemStatusPrefab, transform);
                staminaRecoveryStatus.GetComponent<ItemStatus>().SetPanel("스태미너 회복");
                staminaRecoveryStatus.GetComponent<ItemStatus>().SetValue(itemData.staminaRecovery);
                GameObject hpMaxStatus = Instantiate(itemStatusPrefab, transform);
                hpMaxStatus.GetComponent<ItemStatus>().SetPanel("HP");
                hpMaxStatus.GetComponent<ItemStatus>().SetValue(itemData.hp);
                GameObject hpRecoveryStatus = Instantiate(itemStatusPrefab, transform);
                hpRecoveryStatus.GetComponent<ItemStatus>().SetPanel("HP 회복");
                hpRecoveryStatus.GetComponent<ItemStatus>().SetValue(itemData.hpRecovery);
                break;
            case "Helmet":
                GameObject stunResistanceStatus = Instantiate(itemStatusPrefab, transform);
                stunResistanceStatus.GetComponent<ItemStatus>().SetPanel("마법 방어력 %");
                stunResistanceStatus.GetComponent<ItemStatus>().SetValue(itemData.magicResistance);
                break;
            case "Gloves":
                GameObject attackStatus = Instantiate(itemStatusPrefab, transform);
                attackStatus.GetComponent<ItemStatus>().SetPanel("공격력 %");
                attackStatus.GetComponent<ItemStatus>().SetValue(itemData.attackDamage);
                GameObject attackSpeedStatus = Instantiate(itemStatusPrefab, transform);
                attackSpeedStatus.GetComponent<ItemStatus>().SetPanel("공격속도 %");
                attackSpeedStatus.GetComponent<ItemStatus>().SetValue(itemData.attackSpeed);
                GameObject attackCoolStatus = Instantiate(itemStatusPrefab, transform);
                attackCoolStatus.GetComponent<ItemStatus>().SetPanel("공격스킬 쿨타임 %");
                attackCoolStatus.GetComponent<ItemStatus>().SetValue(itemData.attackCooldown);
                break;
            case "Boot":
                GameObject speedStatus = Instantiate(itemStatusPrefab, transform);
                speedStatus.GetComponent<ItemStatus>().SetPanel("이동속도 %");
                speedStatus.GetComponent<ItemStatus>().SetValue(itemData.moveSpeed);
                GameObject dashCoolStatus = Instantiate(itemStatusPrefab, transform);
                dashCoolStatus.GetComponent<ItemStatus>().SetPanel("대쉬스킬 쿨타임 %");
                dashCoolStatus.GetComponent<ItemStatus>().SetValue(itemData.dashCooldown);
                GameObject dashStaminaStatus = Instantiate(itemStatusPrefab, transform);
                dashStaminaStatus.GetComponent<ItemStatus>().SetPanel("대쉬 스태미너");
                dashStaminaStatus.GetComponent<ItemStatus>().SetValue(itemData.dashStamina);
                break;
        }
        */
    }
}
