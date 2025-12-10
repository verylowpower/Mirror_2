using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    private void Awake() => instance = this;

    public GameObject SpawnEnemy(GameObject prefab, Vector3 pos, Transform parent = null)
    {
        return Instantiate(prefab, pos, Quaternion.identity, parent);
    }
}
