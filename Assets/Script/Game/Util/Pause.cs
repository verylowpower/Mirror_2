using UnityEngine;

public class Pause : MonoBehaviour
{
    public static Pause instance;
    public GameObject pauseMenu;
    public bool isPaused;

    private SaveLoadManager saveLoad;

    void Awake()
    {
        instance = this;
        saveLoad = FindObjectOfType<SaveLoadManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);

        SaveGame();
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    private void SaveGame()
    {
        GameProgress progress = new GameProgress
        {
            id = 1,
            currentWave = Room.instance.currentWave,
            //hasBuff = PlayerBuffManager.instance.buffUIActive,
            currentLevel = PlayerExperience.instance.GetLevel(),
            playerHealth = PlayerHealth.instance.currentHealth,
            // bossDefeated = GameController.instance.BossDefeated,
            playerPosition = PlayerController.instance.transform.position
        };

        saveLoad.SaveProgress(progress);
    }
}
