using UnityEngine;

public class TileMapGenerator : MonoBehaviour
{
    public int width = 50;
    public int height = 50;
    public float noiseScale = 0.1f;

    public Color mapBound = Color.yellow;

    public TileMapData Generate()
    {
        TileMapData mapData = new TileMapData(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float noise = Mathf.PerlinNoise(
                    x * noiseScale,
                    y * noiseScale
                );

                bool walkable = noise > 0.4f;

                mapData.tiles[x, y] = new TileData
                {
                    type = walkable ? TileType.Ground : TileType.Wall,
                    walkable = walkable
                };
            }
        }

        return mapData;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = mapBound;

        // cho gizmo theo transform
        Gizmos.matrix = transform.localToWorldMatrix;

        Vector3 center = new Vector3(
            width / 2f,
            height / 2f,
            0f
        );

        Vector3 size = new Vector3(
            width,
            height,
            0f
        );

        Gizmos.DrawWireCube(center, size);
    }


}
