using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] Bounds bounds;
    [SerializeField] GameObject plane;
    [SerializeField] GameObject player;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject playerSpawnPoint;
    public float dungeonWidth, dungeonLength, dungeonSize, planeCoef;
    [SerializeField] DunGen.Dungeon dungeon;

    [SerializeField] GameObject areaPrefab;

    public bool hasPlane;
    public Vector2 dungeonCenter;
    public Vector2 LT;

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

            dungeonCenter = new Vector2(bounds.center.x, bounds.center.z);
            dungeonWidth = bounds.size.x;
            dungeonLength = bounds.size.z;
            //LT = new Vector2(dungeonCenter.x - dungeonLength / 2, dungeonCenter.y + dungeonWidth / 2);
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
        player = ObjectPoolManager.Instance.GetObject(playerPrefab);
        player.transform.position = playerSpawnPoint.transform.TransformPoint(0, 1, 0);
        player.transform.SetParent(null);
    }

    public void SetAreaCode(Vector2 roomCenter, out int areaCode)
    {
        for (int i = 1; i <= 3; i++)
        {
            for (int j = 1; j <= 3; j++)
            {
                if ((roomCenter.x > LT.x + (j - 1) * dungeonWidth / 3) &&
                    (roomCenter.x <= LT.x + j * dungeonWidth / 3) &&
                    (roomCenter.y < LT.y - (i - 1) * dungeonLength / 3) &&
                    (roomCenter.y >= LT.y - i * dungeonLength / 3))
                {
                    areaCode = (i - 1) * 3 + j;
                    return;
                }
            }
        }
        areaCode = 0;
    }
}
