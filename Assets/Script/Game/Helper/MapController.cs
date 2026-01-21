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
        // 1️⃣ Generate data
        TileMapData mapData = generator.Generate();

        // 2️⃣ Render tilemap
        renderer.Render(mapData);

        // 3️⃣ ĐỢI tilemap có bounds hợp lệ
        yield return new WaitUntil(() => walkableTilemap.cellBounds.size.x > 0);

        // 4️⃣ Force update bounds
        walkableTilemap.CompressBounds();

        // 5️⃣ ĐỢI thêm 1 frame cho Unity sync renderer
        yield return null;

        // 6️⃣ BAKE
        navMeshSurface.BuildNavMesh();

        Debug.Log("NavMesh baked AFTER tilemap rendered");
    }
}
