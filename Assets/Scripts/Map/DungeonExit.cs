/*
    File DungeonExit.cs
    class DungeonExit
    
    담당자 : 김기정
    부 담당자 :
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonExit : MonoBehaviour
{
    DungeonManager dungeonManager;
    private void Start()
    {
        dungeonManager = GameObject.FindGameObjectWithTag("Dungeon").GetComponent<DungeonManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("출구 충돌!");
        if (other.tag == "Player" && dungeonManager.bossCleared)
        {
            dungeonManager.ClearStage();
        }
    }
}
