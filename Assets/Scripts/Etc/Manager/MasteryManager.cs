using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using CSVReader;
////////////////////////////////////////////////////
/*
    File MasteryManager.cs
    class MasteryManager

    담당자 : 김의겸
    부 담당자 : 김기정
*/
////////////////////////////////////////////////////

/// <summary>
///  무기별 스킬 레벨 저장용 클래스
/// </summary>
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
        WeaponSkillLevel copy = MemberwiseClone() as WeaponSkillLevel;
        return copy;
    }
}

/// <summary>
///  마스터리 레벨 ,무기별 숙련도 레벨, 경험치 저장용 클래스
/// </summary>
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
    public int currentDaggerMasteryExp;
    public int currentBluntMasteryExp;
    public int currentStaffMasteryExp;
    public int currentWandMasteryExp;

    [Header("무기별 스킬 해제 상황")]
    public bool currentSwordSkillBReleased;
    public bool currentSwordSkillCReleased;
    public bool currentDaggerSkillBReleased;
    public bool currentDaggerSkillCReleased;
    public bool currentBluntSkillBReleased;
    public bool currentBluntSkillCReleased;
    public bool currentWandSkillBReleased;
    public bool currentWandSkillCReleased;
    public bool currentStaffSkillBReleased;
    public bool currentStaffSkillCReleased;

    public List<int> currentMasteryChoices;

    public CurrentMastery()
    {
        currentMasteryLevel = 5;
        currentSwordMasteryLevel = 1;
        currentDaggerMasteryLevel = 1;
        currentBluntMasteryLevel = 1;
        currentStaffMasteryLevel = 1;
        currentWandMasteryLevel = 1;

        currentSwordMasteryExp = 0;
        currentDaggerMasteryExp = 0;
        currentBluntMasteryExp = 0;
        currentStaffMasteryExp = 0;
        currentWandMasteryExp = 0;

        currentSwordSkillBReleased = false;
        currentSwordSkillCReleased = false;
        currentDaggerSkillBReleased = false;
        currentDaggerSkillCReleased = false;
        currentBluntSkillBReleased = false;
        currentBluntSkillCReleased = false;
        currentWandSkillBReleased = false;
        currentWandSkillCReleased = false;
        currentStaffSkillBReleased = false;
        currentStaffSkillCReleased = false;
        currentMasteryChoices = new List<int>(new int[10]);
    }

    public object Clone()
    {
        CurrentMastery copy = MemberwiseClone() as CurrentMastery;
        copy.currentMasteryChoices = new List<int>();
        copy.currentMasteryChoices.AddRange(currentMasteryChoices);
        return copy;
    }
}

/// <summary>
/// 마스터리 스킬 데이터 저장용
/// </summary>
[CSVReader.Data("id")]
public class PlayerMasteryData
{
    public int id;
    public int level;
    public string masteryName;
    public string masteryDescription;
}

/// <summary>
/// 마스터리 매니저
/// 마스터리 레벨 및 스킬 레벨, 경험치, 스킬 데이터 등을 관리하는 매니저
/// 초기화/세이브/로드
/// </summary>
public class MasteryManager : SingletonBase<MasteryManager>
{
    public CurrentMastery currentMastery;
    public WeaponSkillLevel[] weaponSkillLevel;
    public Dictionary<int,PlayerMasteryData> masteryDictionary;

    public void InitMasteryManager()
    {
        weaponSkillLevel = new WeaponSkillLevel[5];
        Table masteryTable = CSVReader.Reader.ReadCSVToTable("CSVData/MasteryDatabase");
        masteryDictionary = masteryTable.TableToDictionary<int, PlayerMasteryData>();
        LoadCurrentMastery();
        LoadSkillLevel();
    }

    public void SaveCurrentMastery()
    {
        if (currentMastery == null) currentMastery = new CurrentMastery();
        string jsonData = JsonUtility.ToJson(currentMastery, true);
        string path = Path.Combine(Application.persistentDataPath, "playerCurrentMastery.json");
        File.WriteAllText(path, jsonData);
    }

