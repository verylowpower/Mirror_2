using UnityEngine;
using Mono.Data.Sqlite;
using System.IO;

public class GameProgressDatabase : MonoBehaviour
{
    public static GameProgressDatabase Instance { get; private set; }

    private const string TABLE_NAME = "GameProgress";

    [SerializeField] private string dbName = "GameProgress.db";

    public string DbPath { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeDatabase();
    }

    void InitializeDatabase()
    {
        DbPath = "URI=file:" +
            Path.Combine(Application.persistentDataPath, dbName);

        using (var connection = new SqliteConnection(DbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                        $@"CREATE TABLE IF NOT EXISTS {TABLE_NAME} (
                            id INTEGER PRIMARY KEY,
                            currentLevel INTEGER,
                            totalExp INTEGER,
                            playerHealth INTEGER,
                            playerPoint INTEGER,
                            meleeDamage INTEGER,
                            collectRadius REAL,
                            moveSpeed REAL,
                            fireRate REAL,
                            currentSceneIndex INTEGER,
                            unlockedBuffsJson TEXT,
                            completedQuestsJson TEXT,
                            activeQuestJson TEXT
                        );";
                command.ExecuteNonQuery();
            }
        }
    }
}