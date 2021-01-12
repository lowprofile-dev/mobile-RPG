4using UnityEngine;
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
    public int dungeonStage = 1;
    [SerializeField] TextMeshProUGUI stageInfo;
    [SerializeField] GameObject[] BossPrefabs;

    [SerializeField] GameObject[] BossEffects;

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

    //디버깅용
    GameObject plane;

    private void Start()
    {
        playerCurrentArea = -1;
        CardManager.Instance._cntDungeon = this;
        hasPlane = false;
    }

    private void Update()
    {
        if (player == null)
        {
            GameObject.FindGameObjectWithTag("Player");
        }
        InitDungeon();
        SetStageInfo();
        //Invoke("ChangeAreaCheck", 5f);
        ChangeAreaCheck();
        if (isStageCleared)
        {
            ClearStage();
        }
    }

    private void SetStageInfo()
    {
        stageInfo.text = dungeonStage + " - " + nRoomCleared;
    }

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

    private void CreateBoundaryPlane()
    {
        //Bounds에서 받아온 값을 Plane.localScale로 전환하는데 쓰이는 계수
        planeCoef = 1 / 9.5f;

        plane.transform.position += bounds.center;
        plane.transform.localScale += new Vector3(bounds.size.x * planeCoef, 0, bounds.size.z * planeCoef);
        hasPlane = true;
    }

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
    public GameObject SpawnBoss()
    {
        GameObject bossSpawnPoint = GameObject.FindGameObjectWithTag("BossSpawnPoint");
        for (int i = 0; i < BossEffects.Length; i++)
        {
            ObjectPoolManager.Instance.InitObject(BossEffects[i], 2, bossSpawnPoint.transform);
        }
        GameObject boss = Instantiate(BossPrefabs[dungeonStage-1]);
        boss.GetComponent<NavMeshAgent>().enabled = false;
        boss.transform.position = bossSpawnPoint.transform.TransformPoint(0, 0, 0);
        boss.transform.SetParent(null);
        boss.GetComponent<NavMeshAgent>().enabled = true;
        return boss;
    }

    public void ClearStage()
    {
        if (dungeonStage == 4)
        {          
            UILoaderManager.Instance.LoadVillage();
            CardManager.Instance._cntDungeon = null;
            CardManager.Instance.currentStage = 0;
            return;
        }

        CardManager.Instance.currentStage = dungeonStage + 1;
        UINaviationManager.Instance.PushToNav("SubUI_CardUIView");
    }


    public void ToNextStage()
    {
        var runtimeDungeon = FindObjectOfType<DunGen.RuntimeDungeon>();
        runtimeDungeon.Generate();
        isPlayerSpawned = false;
        SpawnPlayer();
        isStageCleared = false;
        hasPlane = false;
        dungeonStage++;
        nRoomCleared = 0;
    }

    public void ChangeAreaCheck()
    {
        if (!isPlayerSpawned) return;
        Debug.Log("current Dungeon : " + playerCurrentArea + "// player Current Dungeon : " + Player.Instance.currentDungeonArea);
        if(playerCurrentArea != Player.Instance.currentDungeonArea)
        {
            if (playerCurrentArea > 0) CardManager.Instance.ExitEffectCards(playerCurrentArea - 1);
            CardManager.Instance.EnterEffectCards(Player.Instance.currentDungeonArea - 1);
            playerCurrentArea = Player.Instance.currentDungeonArea;
            UIManager.Instance.playerUIView.SetEffectList();
        }
    }
}
