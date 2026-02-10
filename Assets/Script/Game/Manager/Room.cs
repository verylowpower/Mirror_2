
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public static Room instance;
    public static event System.Action<int, int> OnWaveStarted;

    [SerializeField] private GameObject summonPrefab;

    [Header("Room Area")]
    [SerializeField] private Collider2D roomArea;
    [SerializeField] private Transform enemyHolder;

    [Header("Waves in Room")]
    [SerializeField] private List<WaveData> waves;
    [SerializeField] private float spawnInterval = 0.3f;

    public int currentWave = 0;
    private int enemiesAlive = 0;
    private bool activated = false;

    [Header("Gate")]
    public GameObject gate;

    void Awake() => instance = this;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            activated = true;
            StartCoroutine(StartWave(currentWave));
        }
    }

    private IEnumerator StartWave(int waveIndex)
    {
        if (waveIndex >= waves.Count) yield break;
        OnWaveStarted?.Invoke(waveIndex + 1, waves.Count);
        WaveData wave = waves[waveIndex];
        //Debug.Log($"[ROOM] start Wave {waveIndex + 1}");
        foreach (var group in wave.groups)
        {
            var g = group;
            for (int i = 0; i < g.count; i++)
            {
                Vector3 spawnPos = GetRandomPointInsideRoom();
                GameObject summon = Instantiate(summonPrefab, spawnPos, Quaternion.identity, enemyHolder);
                summon.GetComponent<SummonEffect>().Init(() =>
                {
                    GameObject enemy = SpawnManager.instance.SpawnEnemy(g.prefab, spawnPos, enemyHolder);
                    Enemy e = enemy.GetComponent<Enemy>();
                    e.canTakeDamage = false;
                    StartCoroutine(EnableAfterDelay(e, 1f));
                    enemiesAlive++;
                    e.onEnemyDeath += OnEnemyDeath;
                });
                yield return new WaitForSeconds(spawnInterval);
            }
        }

    }

    IEnumerator EnableAfterDelay(Enemy e, float time) //enemy take dmg after summon animation
    {
        yield return new WaitForSeconds(time);
        e.canTakeDamage = true;
    }

    private void OnEnemyDeath()
    {
        enemiesAlive--;
        if (enemiesAlive <= 0)
        {
            currentWave++;
            if (currentWave < waves.Count)
            {
                Debug.Log($"[ROOM] Wave {currentWave} cleared → starting next wave");
                StartCoroutine(StartWave(currentWave));
            }
            else
            {
                Debug.Log("[ROOM] All waves cleared → ROOM CLEARED!");
                RoomCleared();
            }
        }
    }

    private void RoomCleared()
    {
        gate.SetActive(true);
        Debug.Log("Gate Open");
        PlayerBuffManager.instance.OnEnterNewRoom();
    }

    private Vector3 GetRandomPointInsideRoom()
    {
        Bounds b = roomArea.bounds;
        float x = Random.Range(b.min.x, b.max.x);
        float y = Random.Range(b.min.y, b.max.y);
        return new Vector3(x, y, 0);
    }
}
