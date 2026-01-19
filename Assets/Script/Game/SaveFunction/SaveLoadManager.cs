using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    //default load
    public GameProgress LoadOrCreateDefault(int id = 1)
    {
        GameProgress data = LoadProgress(id);
        if (data != null) return data;

        return new GameProgress
        {
            id = id,
            currentWave = 0,
            currentLevel = 1,
            playerHealth = 100,
            hasBuff = false,
            bossDefeated = false,
            playerPosition = Vector3.zero
        };
    }

    public void SaveProgress(GameProgress progress)
    {
        using (var connection = new SqliteConnection(GameProgressDatabase.instance.dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                @"INSERT OR REPLACE INTO GameProgress
                  (id, currentWave, hasBuff, currentLevel, playerHealth, bossDefeated, posX, posY, posZ)
                  VALUES
                  (@id, @wave, @buff, @level, @health, @boss, @x, @y, @z);";

                command.Parameters.AddWithValue("@id", progress.id);
                command.Parameters.AddWithValue("@wave", progress.currentWave);
                command.Parameters.AddWithValue("@buff", progress.hasBuff ? 1 : 0);
                command.Parameters.AddWithValue("@level", progress.currentLevel);
                command.Parameters.AddWithValue("@health", progress.playerHealth);
                command.Parameters.AddWithValue("@boss", progress.bossDefeated ? 1 : 0);
                command.Parameters.AddWithValue("@x", progress.playerPosition.x);
                command.Parameters.AddWithValue("@y", progress.playerPosition.y);
                command.Parameters.AddWithValue("@z", progress.playerPosition.z);

                command.ExecuteNonQuery();
            }
        }
    }

    public GameProgress LoadProgress(int id = 1)
    {
        using (var connection = new SqliteConnection(GameProgressDatabase.instance.dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM GameProgress WHERE id=@id";
                command.Parameters.AddWithValue("@id", id);

                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new GameProgress
                        {
                            id = reader.GetInt32(0),
                            currentWave = reader.GetInt32(1),
                            hasBuff = reader.GetInt32(2) == 1,
                            currentLevel = reader.GetInt32(3),
                            playerHealth = reader.GetInt32(4),
                            bossDefeated = reader.GetInt32(5) == 1,
                            playerPosition = new Vector3(
                                reader.GetFloat(6),
                                reader.GetFloat(7),
                                reader.GetFloat(8)
                            )
                        };
                    }
                }
            }
        }
        return null;
    }


}

