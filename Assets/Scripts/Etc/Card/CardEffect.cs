using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CardEffect
{
    public CardEffectData effectData;
    public float[] gradeNum;
    public int level;
    List<float> originalValues = new List<float>();

    public CardEffect(CardEffectData data)
    {
        effectData = data;
    }

    public void RefineEffectData()
    {
        gradeNum = new float[3];
        gradeNum[0] = effectData.gradeOneValue;
        gradeNum[1] = effectData.gradeTwoValue;
        gradeNum[2] = effectData.gradeThreeValue;
    }

    public virtual void StartEffect()
    {
        float dataGrade = effectData.gradeOneValue;
        switch (level)
        {
            case 1:
                dataGrade = effectData.gradeOneValue;
                break;
            case 2:
                dataGrade = effectData.gradeTwoValue;
                break;
            case 3:
                dataGrade = effectData.gradeThreeValue;
                break;
        }
        if (effectData.id == 2)
        {
            originalValues.Add(Player.Instance.dashSpeed);
            Player.Instance.dashSpeed -= Player.Instance.dashSpeed * (dataGrade / 100.0f);
        }
        else if (effectData.id == 3)
        {
            originalValues.Add(Player.Instance.dashSpeed);
            Player.Instance.dashSpeed += Player.Instance.dashSpeed * (dataGrade / 100.0f);
        }
        else if (effectData.id == 4)
        {
            originalValues.Add(StatusManager.Instance.finalStatus.dashStamina);
            StatusManager.Instance.finalStatus.dashStamina -= StatusManager.Instance.finalStatus.dashStamina * dataGrade / 100.0f;
        }
        else if (effectData.id == 5)
        {
            originalValues.Add(StatusManager.Instance.finalStatus.dashStamina);
            StatusManager.Instance.finalStatus.dashStamina += StatusManager.Instance.finalStatus.dashStamina * dataGrade / 100.0f;
        }
        else if (effectData.id == 6)
        {
            originalValues.Add(Player.Instance.dashSpeed);
            Player.Instance.dashSpeed *= (dataGrade / 100.0f);
        }
        else if (effectData.id == 15)
        {
            originalValues.Add(StatusManager.Instance.finalStatus.attackDamage);
            StatusManager.Instance.finalStatus.attackDamage += StatusManager.Instance.finalStatus.attackDamage * dataGrade / 100.0f;
        }
        else if (effectData.id == 21) // TODO : ;를 [:%*{100-Mon_HP%] 증가시킨다
        {
            originalValues.Add(StatusManager.Instance.finalStatus.attackDamage);
            StatusManager.Instance.finalStatus.attackDamage += StatusManager.Instance.finalStatus.attackDamage * dataGrade / 100.0f;
        }
        else if (effectData.id == 26)
        {
            if (Player.Instance.Hp == StatusManager.Instance.finalStatus.maxHp)
            {
                originalValues.Add(StatusManager.Instance.finalStatus.attackDamage);
                StatusManager.Instance.finalStatus.attackDamage += StatusManager.Instance.finalStatus.attackDamage * dataGrade / 100.0f;
            }
        }
        else if (effectData.id == 30) // TODO : 스킬을 사용한 다음 1회 평타 플레이어 가하는 대미지
        {
            if (Player.Instance.Hp == StatusManager.Instance.finalStatus.maxHp)
            {
                originalValues.Add(StatusManager.Instance.finalStatus.attackDamage);
                StatusManager.Instance.finalStatus.attackDamage += StatusManager.Instance.finalStatus.attackDamage * dataGrade / 100.0f;
            }
        }
        else if (effectData.id == 31) // TODO : (if몬스터 체력 80% 이상);를 :% 증가시킨다
        {
            if (Player.Instance.Hp == StatusManager.Instance.finalStatus.maxHp)
            {
                originalValues.Add(StatusManager.Instance.finalStatus.attackDamage);
                StatusManager.Instance.finalStatus.attackDamage += StatusManager.Instance.finalStatus.attackDamage * dataGrade / 100.0f;
            }
        }
        else if (effectData.id == 38) // TODO : (if몬스터 체력 80% 이상);를 :% 증가시킨다
        {
            if (Player.Instance.Hp == StatusManager.Instance.finalStatus.maxHp)
            {
                originalValues.Add(StatusManager.Instance.finalStatus.attackDamage);
                StatusManager.Instance.finalStatus.attackDamage += StatusManager.Instance.finalStatus.attackDamage * dataGrade / 100.0f;
            }
        }
        else if (effectData.id == 40)
        {
            originalValues.Add(StatusManager.Instance.finalStatus.maxStamina);
            StatusManager.Instance.finalStatus.maxStamina -= dataGrade;
        }
        else if (effectData.id == 43)
        {
            originalValues.Add(StatusManager.Instance.finalStatus.maxStamina);
            StatusManager.Instance.finalStatus.maxStamina += dataGrade;
        }
        else if (effectData.id == 53)
        {
            originalValues.Add(StatusManager.Instance.finalStatus.attackDamage);
            StatusManager.Instance.finalStatus.attackDamage += StatusManager.Instance.finalStatus.attackDamage * dataGrade / 100.0f;
            StatusManager.Instance.finalStatus.armor -= StatusManager.Instance.finalStatus.armor * dataGrade / 100.0f;
        }
        else if (effectData.id == 58)
        {
            originalValues.Add(StatusManager.Instance.finalStatus.dashStamina);
            StatusManager.Instance.finalStatus.dashStamina -= StatusManager.Instance.finalStatus.dashStamina * dataGrade / 100.0f;
        }
        else if (effectData.id == 59)
        {
            originalValues.Add(StatusManager.Instance.finalStatus.armor);
            StatusManager.Instance.finalStatus.armor -= StatusManager.Instance.finalStatus.armor * dataGrade / 100.0f;
        }
        else if (effectData.id == 61)
        {
            originalValues.Add(StatusManager.Instance.finalStatus.maxHp);
            StatusManager.Instance.finalStatus.maxHp += StatusManager.Instance.finalStatus.maxHp * dataGrade / 100.0f;
        }
        //     Debug.Log("시작 : " + effectData.effectName);
        CardManager.Instance.activeEffects.Add(this);
    }

    public virtual void UpdateEffect()
    {

    }

    public virtual void EndEffect()
    {
        if (effectData.id == 2)
        {
            Player.Instance.dashSpeed = originalValues[0];
        }
        else if (effectData.id == 3)
        {
            Player.Instance.dashSpeed = originalValues[0];
        }
        else if (effectData.id == 4)
        {
            StatusManager.Instance.finalStatus.dashStamina = originalValues[0];
        }
        else if (effectData.id == 5)
        {
            StatusManager.Instance.finalStatus.dashStamina = originalValues[0];
        }
        else if (effectData.id == 6)
        {
            Player.Instance.dashSpeed = originalValues[0];
        }
        else if (effectData.id == 15)
        {
            StatusManager.Instance.finalStatus.attackDamage = originalValues[0];
        }
        else if (effectData.id == 21) // TODO : ;를 [:%*{100-Mon_HP%] 증가시킨다
        {
            StatusManager.Instance.finalStatus.attackDamage = originalValues[0];
        }
        else if (effectData.id == 26)
        {
            if (Player.Instance.Hp == StatusManager.Instance.finalStatus.maxHp)
            {
                StatusManager.Instance.finalStatus.attackDamage = originalValues[0];
            }
        }
        else if (effectData.id == 30) // TODO : 스킬을 사용한 다음 1회 평타 플레이어 가하는 대미지
        {
            if (Player.Instance.Hp == StatusManager.Instance.finalStatus.maxHp)
            {
                StatusManager.Instance.finalStatus.attackDamage = originalValues[0];
            }
        }
        else if (effectData.id == 31) // TODO : (if몬스터 체력 80% 이상);를 :% 증가시킨다
        {
            if (Player.Instance.Hp == StatusManager.Instance.finalStatus.maxHp)
            {
                StatusManager.Instance.finalStatus.attackDamage = originalValues[0];
            }
        }
        else if (effectData.id == 38) // TODO : (if몬스터 체력 80% 이상);를 :% 증가시킨다
        {
            if (Player.Instance.Hp == StatusManager.Instance.finalStatus.maxHp)
            {
                StatusManager.Instance.finalStatus.attackDamage = originalValues[0];
            }
        }
        else if (effectData.id == 40)
        {
            StatusManager.Instance.finalStatus.maxStamina = originalValues[0];
        }
        else if (effectData.id == 43)
        {
            StatusManager.Instance.finalStatus.maxStamina = originalValues[0];
        }
        else if (effectData.id == 53)
        {
            StatusManager.Instance.finalStatus.attackDamage = originalValues[0];
            StatusManager.Instance.finalStatus.armor = originalValues[1];
        }
        else if (effectData.id == 58)
        {
            StatusManager.Instance.finalStatus.dashStamina = originalValues[0];
        }
        else if (effectData.id == 59)
        {
            StatusManager.Instance.finalStatus.armor = originalValues[0];
        }
        else if (effectData.id == 61)
        {
            StatusManager.Instance.finalStatus.maxHp = originalValues[0];
        }

        //   Debug.Log("종료 : " + effectData.effectName);
        CardManager.Instance.activeEffects.Remove(this);
    }

    public string GetDescription(Card card)
    {
        string description = effectData.description;
        description = description.Replace(";", effectData.effectName);
        description = description.Replace(":", gradeNum[card.level - 1].ToString());
        return description;
    }
}