using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSeeThroughSync : MonoBehaviour
{
    public static int PosID = Shader.PropertyToID("_Position");
    public static int SizeID = Shader.PropertyToID("_Size");

    public Material WallMaterial;
    public Camera Camera;
    public LayerMask Mask;

    private void Start()
    {
        Camera = Camera.main;
    }

    void Update()
    {
        var dir = Camera.transform.position - transform.position;
        var ray = new Ray(transform.position, dir.normalized);

        if (Physics.Raycast(ray, 3000, Mask))
            WallMaterial.SetFloat(SizeID, 1);
        else
            WallMaterial.SetFloat(SizeID, 0);

        var view = Camera.WorldToViewportPoint(transform.position);
        WallMaterial.SetVector(PosID, view);
    }
}
