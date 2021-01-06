using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] Bounds bounds;
    [SerializeField] GameObject plane;
    [SerializeField] GameObject player;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject playerSpawnPoint;
    [SerializeField] bool hasPlane;
    [SerializeField] float dungeonWidth, dungeonLength, dungeonSize, planeCoef;
    [SerializeField] DunGen.Dungeon dungeon;

    private void Start()
    {
        hasPlane = false;
    }

    private void Update()
    {

        if (dungeon == null)
        {
            dungeon = gameObject.GetComponent<DunGen.Dungeon>();
            
        }
        else if (!hasPlane)
        {
            //FindBoundary();
            bounds = dungeon.Bounds;
            CreateBoundaryPlane();
            SpawnPlayer();
        }
    }

    private void CreateBoundaryPlane()
    {
        //plane.GetComponent<Plane>().Set3Points(bounds.center, bounds.ClosestPoint(bounds.center), bounds.ClosestPoint(bounds.center));
        planeCoef = 1 / 9.5f;
        dungeonWidth = bounds.size.x * planeCoef;
        dungeonLength = bounds.size.z * planeCoef;
        dungeonSize = dungeonLength > dungeonWidth ? dungeonLength : dungeonWidth;

        plane.transform.position += bounds.center;
        plane.transform.localScale += new Vector3(dungeonSize, 0, dungeonSize);
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
        player = Instantiate<GameObject>(playerPrefab, transform);
        player.transform.position = playerSpawnPoint.transform.TransformPoint(0, 1, 0);
        player.transform.SetParent(null);
    }
}
