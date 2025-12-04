using UnityEngine;
using System;

public class PlayerExperience : MonoBehaviour
{
    public event Action<int> OnLevelUp;
    public event Action<long, long> OnExpChanged;

    [SerializeField] private long expToNextLevel = 100;
    private long totalExp = 0;
    private int level = 1;

    public void AddExp(long amount)
    {
        totalExp += amount;
        OnExpChanged?.Invoke(totalExp, expToNextLevel);

        if (totalExp >= expToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        level++;
        expToNextLevel += 100; // hoặc dùng curve
        OnLevelUp?.Invoke(level);
    }
}
