using UnityEngine;
using System;

public class PlayerExperience : MonoBehaviour
{
    public static PlayerExperience instance;

    public event Action<int> OnLevelUp;
    public event Action<long, long> OnExpChanged;

    private long expToNextLevel;
    private long totalExp = 0;
    private int level = 1;

    public float collectRadius = 3f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }


    void Start()
    {
        expToNextLevel = ExpTable.GetExpRequired(level);

        OnExpChanged?.Invoke(totalExp, expToNextLevel);
    }

    public void AddExp(long amount)
    {
        totalExp += amount;

        while (totalExp >= expToNextLevel)
        {
            totalExp -= expToNextLevel;
            LevelUp();
        }

        OnExpChanged?.Invoke(totalExp, expToNextLevel);
    }

    void LevelUp()
    {
        level++;

        expToNextLevel = ExpTable.GetExpRequired(level);

        OnLevelUp?.Invoke(level);

        OnExpChanged?.Invoke(totalExp, expToNextLevel);
    }

    public int GetLevel() => level;
    public long GetExp() => totalExp;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, collectRadius);
    }
}
