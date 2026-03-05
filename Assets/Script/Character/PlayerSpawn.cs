using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerSpawner : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(SpawnAtPoint());
    }

    IEnumerator SpawnAtPoint()
    {
        yield return null;
        if (PlayerController.instance == null)
        {
            Debug.LogWarning("PlayerController not found.");
            yield break;
        }
        if (SpawnPoint.instance == null)
        {
            Debug.LogWarning("SpawnPoint not found in scene.");
            yield break;
        }
        PlayerController.instance.transform.position = SpawnPoint.instance.transform.position;
    }
}