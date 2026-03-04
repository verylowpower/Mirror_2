using UnityEngine;

public class PlayerSnapshot : MonoBehaviour
{
    public static PlayerSnapshot Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ApplyToProgress(GameProgress progress)
    {
        Debug.Log("SNAPSHOT");
        if (progress == null) return;

        if (PlayerHealth.instance != null)
            progress.playerHealth = PlayerHealth.instance.currentHealth;

        if (PlayerExperience.instance != null)
            progress.collectRadius = PlayerExperience.instance.collectRadius;

        if (PlayerController.instance != null)
            progress.moveSpeed = PlayerController.instance.moveSpeed;

        if (PointCounter.instance != null)
            progress.playerPoint = PointCounter.instance.point;

        if (PlayerAttack.instance != null)
        {
            progress.meleeDamage = PlayerAttack.instance.meleeDamage;
            progress.fireRate = PlayerAttack.instance.fireRate;
        }

        progress.currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

        progress.currentLevel = PlayerExperience.instance.level;
    }

    // =========================================================
    // LOAD
    // =========================================================

    public void LoadFromProgress(GameProgress progress)
    {
        if (progress == null) return;

        if (PlayerHealth.instance != null)
            PlayerHealth.instance.currentHealth = progress.playerHealth;

        if (PlayerExperience.instance != null)
            PlayerExperience.instance.collectRadius = progress.collectRadius;

        if (PlayerController.instance != null)
            PlayerController.instance.moveSpeed = progress.moveSpeed;

        if (PointCounter.instance != null)
            PointCounter.instance.SetPoint(progress.playerPoint);

        if (PlayerAttack.instance != null)
        {
            PlayerAttack.instance.meleeDamage = progress.meleeDamage;
            PlayerAttack.instance.fireRate = progress.fireRate;
        }
    }
}