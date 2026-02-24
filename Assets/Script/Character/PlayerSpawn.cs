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
        StartCoroutine(SpawnAndLoad(scene.buildIndex));
    }

    IEnumerator SpawnAndLoad(int sceneIndex)
    {
        yield return null;

        if (PlayerController.instance == null ||
            SpawnPoint.instance == null)
            yield break;

        PlayerController.instance.transform.position =
            SpawnPoint.instance.transform.position;
        if (sceneIndex >= 2)
        {
            var progress = SaveLoadManager.Instance.GetProgress();

            if (progress != null && PlayerSnapshot.instance != null)
                PlayerSnapshot.instance.LoadFromProgress(progress);

            if (progress != null && PointCounter.instance != null)
                PointCounter.instance.SetPoint(progress.playerPoint);
        }
    }
}