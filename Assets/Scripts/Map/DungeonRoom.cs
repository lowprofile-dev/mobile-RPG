using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoom : MonoBehaviour
{
    public int areaCode;
    Bounds bounds;
    Vector2 center;
    DungeonManager dungeonManager;
    bool isSetArea = false;

    public Vector2 DungeonCenter;
    public Vector2 DungeonLT;
    public int iidx, jidx;
    public float dungeonLength, dungeonWidth;

    private void Start()
    {
        dungeonManager = transform.parent.gameObject.GetComponent<DungeonManager>();
        //StartCoroutine(roomSetAreaCodeCoroutine());
    }

    private void Update()
    {
        if (dungeonManager.hasPlane && !isSetArea)
        {
            SetArea();
            //this.enabled = false;
        }
    }

    void SetArea()
    {
        bounds = gameObject.GetComponent<BoxCollider>().bounds;
        center = new Vector2(gameObject.GetComponent<BoxCollider>().bounds.center.x, gameObject.GetComponent<BoxCollider>().bounds.center.z);
        //center = new Vector2(gameObject.GetComponent<BoxCollider>().center.x, gameObject.GetComponent<BoxCollider>().center.z);
        //dungeonManager.SetAreaCode(center, out areaCode);
        DungeonCenter = dungeonManager.dungeonCenter;
        DungeonLT = dungeonManager.LT;
        dungeonWidth = dungeonManager.dungeonWidth;
        dungeonLength = dungeonManager.dungeonLength;

        for (int i = 1; i <= 3; i++)
        {
            for (int j = 1; j <= 3; j++)
            {
                if ((center.x >= dungeonManager.LT.x + (j - 1) * dungeonManager.dungeonWidth / 3.0f) &&
                    (center.x <= dungeonManager.LT.x + j * dungeonManager.dungeonWidth / 3.0f) &&
                    (center.y <= dungeonManager.LT.y - (i - 1) * dungeonManager.dungeonLength / 3.0f) &&
                    (center.y >= dungeonManager.LT.y - i * dungeonManager.dungeonLength / 3.0f))
                {
                    iidx = i;
                    jidx = j;
                    areaCode = (i - 1) * 3 + j;
                }
            }
        }
        isSetArea = true;
    }

    IEnumerator roomSetAreaCodeCoroutine()
    {
        while (dungeonManager.hasPlane && !isSetArea) {

            yield return null;
        }
        if (isSetArea)
            StopCoroutine(roomSetAreaCodeCoroutine());
    }
}
