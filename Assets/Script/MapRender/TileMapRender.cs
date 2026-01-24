using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapRenderer : MonoBehaviour
{
    [Header("Tilemaps")]
    public Tilemap walkableTilemap;
    public Tilemap blockTilemap;
    public Tilemap decorTilemap;

    [Header("Tiles")]
    public TileBase groundTile;
    public TileBase wallTile;
    public TileBase decorTile;

    TilemapCollider2D blockCollider;

    void Awake()
    {
        blockCollider = blockTilemap.GetComponent<TilemapCollider2D>();
    }

    public void Render(TileMapData mapData)
    {
        walkableTilemap.ClearAllTiles();
        blockTilemap.ClearAllTiles();
        decorTilemap.ClearAllTiles();

        for (int x = 0; x < mapData.width; x++)
        {
            for (int y = 0; y < mapData.height; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                TileData tile = mapData.tiles[x, y];

                if (tile.walkable)
                {
                    walkableTilemap.SetTile(pos, groundTile);
                }
                else
                {
                    blockTilemap.SetTile(pos, wallTile);
                    decorTilemap.SetTile(pos, decorTile);
                }
            }
        }

        walkableTilemap.CompressBounds();
        blockTilemap.CompressBounds();

        ForceRebuildCollider();
    }

    void ForceRebuildCollider()
    {
        if (blockCollider == null) return;

        blockCollider.enabled = false;
        blockCollider.enabled = true;

        blockCollider.ProcessTilemapChanges();

        Debug.Log("Tilemap collider rebuilt");
    }
}
