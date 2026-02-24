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
        if (EndingManager.instance != null && EndingManager.instance.isBossFightActive)
        {
            Time.timeScale = 1f;
            EndingManager.instance.TriggerPlayerDefeated();
            return;
        }

        deathScreen.SetActive(true);
        Time.timeScale = 0;
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSecondsRealtime(delayBeforeMenu);

        Time.timeScale = 1f;

        CleanupPersistentObjects();

        SceneManager.LoadScene("Hub", LoadSceneMode.Single);
    }

    private void CleanupPersistentObjects()
    {
        var persistents = FindObjectsOfType<PersistentObject>();

        foreach (var obj in persistents)
        {
            if (obj.GetComponent<MetaBuffManager>() != null)
                continue;

            if (obj.GetComponent<SaveLoadManager>() != null)
                continue;

            Destroy(obj.gameObject);
        }
    }
}
