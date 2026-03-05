using System;
using System.Collections;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;

    public GameProgress metaProgress;
    private bool isLoadingGame = false;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // metaProgress = LoadProgress(1);

        // if (metaProgress == null)
        // {
        //     Debug.Log("No save found. Waiting for StartNewGame.");
        // }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        metaProgress = LoadProgress(1);

        if (metaProgress == null)
        {
            StartNewGame();
            Debug.Log("Auto create new save");
        }
    }
    public void StartNewGame()
    {
        isLoadingGame = true;

        metaProgress = new GameProgress
        {
            id = 1,
            currentLevel = 1,
            playerHealth = 100,
            collectRadius = 7.5f,
            moveSpeed = 5f,
            meleeDamage = 10,
            fireRate = 1f,
            playerPoint = 0,
            currentSceneIndex = 1,
            unlockedBuffsJson = "",
            completedQuestsJson = "",
            activeQuestJson = ""
        };

        SaveProgress(metaProgress);
        SceneManager.LoadScene(metaProgress.currentSceneIndex);
    }

    public void LoadGame()
    {
        isLoadingGame = true;

        metaProgress = LoadProgress(1);

        if (metaProgress == null)
        {
            Debug.LogError("No save found!");
            return;
        }

        SceneManager.LoadScene(metaProgress.currentSceneIndex);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (metaProgress == null) return;

        if (isLoadingGame)
        {
            if (scene.buildIndex >= 1)
            {
                StartCoroutine(FullLoadSequence());
            }

            return;
        }

        if (scene.buildIndex >= 1)
        {
            SaveGame();
        }
    }

    private IEnumerator FullLoadSequence()
    {
        yield return null;

        MetaBuffManager.instance?.LoadFromJson(metaProgress.unlockedBuffsJson);
        SkillTreeManager.instance?.LoadBuffData(metaProgress);
        QuestManager.instance?.LoadQuestData(metaProgress);

        yield return null;

        PlayerSnapshot.Instance?.LoadFromProgress(metaProgress);

        Debug.Log("LOAD COMPLETE");

        isLoadingGame = false;
    }

    public void SaveGame()
    {
        if (metaProgress == null)
        {
            Debug.LogError("SaveGame called but metaProgress is null!");
            return;
        }

        metaProgress.currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //metaProgress.totalExp = PlayerExperience.instance.totalExp;

        PlayerSnapshot.Instance?.ApplyToProgress(metaProgress);

        SaveProgress(metaProgress);
        Debug.Log("Saving health: " + PlayerHealth.instance?.currentHealth);
        Debug.Log("Game Saved Scene: " + metaProgress.currentSceneIndex);
    }

    public void UpdatePoint(int newPoint)
    {
        if (metaProgress == null)
            metaProgress = LoadProgress(1);

        if (metaProgress == null)
            return;

        metaProgress.playerPoint = newPoint;
        SaveProgress(metaProgress);
    }

    private void SaveProgress(GameProgress progress)
    {
        using (var connection =
            new SqliteConnection(GameProgressDatabase.Instance.DbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                @"INSERT OR REPLACE INTO GameProgress
                (id,
                 currentLevel,
                 totalExp,
                 playerHealth,
                 playerPoint,
                 meleeDamage,
                 collectRadius,
                 moveSpeed,
                 fireRate,
                 currentSceneIndex,
                 unlockedBuffsJson,
                 completedQuestsJson,
                 activeQuestJson)
                VALUES
                (@id,@level,@totalExp,@health,@point,@damage,
                 @radius,@speed,@fireRate,@sceneIndex,
                 @buffs,@completed,@active);";

                command.Parameters.AddWithValue("@id", progress.id);
                command.Parameters.AddWithValue("@level", progress.currentLevel);
                command.Parameters.AddWithValue("@totalExp", progress.totalExp);
                command.Parameters.AddWithValue("@health", progress.playerHealth);
                command.Parameters.AddWithValue("@point", progress.playerPoint);
                command.Parameters.AddWithValue("@damage", progress.meleeDamage);
                command.Parameters.AddWithValue("@radius", progress.collectRadius);
                command.Parameters.AddWithValue("@speed", progress.moveSpeed);
                command.Parameters.AddWithValue("@fireRate", progress.fireRate);
                command.Parameters.AddWithValue("@sceneIndex", progress.currentSceneIndex);
                command.Parameters.AddWithValue("@buffs", progress.unlockedBuffsJson ?? "");
                command.Parameters.AddWithValue("@completed", progress.completedQuestsJson ?? "");
                command.Parameters.AddWithValue("@active", progress.activeQuestJson ?? "");

                command.ExecuteNonQuery();
            }
        }
    }

    private GameProgress LoadProgress(int id)
    {
        using (var connection =
            new SqliteConnection(GameProgressDatabase.Instance.DbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "SELECT * FROM GameProgress WHERE id=@id";
                command.Parameters.AddWithValue("@id", id);

                using (IDataReader reader =
                    command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new GameProgress
                        {
                            id = reader.GetInt32(0),
                            currentLevel = reader.GetInt32(1),
                            totalExp = reader.GetInt64(2),
                            playerHealth = reader.GetInt32(3),
                            playerPoint = reader.GetInt32(4),
                            meleeDamage = reader.GetInt32(5),
                            collectRadius = (float)reader.GetDouble(6),
                            moveSpeed = (float)reader.GetDouble(7),
                            fireRate = (float)reader.GetDouble(8),
                            currentSceneIndex = reader.GetInt32(9),
                            unlockedBuffsJson = reader.IsDBNull(10) ? "" : reader.GetString(10),
                            completedQuestsJson = reader.IsDBNull(11) ? "" : reader.GetString(11),
                            activeQuestJson = reader.IsDBNull(12) ? "" : reader.GetString(12)
                        };
                    }
                }
            }
        }

        return null;
    }

    public GameProgress GetProgress()
    {
        return metaProgress;
    }
}