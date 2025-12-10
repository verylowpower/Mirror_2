using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Room Area")]
    [SerializeField] private Collider2D roomArea;
    [SerializeField] private Transform enemyHolder;

    [Header("Waves in Room")]
    [SerializeField] private List<WaveData> waves;
    [SerializeField] private float spawnInterval = 0.3f;

    private int currentWave = 0;
    private int enemiesAlive = 0;
    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            activated = true;
            StartCoroutine(StartWave(currentWave));
            Debug.Log("SpawnManager.instance = " + SpawnManager.instance);

        }
    }

    private IEnumerator StartWave(int waveIndex)
    {
        if (waveIndex >= waves.Count) yield break;

        WaveData wave = waves[waveIndex];
        Debug.Log($"[ROOM] Start Wave {waveIndex + 1}");

        foreach (var group in wave.groups)
        {
            for (int i = 0; i < group.count; i++)
            {
                Vector3 spawnPos = GetRandomPointInsideRoom();
                GameObject enemy = SpawnManager.instance.SpawnEnemy(group.prefab, spawnPos, enemyHolder);

                enemiesAlive++;
                enemy.GetComponent<Enemy>().onEnemyDeath += OnEnemyDeath;

                yield return new WaitForSeconds(spawnInterval);
            }
        }
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
        // mở cửa, spawn reward, camera unlock...
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
