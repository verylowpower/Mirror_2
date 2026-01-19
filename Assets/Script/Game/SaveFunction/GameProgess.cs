using UnityEngine;

[System.Serializable]
public class GameProgress
{
    public int id;                 // khóa chính trong database
    public int currentWave;
    public bool hasBuff;
    public int currentLevel;
    public int playerHealth;
    public bool bossDefeated;
    public Vector3 playerPosition;
}
