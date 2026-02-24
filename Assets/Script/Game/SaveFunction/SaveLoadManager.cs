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

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
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
            Debug.Log("Auto create new save");
        }
    }

    // =========================================================
    // NEW GAME
    // =========================================================

    public void StartNewGame()
    {
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

    // =========================================================
    // LOAD GAME
    // =========================================================

    public void LoadGame()
    {
        if (metaProgress == null)
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

        if (scene.buildIndex >= 2)
        {
            SaveGame();
        }

        MetaBuffManager.instance?.LoadFromJson(
            metaProgress.unlockedBuffsJson
        );

        SkillTreeManager.instance?.LoadBuffData(metaProgress);

        QuestManager.instance?.LoadQuestData(metaProgress);

        if (PointCounter.instance != null)
        {
            PointCounter.instance.SetPoint(metaProgress.playerPoint);
        }

        StartCoroutine(ApplyLoadedData());
    }

    private IEnumerator ApplyLoadedData()
    {
        yield return null;

        if (metaProgress == null) yield break;

        int index = metaProgress.currentSceneIndex;

        if (PointCounter.instance != null)
            PointCounter.instance.SetPoint(metaProgress.playerPoint);

        if (index >= 2 && PlayerSnapshot.instance != null)
        {
            PlayerSnapshot.instance.LoadFromProgress(metaProgress);
        }
    }

    // =========================================================
    // SAVE GAME
    // =========================================================

    public void SaveGame()
    {
        if (metaProgress == null)
        {
            Debug.LogError("SaveGame called but metaProgress is null!");
            return;
        }

        int index = SceneManager.GetActiveScene().buildIndex;
        metaProgress.currentSceneIndex = index;

        if (index >= 2 && PlayerSnapshot.instance != null)
        {
            PlayerSnapshot.instance.ApplyToProgress(metaProgress);
        }

        SaveProgress(metaProgress);

        Debug.Log("Saved scene: " + index);
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

    // =========================================================
    // DATABASE
    // =========================================================

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
                (@id,@level,@health,@point,@damage,
                 @radius,@speed,@fireRate,@sceneIndex,
                 @buffs,@completed,@active);";

                command.Parameters.AddWithValue("@id", progress.id);
                command.Parameters.AddWithValue("@level", progress.currentLevel);
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
                            playerHealth = reader.GetInt32(2),
                            playerPoint = reader.GetInt32(3),
                            meleeDamage = reader.GetInt32(4),
                            collectRadius = (float)reader.GetDouble(5),
                            moveSpeed = (float)reader.GetDouble(6),
                            fireRate = (float)reader.GetDouble(7),
                            currentSceneIndex = reader.GetInt32(8),
                            unlockedBuffsJson = reader.GetString(9),
                            completedQuestsJson = reader.GetString(10),
                            activeQuestJson = reader.GetString(11)
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