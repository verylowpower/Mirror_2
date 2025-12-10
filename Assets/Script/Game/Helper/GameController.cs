using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [Header("Game controller")]
    public float inGameTime = 0f;

    public delegate void TimeInGame();
    public event TimeInGame TimeChange;

    public int enemyKilled;
    public delegate void EnemyDied();
    public event EnemyDied KilledEnemy;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {

        inGameTime += Time.deltaTime;
        TimeChange?.Invoke();
    }

    public void AddKill()
    {
        enemyKilled++;
        KilledEnemy?.Invoke();
    }
}
