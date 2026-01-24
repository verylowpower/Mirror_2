using UnityEngine;

public class TileMapGenerator : MonoBehaviour
{
    public int width = 50;
    public int height = 50;
    public float noiseScale = 0.05f;

    [Range(0f, 1f)]
    public float wallChance = 0.05f;

    [Header("Spawn Safe Area")]
    public Transform spawnPoint;
    public float spawnSafeRadius = 5f;

    public Color mapBound = Color.yellow;

    [Header("QuestData")]
    [SerializeField] QuestData startQuest;

    public TileMapData Generate()
    {
        TileMapData mapData = new TileMapData(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 tileWorldPos = new Vector2(
                    x + transform.position.x,
                    y + transform.position.y
                );

                bool isNearSpawn =
                    spawnPoint != null &&
                    Vector2.Distance(tileWorldPos, spawnPoint.position) < spawnSafeRadius;

                bool walkable;

                if (isNearSpawn)
                {
                    walkable = true;
                }
                else
                {
                    float noise = Mathf.PerlinNoise(
                        tileWorldPos.x * noiseScale,
                        tileWorldPos.y * noiseScale
                    );

                    walkable =
                        noise > 0.55f ||
                        Random.value > wallChance;
                }

                mapData.tiles[x, y] = new TileData
                {
                    type = walkable ? TileType.Ground : TileType.Wall,
                    walkable = walkable
                };
            }
            QuestManager.Instance.StartQuest(startQuest);
        }

        return mapData;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = mapBound;
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.DrawWireCube(
            new Vector3(width * 0.5f, height * 0.5f, 0),
            new Vector3(width, height, 0)
        );

        if (spawnPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(spawnPoint.position, spawnSafeRadius);
        }
    }
}
