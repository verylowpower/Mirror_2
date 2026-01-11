using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathHandler : MonoBehaviour
{
    public static DeathHandler instance;

    [SerializeField] private GameObject deathScreen;
    [SerializeField] private float delayBeforeMenu = 3f;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void HandlePlayerDeath()
    {
        deathScreen.SetActive(true);
        Time.timeScale = 0;

        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSecondsRealtime(delayBeforeMenu);

        CleanupPersistentObjects();

        Time.timeScale = 1;
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    private void CleanupPersistentObjects()
    {
        var persistents = FindObjectsOfType<PersistentObject>();
        foreach (var obj in persistents)
        {
            Destroy(obj.gameObject);
        }
    }
}
