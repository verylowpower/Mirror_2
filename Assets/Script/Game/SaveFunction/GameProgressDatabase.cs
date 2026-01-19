using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;


public class GameProgressDatabase : MonoBehaviour
{
    public static GameProgressDatabase instance;
    public string dbPath;

    private void Awake()
    {
        instance = this;
        dbPath = "URI=file:" + Application.persistentDataPath + "/gameprogress.db";
        CreateTable();
    }

    private void CreateTable()
    {
        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                @"CREATE TABLE IF NOT EXISTS GameProgress (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    currentWave INTEGER,
                    hasBuff INTEGER,
                    currentLevel INTEGER,
                    playerHealth INTEGER,
                    bossDefeated INTEGER,
                    posX REAL,
                    posY REAL,
                    posZ REAL
                );";
                command.ExecuteNonQuery();
            }
        }
    }
}