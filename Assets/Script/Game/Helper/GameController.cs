using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [Header("Game controller")]
    public float inGameTime = 0f;

    public delegate void TimeInGame();
    public event TimeInGame TimeChange;

    public int enemyKilled;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "Hub")
        {
            inGameTime += Time.deltaTime;
            TimeChange?.Invoke();
        }
    }

    public void ResetRun()
    {
        inGameTime = 0f;
        enemyKilled = 0;
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}