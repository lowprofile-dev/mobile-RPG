using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public float dungeonWidth;
    public float dungeonLength;
    public Bounds bounds;
    public GameObject plane;
    bool hasPlane;
    public float x, y, z;
    public DunGen.Dungeon dungeon;

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
        }
    }

    private void CreateBoundaryPlane()
    {
        //plane.GetComponent<Plane>().Set3Points(bounds.center, bounds.ClosestPoint(bounds.center), bounds.ClosestPoint(bounds.center));
        plane.transform.position += bounds.center;
        plane.transform.localScale += new Vector3(bounds.size.x/9.5f, 0, bounds.size.z / 9.5f);
        x = bounds.size.x;
        y = bounds.size.y;
        z = bounds.size.z;
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
}
