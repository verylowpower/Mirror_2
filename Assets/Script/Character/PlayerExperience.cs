using UnityEngine;
using System;

public class PlayerExperience : MonoBehaviour
{
    public static PlayerExperience instance;

    public event Action<int> OnLevelUp;
    public event Action<long, long> OnExpChanged;

    private long expToNextLevel;
    public long totalExp = 0;
    public int level = 1;

    public float collectRadius = 3f;

    void Awake()
    {
        instance = this;
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

        if (!PlayerBuffManager.instance.buffUIActive)
        {
            RandomSystem.instance.RandomBuff();
            PlayerBuffManager.instance.buffUIActive = true;
        }
    }

    public void SetData(long exp, int lvl)
    {
        totalExp = exp;
        level = lvl;

        expToNextLevel = ExpTable.GetExpRequired(level);
        OnExpChanged?.Invoke(totalExp, expToNextLevel);
        OnLevelUp?.Invoke(level);
    }


    public int GetLevel() => level;
    public long GetExp() => totalExp;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, collectRadius);
    }
}
