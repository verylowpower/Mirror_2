using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
public class TilemapBoundsGizmo : MonoBehaviour
{
    public Color localBoundsColor = Color.green;
    public Color worldBoundsColor = Color.yellow;

    Tilemap tilemap;
    TilemapRenderer tilemapRenderer;

    void OnEnable()
    {
        tilemap = GetComponent<Tilemap>();
        tilemapRenderer = GetComponent<TilemapRenderer>();
    }

    void OnDrawGizmos()
    {
        if (tilemap == null || tilemapRenderer == null)
            return;

        Gizmos.color = localBoundsColor;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(
            tilemap.localBounds.center,
            tilemap.localBounds.size
        );

        Gizmos.color = worldBoundsColor;
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.DrawWireCube(
            tilemapRenderer.bounds.center,
            tilemapRenderer.bounds.size
        );
    }
}
