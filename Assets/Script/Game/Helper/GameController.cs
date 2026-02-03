using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [Header("Game controller")]
    public float inGameTime = 0f;

    public delegate void TimeInGame();
    public event TimeInGame TimeChange;

    public int enemyKilled;

    // public delegate void EnemyDied();
    // public event EnemyDied KilledEnemy;

    // [Header("CharacterStat")]
    // public int playerHealth;
    // public long playerExp;
    // public int playerLevel;
    // public int playerPoint;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        inGameTime += Time.deltaTime;
        TimeChange?.Invoke();
    }

    // public void AddKill(int point)
    // {
    //     enemyKilled += point;
    //     KilledEnemy?.Invoke();
    // }


    // public void SavePlayerStats()
    // {
    //     playerHealth = PlayerHealth.instance.currentHealth;
    //     playerExp = PlayerExperience.instance.totalExp;
    //     playerLevel = PlayerExperience.instance.level;
    //     playerPoint = enemyKilled;
    // }

    // public void LoadPlayerStats()
    // {
    //     PlayerHealth.instance.SetHealth(playerHealth);
    //     PlayerExperience.instance.SetData(playerExp, playerLevel);
    // }
}
