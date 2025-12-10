using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathHandler : MonoBehaviour
{
    public static DeathHandler instance;

    [SerializeField] private GameObject deathScreen;

    private void Awake()
    {
        instance = this;
    }

    public void HandlePlayerDeath()
    {
        deathScreen.SetActive(true);

        Time.timeScale = 0;

        StartCoroutine(LoadMenuAfterDelay());
    }

    private IEnumerator LoadMenuAfterDelay()
    {
        yield return new WaitForSecondsRealtime(3f);

        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(0);
    }

}
