/*
    File DungeonManager.cs
    class DungeonManager
    
    담당자 : 김기정
    부 담당자 : 안영훈
 */

using UnityEngine;
using TMPro;
using System;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] Bounds bounds;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject playerSpawnPoint;
    [SerializeField] GameObject areaPrefab;
    [SerializeField] DunGen.Dungeon dungeon;
    [SerializeField] TextMeshProUGUI stageInfo;
    [SerializeField] GameObject[] BossPrefabs;
    [SerializeField] List<GameObject> stage1MonsterPrefabs = new List<GameObject>();
    [SerializeField] List<GameObject> stage2MonsterPrefabs = new List<GameObject>();
    [SerializeField] List<GameObject> stage3MonsterPrefabs = new List<GameObject>();
    [SerializeField] List<GameObject> stage4MonsterPrefabs = new List<GameObject>();
    private List<List<GameObject>> stagesMonsterPrefabs = new List<List<GameObject>>();
    public List<GameObject> currentStageMonsterPrefabs = new List<GameObject>();

    public int dungeonStage = 1;
    public GameObject player;
    public bool hasPlane;
    public Vector2 dungeonCenter;
    public Vector2 LT;
    public float dungeonWidth, dungeonLength, dungeonSize, planeCoef;

    public int nRoomCleared = 0;
    public float nMonsterCoef;
    public bool isStageCleared = false;
    public bool bossCleared = false;

    public int playerCurrentArea;

    private GameObject stageExit;
    bool isPlayerSpawned = false;
    DunGen.RuntimeDungeon runtimeDungeon;

    //디버깅용
    GameObject plane;

    private void Start()
    {
        playerCurrentArea = -1;
        CardManager.Instance.cntDungeon = this;
        hasPlane = false;
        stagesMonsterPrefabs.Add(stage1MonsterPrefabs);
        stagesMonsterPrefabs.Add(stage2MonsterPrefabs);
        stagesMonsterPrefabs.Add(stage3MonsterPrefabs);
        stagesMonsterPrefabs.Add(stage4MonsterPrefabs);
        currentStageMonsterPrefabs = stagesMonsterPrefabs[0];
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Dungeon/DungeonFloorStart", 0.9f);
        runtimeDungeon = FindObjectOfType<DunGen.RuntimeDungeon>();
    }

    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        InitDungeon();
        SetStageInfo();
        ChangeAreaCheck();
        if (isStageCleared)
        {
            ClearStage();
        }
    }

    private void SetStageInfo()
    {
        stageInfo.text = "스테이지 " + dungeonStage + " - " + playerCurrentArea + "구역";
    }

    /// <summary>
    /// 던전 초기화 및 맵 재생성
    /// </summary>
    private void InitDungeon()
    {
        if (dungeon == null)
        {
            dungeon = gameObject.GetComponent<DunGen.Dungeon>();
            nRoomCleared = 0;
            stageExit = GameObject.FindGameObjectWithTag("DungeonExit");
        }
        else if (!hasPlane)
        {
            //FindBoundary();
            bounds = dungeon.Bounds;
            //CreateBoundaryPlane();
            hasPlane = true;
            SpawnPlayer();

            dungeonCenter = new Vector2(bounds.center.x, bounds.center.z);
            dungeonWidth = bounds.size.x;
            dungeonLength = bounds.size.z;
            LT = new Vector2(dungeonCenter.x - bounds.extents.x, dungeonCenter.y + bounds.extents.z);
        }
    }

    //deprecated
    private void CreateBoundaryPlane()
    {
        //Bounds에서 받아온 값을 Plane.localScale로 전환하는데 쓰이는 계수
        planeCoef = 1 / 9.5f;

        plane.transform.position += bounds.center;
        plane.transform.localScale += new Vector3(bounds.size.x * planeCoef, 0, bounds.size.z * planeCoef);
        hasPlane = true;
    }

    //deprecated
    private void FindBoundary()
    {
        bounds = gameObject.GetComponent<Collider>().bounds;
        foreach (Transform child in transform)
        {
            bounds.Encapsulate(child.gameObject.GetComponent<Collider>().bounds);
        }
    }

    private void SpawnPlayer()
    {
        playerSpawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawnPoint");
        if (player != null)
        {
            Destroy(player);
        }
        player = Instantiate(playerPrefab);
        player.transform.position = playerSpawnPoint.transform.TransformPoint(0, 1, 0);
        player.transform.SetParent(null);
        player.GetComponent<Player>().currentDungeonArea = transform.GetChild(0).gameObject.GetComponent<DungeonRoom>().areaCode;
        playerCurrentArea = player.GetComponent<Player>().currentDungeonArea;
        isPlayerSpawned = true;
    }

    /// <summary>
    /// 스테이지 별 보스 스폰
    /// </summary>
    public GameObject SpawnBoss(Transform tr)
    {
        GameObject boss = ObjectPoolManager.Instance.GetObject(BossPrefabs[dungeonStage - 1]);
        //Instantiate(BossPrefabs[dungeonStage-1]);        
        return boss;
    }

    public void ClearStage()
    {
        bossCleared = false;
        if (dungeonStage == 4)
        {          
            UILoaderManager.Instance.LoadVillage();
            CardManager.Instance.cntDungeon = null;
            CardManager.Instance.currentFloor = 0;
            TalkManager.Instance.SetQuestCondition(3, 1, 1);
            return;
        }
        
        if(!CardManager.Instance.isAcceptCardData)
        {
            CardManager.Instance.isAcceptCardData = true;
            CardManager.Instance.currentFloor = dungeonStage;
            UINaviationManager.Instance.PushToNav("SubUI_CardUIView");
        }
    }


    public void ToNextStage()
    {
        runtimeDungeon.Generate();
        isPlayerSpawned = false;
        SpawnPlayer();
        isStageCleared = false;
        hasPlane = false;
        dungeonStage++;
        nRoomCleared = 0;
        currentStageMonsterPrefabs = stagesMonsterPrefabs[dungeonStage-1];
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Dungeon/DungeonFloorStart", 0.9f);
    }

    public void ChangeAreaCheck()
    {
        if (!isPlayerSpawned) return;
        if(playerCurrentArea != Player.Instance.currentDungeonArea)
        {
            if (playerCurrentArea > 0) CardManager.Instance.ExitEffectCards(playerCurrentArea - 1);
            CardManager.Instance.EnterEffectCards(Player.Instance.currentDungeonArea - 1);
            playerCurrentArea = Player.Instance.currentDungeonArea;
            UIManager.Instance.playerUIView.SetEffectList();
        }
    }
}
