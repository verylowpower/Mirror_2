using UnityEngine;
using UnityEngine.Tilemaps;
using NavMeshPlus.Components;
using System.Collections;

public class MapController : MonoBehaviour
{
    public TileMapGenerator generator;
    public TileMapRenderer renderer;
    public NavMeshSurface navMeshSurface;
    public Tilemap walkableTilemap;

    private void Start()
    {
        StartCoroutine(GenerateRenderAndBake());
    }

    IEnumerator GenerateRenderAndBake()
    {
        TileMapData mapData = generator.Generate();
        renderer.Render(mapData);
        yield return new WaitUntil(() => walkableTilemap.cellBounds.size.x > 0);
        walkableTilemap.CompressBounds();
        yield return null;
        navMeshSurface.BuildNavMesh();

        Debug.Log("NavMesh baked AFTER tilemap rendered");
    }
}
