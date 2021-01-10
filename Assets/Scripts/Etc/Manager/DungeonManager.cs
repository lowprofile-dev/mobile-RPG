using UnityEngine;
using TMPro;
using System;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] Bounds bounds;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject playerSpawnPoint;
    [SerializeField] GameObject areaPrefab;
    [SerializeField] DunGen.Dungeon dungeon;
    [SerializeField] int dungeonStage = 1;
    [SerializeField] TextMeshProUGUI stageInfo;
    [SerializeField] GameObject[] BossPrefabs;

    public GameObject player;
    public bool hasPlane;
    public Vector2 dungeonCenter;
    public Vector2 LT;
    public float dungeonWidth, dungeonLength, dungeonSize, planeCoef;

    public int nRoomCleared = 0;
    public float nMonsterCoef;
    public bool isStageCleared = false;

    public int playerCurrentArea;

    private GameObject stageExit;

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
    }

    /// <summary>
    /// 스테이지 별 보스 스폰
    /// </summary>
    public GameObject SpawnBoss()
    {
        GameObject bossSpawnPoint = GameObject.FindGameObjectWithTag("BossSpawnPoint");
        GameObject boss = BossPrefabs[dungeonStage-1];
        boss.transform.position = bossSpawnPoint.transform.TransformPoint(0, 1, 0);
        boss.transform.SetParent(null);
        return boss;
    }

    public void ClearStage()
    {
        if (dungeonStage == 4)
        {
            UILoaderManager.Instance.AddScene("VillageScene");
            UILoaderManager.Instance.CloseScene("DungeonScene");

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
        SpawnPlayer();
        isStageCleared = false;
        hasPlane = false;
        dungeonStage++;
        nRoomCleared = 0;
    }

    public void ChangeAreaCheck()
    {
        Debug.Log("current Dungeon : " + playerCurrentArea + "// player Current Dungeon : " + Player.Instance.currentDungeonArea);
        if(playerCurrentArea != Player.Instance.currentDungeonArea)
        {
            if (playerCurrentArea != -1) CardManager.Instance.ExitEffectCards(playerCurrentArea);
            CardManager.Instance.EnterEffectCards(Player.Instance.currentDungeonArea);
            playerCurrentArea = Player.Instance.currentDungeonArea;
            UIManager.Instance.playerUIView.SetEffectList();
        }
    }
}
