using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DungeonRoom : MonoBehaviour
{
    [Header("던전 방 기본 정보")]
    public int areaCode;
    [SerializeField] bool isCleared = false;
    [SerializeField] bool isSetArea = false;
    [SerializeField] bool hasPlayer = false;

    Bounds bounds;
    Vector2 center;
    DungeonManager dungeonManager;

    [Header("방별 몬스터 스폰 데이터")]
    [SerializeField] List<GameObject> monsterSpawnPoints = new List<GameObject>();
    [SerializeField] List<GameObject> monsterPrefabs = new List<GameObject>();
    [SerializeField] float monsterSpawnInterval = 2.0f;
    [SerializeField] int nMonsterPerSpawn = 1;
    [SerializeField] int nMonsterToSpawn = 7;
    [SerializeField] int nMonsterSpawned = 0;
    [SerializeField] bool isSpawning = false;
    [SerializeField] GameObject doorPrefab;
    private List<GameObject> doorways = new List<GameObject>();
    private List<GameObject> doors = new List<GameObject>();
    private List<GameObject> monsters = new List<GameObject>();
    System.Random random = new System.Random();

    int spawnPointIndex;
    int monsterIndex;

    private void Start()
    {
        dungeonManager = transform.parent.gameObject.GetComponent<DungeonManager>();
        //StartCoroutine(roomSetAreaCodeCoroutine());
        GetMonsterSpawnPoints(transform);
    }

    private void CloseDoors()
    {
        GetDoorways(transform);
        for (int i = 0; i < doorways.Count; i++)
        {
            GameObject door = ObjectPoolManager.Instance.GetObject(doorPrefab);
            door.transform.position = doorways[i].transform.TransformPoint(0, 0, 0);
            door.transform.rotation = doorways[i].transform.rotation;
            door.transform.SetParent(null);
            doors.Add(door);
        }
    }

    private void OpenDoors()
    {
        for (int i = 0; i < doors.Count; i++)
        {
            doors[i].SetActive(false);
        }
        doors.Clear();
    }

    private void GetDoorways(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.gameObject.GetComponent<DunGen.Doorway>() != null)
            {
                doorways.Add(child.gameObject);
            }
            if (child.childCount > 0)
            {
                GetDoorways(child);
            }
        }
    }

    private void GetMonsterSpawnPoints(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == "MonsterSpawnPoint")
            {
                monsterSpawnPoints.Add(child.gameObject);
            }
            if (child.childCount > 0)
            {
                GetMonsterSpawnPoints(child);
            }
        }
    }

    private void Update()
    {
        if (dungeonManager.hasPlane && !isSetArea)
        {
            SetArea();
            //this.enabled = false;
        }
        CheckPlayerInRoom();
    }

    private void CheckPlayerInRoom()
    {
        if (bounds.Contains(dungeonManager.player.GetComponent<CapsuleCollider>().bounds.center))
        {
            hasPlayer = true;
            if (!isCleared && !isSpawning)
            {
                isSpawning = true;
                Invoke("CloseDoors", 2f);
                StartCoroutine(SpawnMonsterCoroutine());
            }
        }
        else
        {
            hasPlayer = false;
        }
    }

    void SetArea()
    {
        bounds = gameObject.GetComponent<BoxCollider>().bounds;
        center = new Vector2(gameObject.GetComponent<BoxCollider>().bounds.center.x, gameObject.GetComponent<BoxCollider>().bounds.center.z);

        for (int i = 1; i <= 3; i++)
        {
            for (int j = 1; j <= 3; j++)
            {
                if ((center.x >= dungeonManager.LT.x + (j - 1) * dungeonManager.dungeonWidth / 3.0f) &&
                    (center.x <= dungeonManager.LT.x + j * dungeonManager.dungeonWidth / 3.0f) &&
                    (center.y <= dungeonManager.LT.y - (i - 1) * dungeonManager.dungeonLength / 3.0f) &&
                    (center.y >= dungeonManager.LT.y - i * dungeonManager.dungeonLength / 3.0f))
                {
                    areaCode = (i - 1) * 3 + j;
                }
            }
        }
        isSetArea = true;
    }

    /// <summary>
    /// 방 클리어 : 닫혀있는 문 개방, 몬스터 스폰 중단.
    /// </summary>
    void ClearRoom()
    {
        isCleared = true;
        ++dungeonManager.nRoomCleared;
        Invoke("OpenDoors", 2);
    }

    private bool KilledAllMonster()
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            if (monsters[i].GetComponent<MonsterAction>().currentState != STATE.STATE_DIE)
            {
                return false;
            }
            if (monsters[i] == null)
            {
                continue;
            }
        }
        return true;
    }

    /// <summary>
    /// 주어진 개수만큼 몬스터 스폰
    /// </summary>
    /// <param name="nMonsters">스폰할 몬스터 개수</param>
    void SpawnMonster(int nMonsters = 1)
    {
        if (nMonsterSpawned >= nMonsterToSpawn && KilledAllMonster())
        {
            ClearRoom();
            return;
        }
        if (nMonsterSpawned >= nMonsterToSpawn)
        {
            return;
        }
        for (int i = 0; i < nMonsters; i++)
        {
            spawnPointIndex = random.Next(monsterSpawnPoints.Count);
            monsterIndex = random.Next(monsterPrefabs.Count);
            var monster = Instantiate(monsterPrefabs[monsterIndex]);
            monster.GetComponent<NavMeshAgent>().enabled = false;
            monster.transform.position = monsterSpawnPoints[spawnPointIndex].transform.TransformPoint(0, 0, 0);
            //monster.transform.SetParent(null);
            monster.GetComponent<NavMeshAgent>().enabled = true;
            nMonsterSpawned += nMonsters;
            monsters.Add(monster);
        }
    }

    IEnumerator SpawnMonsterCoroutine()
    {
        while (!isCleared)
        {
            SpawnMonster(nMonsterPerSpawn);
            yield return new WaitForSeconds(monsterSpawnInterval);
        }
        if (isCleared)
            StopCoroutine(SpawnMonsterCoroutine());
    }

    /// <summary>
    /// 업데이트를 사용하지 않는 코루틴 호출 방식. 현재 사용X
    /// </summary>
    /// <returns></returns>
    IEnumerator roomSetAreaCodeCoroutine()
    {
        while (dungeonManager.hasPlane && !isSetArea) {

            yield return null;
        }
        if (isSetArea)
            StopCoroutine(roomSetAreaCodeCoroutine());
    }
}
