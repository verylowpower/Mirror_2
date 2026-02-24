using UnityEngine;

public class PlayerSnapshot : MonoBehaviour
{
    public static PlayerSnapshot instance;

    void Awake()
    {
        instance = this;
    }

    // void Start()
    // {
    //     SaveLoadManager.Instance.SaveGame();
    // }

    public void ApplyToProgress(GameProgress progress)
    {
        progress.playerHealth = PlayerHealth.instance.currentHealth;
        progress.collectRadius = PlayerExperience.instance.collectRadius;
        progress.moveSpeed = PlayerController.instance.moveSpeed;
        progress.playerPoint = PointCounter.instance.point;

        progress.meleeDamage = PlayerAttack.instance.meleeDamage;
        progress.fireRate = PlayerAttack.instance.fireRate;
    }

    public void LoadFromProgress(GameProgress progress)
    {
        PlayerHealth.instance.currentHealth = progress.playerHealth;
        PlayerExperience.instance.collectRadius = progress.collectRadius;
        PlayerController.instance.moveSpeed = progress.moveSpeed;
        PointCounter.instance.SetPoint(progress.playerPoint);

        PlayerAttack.instance.meleeDamage = progress.meleeDamage;
        PlayerAttack.instance.fireRate = progress.fireRate;
    }
}
