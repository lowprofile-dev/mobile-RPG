/*
    File StatusManager.cs
    class StatusManager, AdditionStatus, MultiplicationStatus
    
    담당자 : 김기정
    부 담당자 : 김의겸
 */

using CSVReader;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 합연산용 스텟 클래스
/// </summary>
[System.Serializable]
public class AdditionStatus
{
    public float hp;                    //최대 체력 증가
    public float hpRecovery;            //체력 회복량
    public float stamina;               //최대 스태미너 증가
    public float staminaRecovery;       //스태미너 회복량
    public float attackDamage;          //공격력 증가
    public float armor;                 //방어력 증가
    public float rigidresistance;       //경직 cc 저항
    public float stunresistance;        //스턴 cc 저항
    public float fallresistance;        //넘어짐 cc 저항 
    public float criticalPercent;       //크리티컬 확률
    public float criticalDamage;        //크리티컬 데미지
    public float skillCool;             //스킬 쿨타임
    public AdditionStatus()
    {
        hp = 0;                     //최대 체력 증가
        hpRecovery = 0;             //체력 회복량
        stamina = 0;                //최대 스태미너 증가
        staminaRecovery = 0;        //스태미너 회복량
        attackDamage = 0;           //공격력 증가
        armor = 0;                  //방어력 증가
        rigidresistance = 0;        //경직 cc 저항
        stunresistance = 0;         //스턴 cc 저항
        fallresistance = 0;         //넘어짐 cc 저항 
        criticalDamage = 0;         //크리티컬 데미지
        criticalPercent = 0;        //크리티컬 확률
        skillCool = 0;              //스킬 쿨타임 감소
    }
}

/// <summary>
/// 곱연산용 스텟 클래스
/// </summary>
[System.Serializable]
public class MultiplicationStatus
{
    public float hpIncreaseRate;        //체력 % 증가
    public float attackCooldown;        //공격스킬 쿨타임 % 감소
    public float attackDamage;          //공격력 % 증가
    public float armorIncreaseRate;     //방어력 % 증가
    public float moveSpeed;             //이동 속도 % 증가
    public float maxSpeed;
    public float dashCooldown;          //대쉬 스킬 쿨타임 % 감소
    public float dashStamina;           //대쉬 스킬 스태미너 % 감소
    public float criticalDamage;        //크리티컬 데미지 % 증가
    public float criticalPercent;       //크리티컬 확률 % 증가

    public MultiplicationStatus()
    {
        hpIncreaseRate = 0;         //체력 % 증가
        attackDamage = 0;           //공격력 % 증가
        attackCooldown = 0;         //공격스킬 쿨타임 % 감소
        armorIncreaseRate = 0;      //방어력 % 증가
        moveSpeed = 0;              //이동 속도 % 증가
        maxSpeed = moveSpeed;
        dashCooldown = 0;           //대쉬 스킬 쿨타임 % 감소
        dashStamina = 0;            //대쉬 스킬 스태미너 % 감소
        criticalPercent = 1;        //크리티컬 확률 % 증가
        criticalDamage = 1;         //크리티컬 데미지 % 증가
    }
}

/// <summary>
/// 세이브 및 로드, 이런저런 상황에 사용되는 수치들을 종합한다.
/// </summary>
public class StatusManager : SingletonBase<StatusManager>
{
    [SerializeField] Player player;
    [SerializeField] Dictionary<int, StatusData> statusDictionary;
    public CurrentStatus playerStatus;
    public AdditionStatus additionStatus;
    public MultiplicationStatus multiplicationStatus;
    public CurrentStatus finalStatus;

    // 카드 리롤
    public int needToCardRerollCoin;
    public int rerollCount;    

    /// <summary>
    /// 스테이터스 정보들을 초기화한다.
    /// </summary>
    public void InitStatusManager()
    {
        needToCardRerollCoin = 0;
        rerollCount = 0;

        player = Player.Instance;
        playerStatus = new CurrentStatus();
        additionStatus = new AdditionStatus();
        multiplicationStatus = new MultiplicationStatus();
        finalStatus = new CurrentStatus();
        Table statusTable = CSVReader.Reader.ReadCSVToTable("CSVData/StatusDatabase");
        statusDictionary = statusTable.TableToDictionary<int, StatusData>();
        LoadCurrentStatus();
    }

    public void UpdateFinalStatus()
    {
        if (player == null)
        {
            player = Player.Instance;
        }

        finalStatus = (CurrentStatus)playerStatus.Clone();
        AddCurrentStatus();
        MultiplyCurrentStatus();
        SetLimitToStatus();
    }

    private void SetLimitToStatus()
    {
        finalStatus.armor = finalStatus.armor > 80 ? 80 : finalStatus.armor; // 아머는 최대 80
    }

