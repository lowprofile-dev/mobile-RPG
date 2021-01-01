using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemDetail : MonoBehaviour
{
    [SerializeField] Transform itemBasicInfo;
    [SerializeField] GameObject itemStatusPrefab;
    void Start()
    {
        itemBasicInfo = transform.GetChild(0);
    }

    public void LoadItemDetail(ItemData itemData)
    {
        itemBasicInfo.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(itemData.itemName);
        itemBasicInfo.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(itemData.itemType);
        switch(itemData.itemType)
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
    }
}
