using CSVReader;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AdditionStatus
{
    public float hp;                    //최대 체력 증가
    public float hpRecovery;            //체력 회복량
    public float stamina;               //최대 스태미너 증가
    public float staminaRecovery;       //스태미너 회복량
    public float attackDamage;          //공격력 증가
    public float armor;                 //방어력 증가
    public float magicResistance;       //마법 방어력 증가
    public float rigidresistance;       //경직 cc 저항
    public float stunresistance;        //스턴 cc 저항
    public float fallresistance;        //넘어짐 cc 저항 

    public AdditionStatus()
    {
        hp = 0;                    //최대 체력 증가
        hpRecovery = 0;            //체력 회복량
        stamina = 0;               //최대 스태미너 증가
        staminaRecovery = 0;       //스태미너 회복량
        attackDamage = 0;          //공격력 증가
        armor = 0;                 //방어력 증가
        magicResistance = 0;       //마법 방어력 증가
        rigidresistance = 0;       //경직 cc 저항
        stunresistance = 0;        //스턴 cc 저항
        fallresistance = 0;        //넘어짐 cc 저항 
    }
}

public class MultiplicationStatus
{
    public float hpIncreaseRate;        //체력 % 증가
    public float attackSpeed;           //공격속도 % 증가
    public float attackCooldown;        //공격스킬 쿨타임 % 감소
    public float armorIncreaseRate;     //방어력 % 증가
    public float moveSpeed;             //이동 속도 % 증가
    public float dashCooldown;          //대쉬 스킬 쿨타임 % 감소
    public float dashStamina;           //대쉬 스킬 스태미너 % 감소

    public MultiplicationStatus()
    {
        hpIncreaseRate = 0;        //체력 % 증가
        attackSpeed = 0;           //공격속도 % 증가
        attackCooldown = 0;        //공격스킬 쿨타임 % 감소
        armorIncreaseRate = 0;     //방어력 % 증가
        moveSpeed = 0;             //이동 속도 % 증가
        dashCooldown = 0;          //대쉬 스킬 쿨타임 % 감소
        dashStamina = 0;           //대쉬 스킬 스태미너 % 감소
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
    public CurrentStatus itemAppliedStatus;
    public CurrentStatus finalStatus;

    // 카드 리롤
    public int cardRerollCoin;
    public int needToCardRerollCoin;
    public int rerollCount;

    /// <summary>
    /// 스테이터스 정보들을 초기화한다.
    /// </summary>
    public void InitStatusManager()
    {
        cardRerollCoin = 10000;
        needToCardRerollCoin = 0;
        rerollCount = 0;

        player = Player.Instance;
        playerStatus = new CurrentStatus();
        additionStatus = new AdditionStatus();
        multiplicationStatus = new MultiplicationStatus();
        itemAppliedStatus = new CurrentStatus();
        finalStatus = new CurrentStatus();
        Table statusTable = CSVReader.Reader.ReadCSVToTable("CSVData/StatusDatabase");
        statusDictionary = statusTable.TableToDictionary<int, StatusData>();
        LoadCurrentStatus();
    }


    void Start()
    {
        //player = Player.Instance;
        //playerStatus = new CurrentStatus();
        //itemAdditionStatus = new AdditionStatus();
        //itemMultiplicationStatus = new MultiplicationStatus();
        //itemAppliedStatus = new CurrentStatus();
        //finalStatus = new CurrentStatus();
        //Table statusTable = CSVReader.Reader.ReadCSVToTable("CSVData/StatusDatabase");
        //statusDictionary = statusTable.TableToDictionary<int, StatusData>();
        //LoadCurrentStatus();
    }

    void Update()
    {
        if (player == null)
        {
            player = Player.Instance;
        }
    }



    public void UpdateFinalStatus()
    {
        AddCurrentStatus();
        MultiplyCurrentStatus();
        SetFinalStatus();
    }

    private void SetFinalStatus()
    {
        finalStatus = itemAppliedStatus;
    }

    private void AddCurrentStatus()
    {
        //hp = 0;                    //최대 체력 증가
        //hpRecovery = 0;            //체력 회복량
        //stamina = 0;               //최대 스태미너 증가
        //staminaRecovery = 0;       //스태미너 회복량
        //attackDamage = 0;          //공격력 증가
        //armor = 0;                 //방어력 증가
        //magicResistance = 0;       //마법 방어력 증가
        //rigidresistance = 0;       //경직 cc 저항
        //stunresistance = 0;        //스턴 cc 저항
        //fallresistance = 0;        //넘어짐 cc 저항 
        itemAppliedStatus.maxHp = playerStatus.maxHp + additionStatus.hp;
        itemAppliedStatus.hpRecovery = playerStatus.hpRecovery + additionStatus.hpRecovery;
        itemAppliedStatus.maxStamina = playerStatus.maxStamina + additionStatus.stamina;
        itemAppliedStatus.staminaRecovery = playerStatus.staminaRecovery + additionStatus.staminaRecovery;
        itemAppliedStatus.attackDamage = playerStatus.attackDamage + additionStatus.attackDamage;
        itemAppliedStatus.armor = playerStatus.armor + additionStatus.armor;
        itemAppliedStatus.magicResistance = playerStatus.magicResistance + additionStatus.magicResistance;
        itemAppliedStatus.rigidresistance = playerStatus.rigidresistance + additionStatus.rigidresistance;
        itemAppliedStatus.stunresistance = playerStatus.stunresistance + additionStatus.stunresistance;
        itemAppliedStatus.fallresistance = playerStatus.fallresistance + additionStatus.fallresistance;
    }

    private void MultiplyCurrentStatus()
    {
        itemAppliedStatus.armor = itemAppliedStatus.armor * (1 + multiplicationStatus.armorIncreaseRate / 100.0f);
        itemAppliedStatus.attackCooldown = itemAppliedStatus.attackCooldown - (multiplicationStatus.armorIncreaseRate / 100.0f);
        itemAppliedStatus.dashCooldown = itemAppliedStatus.dashCooldown - (multiplicationStatus.dashCooldown / 100.0f);
        itemAppliedStatus.dashStamina = itemAppliedStatus.dashStamina * (1 - multiplicationStatus.dashStamina / 100f);
    }

    private void LoadCurrentStatus()
    {
        PlayerPrefs.SetInt("LoadCurrentStatusCount", PlayerPrefs.GetInt("LoadCurrentStatusCount", 0));
        if (PlayerPrefs.GetInt("LoadCurrentStatusCount") == 0)
        {
            Debug.Log("최초 스테이터스 데이터 로드 실행입니다.");
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

    public float GetCurrentHpPercent()
    {
        return Player.Instance.Hp / finalStatus.maxHp;
    }

    public float GetCurrentSteminaPercent()
    {
        return Player.Instance.Stemina / finalStatus.maxStamina;
    }
}