    private void AddCurrentStatus()
    {
        finalStatus.maxHp = playerStatus.maxHp + additionStatus.hp;
        finalStatus.hpRecovery = playerStatus.hpRecovery + additionStatus.hpRecovery;
        finalStatus.maxStamina = playerStatus.maxStamina + additionStatus.stamina;
        finalStatus.staminaRecovery = playerStatus.staminaRecovery + additionStatus.staminaRecovery;
        finalStatus.attackDamage = playerStatus.attackDamage + additionStatus.attackDamage;
        finalStatus.armor = playerStatus.armor + additionStatus.armor;
        finalStatus.rigidresistance = playerStatus.rigidresistance + additionStatus.rigidresistance;
        finalStatus.stunresistance = playerStatus.stunresistance + additionStatus.stunresistance;
        finalStatus.fallresistance = playerStatus.fallresistance + additionStatus.fallresistance;
        finalStatus.criticalDamage = playerStatus.criticalDamage + additionStatus.criticalDamage;
        finalStatus.criticalPercent = playerStatus.criticalPercent + additionStatus.criticalPercent;
        finalStatus.attackCooldown = playerStatus.attackCooldown + additionStatus.skillCool;
    }

    private void MultiplyCurrentStatus()
    {
        finalStatus.maxHp = finalStatus.maxHp * (1 + (multiplicationStatus.hpIncreaseRate / 100.0f));
        finalStatus.attackDamage = finalStatus.attackDamage * (1 + multiplicationStatus.attackDamage / 100.0f);
        finalStatus.armor = finalStatus.armor * (1 + multiplicationStatus.armorIncreaseRate / 100.0f);
        finalStatus.attackCooldown = finalStatus.attackCooldown + (multiplicationStatus.attackCooldown / 100.0f);
        finalStatus.dashCooldown = finalStatus.dashCooldown - (multiplicationStatus.dashCooldown / 100.0f);
        finalStatus.dashStamina = finalStatus.dashStamina * (1 - multiplicationStatus.dashStamina / 100f);
        finalStatus.moveSpeed = finalStatus.moveSpeed * (1 + multiplicationStatus.moveSpeed / 100f);
        finalStatus.criticalPercent = finalStatus.criticalPercent * multiplicationStatus.criticalPercent;
        finalStatus.criticalDamage = finalStatus.criticalDamage * multiplicationStatus.criticalDamage;
    }
    
    private void LoadCurrentStatus()
    {
        PlayerPrefs.SetInt("LoadCurrentStatusCount", PlayerPrefs.GetInt("LoadCurrentStatusCount", 0));
        if (PlayerPrefs.GetInt("LoadCurrentStatusCount") == 0)
        {
           // Debug.Log("최초 스테이터스 데이터 로드 실행입니다.");
            PlayerPrefs.SetInt("LoadCurrentStatusCount", 1);
            SaveCurrentStatus();
            PlayerPrefs.Save();
        }
        string path = Path.Combine(Application.persistentDataPath, "playerCurrentStatus.json");
        string jsonData = File.ReadAllText(path);
        if (jsonData == null) return;
        playerStatus = JsonUtility.FromJson<CurrentStatus>(jsonData);
    }

    private void SaveCurrentStatus()
    {
        string jsonData = JsonUtility.ToJson(playerStatus, true);
        string path = Path.Combine(Application.persistentDataPath, "playerCurrentStatus.json");
        File.WriteAllText(path, jsonData);
    }


    /////////////////// 기타 기능 ////////////////////

    
    /// <summary>
    /// 현재 HP의 %
    /// </summary>
    public float GetCurrentHpPercent()
    {
        if (Player.Instance != null) return Player.Instance.Hp / finalStatus.maxHp;
        return 0f;
    }

    /// <summary>
    /// 현재 스테미너의 %
    /// </summary>
    /// <returns></returns>
    public float GetCurrentSteminaPercent()
    {
        if (Player.Instance != null) return Player.Instance.Stemina / finalStatus.maxStamina;
        return 0f;
    }


    /// <summary>
    /// 스테이터스 계수에 따른 최종 데미지 적용 
    /// </summary>
    public int GetFinalDamageRandomly()
    {
        return (int)UnityEngine.Random.Range(finalStatus.attackDamage * finalStatus.minDamagePer, finalStatus.attackDamage * finalStatus.maxDamagePer);
    }

    /// <summary>
    /// 계수 고정형 최종 데미지 적용
    /// </summary>
    public int GetFinalDamageRandomly(float minDamagePer, float maxDamagePer)
    {
        return (int)UnityEngine.Random.Range(finalStatus.attackDamage * minDamagePer, finalStatus.attackDamage * maxDamagePer);
    }

    /// <summary>
    /// 크리티컬 적용 데미지
    /// </summary>
    public int GetCriticalDamageRandomly()
    {
        return (int)(GetFinalDamageRandomly() * finalStatus.criticalDamage);
    }
}
