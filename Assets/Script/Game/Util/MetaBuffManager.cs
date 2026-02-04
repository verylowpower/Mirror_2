using System.Collections.Generic;
using UnityEngine;

public class MetaBuffManager : MonoBehaviour
{
    public static MetaBuffManager instance;

    public HashSet<string> unlockedBuffs = new();

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool IsUnlocked(string id)
    {
        return unlockedBuffs.Contains(id);
    }

    public void Unlock(string id)
    {
        unlockedBuffs.Add(id);
    }
}
