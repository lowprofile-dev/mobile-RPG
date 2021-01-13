using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnPoint : MonoBehaviour
{
    [SerializeField] Transform[] points; public Transform[] Points { get { return points; } }
    [SerializeField] GameObject spawnEffect; public GameObject SpawnEffect { get { return spawnEffect; } }

}
