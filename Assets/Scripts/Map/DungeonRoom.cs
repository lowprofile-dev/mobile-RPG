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
    [SerializeField] bool isBossRoom = false;

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
    private List<GameObject> lights = new List<GameObject>();
    System.Random random = new System.Random();

    int spawnPointIndex;
    int monsterIndex;

    private void Start()
    {
        dungeonManager = transform.parent.gameObject.GetComponent<DungeonManager>();
        //StartCoroutine(roomSetAreaCodeCoroutine());
        GetMonsterSpawnPoints(transform);
        if (dungeonManager.hasPlane && !isSetArea)
        {
            SetArea();
            //this.enabled = false;
        }
    }

    private void ChangeLights()
    {
        GetLights(transform);
        for (int i = 0; i < lights.Count; i++)
        {
            //lights[i].GetComponent<Light>().color
            Color color;
            switch(this.areaCode-1)
            {
                case 0:
                    lights[i].GetComponent<Light>().color = Color.red;
                    break;
                case 1:
                    ColorUtility.TryParseHtmlString("#ff7f00", out color);
                    lights[i].GetComponent<Light>().color = color;
                    break;
                case 2:
                    lights[i].GetComponent<Light>().color = Color.yellow;
                    break;
                case 3:
                    lights[i].GetComponent<Light>().color = Color.green;
                    break;
                case 4:
                    lights[i].GetComponent<Light>().color = Color.blue;
                    break;
                case 5:
                    lights[i].GetComponent<Light>().color = Color.cyan;
                    break;
                case 6:
                    ColorUtility.TryParseHtmlString("#8b00ff", out color);
                    lights[i].GetComponent<Light>().color = color;
                    break;
                case 7:
                    lights[i].GetComponent<Light>().color = Color.white;
                    break;
                case 8:
                    lights[i].GetComponent<Light>().color = Color.black;
                    break;
            }
            lights[i].GetComponent<Light>().intensity = 30;
        }
    }

    private void CloseDoors()
    {
        GetDoorways(transform);
        for (int i = 0; i < doorways.Count; i++)
        {
            //GameObject door = ObjectPoolManager.Instance.GetObject(doorPrefab);
            GameObject door = Instantiate(doorPrefab);
            door.transform.position = doorways[i].transform.TransformPoint(0, 0, 0);
            door.transform.rotation = doorways[i].transform.rotation;
            //door.transform.SetParent(null);
            doors.Add(door);
        }
    }

    private void OpenDoors()
    {
        for (int i = 0; i < doors.Count; i++)
        {
            doors[i].gameObject.SetActive(false);
        }
        //doors.Clear();
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

    private void GetLights(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.gameObject.GetComponent<Light>() != null)
            {
                lights.Add(child.gameObject);
            }
            if (child.childCount > 0)
            {
                GetLights(child);
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
            ChangeLights();
        }
        CheckPlayerInRoom();
        //CheckMonstersInRoom();
    }

    /// <summary>
    /// 방 밖에 생성된 몬스터는 방에 강제 스폰처리!
    /// </summary>
    private void CheckMonstersInRoom()
    {
        for (int i = monsters.Count -1; i >= 0; i--)
        {
            if (monsters[i] == null) continue;
            if (!bounds.Contains(monsters[i].GetComponent<CapsuleCollider>().bounds.center))
            {
                monsters[i].transform.position = monsterSpawnPoints[i].transform.TransformPoint(0, 1, 0);
            }
        }
    }

    private void CheckPlayerInRoom()
    {
        if (bounds.Contains(dungeonManager.player.GetComponent<CapsuleCollider>().bounds.center))
        {
            hasPlayer = true;
            dungeonManager.player.GetComponent<Player>().currentDungeonArea = areaCode;
            if (!isCleared && !isSpawning)
            {
                ItemManager itemManager = ItemManager.Instance;
                if (isBossRoom)
                {
                    //마스터리 스킬 보스 처치시 드랍율 향상
                    if(MasteryManager.Instance.currentMastery.currentMasteryChoices[0] == 1)
                    {
                        if (Player.Instance.masterySet[0] == false)
                        {
                            for (int i = 0; i < itemManager.bossProbability.Length; i++)
                            {
                                itemManager.itemDropProbability[i] = itemManager.bossProbability[i] + 30;
                            }
                            Player.Instance.masterySet[0] = true;
                        }
                        
                    }
                    else
                    {
                        itemManager.itemDropProbability = itemManager.bossProbability;
                    }
                }
                else if (dungeonManager.dungeonStage == 1)
                {
                    //마스터리 스킬 골드, 아이템 획득량 증가
                    if(MasteryManager.Instance.currentMastery.currentMasteryChoices[4]== -1 
                        || MasteryManager.Instance.currentMastery.currentMasteryChoices[4] == 1)
                    {
                        if (Player.Instance.masterySet[4] == false)
                        {
                            for (int i = 0; i < itemManager.itemDropProbability.Length; i++)
                            {
                                itemManager.itemDropProbability[i] = itemManager.stage1Probability[i] * 1.1f;
                            }
                            Player.Instance.masterySet[4] = true;
                        }
                    }
                    else
                    {
                        itemManager.itemDropProbability = itemManager.stage1Probability;
                    }
                }
                else
                {
                    //마스터리 스킬 골드, 아이템 획득량 증가
                    if (MasteryManager.Instance.currentMastery.currentMasteryChoices[4] == -1
                        || MasteryManager.Instance.currentMastery.currentMasteryChoices[4] == 1)
                    {
                        if (Player.Instance.masterySet[4] == false)
                        {
                            for (int i = 0; i < itemManager.itemDropProbability.Length; i++)
                            {
                                itemManager.itemDropProbability[i] = itemManager.stage2Probability[i] * 1.1f;
                            }
                            Player.Instance.masterySet[4] = true;
                        }
                    }
                    else
                    {
                        itemManager.itemDropProbability = itemManager.stage2Probability;
                    }
                }
                isSpawning = true;
                Invoke("CloseDoors", 2f);
                if (isBossRoom)
                {
                    SoundManager.Instance.PlayBGM("BossBGM", 0.6f);
                    monsters.Add(dungeonManager.SpawnBoss());
                    nMonsterSpawned++;
                }
                StartCoroutine(SpawnMonsterCoroutine());
            }
        }
        else
        {
            hasPlayer = false;
            if (isSpawning)
            {
                isSpawning = false;
                OpenDoors();
                StopCoroutine(SpawnMonsterCoroutine());
            }
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
        if (isBossRoom)
        {
            dungeonManager.bossCleared = true;
            SoundManager.Instance.PlayBGM("DungeonBGM", 0.6f);
        }
        Invoke("OpenDoors", 2);
    }

    private bool KilledAllMonster()
    {
        for (int i = monsters.Count - 1; i > -1; i--)
        {
            if (monsters[i] == null)
                monsters.RemoveAt(i);
        }
        for (int i = 0; i < monsters.Count; i++)
        {
            if (monsters[i].GetComponent<MonsterAction>().currentState != MONSTER_STATE.STATE_DIE)
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
            CheckMonstersInRoom();
            return;
        }
        //spawnPointIndex = random.Next(monsterSpawnPoints.Count);
        //monsterIndex = random.Next(monsterPrefabs.Count);
        //var monster = Instantiate(monsterPrefabs[monsterIndex]);
        //monster.GetComponent<NavMeshAgent>().enabled = false;
        //monster.transform.position = monsterSpawnPoints[spawnPointIndex].transform.TransformPoint(0, 0, 0);
        //monster.transform.SetParent(null);
        //monster.GetComponent<NavMeshAgent>().enabled = true;
        //nMonsterSpawned += 1;
        //monsters.Add(monster);
        for (int i = 0; i < nMonsters; i++)
        {
            spawnPointIndex = random.Next(monsterSpawnPoints.Count);
            //spawnPointIndex = i;
            monsterIndex = random.Next(monsterPrefabs.Count);
            var monster = Instantiate(monsterPrefabs[monsterIndex]);
            monster.GetComponent<NavMeshAgent>().enabled = false;
            monster.transform.position = monsterSpawnPoints[spawnPointIndex].transform.TransformPoint(0, 1, 0);
            //monster.transform.SetParent(null);
            monster.GetComponent<NavMeshAgent>().enabled = true;
            nMonsterSpawned += 1;
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