    public void LoadCurrentMastery()
    {
        PlayerPrefs.SetInt("LoadCurrentMasteryCount", PlayerPrefs.GetInt("LoadCurrentMasteryCount", 0));
        if (PlayerPrefs.GetInt("LoadCurrentMasteryCount") == 0)
        {
            //Debug.log("최초 마스터리 데이터 로드 실행입니다.");
            PlayerPrefs.SetInt("LoadCurrentMasteryCount", 1);
            SaveCurrentMastery();
            PlayerPrefs.Save();
        }

        string path = Path.Combine(Application.persistentDataPath, "playerCurrentMastery.json");
        string jsonData = File.ReadAllText(path);
        if (jsonData == null) return;
        currentMastery = JsonUtility.FromJson<CurrentMastery>(jsonData);
    }

    public void SaveSkillLevel()
    {
        for (int i = 0; i < 5; i++)
        {
            if (weaponSkillLevel[i] == null) weaponSkillLevel[i] = new WeaponSkillLevel();
            string jsonData = JsonUtility.ToJson(weaponSkillLevel[i], true);
            string path = Path.Combine(Application.persistentDataPath, "weaponSKillLevel" + i + ".json");
            File.WriteAllText(path, jsonData);
        }
        //string swordSkillData = JsonUtility.ToJson(currentMastery, true);
        //string path = Path.Combine(Application.persistentDataPath, "playerCurrentMastery.json");
        //File.WriteAllText(path, jsonData);
    }

