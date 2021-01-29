////////////////////////////////////////////////////
/*
    File MasteryManager.cs
    class MasteryManager

    담당자 : 김의겸
    부 담당자 : 김기정
*/
////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using CSVReader;

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
        currentMasteryLevel = 63;
        currentSwordMasteryLevel = 30;
        currentDaggerMasteryLevel = 1;
        currentBluntMasteryLevel = 1;
        currentStaffMasteryLevel = 1;
        currentWandMasteryLevel = 30;

        currentSwordMasteryExp = 0;
        currentDaggerMasteryExp = 0;
        currentBluntMasteryExp = 0;
        currentStaffMasteryExp = 0;
        currentWandMasteryExp = 0;

        currentSwordSkillBReleased = true;
        currentSwordSkillCReleased = true;
        currentDaggerSkillBReleased = false;
        currentDaggerSkillCReleased = false;
        currentBluntSkillBReleased = false;
        currentBluntSkillCReleased = false;
        currentWandSkillBReleased = true;
        currentWandSkillCReleased = true;
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
    //Player player; // 마스터리 매니저는 플레이어에 종속되어서는 안됨.
    StatusManager statusManager;
    WeaponManager weaponManager;
    public bool rage;
    public bool resurrection;
    public bool[,] masterySet = new bool[10, 2]; // 마스터리 활성화 정보, 플레이어에 있으면 플레이어에 종속되기에 옮겨놓음.

    public void InitMasteryManager()
    {
        weaponSkillLevel = new WeaponSkillLevel[5];
        Table masteryTable = CSVReader.Reader.ReadCSVToTable("CSVData/MasteryDatabase");
        masteryDictionary = masteryTable.TableToDictionary<int, PlayerMasteryData>();
        LoadCurrentMastery();
        LoadSkillLevel();
        rage = false;
        resurrection = false;
        MasteryApply();
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




    ///////////////// 마스터리 관련 //////////////////

    //마스터리 강화 관련 
    public void MasteryApply()
    {
        if (StatusManager.Instance != null ) statusManager = StatusManager.Instance;
        //카드 리롤 확률 증가
        //CardManager.cs 187

        //보스 처치 시 더 많은 코인 획득 미구현 -> 아이템 드랍 확률 증가
        //DungeonRoom.cs 186

        //크리티컬 확률 증가
        if (currentMastery.currentMasteryChoices[1] == -1)
        {
            if (masterySet[1, 0] == false)
            {
                statusManager.additionStatus.criticalPercent += 20f;
                masterySet[1, 0] = true;
            }
        }
        else
        {
            if (masterySet[1, 0] == true)
            {
                statusManager.additionStatus.criticalPercent -= 20f;
                masterySet[1, 0] = false;
            }
        }

        //크리티컬 데미지 증가
        if (currentMastery.currentMasteryChoices[1] == 1)
        {
            if (masterySet[1, 1] == false)
            {
                statusManager.additionStatus.criticalDamage += 0.5f;
                masterySet[1, 1] = true;
            }
        }
        else
        {
            if (masterySet[1, 1] == true)
            {
                statusManager.additionStatus.criticalDamage -= 0.5f;
                masterySet[1, 1] = false;
            }
        }

        // 체력 증가
        if (currentMastery.currentMasteryChoices[2] == -1)
        {
            if (masterySet[2, 0] == false)
            {
                statusManager.additionStatus.hpRecovery += 2f;
                masterySet[2, 0] = true;
            }
        }
        else
        {
            if (masterySet[2, 0] == true)
            {
                statusManager.additionStatus.hpRecovery -= 2f;
                masterySet[2, 0] = false;
            }
        }

        //기력 증가
        if (currentMastery.currentMasteryChoices[2] == 1)
        {
            if (masterySet[2, 1] == false)
            {
                statusManager.additionStatus.staminaRecovery += 1f;
                masterySet[2, 1] = true;
            }
        }
        else
        {
            if (masterySet[2, 1] == true)
            {
                statusManager.additionStatus.staminaRecovery -= 1f;
                masterySet[2, 1] = false;
            }
        }

        //이속 증가
        if (currentMastery.currentMasteryChoices[3] == -1)
        {
            if (masterySet[3, 0] == false)
            {
                statusManager.multiplicationStatus.moveSpeed += 25f;
                masterySet[3, 0] = true;
            }
        }
        else
        {
            if (masterySet[3, 0] == true)
            {
                statusManager.multiplicationStatus.moveSpeed -= 25f;
                masterySet[3, 0] = false;
            }
        }
        //방어 증가
        if (currentMastery.currentMasteryChoices[3] == 1)
        {
            if (masterySet[3, 1] == false)
            {
                statusManager.additionStatus.armor += 15;
                masterySet[3, 1] = true;
            }
        }
        else
        {
            if (masterySet[3, 1] == true)
            {
                statusManager.additionStatus.armor -= 15;
                masterySet[3, 1] = false;
            }
        }

        //골드 획득량 증가 미구현 && 아이템 확률 증가 통합
        //DungeonRoom.cs 207

        //모든 몬스터 피해량 10%
        //MonsterAction.cs 778

        //보스 피해량 20%
        //BossSkeltonKingAction.cs 426
        //BossSkeltonPase2.cs 416

        //회피시 무적
        //BossAttack.cs 87
        //MonsterAction.cs 778
        if (currentMastery.currentMasteryChoices[6] == -1)
        {
            if (masterySet[6, 0] == false)
            {
                //statusManager.multiplicationStatus.dashStamina += 50; 
                masterySet[6, 0] = true;
            }
        }
        else
        {
            if (masterySet[6, 0] == true)
            {
                //statusManager.multiplicationStatus.dashStamina += 50; 
                masterySet[6, 0] = false;
            }
        }
        //회피 사용 기력 감소
        if (currentMastery.currentMasteryChoices[6] == -1)
        {
            if (masterySet[6, 1] == false)
            {
                statusManager.multiplicationStatus.dashStamina += 50;
                masterySet[6, 1] = true;
            }
        }
        else
        {
            if (masterySet[6, 1] == true)
            {
                statusManager.multiplicationStatus.dashStamina -= 50;
                masterySet[6, 1] = false;
            }
        }
        //기본공격 10% 강화, hp 2% 흡수
        // PlayerAttack.cs 76;
        if (currentMastery.currentMasteryChoices[7] == -1)
        {
            if (masterySet[7, 0] == false)
            {
                StatusManager.Instance.additionStatus.attackDamage += 10f;
                masterySet[7, 0] = true;
            }
        }
        else
        {
            if (masterySet[7, 0] == true)
            {
                StatusManager.Instance.additionStatus.attackDamage -= 10f;
                masterySet[7, 0] = false;
            }
        }

        //스킬 쿨타임 1초 감소
        if (currentMastery.currentMasteryChoices[7] == 1)
        {
            if(weaponManager != null)
            {
                if (masterySet[7, 1] == false)
                {
                    weaponManager.WeaponCoolTimeReduce();
                    masterySet[7, 1] = true;
                }
            }
        }

        else
        {
            if (weaponManager != null)
            {
                if (masterySet[7, 1] == true)
                {
                    weaponManager.WeaponCoolTimeincrease();
                    masterySet[7, 1] = false;
                }
            }
        }

        // 물리 스킬 총 피해량 20% 증가
        if (currentMastery.currentMasteryChoices[8] == -1)
        {
            if (masterySet[8, 0] == false)
            {
                statusManager.multiplicationStatus.attackDamage += 20f;
                masterySet[8, 0] = true;
            }
        }
        else
        {
            if (masterySet[8, 0] == true)
            {
                statusManager.multiplicationStatus.attackDamage -= 20f;
                masterySet[8, 0] = false;
            }
        }

        // 크리티컬 강화
        if (currentMastery.currentMasteryChoices[8] == 1)
        {
            if (masterySet[8, 1] == false)
            {
                statusManager.additionStatus.criticalPercent += 10f;
                statusManager.additionStatus.criticalDamage += 0.3f;

                masterySet[8, 1] = true;
            }
        }
        else
        {
            if (masterySet[8, 1] == true)
            {
                statusManager.additionStatus.criticalPercent -= 10f;
                statusManager.additionStatus.criticalDamage -= 0.3f;
                masterySet[8, 1] = false;
            }
        }
        // HP 20% 이하 물리/마법 공격력 30% 증가
        if (currentMastery.currentMasteryChoices[9] == -1)
        {
            if (masterySet[9, 0] == false)
            {
                rage = true;
                masterySet[9, 0] = true;
            }

            //버그 일어나기 좋음, 적용 안됨

            //if (statusManager.GetCurrentHpPercent() <= 0.2)
            //{
            //    statusManager.multiplicationStatus.attackDamage += 30f;
            //}
            //else
            //{
            //    statusManager.multiplicationStatus.attackDamage -= 30f;
            //}
        }
        else
        {
            if (masterySet[9, 0] == true)
            {
                rage = false;
                masterySet[9, 0] = false;
            }
        }
        //부활 
        //Player.cs 1043
        if (currentMastery.currentMasteryChoices[9] == 1)
        {
            if (masterySet[9, 1] == false)
            {
                resurrection = true;
                masterySet[9, 1] = true;
            }
        }
        else
        {
            if (masterySet[9, 1] == true)
            {
                resurrection = false;
                masterySet[9, 1] = false;
            }
        }
        statusManager.UpdateFinalStatus();
    }

}


