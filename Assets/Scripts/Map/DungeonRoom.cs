/*
    File DungeonRoom.cs
    class DungeonRoom
    
    담당자 : 김기정
    부 담당자 : 안영훈
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class DungeonRoom : MonoBehaviour
{
    [Header("던전 방 기본 정보")]
    public int areaCode;
    [SerializeField] bool isCleared = false;
    [SerializeField] bool isSetArea = false;
    [SerializeField] bool hasPlayer = false;
    [SerializeField] bool isBossRoom = false;

    bool isClosedPrev = false;
    bool isMinimapSet = false;

    Bounds bounds;
    Vector2 center;
    DungeonManager dungeonManager; public DungeonManager DungeonManager { get { return dungeonManager; } }

    [Header("방별 몬스터 스폰 데이터")]
    [SerializeField] List<GameObject> monsterSpawnPoints = new List<GameObject>();
    [SerializeField] List<GameObject> monsterPrefabs = new List<GameObject>();
    [SerializeField] float monsterSpawnInterval = 2.0f;
    [SerializeField] int nMonsterPerSpawn = 1;
    [SerializeField] int nMonsterToSpawn = 7;
    [SerializeField] int nMonsterSpawned = 0;
    [SerializeField] int nMonsterAlive = 0;
    [SerializeField] bool isSpawning = false;
    [SerializeField] GameObject doorPrefab;
    [SerializeField] GameObject bossSpawnPoint;
    private List<GameObject> doorways = new List<GameObject>();
    private List<GameObject> doors = new List<GameObject>();
    private List<GameObject> monsters = new List<GameObject>(); public List<GameObject> MonsterList { get { return monsters; } }
    private List<GameObject> lights = new List<GameObject>();
    private List<GameObject> minimapIcons = new List<GameObject>();
    System.Random random = new System.Random();

    int spawnPointIndex = 0;
    int monsterIndex;

    private void Start()
    {
        dungeonManager = transform.parent.gameObject.GetComponent<DungeonManager>();
        GetMonsterSpawnPoints(transform);
        GetBossSpawnPoint(transform);
        GetMinimapImages(transform);
        if (dungeonManager.hasPlane && !isSetArea)
        {
            SetArea();
        }
    }

    private void GetMinimapImages(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.gameObject.layer == 14 && child.gameObject.GetComponent<Image>() != null)
            {
                minimapIcons.Add(child.gameObject);
            }
            if (child.childCount > 0)
            {
                GetMinimapImages(child);
            }
        }
    }

    private void GetBossSpawnPoint(Transform parent)
    {
        if (!isBossRoom) return;
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == "BossSpawnPoint")
            {
                bossSpawnPoint = child.gameObject;
                return;
            }
            if (child.childCount > 0)
            {
                GetBossSpawnPoint(child);
            }
        }
    }

    private void OnDestroy()
    {
        for (int i = monsters.Count - 1; i >= 0; i--)
        {
            if (monsters[i] == null) continue;
            monsters[i].GetComponent<MonsterAction>().parentRoom = null;
            ObjectPoolManager.Instance.ReturnObject(monsters[i]);
            monsters[i].GetComponent<MonsterAction>().ChangeState(MONSTER_STATE.STATE_IDLE);
            
        }
        monsters.Clear();
        for (int i = doors.Count - 1; i >= 0; i--)
        {
            ObjectPoolManager.Instance.ReturnObject(doors[i]);
        }
        doors.Clear();
    }

    private void ChangeLights()
    {
        GetLights(transform);
        for (int i = 0; i < lights.Count; i++)
        {
            Color color;
            switch (areaCode - 1)
            {
                case 0:
                    lights[i].GetComponent<Light>().color = Color.red;
            lights[i].GetComponent<Light>().intensity = 8f;
                    break;
                case 1:
                    ColorUtility.TryParseHtmlString("#ff7f00", out color);
                    lights[i].GetComponent<Light>().color = color;
            lights[i].GetComponent<Light>().intensity = 6f;
                    break;
                case 2:
                    lights[i].GetComponent<Light>().color = Color.yellow;
            lights[i].GetComponent<Light>().intensity = 5f;
                    break;
                case 3:
                    lights[i].GetComponent<Light>().color = Color.green;
            lights[i].GetComponent<Light>().intensity = 4;
                    break;
                case 4:
                    lights[i].GetComponent<Light>().color = new Color(0, 0.0978f, 0.5943f);
                    lights[i].GetComponent<Light>().intensity = 12f;
                    break;
                case 5:
                    lights[i].GetComponent<Light>().color = new Color(0, 0.6427f, 1);
            lights[i].GetComponent<Light>().intensity = 8f;
                    break;
                case 6:
                    ColorUtility.TryParseHtmlString("#8b00ff", out color);
                    lights[i].GetComponent<Light>().color = color;
            lights[i].GetComponent<Light>().intensity = 7f;
                    break;
                case 7:
                    lights[i].GetComponent<Light>().color = Color.white;
            lights[i].GetComponent<Light>().intensity = 12f;
                    break;
                case 8:
                    lights[i].GetComponent<Light>().color = Color.black;
            lights[i].GetComponent<Light>().intensity = 12f;
                    break;
            }
            lights[i].GetComponent<Light>().range = 15;
            if (lights[i].GetComponent<Animator>()) lights[i].GetComponent<Animator>().enabled = false;
        }
    }

    private void CloseDoors()
    {
        GetDoorways(transform);
        for (int i = 0; i < doorways.Count; i++)
        {
            //GameObject door = Instantiate(doorPrefab);
            GameObject door = ObjectPoolManager.Instance.GetObject(doorPrefab);
            door.transform.position = doorways[i].transform.TransformPoint(0, 0, 0);
            door.transform.rotation = doorways[i].transform.rotation;
            doors.Add(door);
        }

        AudioSource source = SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Dungeon/DungeonRoomStart", 0.9f);
        SoundManager.Instance.SetAudioReverbEffect(source, AudioReverbPreset.Cave);
        isClosedPrev = true;
    }

    private void OpenDoors()
    {
        for (int i = 0; i < doors.Count; i++)
        {
            //doors[i].gameObject.SetActive(false);
            ObjectPoolManager.Instance.ReturnObject(doors[i].gameObject);
        }
        doors.Clear();

        if (isClosedPrev)
        {
            AudioSource source = SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Dungeon/DungeonRoomEnd", 0.9f);
            SoundManager.Instance.SetAudioReverbEffect(source, AudioReverbPreset.Cave);
            isClosedPrev = false;
        }
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

    private void SetMinimapColor()
    {
        isMinimapSet = true;
        GetMinimapImages(transform);
        for (int i = 0; i < minimapIcons.Count; i++)
        {
            Color color;
            switch (areaCode - 1)
            {
                case 0:
                    minimapIcons[i].GetComponent<Image>().color = Color.red;
                    break;
                case 1:
                    ColorUtility.TryParseHtmlString("#ff7f00", out color);
                    minimapIcons[i].GetComponent<Image>().color = color;
                    break;
                case 2:
                    minimapIcons[i].GetComponent<Image>().color = Color.yellow;
                    break;
                case 3:
                    minimapIcons[i].GetComponent<Image>().color = Color.green;
                    break;
                case 4:
                    minimapIcons[i].GetComponent<Image>().color = Color.blue;
                    break;
                case 5:
                    minimapIcons[i].GetComponent<Image>().color = Color.cyan;
                    break;
                case 6:
                    ColorUtility.TryParseHtmlString("#8b00ff", out color);
                    minimapIcons[i].GetComponent<Image>().color = color;
                    break;
                case 7:
                    minimapIcons[i].GetComponent<Image>().color = Color.white;
                    break;
                case 8:
                    minimapIcons[i].GetComponent<Image>().color = Color.black;
                    break;
            }
            Color minimapColor = minimapIcons[i].GetComponent<Image>().color;
            minimapColor.a = 0.7f;
            minimapIcons[i].GetComponent<Image>().color = minimapColor;
            minimapIcons[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (dungeonManager.hasPlane && !isSetArea)
        {
            SetArea();
            ChangeLights();
            SetMinimapColor();
        }
        if (!isMinimapSet)
            SetMinimapColor();
        CheckPlayerInRoom();
    }

    public void CheckClear()
    {
        if (nMonsterSpawned >= nMonsterToSpawn && KilledAllMonster())
        {
            ClearRoom();
            return;
        }
    }

    /// <summary>
    /// 방 밖에 생성된 몬스터는 방에 강제 스폰처리!
    /// </summary>
    private void CheckMonstersInRoom()
    {
        for (int i = monsters.Count - 1; i >= 0; i--)
        {
            if (monsters[i] == null) continue;
            if (!bounds.Contains(monsters[i].GetComponent<CapsuleCollider>().bounds.center) || monsters[i].transform.position.y < Player.Instance.transform.position.y - 12)
            {
                //monsters[i].GetComponent<NavMeshAgent>().enabled = false;
                //monsters[i].transform.position = monsterSpawnPoints[i].transform.TransformPoint(0, 1, 0);
                //monsters[i].GetComponent<NavMeshAgent>().enabled = true;
                //monsters[i].GetComponent<Collider>().enabled = true;
                //monsters[i].GetComponent<EPOOutline.Outlinable>().enabled = true;

                monsters[i].GetComponent<MonsterAction>().ChangeState(MONSTER_STATE.STATE_DIE);
                // DEATH로 강제 Change 시킨다.
            }
        }
    }

    private void CheckPlayerInRoom()
    {
        if (Player.Instance == null) return;
        if (bounds.Contains(dungeonManager.player.GetComponent<CapsuleCollider>().bounds.center))
        {
            hasPlayer = true;
            for (int i = 0; i < minimapIcons.Count; i++)
            {
                minimapIcons[i].SetActive(true);
            }
            if (monsterSpawnPoints.Count > 0)
                dungeonManager.player.GetComponent<Player>().lastRoomTransformPos = monsterSpawnPoints[0].transform.TransformPoint(0, 1, 0);
            dungeonManager.player.GetComponent<Player>().currentDungeonArea = areaCode;
            if (!isCleared && !isSpawning)
            {
                ItemManager itemManager = ItemManager.Instance;
                /*
                if (isBossRoom)
                {
                    //마스터리 스킬 보스 처치시 드랍율 향상
                    if (MasteryManager.Instance.currentMastery.currentMasteryChoices[0] == 1)
                    {
                        if (MasteryManager.Instance.masterySet[0,1] == false)
                        {
                            for (int i = 0; i < itemManager.bossProbability.Length; i++)
                            {
                                itemManager.itemDropProbability[i] = itemManager.bossProbability[i] + 30;
                            }
                            MasteryManager.Instance.masterySet[0,1] = true;
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
                    if (MasteryManager.Instance.currentMastery.currentMasteryChoices[4] == -1
                        || MasteryManager.Instance.currentMastery.currentMasteryChoices[4] == 1)
                    {
                        if (MasteryManager.Instance.masterySet[4,0] == false)
                        {
                            for (int i = 0; i < itemManager.itemDropProbability.Length; i++)
                            {
                                itemManager.itemDropProbability[i] = itemManager.stage1Probability[i] * 1.1f;
                            }
                            MasteryManager.Instance.masterySet[4,0] = true;
                        }
                    }
                    else
                    {
                        itemManager.itemDropProbability = itemManager.stage1Probability;
                    }
                }
                else
                {
                */
                    //마스터리 스킬 골드, 아이템 획득량 증가
                    if (MasteryManager.Instance.currentMastery.currentMasteryChoices[4] == -1
                        || MasteryManager.Instance.currentMastery.currentMasteryChoices[4] == 1)
                    {
                        if (MasteryManager.Instance.masterySet[4,0] == false)
                        {
                            for (int i = 0; i < itemManager.itemDropProbability.Length; i++)
                            {
                                itemManager.itemDropProbability[i] = itemManager.stage2Probability[i] * 1.2f;
                            }
                            MasteryManager.Instance.masterySet[4,0] = true;
                        }
                    }
                    else
                    {
                        itemManager.itemDropProbability = itemManager.stage2Probability;
                    }
                //}

                isSpawning = true;
                Invoke("CloseDoors", 1f);
                if (isBossRoom)
                {
                    Invoke("SpawnBoss", 1f);
                }
                else
                    StartCoroutine(SpawnMonsterCoroutine());
            }
        }
        else
        {
            hasPlayer = false;
            OpenDoors();
        }
    }

    private void SpawnBoss()
    {
        if (nMonsterSpawned >= nMonsterToSpawn)
        {
            return;
        }
        SoundManager.Instance.PlayBGM("BossBGM", 0.6f);
        GameObject boss = dungeonManager.SpawnBoss(bossSpawnPoint.transform);
        boss.GetComponent<NavMeshAgent>().enabled = false;
        boss.transform.position = bossSpawnPoint.transform.TransformPoint(0, 0, 0);
        boss.GetComponent<NavMeshAgent>().enabled = true;
        boss.GetComponent<MonsterAction>().parentRoom = this;
        boss.GetComponent<Monster>().InitMonster();
        boss.transform.LookAt(Player.Instance.gameObject.transform);
        boss.transform.SetParent(null);
        monsters.Add(boss);
        nMonsterSpawned++;
        CameraManager.Instance.CameraSetTarget(boss);
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
        OpenDoors();
    }

    private bool KilledAllMonster()
    {
        if (nMonsterSpawned >= nMonsterToSpawn && nMonsterAlive <= 0)
            return true;
        return false;
    }

    /// <summary>
    /// 주어진 개수만큼 몬스터 스폰
    /// </summary>
    /// <param name="nMonsters">스폰할 몬스터 개수</param>
    void SpawnMonster(int nMonsters = 1)
    {
        if (nMonsterSpawned >= nMonsterToSpawn)
        {
            CheckMonstersInRoom();
            return;
        }
        for (int i = 0; i < nMonsters; i++)
        {
            nMonsterAlive++;
            spawnPointIndex = random.Next(monsterSpawnPoints.Count);
            monsterIndex = random.Next(dungeonManager.currentStageMonsterPrefabs.Count);
            GameObject monster = ObjectPoolManager.Instance.GetObject(dungeonManager.currentStageMonsterPrefabs[monsterIndex]);
            monster.GetComponent<NavMeshAgent>().enabled = false;
            monster.transform.position = monsterSpawnPoints[spawnPointIndex].transform.TransformPoint(0, 0, 0);
            monster.GetComponent<NavMeshAgent>().enabled = true;
            monster.GetComponent<MonsterAction>().parentRoom = this;
            monster.GetComponent<Monster>().InitMonster();
            nMonsterSpawned += 1;
            monster.GetComponent<MonsterAction>().SpawnPos = monsterSpawnPoints[spawnPointIndex].transform.TransformPoint(0, 0, 0);
            monster.GetComponent<Collider>().enabled = true;
            monster.GetComponent<EPOOutline.Outlinable>().enabled = true;
            monster.SetActive(true);
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
        while (dungeonManager.hasPlane && !isSetArea)
        {

            yield return null;
        }
        if (isSetArea)
            StopCoroutine(roomSetAreaCodeCoroutine());
    }

    /// <summary>
    /// 몬스터가 사망했다는 것을 알림
    /// </summary>
    public void MonsterDeathCheck()
    {
        nMonsterAlive--;
        CheckClear();
    }
}