    public void LoadSkillLevel()
    {
        PlayerPrefs.SetInt("LoadWeaponSkillCount", PlayerPrefs.GetInt("LoadWeaponSkillCount", 0));
        if (PlayerPrefs.GetInt("LoadWeaponSkillCount") == 0)
        {
            //Debug.log("최초 스킬 레벨 데이터 로드 실행입니다.");
            PlayerPrefs.SetInt("LoadWeaponSkillCount", 1);
            SaveSkillLevel();
            PlayerPrefs.Save();
        }
        for (int i = 0; i < 5; i++)
        {
            string path = Path.Combine(Application.persistentDataPath, "weaponSKillLevel" + i + ".json");
            string jsonData = File.ReadAllText(path);
            if (jsonData == null) return;
            weaponSkillLevel[i] = JsonUtility.FromJson<WeaponSkillLevel>(jsonData);
        }

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
                currentMastery.currentSwordMasteryExp = currentMastery.currentSwordMasteryExp - WeaponManager.Instance.GetWeapon().expMax;
                break;
            case "dagger":
                currentMastery.currentDaggerMasteryLevel++;
                currentMastery.currentDaggerMasteryExp = currentMastery.currentDaggerMasteryExp - WeaponManager.Instance.GetWeapon().expMax;
                break;
            case "blunt":
                currentMastery.currentBluntMasteryLevel++;
                currentMastery.currentBluntMasteryExp = currentMastery.currentBluntMasteryExp - WeaponManager.Instance.GetWeapon().expMax;
                break;
            case "staff":
                currentMastery.currentStaffMasteryLevel++;
                currentMastery.currentStaffMasteryExp = currentMastery.currentStaffMasteryExp - WeaponManager.Instance.GetWeapon().expMax;
                break;
            case "wand":
                currentMastery.currentWandMasteryLevel++;
                currentMastery.currentWandMasteryExp = currentMastery.currentWandMasteryExp - WeaponManager.Instance.GetWeapon().expMax;
                break;
        }
        SaveCurrentMastery();
    }
    /// <summary>
    /// 무기와 스킬 이름을 입력받아 스킬의 레벨을 1씩 증가시킨다.
    /// </summary>
    /// <param name="weapon"> 현재 무기를 입력받는다.</param>
    /// <param name="skillName">무기의 스킬 이름을 입력받는다</param>
    public void incrementSkillLevel(string weapon, string skillName)
    {
        switch (weapon)
        {
            case "sword":
                switch (skillName)
                {
                    case "autoAttack":
                        weaponSkillLevel[0].autoAttackLevel++;
                        break;
                    case "skillA":
                        weaponSkillLevel[0].skillALevel++;
                        break;
                    case "skillB":
                        weaponSkillLevel[0].skillBLevel++;
                        break;
                    case "skillC":
                        weaponSkillLevel[0].skillCLevel++;
                        break;
                }
                break;
            case "dagger":
                switch (skillName)
                {
                    case "autoAttack":
                        weaponSkillLevel[1].autoAttackLevel++;
                        break;
                    case "skillA":
                        weaponSkillLevel[1].skillALevel++;
                        break;
                    case "skillB":
                        weaponSkillLevel[1].skillBLevel++;
                        break;
                    case "skillC":
                        weaponSkillLevel[1].skillCLevel++;
                        break;
                }
                break;
            case "blunt":
                switch (skillName)
                {
                    case "autoAttack":
                        weaponSkillLevel[2].autoAttackLevel++;
                        break;
                    case "skillA":
                        weaponSkillLevel[2].skillALevel++;
                        break;
                    case "skillB":
                        weaponSkillLevel[2].skillBLevel++;
                        break;
                    case "skillC":
                        weaponSkillLevel[2].skillCLevel++;
                        break;
                }
                break;
            case "wand":
                switch (skillName)
                {
                    case "autoAttack":
                        weaponSkillLevel[3].autoAttackLevel++;
                        break;
                    case "skillA":
                        weaponSkillLevel[3].skillALevel++;
                        break;
                    case "skillB":
                        weaponSkillLevel[3].skillBLevel++;
                        break;
                    case "skillC":
                        weaponSkillLevel[3].skillCLevel++;
                        break;
                }
                break;
            case "staff":
                switch (skillName)
                {
                    case "autoAttack":
                        weaponSkillLevel[4].autoAttackLevel++;
                        break;
                    case "skillA":
                        weaponSkillLevel[4].skillALevel++;
                        break;
                    case "skillB":
                        weaponSkillLevel[4].skillBLevel++;
                        break;
                    case "skillC":
                        weaponSkillLevel[4].skillCLevel++;
                        break;
                }
                break;
        }
        SaveSkillLevel();
    }
    /// <summary>
    /// 무기의 경험치와 마스터리매니저의 경험치가 맞지 않을경우 최신화 시킨다.
    /// </summary>
    public void UpdateCurrentExp()
    {
        if (WeaponManager.Instance != null)
        {
            switch (WeaponManager.Instance.GetWeapon().name)
            {
                case "sword":
                    if (WeaponManager.Instance.GetWeapon().exp != currentMastery.currentSwordMasteryExp)
                    {
                        WeaponManager.Instance.GetWeapon().exp = currentMastery.currentSwordMasteryExp;
                    }
                    break;
                case "wand":
                    if (WeaponManager.Instance.GetWeapon().exp != currentMastery.currentWandMasteryExp)
                    {
                        WeaponManager.Instance.GetWeapon().exp = currentMastery.currentWandMasteryExp;
                    }
                    break;
                case "dagger":
                    if (WeaponManager.Instance.GetWeapon().exp != currentMastery.currentDaggerMasteryExp)
                    {
                        WeaponManager.Instance.GetWeapon().exp = currentMastery.currentWandMasteryExp;
                    }
                    break;
                case "blunt":
                    if (WeaponManager.Instance.GetWeapon().exp != currentMastery.currentBluntMasteryExp)
                    {
                        WeaponManager.Instance.GetWeapon().exp = currentMastery.currentBluntMasteryExp;
                    }
                    break;
                case "staff":
                    if (WeaponManager.Instance.GetWeapon().exp != currentMastery.currentStaffMasteryExp)
                    {
                        WeaponManager.Instance.GetWeapon().exp = currentMastery.currentStaffMasteryExp;
                    }
                    break;
            }
            SaveCurrentMastery();
        }
    }
    
    /// <summary>
    /// 게임이 강제 종료 될 경우
    /// 데이터를 저장한다.
    /// </summary>
    private void OnApplicationQuit()
    {
        SaveCurrentMastery();
        SaveSkillLevel();
    }
}
