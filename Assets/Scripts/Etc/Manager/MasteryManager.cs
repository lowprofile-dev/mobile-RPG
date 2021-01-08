using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using CSVReader;

[System.Serializable]
public class WeaponSkillLevel : ICloneable
{
    public int autoAttackLevel;
    public int skillALevel;
    public int skillBLevel;
    public int skillCLevel;

    public WeaponSkillLevel()
    {
        autoAttackLevel = 1;
        skillALevel = 1;
        skillBLevel = 1;
        skillCLevel = 1;
    }

    public object Clone()
    {
        WeaponSkillLevel copy = this.MemberwiseClone() as WeaponSkillLevel;
        return copy;
    }
}

[System.Serializable]
public class CurrentMastery : ICloneable
{
    [Header("숙련도 및 마스터리 레벨")]
    public int currentMasteryLevel;
    public int currentSwordMasteryLevel;
    public int currentDaggerMasteryLevel;
    public int currentBluntMasteryLevel;
    public int currentStaffMasteryLevel;
    public int currentWandMasteryLevel;

    [Header("무기별 경험치 획득 상황")]
    public int currentSwordMasteryExp;
    public int currentDaggerMasterExp;
    public int currentBluntMasteryExp;
    public int currentStaffMasteryExp;
    public int currentWandMasteryExp;

    public List<int> currentMasteryChoices;

    public CurrentMastery()
    {
        currentMasteryLevel = 0;
        currentSwordMasteryLevel = 0;
        currentDaggerMasteryLevel = 0;
        currentBluntMasteryLevel = 0;
        currentStaffMasteryLevel = 0;
        currentWandMasteryLevel = 0;

        currentSwordMasteryExp = 0;
        currentDaggerMasterExp = 0;
        currentBluntMasteryExp = 0;
        currentStaffMasteryExp = 0;
        currentWandMasteryExp = 0;
    }

    public object Clone()
    {
        CurrentMastery copy = this.MemberwiseClone() as CurrentMastery;
        copy.currentMasteryChoices = new List<int>();
        copy.currentMasteryChoices.AddRange(currentMasteryChoices);
        return copy;
    }
}

[CSVReader.Data("id")]
public class PlayerMasteryData
{
    public int id;
    public int level;
    public string masteryName;
    public string masteryDescription;
    public string detailedDescription;
}

public class MasteryManager : SingletonBase<MasteryManager>
{
    public CurrentMastery currentMastery;

    private void SaveCurrentMastery()
    {
        string jsonData = JsonUtility.ToJson(currentMastery, true);
        string path = Path.Combine(Application.persistentDataPath, "playerCurrentMastery.json");
        File.WriteAllText(path, jsonData);
    }

    private void LoadCurrentMastery()
    {
        PlayerPrefs.SetInt("LoadCurrentMasteryCount", PlayerPrefs.GetInt("LoadCurrentMasteryCount", 0));
        if (PlayerPrefs.GetInt("LoadCurrentMasteryCount") == 0)
        {
            Debug.Log("최초 마스터리 데이터 로드 실행입니다.");
            PlayerPrefs.SetInt("LoadCurrentMasteryCount", 1);
            SaveCurrentMastery();
            PlayerPrefs.Save();
        }

        string path = Path.Combine(Application.persistentDataPath, "playerCurrentMastery.json");
        string jsonData = File.ReadAllText(path);
        if (jsonData == null) return;
        currentMastery = JsonUtility.FromJson<CurrentMastery>(jsonData);
    }

    /// <summary>
    /// 무기 숙련도 증가
    /// </summary>
    /// <param name="weapon">무기. 
    /// 종류별로 string값 입력
    /// sword : 한손검
    /// dagger : 단검
    /// blunt : 둔기
    /// staff : 전투 지팡이
    /// wand : 지팡이
    /// </param>
    public void incrementMasteryLevel(string weapon)
    {
        if (currentMastery.currentMasteryLevel < 50)
            currentMastery.currentMasteryLevel++;
        switch (weapon)
        {
            case "sword":
                currentMastery.currentSwordMasteryLevel++;
                break;
            case "dagger":
                currentMastery.currentDaggerMasteryLevel++;
                break;
            case "blunt":
                currentMastery.currentBluntMasteryLevel++;
                break;
            case "staff":
                currentMastery.currentStaffMasteryLevel++;
                break;
            case "wand":
                currentMastery.currentWandMasteryLevel++;
                break;
        }
        SaveCurrentMastery();
    }

    public void SetMastery(int choice)
    {
        currentMastery.currentMasteryChoices.Add(choice);
    }
}
