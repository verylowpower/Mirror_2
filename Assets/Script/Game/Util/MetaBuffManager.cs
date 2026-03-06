using System.Collections.Generic;
using UnityEngine;

public class MetaBuffManager : MonoBehaviour
{
    public static MetaBuffManager instance;

    public HashSet<string> unlockedBuffs = new();
    public HashSet<string> questUnlockedBuffs = new();

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        var progress = SaveLoadManager.Instance?.GetProgress();
        if (progress != null)
        {
            LoadFromJson(progress.unlockedBuffsJson);
            Debug.Log("Buff loaded in MetaBuffManager Awake");
        }
    }

    public bool IsQuestUnlocked(string id)
    {
        return questUnlockedBuffs.Contains(id);
    }

    public void UnlockByQuest(string id)
    {
        if (string.IsNullOrEmpty(id)) return;

        if (questUnlockedBuffs.Add(id))
        {
            SaveBuffs();
            Debug.Log($"Buff {id} unlocked by quest");
        }
    }

    public bool IsUnlocked(string id)
    {
        return unlockedBuffs.Contains(id);
    }

    public void Unlock(string id)
    {
        if (string.IsNullOrEmpty(id)) return;

        if (unlockedBuffs.Add(id))
        {
            SaveBuffs();
        }
    }
    void SaveBuffs()
    {
        var progress = SaveLoadManager.Instance?.GetProgress();
        if (progress == null) return;

        progress.unlockedBuffsJson = GetSaveJson();

        SaveLoadManager.Instance.SaveGame();
    }

    public void ClearAll()
    {
        unlockedBuffs.Clear();
    }
    
    public string GetSaveJson()
    {
        BuffSaveWrapper wrapper =
            new BuffSaveWrapper(unlockedBuffs, questUnlockedBuffs);

        return JsonUtility.ToJson(wrapper);
    }

    public void LoadFromJson(string json)
    {
        if (string.IsNullOrEmpty(json)) return;

        BuffSaveWrapper wrapper =
            JsonUtility.FromJson<BuffSaveWrapper>(json);

        if (wrapper == null) return;

        unlockedBuffs.Clear();
        questUnlockedBuffs.Clear();

        if (wrapper.buffIds != null)
        {
            foreach (var id in wrapper.buffIds)
                unlockedBuffs.Add(id);
        }

        if (wrapper.questUnlockedIds != null)
        {
            foreach (var id in wrapper.questUnlockedIds)
                questUnlockedBuffs.Add(id);
        }
    }

    public bool AreAllBuffsUnlocked()
    {
        foreach (var skill in SkillTreeManager.instance.skills)
        {
            if (!unlockedBuffs.Contains(skill.data.skillId))
                return false;
        }
        return true;
    }
}
